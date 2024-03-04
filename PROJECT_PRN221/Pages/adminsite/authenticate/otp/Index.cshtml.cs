using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PROJECT_PRN221.Pages.adminsite.authenticate.logout
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public IActionResult OnPost(string otp)
        {
            string sessionOTP = HttpContext.Session.GetString("otp");

            if (sessionOTP.Equals(otp))
            {
                return RedirectToPage("/adminsite/authenticate/changespassword/Index");
            }
            else
            {
                HttpContext.Session.Remove("username");
                HttpContext.Session.Remove("otp");
                return RedirectToPage("/adminsite/authenticate/forgotpassword/Index");
            }
        }

    }
}
