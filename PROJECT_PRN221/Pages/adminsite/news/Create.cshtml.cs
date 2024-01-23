using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.news
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
        ViewData["Createdby"] = new SelectList(_context.Admins, "AdminId", "AdminId");
        ViewData["NewsgroupId"] = new SelectList(_context.NewsGroups, "NewsgroupId", "NewsgroupId");
            return Page();
        }

        [BindProperty]
        public News News { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.News == null || News == null)
            {
                return Page();
            }

            _context.News.Add(News);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
