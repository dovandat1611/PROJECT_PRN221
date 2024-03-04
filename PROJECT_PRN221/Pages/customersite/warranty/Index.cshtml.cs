using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.customersite.warranty
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public int Wait { get; set; }
        public int Process { get; set; }
        public int Done { get; set; }
        public int Cancel { get; set; }
        public Customer Customer { get; set; }
        public List<Warranty> warranties { get; set; }

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

        public IActionResult OnGet()
        {
            if (checkSession() == false)
            {
                return RedirectToPage("/customersite/authenticate/login/Index");
            }

            Wait = _context.Warranties.Where(x => x.Status.Equals("Wait")).Count();
            Process = _context.Warranties.Where(x => x.Status.Equals("Process")).Count();
            Done = _context.Warranties.Where(x => x.Status.Equals("Done")).Count();
            Cancel = _context.Warranties.Where(x => x.Status.Equals("Cancel")).Count();

            string customerJson = HttpContext.Session.GetString("customer");
            if (!string.IsNullOrEmpty(customerJson))
            {
                Customer = JsonConvert.DeserializeObject<Customer>(customerJson);
            }
            warranties = _context.Warranties.Include(x => x.Product).Where(x => x.CustomerId == Customer.CustomerId).ToList();

            return Page();
        }
    }
}
