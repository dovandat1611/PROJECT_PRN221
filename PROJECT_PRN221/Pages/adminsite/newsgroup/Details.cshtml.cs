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
    public class DetailsModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public DetailsModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

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
    }
}
