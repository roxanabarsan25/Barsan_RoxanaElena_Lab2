using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Barsan_RoxanaElena_Lab2.Data;
using Barsan_RoxanaElena_Lab2.Models;

namespace Barsan_RoxanaElena_Lab2.Pages.Members
{
    public class IndexModel : PageModel
    {
        private readonly Barsan_RoxanaElena_Lab2.Data.Barsan_RoxanaElena_Lab2Context _context;

        public IndexModel(Barsan_RoxanaElena_Lab2.Data.Barsan_RoxanaElena_Lab2Context context)
        {
            _context = context;
        }

        public IList<Member> Member { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Member = await _context.Member.ToListAsync();
        }
    }
}
