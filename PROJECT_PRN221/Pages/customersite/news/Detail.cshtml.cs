using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;
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
        public NewsView Newsview { get; set; } = default!;

        public News News { get; set; } = default!;

        public IList<News> RelateNews { get; set; } = default!;
        public string isCustomerAuthenticated { get; set; } = null;
        public void checkSession()
        {
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                isCustomerAuthenticated = httpContext.Session.GetString("customer");
            }
        }

        public async Task OnGetAsync(int id)
        {
            checkSession();

            Newsview = (from s in _context.News
                    join ng in _context.NewsGroups on s.NewsgroupId equals ng.NewsgroupId
                    where s.NewsId == id
                    orderby s.CreatedDate descending
                    select new NewsView
                    {
                        news_id = s.NewsId,
                        newsgroup_name = ng.NewsgroupName,
                        image = s.Image,
                        title = s.Title,
                        content = s.Content,
                        day = s.CreatedDate.Value.Day.ToString(),
                        month = s.CreatedDate.Value.Month.ToString(),
                        year = s.CreatedDate.Value.Year.ToString()
                    })
                    .FirstOrDefault();

            Newsview.month = Validation.ConvertMonthNumberToName(Newsview.month);

            News = _context.News.Include(x => x.Newsgroup).FirstOrDefault(x => x.NewsId == id);

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