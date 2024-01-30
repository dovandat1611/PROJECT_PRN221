using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public async Task OnGetAsync(int? pageIndex)
        {
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
        }
    }
}
