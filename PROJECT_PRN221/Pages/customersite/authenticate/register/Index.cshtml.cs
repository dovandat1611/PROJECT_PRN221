using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;
using static PROJECT_PRN221.Utils.Mail;

namespace PROJECT_PRN221.Pages.customersite.authenticate.register
{
    public class IndexModel : PageModel
    {
        private readonly ProjectPrn221Context _context;

        public IndexModel(ProjectPrn221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public void OnGet()
        {

        }
        public IActionResult OnPost(string gender)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(gender == null)
            {
                Customer.Gender = true;
            }

            if (gender.Equals("1"))
            {
                Customer.Gender = true;
            }
            else
            {
                Customer.Gender = false;
            }

            bool checkInput = true;

            if (!Validation.IsUsernameUnique(Customer.Username, _context))
            {
                ModelState.AddModelError("Customer.Username", "Username already exists. Please choose a different one.");
                checkInput = false;
            }

            if (!Validation.IsEmailValid(Customer.Email))
            {
                ModelState.AddModelError("Customer.Email", "Invalid email format.");
                checkInput = false;
            }

            if (!Validation.IsPasswordValid(Customer.Password))
            {
                ModelState.AddModelError("Customer.Password", "Password must have at least 6 characters, including lowercase, uppercase, and a number.");
                checkInput = false;
            }

            if (checkInput == true)
            {
                Customer.Password = Validation.HashPassword(Customer.Password);
                _context.Customers.Add(Customer);

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

                sendMailService.SendEmailAsync(Customer.Email,"Register", "Hello,\n\nYou have successfully registered.\n\nThank you for joining!");

                _context.SaveChanges();
                return RedirectToPage("/customersite/authenticate/login/Index");
            }
            return Page();
        }
    }
}
