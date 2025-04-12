namespace TP4.Models
{
    public class Reservation : IReservable
    {
        public int Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public IReservable? ObjetDeLaReservation { get; set; }
        public int Prix { get; set; }
        public int PrixJournalier { get; set; }
    }
}
