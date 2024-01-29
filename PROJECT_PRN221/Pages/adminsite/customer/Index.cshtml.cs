using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.customer
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public IList<Customer> Customer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Customers != null)
            {
                Customer = await _context.Customers.ToListAsync();
            }
        }


        public async Task OnPostAsync(int id, string action, string status, string name)
        {
            if (_context.Customers != null)
            {

                if (action.Equals("updateStatus"))
                {
                    Customer customer = _context.Customers.FirstOrDefault(x => x.CustomerId == id);
                    customer.Status = status;
                    _context.SaveChanges();
                }
                if (action.Equals("searchByName"))
                {
                    Customer = await _context.Customers.Where(x => x.Name.Contains(name)).ToListAsync();
                }
            }
        }
    }
}
