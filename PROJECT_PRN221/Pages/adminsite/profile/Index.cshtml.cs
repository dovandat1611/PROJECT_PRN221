using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.profile
{
    public class IndexModel : PageModel
    {   
        public Admin Admin { get; set; }
        public async Task OnGetAsync()
        {
            string adminJson = HttpContext.Session.GetString("admin");
            if (!string.IsNullOrEmpty(adminJson))
            {
                Admin = JsonConvert.DeserializeObject<Admin>(adminJson);
            }
        }
    }
}
