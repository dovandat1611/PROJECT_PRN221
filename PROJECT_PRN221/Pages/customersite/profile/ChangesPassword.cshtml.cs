using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;

namespace PROJECT_PRN221.Pages.customersite.profile
{
    public class ChangesPasswordModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public ChangesPasswordModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }
        public string Mess { get; set; }

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
            return Page();
        }

        public IActionResult OnPost(string spass, string respass)
        {
            string customerJson = HttpContext.Session.GetString("customer");
            Customer customer = JsonConvert.DeserializeObject<Customer>(customerJson);
            if (spass.Equals(respass))
            {
                Customer Customer = _context.Customers.FirstOrDefault(x => x.CustomerId == customer.CustomerId);
                Customer.Password = Validation.HashPassword(spass);
                _context.Customers.Update(Customer);
                _context.SaveChanges();
                return RedirectToPage("/customersite/home/Index");
            }
            else
            {
                Mess = "Password and Re-Password is not the same!";
                return Page();
            }
        }

    }
}
