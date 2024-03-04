using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;
using static PROJECT_PRN221.Utils.Mail;

namespace PROJECT_PRN221.Pages.customersite.authenticate.forgotpassword
{
    public class IndexModel : PageModel
    {
        private readonly ProjectPrn221Context _context;

        public IndexModel(ProjectPrn221Context context)
        {
            _context = context;
        }
        public string ErrorMess {get; set;}
        public void OnGet()
        {
        }
        public IActionResult OnPost(string username, string email)
        {
            bool checkInput = true;

            Customer customer = _context.Customers.FirstOrDefault(x => x.Username.Equals(username) && x.Email.Equals(email));

            if(customer != null)
            {
                string otp = Validation.GenerateOTP(6);
                HttpContext.Session.SetString("otp", otp);
                HttpContext.Session.SetString("username", username);

                var mailSettings = new MailSettings
                {
                    Mail = "hightech05vn@gmail.com",
                    DisplayName = "HighTech Store",
                    Password = "hhhbasicfoexmpyw",
                    Host = "smtp.gmail.com",
                    Port = 587
                };

                var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<SendMailService>();
                Mail.SendMailService sendMailService = new Mail.SendMailService(Options.Create(mailSettings), logger);
                sendMailService.SendEmailAsync(email, "OTP", otp);

                return RedirectToPage("/customersite/authenticate/otp/Index");
            }
            else
            {
                ErrorMess = "Username or Email invalid!";
                return Page();
            }
        }
    }
}
