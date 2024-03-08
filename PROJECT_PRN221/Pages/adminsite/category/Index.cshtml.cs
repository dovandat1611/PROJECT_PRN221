using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;

namespace PROJECT_PRN221.Pages.adminsite.category
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

        public PaginatedList<Category> Category { get;set; } = default!;
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

            if (_context.Categories != null)
            {
                IQueryable<Category> IQ = from s in _context.Categories select s;
                var pageSize = Configuration.GetValue("PageSize", 10);
                Category = await PaginatedList<Category>.CreateAsync(
                IQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? pageIndex, string service, string name)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (service == null)
            {
                return Page();
            }

            if (service.Equals("searchCategory"))
            {
                if (_context.Categories != null)
                {
                    IQueryable<Category> IQ = from s in _context.Categories where s.CategoryName.Contains(name) select s;
                    var pageSize = Configuration.GetValue("PageSize", 10);
                    Category = await PaginatedList<Category>.CreateAsync(
                    IQ.AsNoTracking(), pageIndex ?? 1, pageSize);
                }
            }
            return Page();
        }

    }
}
