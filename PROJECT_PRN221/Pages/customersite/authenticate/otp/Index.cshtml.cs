using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.customersite.authenticate.otp
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string otp)
        {
            string sessionOTP = HttpContext.Session.GetString("otp");

            if(string.IsNullOrEmpty(sessionOTP))
            {
                if (sessionOTP.Equals(otp))
                {
                    return RedirectToPage("/customersite/authenticate/changespassword/Index");
                }
                else
                {
                    HttpContext.Session.Remove("username");
                    HttpContext.Session.Remove("otp");
                    return RedirectToPage("/customersite/authenticate/forgotpassword/Index");
                }
            }
            return RedirectToPage("/customersite/authenticate/changespassword/Index");
        }
    }
}
