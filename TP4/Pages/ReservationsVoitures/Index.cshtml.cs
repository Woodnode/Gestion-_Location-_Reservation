using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;


namespace TP4.Pages.ReservationsVoitures
{
    public class IndexModel : PageModel
    {
        public required List<Reservation> Reservations { get; set; } 

        public void OnGet()
        {
            GestionReservable.ChargerListeDepuisFichier("Reservation");
            var reservations = GestionReservable.ObtenirListeReservable("Reservation").Cast<Reservation>().ToList();

            Reservations = reservations.Count != 0 ? reservations : [];
        }
    }
}
