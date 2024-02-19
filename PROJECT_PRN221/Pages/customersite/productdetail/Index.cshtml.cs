using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;
using Microsoft.IdentityModel.Tokens;

namespace PROJECT_PRN221.Pages.customersite.productdetail
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public Product Product { get; set; } = default!;
        public List<Comment> Comment { get; set; } = default!;
        public IList<Product> RelateProduct { get; set; } = new List<Product>(); // Initialize with an empty list
        public string isCustomerAuthenticated { get; set; } = null;
        public void checkSession()
        {
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                isCustomerAuthenticated = httpContext.Session.GetString("customer");
            }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            checkSession();

            Comment = _context.Comments
                .Include(p => p.Customer)
                .Include(p => p.Product)
                .Where(x => x.ProductId == id)
                .ToList();

            Product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.ProductId == id);

            

            if (Product == null)
            {
                return NotFound(); 
            }

            RelateProduct = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Where(x => x.CategoryId == Product.CategoryId && x.ProductId != id)
                .ToListAsync();

            return Page();
        }

        public IActionResult OnPost(int id, int quantity, string service,int product_id, string comment_content)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == id);

            if (product == null)
            {
                return NotFound(); 
            }

            if (service.Equals("insertComment"))
            {
                string customerJson = HttpContext.Session.GetString("customer");
                Customer customer = JsonConvert.DeserializeObject<Customer>(customerJson);

                Comment comment = new Comment()
                {
                    ProductId = product_id,
                    CustomerId = customer.CustomerId,
                    CommentContent = comment_content,
                    CommentDate = DateTime.Now,
                };
                _context.Comments.Add(comment);
                _context.SaveChanges();
            }


            if (service.Equals("Create"))
            {
                Cart cart = new Cart()
                {
                    id = id,
                    name = product.ProductName,
                    quantity = quantity,
                    price = (double)product.ListPrice,
                    image = product.Image,
                    subtotal = (double)(product.ListPrice * quantity * (double)(1 - product.Discount))
                };

                List<Cart> carts = new List<Cart>();

                string getSession = HttpContext.Session.GetString("cart");

                if (getSession.IsNullOrEmpty())
                {
                    carts.Add(cart);
                }
                else
                {
                    carts = JsonConvert.DeserializeObject<List<Cart>>(getSession);

                    Cart c = carts.FirstOrDefault(x => x.id == cart.id);

                    if(c != null)
                    {
                        foreach(var item in carts)
                        {
                            if(item.id == c.id)
                            {
                                item.quantity += quantity;
                                item.subtotal += (double)(product.ListPrice * item.quantity * (double)(1 - product.Discount));
                            }
                        }
                    }
                    else
                    {
                        carts.Add(cart);
                    }
                }

                string cartJson = JsonConvert.SerializeObject(carts);
                HttpContext.Session.SetString("cart", cartJson);

                return RedirectToPage("/customersite/cart/Index");
            }
            return RedirectToPage("/customersite/productdetail/Index", new { id = id });
        }
    }
}
