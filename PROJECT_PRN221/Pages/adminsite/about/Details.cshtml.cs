using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.about
{
    public class DetailsModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public DetailsModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

      public About About { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about = await _context.Abouts.FirstOrDefaultAsync(m => m.aId == id);
            if (about == null)
            {
                return NotFound();
            }
            else 
            {
                About = about;
            }
            return Page();
        }
    }
}
