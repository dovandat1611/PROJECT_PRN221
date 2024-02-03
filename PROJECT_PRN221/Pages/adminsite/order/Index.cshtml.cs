using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;
using static NuGet.Packaging.PackagingConstants;

namespace PROJECT_PRN221.Pages.adminsite.order
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

        public PaginatedList<Order> Order { get;set; } = default!;
        public int Wait { get; set; }
        public int Process { get; set; }
        public int Done { get; set; }
        public int Cancel { get; set; }

        public async Task OnGetAsync(string service, string status, int? pageIndex)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            Wait = _context.Orders.Where(x => x.Status.Equals("Wait")).Count();
            Process = _context.Orders.Where(x => x.Status.Equals("Process")).Count();
            Done = _context.Orders.Where(x => x.Status.Equals("Done")).Count();
            Cancel = _context.Orders.Where(x => x.Status.Equals("Cancel")).Count();

            if (string.IsNullOrWhiteSpace(service))
            {
                service = "DisplayOrder";
            }
            if (service.Equals("DisplayOrder"))
            {
                if (_context.Orders != null)
                {
                    IQueryable<Order> ordersIQ = _context.Orders.Include(o => o.Customer);

                    var pageSize = Configuration.GetValue("PageSize", 10);
                    Order = await PaginatedList<Order>.CreateAsync(
                    ordersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

                }
            }
             
            if(service == "displayOrderStatus")
            {
                if (_context != null)
                {
                    IQueryable<Order> ordersIQ = _context.Orders.Include(o => o.Customer).Where(x => x.Status.Equals(status));

                    var pageSize = Configuration.GetValue("PageSize", 10);
                    Order = await PaginatedList<Order>.CreateAsync(
                    ordersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
                }
            }
            
        }
    }
}
