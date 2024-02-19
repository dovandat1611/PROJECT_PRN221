using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace PROJECT_PRN221.Pages.customersite.authenticate.logout
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var session = HttpContext.Session;

            if (session.Keys.Contains("customer"))
            {
                session.Remove("customer");
            }

            if (session.Keys.Contains("cart"))
            {
                session.Remove("cart");
            }

            return RedirectToPage("/customersite/authenticate/login/Index");
        }
    }
}
