using Barsan_RoxanaElena_Lab2.Data;
using Barsan_RoxanaElena_Lab2.Models;
using Barsan_RoxanaElena_Lab2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barsan_RoxanaElena_Lab2.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Barsan_RoxanaElena_Lab2.Data.Barsan_RoxanaElena_Lab2Context _context;

        public IndexModel(Barsan_RoxanaElena_Lab2.Data.Barsan_RoxanaElena_Lab2Context context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; } = default!;

        public CategoryIndexData CategoryData { get; set; } = default!;
        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id)
        {

            CategoryData = new CategoryIndexData();
            CategoryData.Categories = await _context.Category
                .Include(c => c.BookCategories)
                    .ThenInclude(bc => bc.Book)
                        .ThenInclude(b => b.Author)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            if (id != null)
            {
                CategoryID = id.Value;
                Category category = CategoryData.Categories
                    .FirstOrDefault(c => c.ID == id.Value);
                CategoryData.Books = category.BookCategories.Select(bc => bc.Book);
            }
            Category = await _context.Category.ToListAsync();
        }
    }
}
