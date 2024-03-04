using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Dto;
using PROJECT_PRN221.Models;
using PROJECT_PRN221.Utils;

namespace PROJECT_PRN221.Pages.customersite.home
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }
        public string isCustomerAuthenticated { get; set; } = null;

        public List<Product> BestSellingProduct { get; set; } = default!;
        public List<Product> NewProduct { get; set; } = default!;
        public List<Product> SaleProduct { get; set; } = default!;
        public IList<NewsView> News { get; set; } = default!;

        public void checkSession()
        {
            var httpContext = HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                isCustomerAuthenticated = httpContext.Session.GetString("customer");
            }
        }

        public async Task OnGetAsync()
        {
            checkSession();
            if (_context.Products != null)
            {
                BestSellingProduct = await (
                    from p in _context.Products
                    where p.Status == "Stocking"
                    join od in _context.OrderDetails on p.ProductId equals od.ProductId into odGroup
                    orderby odGroup.Sum(x => x.QuantityOrder) descending
                    select p
                ).Include(p => p.Brand)
                 .Include(p => p.Category)
                .Take(10)
                .ToListAsync();


                NewProduct = await _context.Products
                                .Include(p => p.Brand)
                                .Include(p => p.Category)
                                .OrderByDescending(x => x.ProductId)
                                .Take(12)
                                .ToListAsync();

                SaleProduct = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Where(p => p.Discount != null && p.Discount != 0 && p.Status == "Stocking")
                    .ToListAsync();


                News = (from s in _context.News
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
                        })
                        .Take(3)
                        .ToList();

                foreach (NewsView s in News)
                {
                    s.month = Validation.ConvertMonthNumberToName(s.month);
                }
            }
        }
    }
}