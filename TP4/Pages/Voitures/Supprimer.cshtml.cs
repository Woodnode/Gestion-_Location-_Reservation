using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Voitures
{
    public class SupprimerModel : PageModel
    {
        [BindProperty]
        public required Voiture Voiture { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (GestionReservable.ObtenirReservableParId("Voiture", id.Value) is not Voiture voiture)
            {
                return NotFound();
            }

            Voiture = voiture;
            return Page();
        }

        public ActionResult OnPost()
        {
            GestionReservable.SupprimerReservable(Voiture);

            return RedirectToPage("Index");
        }
    }
}

