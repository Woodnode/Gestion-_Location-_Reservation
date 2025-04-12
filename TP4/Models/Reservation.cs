namespace TP4.Models
{
    public class Reservation : IReservable
    {
        public int Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public IReservable ObjetDeLaReservation { get; set; }
        public int Prix { get; set; }
        public int PrixJournalier { get; set; }

        public Reservation(int id, DateTime dateDebut, DateTime dateFin, IReservable objetDeLaReservation)
        {
            Id = id;
            DateDebut = dateDebut;
            DateFin = dateFin;
            ObjetDeLaReservation = objetDeLaReservation;
            PrixJournalier = objetDeLaReservation.PrixJournalier;
            Prix = CalculerPrix();
        }

        public int CalculerPrix()
        {
           return PrixJournalier * (DateFin - DateDebut).Days;
        }
    }
}
