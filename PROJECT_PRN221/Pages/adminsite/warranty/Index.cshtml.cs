using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.warranty
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public IList<Warranty> Warranty { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Warranties != null)
            {
                Warranty = await _context.Warranties
                .Include(w => w.Customer)
                .Include(w => w.Orderdetail)
                .Include(w => w.Product).ToListAsync();
            }
        }
    }
}
