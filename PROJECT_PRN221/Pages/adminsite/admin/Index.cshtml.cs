using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.admin
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public IndexModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public IList<Admin> Admin { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Admins != null)
            {
                Admin = await _context.Admins.ToListAsync();
            }
        }
        public async Task OnPostAsync(int id, string service, string status, string name)
        {
            if (_context.Customers != null)
            {
                if (service.Equals("updateStatus"))
                {
                    Admin admin = _context.Admins.FirstOrDefault(x => x.AdminId == id);
                    admin.Status = status;
                    _context.Update(admin);
                    _context.SaveChanges();
                    Admin = await _context.Admins.ToListAsync();
                }
                if (service.Equals("searchByName"))
                {
                    Admin = await _context.Admins.Where(x => x.Name.Contains(name)).ToListAsync();
                }
            }
        }
    }
}
