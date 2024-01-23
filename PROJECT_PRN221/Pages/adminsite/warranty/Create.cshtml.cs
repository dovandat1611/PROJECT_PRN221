using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.warranty
{
    public class CreateModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;

        public CreateModel(PROJECT_PRN221.Models.ProjectPrn221Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId");
        ViewData["OrderdetailId"] = new SelectList(_context.OrderDetails, "OrderdetailId", "OrderdetailId");
        ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductId");
            return Page();
        }

        [BindProperty]
        public Warranty Warranty { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Warranties == null || Warranty == null)
            {
                return Page();
            }

            _context.Warranties.Add(Warranty);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
