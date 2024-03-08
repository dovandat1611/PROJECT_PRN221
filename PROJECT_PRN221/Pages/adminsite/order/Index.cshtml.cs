using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;
using static NuGet.Packaging.PackagingConstants;

namespace PROJECT_PRN221.Pages.adminsite.order
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

        public PaginatedList<Order> Order { get;set; } = default!;
        public int Wait { get; set; }
        public int Process { get; set; }
        public int Done { get; set; }
        public int Cancel { get; set; }
        public string Status { get; set; }

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
                return RedirectToPage("/BadRequest");
            }

            if (pageIndex == null)
            {
                pageIndex = 1;
            }

            Wait = _context.Orders.Where(x => x.Status.Equals("Wait")).Count();
            Process = _context.Orders.Where(x => x.Status.Equals("Process")).Count();
            Done = _context.Orders.Where(x => x.Status.Equals("Done")).Count();
            Cancel = _context.Orders.Where(x => x.Status.Equals("Cancel")).Count();
            Status = status;
            if (string.IsNullOrWhiteSpace(service))
            {
                service = "DisplayOrder";
            }
            if (service.Equals("DisplayOrder"))
            {
                if (_context.Orders != null)
                {
                    IQueryable<Order> ordersIQ = _context.Orders.Include(o => o.Customer);

                    var pageSize = Configuration.GetValue("PageSize", 10);
                    Order = await PaginatedList<Order>.CreateAsync(
                    ordersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

                }
            }
             
            if(service == "displayOrderStatus")
            {
                if (_context.Orders != null)
                {
                    IQueryable<Order> ordersIQ = _context.Orders.Include(o => o.Customer).Where(x => x.Status.Equals(status));

                    var pageSize = Configuration.GetValue("PageSize", 10);
                    Order = await PaginatedList<Order>.CreateAsync(
                    ordersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id, string status, string service, int? pageIndex, DateTime? month, DateTime? date)
        {
            IQueryable<Order> ordersIQ = null;
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (_context.Orders != null)
            {
                if (service.Equals("updateStatus"))
                {
                    Order order = _context.Orders.FirstOrDefault(x => x.OrderId == id);
                    order.Status = status;
                    _context.Orders.Update(order);
                    _context.SaveChanges();

                    ordersIQ = _context.Orders.Include(o => o.Customer).Where(x => x.Status.Equals(status));
                }
                if (service.Equals("SearchByMonth"))
                {
                    if(month != null)
                    {
                        int selectedMonth = month.Value.Month;
                        int selectedYear = month.Value.Year;
                        ordersIQ = _context.Orders.Include(o => o.Customer).Where(x => x.OderDate.Value.Month == selectedMonth && x.OderDate.Value.Year == selectedYear);
                    }
                    else
                    {
                       ordersIQ = _context.Orders.Include(o => o.Customer);
                    }
                }
                if (service.Equals("SearchByDate"))
                {
                    if (date != null)
                    {
                        int selectedDay = date.Value.Day;
                        int selectedMonth = date.Value.Month;
                        int selectedYear = date.Value.Year;
                        ordersIQ = _context.Orders.Include(o => o.Customer).
                            Where(x => x.OderDate.Value.Day == selectedDay && x.OderDate.Value.Month == selectedMonth && x.OderDate.Value.Year == selectedYear);
                    }
                    else
                    {
                        ordersIQ = _context.Orders.Include(o => o.Customer);
                    }
                }

            }

            var pageSize = Configuration.GetValue("PageSize", 10);
            Order = await PaginatedList<Order>.CreateAsync(
            ordersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            Wait = _context.Orders.Where(x => x.Status.Equals("Wait")).Count();
            Process = _context.Orders.Where(x => x.Status.Equals("Process")).Count();
            Done = _context.Orders.Where(x => x.Status.Equals("Done")).Count();
            Cancel = _context.Orders.Where(x => x.Status.Equals("Cancel")).Count();
            Status = status;
            return Page();
        }
    }
}
