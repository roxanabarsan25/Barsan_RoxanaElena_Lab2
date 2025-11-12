using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Barsan_RoxanaElena_Lab2.Data;
using Barsan_RoxanaElena_Lab2.Models;

namespace Barsan_RoxanaElena_Lab2.Pages.Borrowings
{
    public class EditModel : PageModel
    {
        private readonly Barsan_RoxanaElena_Lab2.Data.Barsan_RoxanaElena_Lab2Context _context;

        public EditModel(Barsan_RoxanaElena_Lab2.Data.Barsan_RoxanaElena_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // includem datele despre Book → Author și Member
            Borrowing = await _context.Borrowing
                .Include(b => b.Book)
                    .ThenInclude(b => b.Author)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Borrowing == null)
            {
                return NotFound();
            }

            // lista de cărți cu "Titlu - Autor"
            var bookList = _context.Book
                .Include(b => b.Author)
                .Select(b => new
                {
                    b.ID,
                    BookFullName = b.Title + " - " +
                        (b.Author != null ? b.Author.LastName + " " + b.Author.FirstName : "")
                })
                .ToList();

            // lista de membri cu nume complet
            var memberList = _context.Member
                .Select(m => new
                {
                    m.ID,
                    FullName = m.FullName
                })
                .ToList();

            ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName", Borrowing.BookID);
            ViewData["MemberID"] = new SelectList(memberList, "ID", "FullName", Borrowing.MemberID);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // reconstruim listele pentru dropdown-uri
                var bookList = _context.Book
                    .Include(b => b.Author)
                    .Select(b => new
                    {
                        b.ID,
                        BookFullName = b.Title + " - " +
                            (b.Author != null ? b.Author.LastName + " " + b.Author.FirstName : "")
                    })
                    .ToList();

                var memberList = _context.Member
                    .Select(m => new
                    {
                        m.ID,
                        FullName = m.FullName
                    })
                    .ToList();

                ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName", Borrowing.BookID);
                ViewData["MemberID"] = new SelectList(memberList, "ID", "FullName", Borrowing.MemberID);

                return Page();
            }

            _context.Attach(Borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(Borrowing.ID))
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

        private bool BorrowingExists(int id)
        {
            return _context.Borrowing.Any(e => e.ID == id);
        }
    }
}
