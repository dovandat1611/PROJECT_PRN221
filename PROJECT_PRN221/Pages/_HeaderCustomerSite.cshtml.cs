using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PROJECT_PRN221.Pages
{
    public class _HeaderCustomerSiteModel : PageModel
    {   
        //public string isCustomerAuthenticated { get; set; }
        public void OnGet()
        {
            //isCustomerAuthenticated = HttpContext.Session.GetString("customer");
            //Console.WriteLine(isCustomerAuthenticated);
        }
    }
}
