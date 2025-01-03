using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.profile
{
    public class IndexModel : PageModel
    {

        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public Admin Admin { get; set; }
        public bool checkSession()
        {
            bool checkS = true;
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                string isCustomerAuthenticated = httpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(isCustomerAuthenticated))
                {
                    checkS = false;
                }
            }
            return checkS;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (checkSession() == false)
            {
                return RedirectToPage("/BadRequest");
            }
            int id = 0;
            string adminJson = HttpContext.Session.GetString("admin");
            if (!string.IsNullOrEmpty(adminJson))
            {
                Admin admin = JsonConvert.DeserializeObject<Admin>(adminJson);
                id = admin.AdminId;
            }
            Admin = _context.Admins.FirstOrDefault(x => x.AdminId == id);

            return Page();
        }
    }
}
