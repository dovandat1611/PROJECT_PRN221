using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;
using RazorPagesLab.utils;
using System.Configuration;

namespace PROJECT_PRN221.Pages.customersite.news
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        private readonly IConfiguration Configuration;
        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }
        public PaginatedList<NewsView> News { get; set; } = default!;

        public string isCustomerAuthenticated { get; set; } = null;
        public void checkSession()
        {
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                isCustomerAuthenticated = httpContext.Session.GetString("customer");
            }
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            checkSession();
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (_context.News != null)
            {   
                IQueryable<NewsView> newsIQ =
                    from s in _context.News
                    join ng in _context.NewsGroups on s.NewsgroupId equals ng.NewsgroupId
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
                    };

                News = await PaginatedList<NewsView>.CreateAsync(
                newsIQ.AsNoTracking(), pageIndex ?? 1, 6);

                foreach (NewsView s in News)
                {
                    s.month = Validation.ConvertMonthNumberToName(s.month);
                }

            }
        }
    }
}