using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP4.Services;
using TP4.Models;

namespace TP4.Pages.Chambres
{
    public class ConsulterModel : PageModel
    {
        [BindProperty]
        public required Chambre Chambre { get; set; }

        public ActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var chambre = GestionReservable.ObtenirReservableParId("chambre", id.Value) as Chambre;

            if (chambre == null)
            {
                return NotFound();
            }

            Chambre = chambre;
            return Page();
        }
    }
}
