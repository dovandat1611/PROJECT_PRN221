using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;
using System.Configuration;

namespace PROJECT_PRN221.Pages.customersite.shop
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        private readonly IConfiguration Configuration;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }


        public PaginatedList<Product> Product { get; set; } = default!;

        public List<Category> Category { get; set; }
        public List<Brand> Brand { get; set; }

        public string isCustomerAuthenticated { get; set; } = null;
        public void checkSession()
        {
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                isCustomerAuthenticated = httpContext.Session.GetString("customer");
            }
        }
        public async Task OnGet(int? pageIndex, string service, int categoryId, int brandId)
        {
            checkSession();
            Category = _context.Categories.ToList();
            Brand = _context.Brands.ToList();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }

            if (service == null)
            {
                service = "displayAll";
            }

            if (service.Equals("displayAll"))
            {
                if (_context.Products != null)
                {
                    IQueryable<Product> productsIQ = _context.Products.Include(n => n.Category).Include(p => p.Brand);

                    Product = await PaginatedList<Product>.CreateAsync(
                    productsIQ.AsNoTracking(), pageIndex ?? 1, 6);
                }
            }

            if (service.Equals("displayProductbyCategory"))
            {
                if (_context.Products != null)
                {
                    IQueryable<Product> productsIQ = _context.Products.Include(n => n.Category).Include(p => p.Brand).Where(x => x.CategoryId == categoryId);

                    Product = await PaginatedList<Product>.CreateAsync(
                    productsIQ.AsNoTracking(), pageIndex ?? 1, 6);
                }
            }

            if (service.Equals("displayProductbyBrand"))
            {
                if (_context.Products != null)
                {
                    IQueryable<Product> productsIQ = _context.Products.Include(n => n.Category).Include(p => p.Brand).Where(x => x.BrandId == brandId);

                    Product = await PaginatedList<Product>.CreateAsync(
                    productsIQ.AsNoTracking(), pageIndex ?? 1, 6);
                }
            }


        }


        public async Task OnPost(int? pageIndex, string service, string productName)
        {
            checkSession();

            Category = _context.Categories.ToList();
            Brand = _context.Brands.ToList();

            if (pageIndex == null)
            {
                pageIndex = 1;
            }

            if (service.Equals("displayProductbyName"))
            {
                if (_context.Products != null)
                {
                    IQueryable<Product> productsIQ = _context.Products.Include(n => n.Category).Include(p => p.Brand).Where(x => x.ProductName.Contains(productName));
                    Product = await PaginatedList<Product>.CreateAsync(
                    productsIQ.AsNoTracking(), pageIndex ?? 1, 6);
                }
            }

        }

    }
}
