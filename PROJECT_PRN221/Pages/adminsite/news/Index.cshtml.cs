using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.news
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public IList<News> News { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.News != null)
            {
                News = await _context.News
                .Include(n => n.CreatedbyNavigation)
                .Include(n => n.Newsgroup).ToListAsync();
            }
        }
    }
}
