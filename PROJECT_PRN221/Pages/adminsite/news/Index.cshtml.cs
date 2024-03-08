using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public bool checkSession()
        {
            bool checkS = true;
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                string isCustomerAuthenticated = httpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(isCustomerAuthenticated))
                {
                    checkS = false;
                }
            }
            return checkS;
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {

            if (checkSession() == false)
            {
                return RedirectToPage("/BadRequest");
            }

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile inputFile, string service, string title, int newsgroup_id, int? pageIndex)
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

                if (service.Equals("inputFile"))
                {
                    string fileContent = string.Empty;
                    if (inputFile != null)
                    {
                        using (var reader = new StreamReader(inputFile.OpenReadStream()))
                        {
                            fileContent = reader.ReadToEnd();
                        }
                        if (!string.IsNullOrEmpty(fileContent))
                        {
                            var listOfNews = JsonConvert.DeserializeObject<List<News>>(fileContent);
                            if (listOfNews.Count > 0)
                            {
                                foreach (var item in listOfNews)
                                {
                                    item.NewsId = 0;
                                }
                                _context.News.AddRange(listOfNews);
                                _context.SaveChanges();
                            }
                        }
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
