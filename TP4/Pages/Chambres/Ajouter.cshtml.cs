using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Chambres
{
    public class AjouterModel : PageModel
    {
        [BindProperty]
        public required Chambre Chambre { get; set; }

        public ActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            GestionReservable.AjouterReservable(Chambre);

            return RedirectToPage("Index");
        }
    }
}
