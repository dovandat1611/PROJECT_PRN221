using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;

namespace PROJECT_PRN221.Pages.adminsite.product
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

        public PaginatedList<Product> Product { get;set; } = default!;
        public bool checkSession()
        {
            bool checkS = true;
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                string isCustomerAuthenticated = httpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(isCustomerAuthenticated))
                {
                    checkS = false;
                }
            }
            return checkS;
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            if (checkSession() == false)
            {
                return RedirectToPage("/BadRequest");
            }
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (_context.Products != null)
            {

                IQueryable<Product> productsIQ = _context.Products.Include(p => p.Brand).Include(p => p.Category);

                var pageSize = Configuration.GetValue("PageSize", 10);
                Product = await PaginatedList<Product>.CreateAsync(
                productsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile inputFile, string service, string name, int? pageIndex)
        {


            if(string.IsNullOrEmpty(service))
            {
                return Page();
            }
            if (service.Equals("searchProduct"))
            {
                IQueryable<Product> productsIQ = _context.Products.Include(p => p.Brand).Include(p => p.Category).Where(x => x.ProductName.Contains(name));
                var pageSize = Configuration.GetValue("PageSize", 10);
                Product = await PaginatedList<Product>.CreateAsync(
                productsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            if (service.Equals("inputFile"))
            {
                string fileContent = string.Empty;
                if (inputFile != null)
                {
                    using (var reader = new StreamReader(inputFile.OpenReadStream()))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    if (!string.IsNullOrEmpty(fileContent))
                    {
                        var listOfProduct = JsonConvert.DeserializeObject<List<Product>>(fileContent);
                        if (listOfProduct.Count > 0)
                        {
                            foreach (var item in listOfProduct)
                            {
                                item.ProductId = 0;
                            }
                            _context.Products.AddRange(listOfProduct);
                            _context.SaveChanges();
                        }
                    }
                }
                IQueryable<Product> productsIQ = _context.Products.Include(p => p.Brand).Include(p => p.Category);

                var pageSize = Configuration.GetValue("PageSize", 10);
                Product = await PaginatedList<Product>.CreateAsync(
                productsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            return Page();
        }

    }
}
