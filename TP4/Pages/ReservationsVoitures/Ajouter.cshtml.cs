using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.ReservationsVoitures
{
    public class AjouterModel : PageModel
    {
        [BindProperty]
        public required Reservation Reservation { get; set; }
        
        public int ReservationId { get; set; }

        public required List<Reservation> ListeDeReservations { get; set; }

        public required List<Voiture> ListeDeVoitures { get; set; }

        [BindProperty]
        public int IdVoiture { get; set; }

        public ActionResult OnGet()
        {
            ChargementDePage();
            return Page();
        }

        public IActionResult OnPost()
        {
            ChargementDePage();
            Voiture voiture = ListeDeVoitures.First(v => v.Id == IdVoiture);

            if (voiture == null) ModelState.AddModelError("IdVoiture", "Veuillez sélectionner une voiture.");
            else
            {
                Reservation.ObjetDeLaReservation = voiture;
                Reservation.PrixJournalier = voiture.PrixJournalier;
                Reservation.Prix = Reservation.PrixJournalier * ((Reservation.DateFin - Reservation.DateDebut).Days + 1);
            }

            if (Reservation.DateDebut < DateTime.Now.Date)
            {
                ModelState.AddModelError("Reservation.DateDebut", $"La date de début doit être supérieure ou égale à {DateTime.Now}.");
            }
            if (Reservation.DateFin < Reservation.DateDebut)
            {
                ModelState.AddModelError("Reservation.DateFin", $"La date de fin doit être supérieure ou égale à {Reservation.DateDebut}.");
            }

            if (ListeDeReservations.Where(r => r.ObjetDeLaReservation.Id == Reservation.ObjetDeLaReservation.Id)
                .Any(r => r.DateFin >= Reservation.DateDebut && r.DateDebut <= Reservation.DateFin))
            {
                ModelState.AddModelError("Reservation.ObjetDeLaReservation", "Cette voiture est déjà réservée pour ces dates.");
            }

            if (!ModelState.IsValid) return Page();

            GestionReservable.AjouterReservable(Reservation);

            return RedirectToPage("Index");
        }

        private void ChargementDePage()
        {
            ReservationId = GestionReservable.GenererId("Reservation");
            ListeDeReservations = [.. GestionReservable.ObtenirListeReservable("Reservation").Cast<Reservation>()];
            ListeDeVoitures = [];
            List<Voiture> ListeToutesVoitures = [.. GestionReservable.ObtenirListeReservable("Voiture").Cast<Voiture>()];
            int anneeMax = DateTime.Now.Year;
            int anneeMin = anneeMax - 10;
            foreach (var voiture in ListeToutesVoitures)
            {
                if (voiture.AnneeFabrication > anneeMin)  ListeDeVoitures.Add(voiture);
            }
        }
    }
}
