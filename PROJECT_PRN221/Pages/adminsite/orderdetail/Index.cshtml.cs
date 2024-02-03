using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;

namespace PROJECT_PRN221.Pages.adminsite.orderdetail
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

        public PaginatedList<OrderDetail> OrderDetail { get;set; } = default!;

        public async Task OnGetAsync(int id, int? pageIndex)
        {
            IQueryable<OrderDetail> orderDetailIQ = null;
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (id == null)
            {
                id = 0;
            }
            if(id != 0)
            {
                orderDetailIQ = _context.OrderDetails
                    .Include(o => o.Order).Include(x => x.Product).Where(x => x.OrderId == id);
            }
            else
            {
                orderDetailIQ = _context.OrderDetails
                    .Include(o => o.Order).Include(x => x.Product);
            }

            var pageSize = Configuration.GetValue("PageSize", 10);
            OrderDetail = await PaginatedList<OrderDetail>.CreateAsync(
            orderDetailIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

        }
    }
}
