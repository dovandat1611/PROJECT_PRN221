using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.newsgroup
{
    public class DeleteModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public DeleteModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
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

            var newsgroup = await _context.NewsGroups.FirstOrDefaultAsync(m => m.NewsgroupId == id);

            if (newsgroup == null)
            {
                return NotFound();
            }
            else 
            {
                NewsGroup = newsgroup;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.NewsGroups == null)
            {
                return NotFound();
            }
            var newsgroup = await _context.NewsGroups.FindAsync(id);

            if (newsgroup != null)
            {
                NewsGroup = newsgroup;
                _context.NewsGroups.Remove(NewsGroup);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
