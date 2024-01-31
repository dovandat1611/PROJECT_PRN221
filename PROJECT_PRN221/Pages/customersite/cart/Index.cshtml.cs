using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.customersite.cart
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
            Carts = new List<Cart>();
        }

        public List<Cart> Carts { get; set; }

        public double totalprice { get; set; }


        public async Task OnGetAsync(string service, int? proid)
        {
            if (service == null)
            {
                service = "ListCart";
            }

            if (service.Equals("ListCart"))
            {
                ReturnPage();
            }

            if (service.Equals("Delete"))
            {
                List<Cart> carts = new List<Cart>();

                string GetCart = HttpContext.Session.GetString("cart");
                if (!string.IsNullOrEmpty(GetCart))
                {
                    carts = JsonConvert.DeserializeObject<List<Cart>>(GetCart);
                }

                Cart cartToDelete = carts.FirstOrDefault(c => c.id == proid);
                carts.Remove(cartToDelete);

                string WriteCart = JsonConvert.SerializeObject(carts);
                HttpContext.Session.SetString("cart", WriteCart);

                ReturnPage();
            }

        }


        public async Task OnPostAsync(int[] quantity, int[] id, string service) 
        {
            List<Cart> carts = new List<Cart>();

            string GetCart = HttpContext.Session.GetString("cart");
            if (!string.IsNullOrEmpty(GetCart))
            {
                carts = JsonConvert.DeserializeObject<List<Cart>>(GetCart);
            }

            if (service.Equals("Update"))
            {
                for (int i = 0; i < id.Length; i++)
                {

                    int itemId = id[i];
                    int itemQuantity = quantity[i];

                    var product = _context.Products.FirstOrDefault(x => x.ProductId == itemId);

                    Cart cartToUpdate = carts.FirstOrDefault(c => c.id == itemId);

                    if (cartToUpdate != null)
                    {
                        foreach (var item in carts)
                        {
                            if (item.id == cartToUpdate.id)
                            {
                                item.quantity = itemQuantity;
                                item.subtotal = (double)(product.ListPrice * item.quantity * (1 - product.Discount));
                            }
                        }
                    }
                }
                string WriteCart = JsonConvert.SerializeObject(carts);
                HttpContext.Session.SetString("cart", WriteCart);
            }
            ReturnPage();
        }

        public void ReturnPage()
        {
            string json = HttpContext.Session.GetString("cart");
            
            if (!string.IsNullOrEmpty(json))
            {
                Carts = JsonConvert.DeserializeObject<List<Cart>>(json);
                if (!Carts.IsNullOrEmpty())
                {
                    totalprice = Carts.Sum(c => c.subtotal);
                }
            }
        }
        //public void GetCart(List<Cart> carts)
        //{
        //    string GetCart = HttpContext.Session.GetString("cart");
        //    if (!string.IsNullOrEmpty(GetCart))
        //    {
        //        carts = JsonConvert.DeserializeObject<List<Cart>>(GetCart);
        //    }
        //}

        //public void WriteCart(List<Cart> carts)
        //{
        //    string WriteCart = JsonConvert.SerializeObject(carts);
        //    HttpContext.Session.SetString("cart", WriteCart);
        //}

    }
}
