using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.about
{
    public class EditModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly IHubContext<HubServer> _hubContext;

        public EditModel(PROJECT_PRN221.Models.ProjectPrn221Context context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, IHubContext<HubServer> hubContext)
        {
            _context = context;
            _environment = environment;
            _hubContext = hubContext;
        }

        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        [BindProperty]
        public About About { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Abouts == null)
            {
                return NotFound();
            }

            var about =  await _context.Abouts.FirstOrDefaultAsync(m => m.aId == id);
            if (about == null)
            {
                return NotFound();
            }
            About = about;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Upload file 
            string fileURL = string.Empty;
            if (FileUploads != null)
            {
                foreach (var FileUpload in FileUploads)
                {
                    var file = Path.Combine(_environment.ContentRootPath, "wwwroot/Images/news",
                    FileUpload.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(fileStream);
                        fileURL = "/Images/news/" + FileUpload.FileName;
                    }
                }
            }


            // Add and save changes
            if (fileURL != string.Empty)
            {
                About.Image = fileURL;
            }

            _context.Attach(About).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReloadData");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutExists(About.aId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Index");
        }

        private bool AboutExists(int id)
        {
          return (_context.Abouts?.Any(e => e.aId == id)).GetValueOrDefault();
        }
    }
}
