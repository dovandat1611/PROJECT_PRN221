using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PROJECT_PRN221.Pages.adminsite.authenticate.logout
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var session = HttpContext.Session;

            if (session.Keys.Contains("admin"))
            {
                session.Remove("admin");
            }
            return RedirectToPage("/admin/authenticate/login/Index");
        }
    }
}
