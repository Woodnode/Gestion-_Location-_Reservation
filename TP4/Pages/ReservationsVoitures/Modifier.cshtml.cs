using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.ReservationsVoitures
{
    public class ModifierModel : PageModel
    {
        [BindProperty]
        public required Reservation Reservation { get; set; }

        public required List<Voiture> ListeDeVoitures { get; set; }

        [BindProperty]
        public int IdVoiture { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (GestionReservable.ObtenirReservableParId("Reservation", id.Value) is not Reservation reservation || reservation is null)
            {
                return NotFound();
            }

            if (reservation.ObjetDeLaReservation is null || GestionReservable.ObtenirReservableParId("Voiture", reservation.ObjetDeLaReservation.Id) is not Voiture voiture)
            {
                return NotFound();
            }

            Reservation = reservation;
            ChargementDePage();
            return Page();
        }

        public IActionResult OnPost()
        {
            ChargementDePage();
            Voiture voiture = ListeDeVoitures.First(v => v.Id == IdVoiture);

            if (voiture == null)
            {
                ModelState.AddModelError("IdVoiture", "Veuillez sélectionner une voiture.");
            }

            else
            {
                Reservation.ObjetDeLaReservation = voiture;
                Reservation.PrixJournalier = voiture.PrixJournalier;
                Reservation.Prix = Reservation.PrixJournalier * ((Reservation.DateFin - Reservation.DateDebut).Days + 1);
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Reservation.DateFin < DateTime.Now.Date)
            {
                ModelState.AddModelError(string.Empty, "Vous ne pouvez pas modifier une réservation passée ou en cours.");
                return Page();
            }

            GestionReservable.ModifierReservable(Reservation);
            return RedirectToPage("Index");
        }

        private void ChargementDePage()
        {
            ListeDeVoitures = [.. GestionReservable.ObtenirListeReservable("Voiture").Cast<Voiture>()];
        }
    }
}
