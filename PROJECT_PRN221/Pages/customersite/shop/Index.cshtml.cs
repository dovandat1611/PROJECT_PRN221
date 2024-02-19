using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PROJECT_PRN221.Pages.customersite.shop
{
    public class IndexModel : PageModel
    {
        public string isCustomerAuthenticated { get; set; } = null;
        public void checkSession()
        {
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                isCustomerAuthenticated = httpContext.Session.GetString("customer");
            }
        }
        public void OnGet()
        {
            checkSession();
        }
    }
}
