using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

                ViewData["NewsGroups"] = new SelectList(_context.NewsGroups, "NewsgroupId", "NewsgroupName");
                ViewData["NewsNewsgroupId"] = 0;
            }
        }

        public async Task<IActionResult> OnPostAsync(string service, string title, int newsgroup_id)
        {
            Console.WriteLine("NEWS: " + service);
            if (_context.News != null)
            {
                IQueryable<News> newsQuery = _context.News
                    .Include(n => n.CreatedbyNavigation)
                    .Include(n => n.Newsgroup);

                ViewData["NewsGroups"] = new SelectList(_context.NewsGroups, "NewsgroupId", "NewsgroupName");

                if (service.Equals("searchNewsGroup"))
                {
                    newsQuery = newsQuery.Where(x => x.NewsgroupId == newsgroup_id);
                }

                if (service.Equals("searchNews"))
                {
                    if (newsgroup_id == 0)
                    {
                        newsQuery = newsQuery.Where(x => x.Title.Contains(title));
                    }
                    else
                    {
                        newsQuery = newsQuery.Where(x => x.Title.Contains(title) && x.NewsgroupId == newsgroup_id);
                    }
                }

                ViewData["NewsNewsgroupId"] = newsgroup_id != 0 ? newsgroup_id : 0;

                // Execute the query and return the result to the view
                News = await newsQuery.ToListAsync();
            }
            return Page();
        }

    }
}
