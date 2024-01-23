using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.newsgroup
{
    public class EditModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public EditModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public NewsGroup NewsGroup { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.NewsGroups == null)
            {
                return NotFound();
            }

            var newsgroup =  await _context.NewsGroups.FirstOrDefaultAsync(m => m.NewsgroupId == id);
            if (newsgroup == null)
            {
                return NotFound();
            }
            NewsGroup = newsgroup;
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

            _context.Attach(NewsGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsGroupExists(NewsGroup.NewsgroupId))
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

        private bool NewsGroupExists(int id)
        {
          return (_context.NewsGroups?.Any(e => e.NewsgroupId == id)).GetValueOrDefault();
        }
    }
}
