using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.customersite.authenticate.login
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }

        public string ErrorMess { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            if (_context != null)
            {
                if (username == string.Empty || password == string.Empty)
                {
                    ErrorMess = "Username and password is not empty";
                    return Page();
                }
                else
                {
                    Customer customer = _context.Customers.FirstOrDefault(x => x.Username.Equals(username) && x.Password.Equals(password));
                    if (customer != null)
                    {
                        string customerJson = JsonConvert.SerializeObject(customer);
                        HttpContext.Session.SetString("customer", customerJson);
                        return RedirectToPage("/customersite/home/Index");
                    }
                    else
                    {
                        ErrorMess = "Username or password is not correct";
                        return Page();
                    }

                }
            }
            return Page();
        }
    }
}
