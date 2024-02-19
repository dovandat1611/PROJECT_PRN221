using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace PROJECT_PRN221.Pages
{
    public class _HeaderCustomerSiteModel : PageModel
    {
        //public string isCustomerAuthenticated { get; set; } = null;
        public void OnGet()
        {
            //var httpContext = HttpContext;
            //if (httpContext != null && httpContext.Session != null)
            //{
            //    isCustomerAuthenticated = httpContext.Session.GetString("customer");
            //}
        }
    }
}
