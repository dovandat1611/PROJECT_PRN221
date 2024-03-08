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

namespace PROJECT_PRN221.Pages.adminsite.profile
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
        public Admin Admin { get; set; } = default!;


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

            // get adminid form session
            string adminJson = HttpContext.Session.GetString("admin");
            Admin adminjson = JsonConvert.DeserializeObject<Admin>(adminJson);
            int id = adminjson.AdminId;

            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin =  await _context.Admins.FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }
            Admin = admin;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            bool checkInput = true;

            if (!Validation.IsEmailValid(Admin.Gmail))
            {
                ModelState.AddModelError("Admin.Gmail", "Invalid email format.");
                checkInput = false;
            }

            if (checkInput == true)
            {

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
                    Admin.Image = fileURL;
                }


                _context.Attach(Admin).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(Admin.AdminId))
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

        private bool AdminExists(int id)
        {
          return (_context.Admins?.Any(e => e.AdminId == id)).GetValueOrDefault();
        }
    }
}
