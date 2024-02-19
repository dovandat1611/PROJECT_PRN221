using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using System.Net;

namespace PROJECT_PRN221.Pages.customersite.news
{
    public class DetailModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public DetailModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }
        public News News { get; set; } = default!;
        public IList<News> RelateNews { get; set; } = default!;
        public async Task OnGetAsync(int id)
        {
            News = await _context.News
                .Include(n => n.Newsgroup)
                .FirstOrDefaultAsync(n => n.NewsId == id);

            if (News != null)
            {
                RelateNews = await _context.News
                    .Include(n => n.Newsgroup)
                    .Where(n => n.NewsgroupId == News.NewsgroupId && n.NewsId != id)
                    .ToListAsync();
            }
        }
    }
}