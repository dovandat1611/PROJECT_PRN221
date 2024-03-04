using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;

namespace PROJECT_PRN221.Pages.adminsite.admin
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

        public PaginatedList<Admin> Admin { get;set; } = default!;

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
                return RedirectToPage("/adminsite/authenticate/login/Index");
            }
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (_context.Admins != null)
            {
                IQueryable<Admin> adminsIQ = from s in _context.Admins select s;

                var pageSize = Configuration.GetValue("PageSize", 10);
                Admin = await PaginatedList<Admin>.CreateAsync(
                adminsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            return Page();
        }
        public async Task OnPostAsync(int id, string service, string status, string name, int? pageIndex)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }

            if (_context.Customers != null)
            {
                IQueryable<Admin> adminsIQ = null;
                if (service.Equals("updateStatus"))
                {
                    Admin admin = _context.Admins.FirstOrDefault(x => x.AdminId == id);
                    admin.Status = status;
                    _context.Admins.Update(admin);
                    _context.SaveChanges();

                    adminsIQ = from s in _context.Admins select s;
                    
                }
                if (service.Equals("searchByName"))
                {
                    adminsIQ = from s in _context.Admins where s.Name.Contains(name) select s;
                }
                var pageSize = Configuration.GetValue("PageSize", 1);
                Admin = await PaginatedList<Admin>.CreateAsync(
                adminsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
        }
    }
}
