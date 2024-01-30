using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using RazorPagesLab.utils;

namespace PROJECT_PRN221.Pages.adminsite.customer
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

        public PaginatedList<Customer> Customer { get;set; } = default!;

        public async Task OnGetAsync(int? pageIndex)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (_context.Customers != null)
            {
                IQueryable<Customer> customersIQ = from s in _context.Customers select s;

                var pageSize = Configuration.GetValue("PageSize", 10);
                Customer = await PaginatedList<Customer>.CreateAsync(
                customersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
        }


        public async Task OnPostAsync(int id, string action, string status, string name, int? pageIndex)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }
            if (_context.Customers != null)
            {

                if (action.Equals("updateStatus"))
                {
                    Customer customer = _context.Customers.FirstOrDefault(x => x.CustomerId == id);
                    customer.Status = status;
                    _context.SaveChanges();

                    IQueryable<Customer> customersIQ = from s in _context.Customers select s;

                    var pageSize = Configuration.GetValue("PageSize", 10);
                    Customer = await PaginatedList<Customer>.CreateAsync(
                    customersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
                }
                if (action.Equals("searchByName"))
                {
                    IQueryable<Customer> customersIQ = from s in _context.Customers where s.Name.Contains(name) select s;
                    var pageSize = Configuration.GetValue("PageSize", 1);
                    Customer = await PaginatedList<Customer>.CreateAsync(
                    customersIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
                }
            }
        }
    }
}
