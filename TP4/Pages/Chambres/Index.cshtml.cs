using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Chambres
{
    public class IndexModel : PageModel
    {
        public required List<Chambre> Chambres { get; set; }

        public void OnGet()
        {
            var chambres = GestionReservable.ObtenirListeReservable("Chambre").Cast<Chambre>().ToList();
            
            Chambres = chambres.Count != 0 ? chambres : [];
        }

        public string ReduireDescription(Chambre chambre)
        {
            return chambre.Description.Length > 50 ? string.Concat(chambre.Description.AsSpan(0, 50), "...") : chambre.Description;
        }
    }
}
