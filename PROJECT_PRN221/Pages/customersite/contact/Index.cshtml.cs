using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;
using RazorPagesLab.utils;
using System.Configuration;
using static PROJECT_PRN221.Utils.Mail;

namespace PROJECT_PRN221.Pages.customersite.contact
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        private readonly IConfiguration Configuration;
        private readonly IHubContext<HubServer> _hubContext;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context, IConfiguration configuration, IHubContext<HubServer> hubContext)
        {
            _context = context;
            Configuration = configuration;
            _hubContext = hubContext;
        }

        public string isCustomerAuthenticated { get; set; } = null;
        public Customer Customers { get; set; }
        public string ms { get; set; }
        public bool checkSession()
        {
            bool check = true;
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                isCustomerAuthenticated = httpContext.Session.GetString("customer");
                if(isCustomerAuthenticated == null || isCustomerAuthenticated.IsNullOrEmpty())
                {
                    check = false;
                }
            }
            return check;
        }
        public void OnGet()
        {
            if(checkSession() == true)
            {
                Customers = JsonConvert.DeserializeObject<Customer>(isCustomerAuthenticated);
            }
        }

        public async Task OnPostAsync(string service, string name, string email, string message)
        {
            if (checkSession() == true)
            {
                Customers = JsonConvert.DeserializeObject<Customer>(isCustomerAuthenticated);
            }

            if (service.Equals("addContact"))
            {

                bool checkInput = true;

                if (!Validation.IsEmailValid(email))
                {
                    ms = "Invalid email format.";
                    checkInput = false;
                }

                if (checkInput == true)
                {
                    Contact contact = new Contact
                    {
                        ContactDate = DateTime.Now,
                        Name = name,
                        Gmail = email,
                        ContactContent = message
                    };

                    _context.Contacts.Add(contact);
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("ReloadData");


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

                    sendMailService.SendEmailAsync(email, "Contact", "Thank you for contacting us. We will get back to you as soon as possible!");

                }
            }

        }

    }
}
