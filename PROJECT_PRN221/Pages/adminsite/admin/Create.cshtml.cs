using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;

namespace PROJECT_PRN221.Pages.adminsite.admin
{
    public class CreateModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public CreateModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Admin Admin { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {   
          if (!ModelState.IsValid || _context.Admins == null || Admin == null)
            {
                return Page();
            }

            bool checkInput = true;

            if (!Validation.IsUsernameAdmin(Admin.Username, _context))
            {
                ModelState.AddModelError("Admin.Username", "Username already exists. Please choose a different one.");
                checkInput = false;
            }

            if (!Validation.IsEmailValid(Admin.Gmail))
            {
                ModelState.AddModelError("Admin.Gmail", "Invalid email format.");
                checkInput = false;
            }

            if (!Validation.IsPasswordValid(Admin.Password))
            {
                ModelState.AddModelError("Admin.Password", "Password must have at least 6 characters, including lowercase, uppercase, and a number.");
                checkInput = false;
            }
            
            if(checkInput == true)
            {
                Admin.Password = Validation.HashPassword(Admin.Password);
                _context.Admins.Add(Admin);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
