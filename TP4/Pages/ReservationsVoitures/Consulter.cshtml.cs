using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Models;
using TP4.Services;

namespace TP4.Pages.ReservationsVoitures
{
    public class ConsulterModel : PageModel
    {
        [BindProperty]
        public required Reservation Reservation { get; set; }

        public required Voiture Voiture { get; set; }

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

            Voiture = voiture;
            Reservation = reservation;
            return Page();
        }
    }
}
