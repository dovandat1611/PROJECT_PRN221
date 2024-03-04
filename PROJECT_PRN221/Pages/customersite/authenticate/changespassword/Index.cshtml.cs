using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;

namespace PROJECT_PRN221.Pages.customersite.authenticate.changespassword
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }
        public string Mess { get; set; }
        public void OnGet()
        {

        }
        
        public IActionResult OnPost(string spass, string respass)
        {
            string username = HttpContext.Session.GetString("username");
            if (spass.Equals(respass))
            {
                Customer customer = _context.Customers.FirstOrDefault(x => x.Username.Equals(username));
                customer.Password = Validation.HashPassword(spass);
                _context.Customers.Update(customer);
                _context.SaveChanges();

                HttpContext.Session.Remove("username");
                HttpContext.Session.Remove("otp");
                return RedirectToPage("/customersite/authenticate/login/Index");
            }
            else
            {
                Mess = "Password and Re-Password is not the same!";
                return Page();
            }
        }

    }
}
