using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
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
        public PaginatedList<News> News { get; set; } = default!;

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
            if (_context.Admins != null)
            {
                IQueryable<News> newsIQ = _context.News.Include(n => n.Newsgroup).OrderByDescending(n => n.CreatedDate);

                var pageSize = Configuration.GetValue("PageSize", 1);
                News = await PaginatedList<News>.CreateAsync(
                newsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
        }

    }
}