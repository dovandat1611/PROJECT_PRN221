using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.customersite.profile
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public Customer Customer { get; set; }
        public string gender { get; set; }

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

        public async Task<IActionResult> OnGetAsync()
        {
            if (checkSession() == false)
            {
                return RedirectToPage("/customersite/authenticate/login/Index");
            }

            int id = 0;
            string customerJson = HttpContext.Session.GetString("customer");
            if (!string.IsNullOrEmpty(customerJson))
            {
                Customer customer = JsonConvert.DeserializeObject<Customer>(customerJson);
                id = customer.CustomerId;
            }
            Customer = _context.Customers.FirstOrDefault(x => x.CustomerId == id);

            if (Customer.Gender == true)
            {
                gender = "Male";
            }
            else
            {
                gender = "Female";
            }

            return Page();
        }
    }
}
