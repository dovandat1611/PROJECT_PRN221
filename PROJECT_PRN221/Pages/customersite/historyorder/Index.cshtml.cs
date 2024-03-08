using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;

namespace PROJECT_PRN221.Pages.customersite.historyorder
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public PaginatedList<Order> Orders { get; set; }
        public int Wait { get; set; }
        public int Process { get; set; }
        public int Done { get; set; }
        public int Cancel { get; set; }

        public bool checkSession()
        {
            bool checkS = true;
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                string isCustomerAuthenticated = httpContext.Session.GetString("customer");
                if (string.IsNullOrEmpty(isCustomerAuthenticated))
                {
                    checkS = false;
                }
            }

            return checkS;
        }

        public async Task<IActionResult> OnGetAsync(string service, string status, int? pageIndex)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (checkSession() == false)
            {
                return RedirectToPage("/customersite/authenticate/login/Index");
            }

            string customerJson = HttpContext.Session.GetString("customer");
            Customer customer = null;
            if (!string.IsNullOrEmpty(customerJson))
            {
                customer = JsonConvert.DeserializeObject<Customer>(customerJson);
            }

            Wait = _context.Orders.Where(x => x.CustomerId == customer.CustomerId && x.Status.Equals("Wait")).Count();
            Process = _context.Orders.Where(x => x.CustomerId == customer.CustomerId && x.Status.Equals("Process")).Count();
            Done = _context.Orders.Where(x => x.CustomerId == customer.CustomerId && x.Status.Equals("Done")).Count();
            Cancel = _context.Orders.Where(x => x.CustomerId == customer.CustomerId && x.Status.Equals("Cancel")).Count();

            if (string.IsNullOrWhiteSpace(service))
            {
                service = "DisplayOrderHistpory";
            }

            IQueryable<Order> IQ = null;
            if (service == "DisplayOrderHistpory")
            {
                if (_context != null)
                {

                    IQ = _context.Orders
                    .Where(x => x.CustomerId == customer.CustomerId)
                    .OrderByDescending(x => x.OderDate);

                }
            }
            if (service == "displayOrderStatus")
            {
                if (_context != null)
                {
                    IQ = _context.Orders
                    .Where(x => x.CustomerId == customer.CustomerId && x.Status.Equals(status))
                    .OrderByDescending(x => x.OrderId);
                }
            }

            Orders = await PaginatedList<Order>.CreateAsync(
            IQ.AsNoTracking(), pageIndex ?? 1, 6);

            return Page();
        }


        public async Task OnPostAsync()
        {

        }

    }
}
