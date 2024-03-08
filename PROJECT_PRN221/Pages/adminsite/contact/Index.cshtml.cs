using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;

namespace PROJECT_PRN221.Pages.adminsite.contact
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

        public PaginatedList<Contact> Contact { get;set; } = default!;
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

            if (_context.Contacts != null)
            {
                IQueryable<Contact> IQ = _context.Contacts;

                var pageSize = Configuration.GetValue("PageSize", 10);
                Contact = await PaginatedList<Contact>.CreateAsync(
                IQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? pageIndex, string service, string content)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }

            if (service == null)
            {
                return Page();
            }

            if (service.Equals("searchContact"))
            {
                if (_context.Contacts != null)
                {
                    IQueryable<Contact> IQ = _context.Contacts.Where(x => x.ContactContent.Contains(content));

                    var pageSize = Configuration.GetValue("PageSize", 10);
                    Contact = await PaginatedList<Contact>.CreateAsync(
                    IQ.AsNoTracking(), pageIndex ?? 1, pageSize);
                }
            }
            return Page();
        }

    }
}
