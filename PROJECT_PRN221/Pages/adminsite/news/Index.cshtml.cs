using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;

namespace PROJECT_PRN221.Pages.adminsite.news
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



        public PaginatedList<News> News { get;set; } = default!;

        public async Task OnGetAsync(int? pageIndex)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (_context.News != null)
            {

                IQueryable<News> newsIQ = _context.News.Include(n => n.CreatedbyNavigation).Include(n => n.Newsgroup);

                var pageSize = Configuration.GetValue("PageSize", 10);
                News = await PaginatedList<News>.CreateAsync(
                newsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

                ViewData["NewsGroups"] = new SelectList(_context.NewsGroups, "NewsgroupId", "NewsgroupName");
                ViewData["NewsNewsgroupId"] = 0;
            }
        }

        public async Task<IActionResult> OnPostAsync(string service, string title, int newsgroup_id, int? pageIndex)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
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

                var pageSize = Configuration.GetValue("PageSize", 10);
                News = await PaginatedList<News>.CreateAsync(
                newsQuery.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            return Page();
        }

    }
}
