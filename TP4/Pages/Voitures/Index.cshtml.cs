using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Voitures
{
    public class IndexModel : PageModel
    {
        public required List<Voiture> Voitures { get; set; }

        public void OnGet()
        {
            var voitures = GestionReservable.ObtenirListeReservable("Voiture").Cast<Voiture>().ToList();

            Voitures = voitures.Count != 0 ? voitures : [];
        }

        public string ReduireDescription(Voiture voiture)
        {
            return voiture.Description.Length > 50 ? string.Concat(voiture.Description.AsSpan(0, 50), "...") : voiture.Description;
        }
    }
}
