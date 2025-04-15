using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Models;
using TP4.Services;

namespace TP4.Pages.ReservationsVoitures
{
    public class SupprimerModel : PageModel
    {
        [BindProperty]
        public required Reservation Reservation { get; set; }

        public required Voiture Voiture { get; set; }

        public void OnGet(int id)
        {
            ChargementDePage(id);

        }

        public IActionResult OnPost()
        {
            GestionReservable.SupprimerReservable(Reservation);
            return RedirectToPage("Index");
        }

        private void ChargementDePage(int id)
        {
            Reservation = GestionReservable.ObtenirReservableParId("Reservation", id) as Reservation;

            if (Reservation == null)
            {
                RedirectToPage("Index");
            }

            Voiture = GestionReservable.ObtenirReservableParId("Voiture", Reservation.ObjetDeLaReservation.Id) as Voiture;
        }
    }
}
