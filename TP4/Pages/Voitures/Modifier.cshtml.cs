using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Voitures
{
    public class ModifierModel : PageModel
    {
        [BindProperty]
        public required Voiture Voiture { get; set; }

        public required List<string> ListeDeMarques { get; set; }

        public ActionResult OnGet(int? id)
        {
            ListeDeMarques = GestionReservable.ListeDeMarques;

            if (id == null)
            {
                return NotFound();
            }
            var voiture = GestionReservable.ObtenirReservableParId("voiture", id.Value) as Voiture;

            if (voiture == null)
            {
                return NotFound();
            }

            Voiture = voiture;
            return Page();
        }

        public IActionResult OnPost()
        {
            ListeDeMarques = GestionReservable.ListeDeMarques;
            int anneeMax = DateTime.Now.Year;
            int anneeMin = anneeMax - 10;

            if (Voiture.AnneeFabrication < anneeMin || Voiture.AnneeFabrication > anneeMax)
            {
                ModelState.AddModelError("Voiture.AnneeFabrication", $"L'année de fabrication doit être comprise entre {anneeMin} et {anneeMax}");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            GestionReservable.ModifierReservable(Voiture);

            return RedirectToPage("Index");
        }
    }
}
