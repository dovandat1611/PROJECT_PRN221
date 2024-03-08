using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;

namespace PROJECT_PRN221.Pages.customersite.profile
{
    public class EditModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public EditModel(PROJECT_PRN221.Models.ProjectPrn221Context context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        [BindProperty]
        public Customer Customer { get; set; } = default!;


        public bool checkSession()
        {
            bool checkS = true;
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                string isCustomerAuthenticated = httpContext.Session.GetString("customer");
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
                return RedirectToPage("/customersite/authenticate/login/Index");
            }

            string customerJson = HttpContext.Session.GetString("customer");
            Customer customerjson = JsonConvert.DeserializeObject<Customer>(customerJson);
            int id = customerjson.CustomerId;

            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer =  await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string gender)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool checkInput = true;

            if (!Validation.IsEmailValid(Customer.Email))
            {
                ModelState.AddModelError("Customer.Email", "Invalid email format.");
                checkInput = false;
            }

            if (checkInput == true) {

                // Upload file 
                string fileURL = string.Empty;
                if (FileUploads != null)
                {
                    foreach (var FileUpload in FileUploads)
                    {
                        var file = Path.Combine(_environment.ContentRootPath, "wwwroot/Images/avatar",
                        FileUpload.FileName);
                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await FileUpload.CopyToAsync(fileStream);
                            fileURL = "/Images/avatar/" + FileUpload.FileName;
                        }
                    }
                }

                // Add and save changes
                if (fileURL != string.Empty)
                {
                    Customer.Image = fileURL;
                }


                // SET GENDER
                bool gd = false;
                if (gender.Equals("male"))
                {
                    gd = true;
                }

                Customer.Gender = gd;

                _context.Attach(Customer).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    HttpContext.Session.Remove("customer");
                    string cusjson = JsonConvert.SerializeObject(Customer);
                    HttpContext.Session.SetString("customer", cusjson);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(Customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
