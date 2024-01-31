using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.authenticate.login
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public string ErrorMess { get; set; } = default!;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string username, string password) {
            if(_context != null)
            {
                if(username == string.Empty || password == string.Empty)
                {
                    ErrorMess = "Username and password is not empty";
                    return Page();
                }
                else
                {   
                    Admin admin = _context.Admins.FirstOrDefault(x => x.Username.Equals(username) && x.Password.Equals(password));
                    if(admin != null)
                    {
                        string adminJson = JsonConvert.SerializeObject(admin);
                        HttpContext.Session.SetString("admin", adminJson);
                        return RedirectToPage("/adminsite/profile/Index");
                    }
                    else
                    {
                        ErrorMess = "Username or password is not correct";
                        return Page();
                    }
                   
                }
            }
            return Page();
        }
    }
}
