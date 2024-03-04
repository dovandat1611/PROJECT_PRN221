using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;

namespace PROJECT_PRN221.Pages.adminsite.warranty
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

        public int Wait { get; set; }
        public int Process { get; set; }
        public int Done { get; set; }
        public int Cancel { get; set; }

        public string Status { get; set; }

        public PaginatedList<Warranty> Warranty { get;set; } = default!;

        public Admin Admin { get; set; }

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

        public async Task<IActionResult> OnGetAsync(string service, string status, int? pageIndex)
        {

            if (checkSession() == false)
            {
                return RedirectToPage("/adminsite/authenticate/login/Index");
            }

            if (pageIndex == null)
            {
                pageIndex = 1;
            }

            Wait = _context.Warranties.Where(x => x.Status.Equals("Wait")).Count();
            Process = _context.Warranties.Where(x => x.Status.Equals("Process")).Count();
            Done = _context.Warranties.Where(x => x.Status.Equals("Done")).Count();
            Cancel = _context.Warranties.Where(x => x.Status.Equals("Cancel")).Count();
            Status = status;
            string adminJson = HttpContext.Session.GetString("admin");
            if (!string.IsNullOrEmpty(adminJson))
            {
                Admin = JsonConvert.DeserializeObject<Admin>(adminJson);
            }

            if (string.IsNullOrWhiteSpace(service))
            {
                service = "Display";
            }
            if (service.Equals("Display"))
            {
                if (_context.Warranties != null)
                {
                    IQueryable<Warranty> WarrantyIQ = _context.Warranties
                    .Include(w => w.Customer)
                    .Include(w => w.Orderdetail)
                    .Include(w => w.Product);
                    var pageSize = Configuration.GetValue("PageSize", 10);
                    Warranty = await PaginatedList<Warranty>.CreateAsync(
                    WarrantyIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
                }
            }

            if (service == "displayWarrantyStatus")
            {
                if (_context != null)
                {
                    IQueryable<Warranty> WarrantyIQ = _context.Warranties
                                                    .Include(w => w.Customer)
                                                    .Include(w => w.Orderdetail)
                                                    .Include(w => w.Product).Where(x => x.Status == status);

                    var pageSize = Configuration.GetValue("PageSize", 10);
                    Warranty = await PaginatedList<Warranty>.CreateAsync(
                    WarrantyIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
                }
            }
            return Page();
        }

        public async Task OnPostAsync(string service, string status, int id, int? pageIndex)
        {   
            if (service.Equals("updateStatus"))
            {   
                Warranty warranty = _context.Warranties.FirstOrDefault(x => x.WarrantyId == id);
                warranty.Status = status;
                _context.Update(warranty);
                _context.SaveChanges();
            }
            string adminJson = HttpContext.Session.GetString("admin");
            if (!string.IsNullOrEmpty(adminJson))
            {
                Admin = JsonConvert.DeserializeObject<Admin>(adminJson);
            }
            if (_context.Warranties != null)
            {
                IQueryable<Warranty> WarrantyIQ = _context.Warranties
                .Include(w => w.Customer)
                .Include(w => w.Orderdetail)
                .Include(w => w.Product);
                var pageSize = Configuration.GetValue("PageSize", 10);
                Warranty = await PaginatedList<Warranty>.CreateAsync(
                WarrantyIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            Wait = _context.Warranties.Where(x => x.Status.Equals("Wait")).Count();
            Process = _context.Warranties.Where(x => x.Status.Equals("Process")).Count();
            Done = _context.Warranties.Where(x => x.Status.Equals("Done")).Count();
            Cancel = _context.Warranties.Where(x => x.Status.Equals("Cancel")).Count();
            Status = status;
        }
       }
}
