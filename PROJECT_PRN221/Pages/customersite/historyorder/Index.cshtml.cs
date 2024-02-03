using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.customersite.historyorder
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; }
        public int Wait { get; set; }
        public int Process { get; set; }
        public int Done { get; set; }
        public int Cancel { get; set; }

        public async Task OnGetAsync(string service, string status)
        {
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
            if (service == "DisplayOrderHistpory")
            {
                if (_context != null)
                {
                    Orders = _context.Orders
                    .Where(x => x.CustomerId == customer.CustomerId)
                    .OrderByDescending(x => x.OderDate)
                    .ToList();
                }
            }
            if (service == "displayOrderStatus")
            {
                if (_context != null)
                {
                    Orders = _context.Orders
                    .Where(x => x.CustomerId == customer.CustomerId && x.Status.Equals(status))
                    .OrderByDescending(x => x.OderDate)
                    .ToList();
                }
            }
        }


        public async Task OnPostAsync()
        {

        }

    }
}
