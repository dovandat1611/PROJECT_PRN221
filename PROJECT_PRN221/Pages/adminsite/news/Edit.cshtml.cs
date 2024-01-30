﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.IO;
using PROJECT_PRN221.Models;

namespace PROJECT_PRN221.Pages.adminsite.news
{
    public class EditModel : PageModel
    {
        private readonly PROJECT_PRN221.Models.ProjectPrn221Context _context;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public EditModel(PROJECT_PRN221.Models.ProjectPrn221Context context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        [BindProperty]
        public News News { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news =  await _context.News.FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }
           News = news;
           ViewData["NewsgroupId"] = new SelectList(_context.NewsGroups, "NewsgroupId", "NewsgroupName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {       

            if (!ModelState.IsValid)
            {
                ViewData["NewsgroupId"] = new SelectList(_context.NewsGroups, "NewsgroupId", "NewsgroupName");
                return Page();
            }

            // Upload file 
            string fileURL = string.Empty;
            if (FileUploads != null)
            {
                foreach (var FileUpload in FileUploads)
                {
                    var file = Path.Combine(_environment.ContentRootPath, "Images/news",
                    FileUpload.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(fileStream);
                        fileURL = "/Images/news/"+ FileUpload.FileName;
                    }
                }
            }

            // Add and save changes
            if (fileURL != string.Empty)
            {
                News.Image = fileURL;
            }
           
            _context.Attach(News).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(News.NewsId))
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

        private bool NewsExists(int id)
        {
          return (_context.News?.Any(e => e.NewsId == id)).GetValueOrDefault();
        }
    }
}
