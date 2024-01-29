using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

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
        public IList<Product> RelateProduct { get; set; } = default!;
        public async Task OnGetAsync(int id)
        {
            if (_context.Products != null)
            {
                Product = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category).Where(x => x.ProductId == id).FirstOrDefault();

                RelateProduct = await _context.Products.Include(p => p.Brand)
               .Include(p => p.Category).Where(x => x.CategoryId == Product.CategoryId).ToListAsync();
            }
        }

        public async Task OnPostAsync()
        {

        }

    }
}