using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using TP4.Models;

namespace TP4.Services
{
    public class GestionReservable
    {
        private static readonly List<Voiture> ListeVoitures = [];
        private static readonly List<Chambre> ListeChambres = [];
        public static readonly List<Reservation> ListeReservations = [];
        public static readonly List<string> ListeDeMarques = ["Kia", "Ford", "Mazda", "Toyota", "Hyundai", "Honda"];
        public static readonly string CheminFichierVoiture = $@".\monfichierVoiture.txt";
        public static readonly string CheminFichierChambre = $@".\monfichierChambre.txt";
        public static readonly string CheminFichierReservation = $@".\monfichierReservation.txt";

        public static int GenererId(string type) 
        {
            int id = 0;
            string cheminFichier = type == "Voiture" ? CheminFichierVoiture : CheminFichierChambre;

            var lignes = File.ReadAllLines(cheminFichier);

            if (lignes.Length > 0)
            {
               var derniereLigne = lignes[^1];
               var champs = derniereLigne.Split(';');
               id = int.Parse(champs[0]);
            }

            return id + 1;
        }

        public static int GenererIdAleatoire()
        {
            Random random = new();
            int id = random.Next(1, 10000);

            while (ListeReservations.Any(reservation => reservation.Id == id))
            {
                id = random.Next(1, 10000);
            }

            return id;
        }

        protected static void SauvegarderListeReservable(string type)
        {
            if (type == "voiture")
            {
                using StreamWriter sw = new(CheminFichierVoiture);
                foreach (var voiture in ListeVoitures)
                {
                    sw.WriteLine($"{voiture.Id};{voiture.Marque};{voiture.PrixJournalier};{voiture.Description};{voiture.AnneeFabrication}");
                }
            }
            else if (type == "chambre")
            {
                using StreamWriter sw = new(CheminFichierChambre);
                foreach (var chambre in ListeChambres)
                {
                    sw.WriteLine($"{chambre.Id};{chambre.Description};{chambre.PrixJournalier}");
                }
            }
            else 
            {
                using StreamWriter sw = new(CheminFichierReservation);
                foreach (var reservation in ListeReservations)
                {
                    sw.WriteLine($"{reservation.Id};{reservation.DateDebut};{reservation.DateFin};" +
                        $"{reservation.ObjetDeLaReservation.GetType().Name};{reservation.ObjetDeLaReservation.Id};" +
                        $"{reservation.Prix}");
                }
            }
        }

        public static List<IReservable> ObtenirListeReservable(string type)
        {
            if (type == "voiture")
            {
                ListeVoitures.Clear();
                if (!File.Exists(CheminFichierVoiture))
                {
                    File.Create(CheminFichierVoiture).Close();
                }
                else
                {
                    var lignes = File.ReadAllLines(CheminFichierVoiture);
                    foreach (var ligne in lignes)
                    {
                        var champs = ligne.Split(';');
                        Voiture voiture = new()
                        {
                            Id = int.Parse(champs[0]),
                            Marque = champs[1],
                            PrixJournalier = int.Parse(champs[2]),
                            Description = champs[3],
                            AnneeFabrication = int.Parse(champs[4])
                        };
                        ListeVoitures.Add(voiture);
                    }
                }
                return ListeVoitures.Cast<IReservable>().ToList();
            }
            else if (type == "chambre")
            {
                ListeChambres.Clear();
                if (!File.Exists(CheminFichierChambre))
                {
                    File.Create(CheminFichierChambre).Close();
                }
                else
                {
                    var lignes = File.ReadAllLines(CheminFichierChambre);
                    foreach (var ligne in lignes)
                    {
                        var champs = ligne.Split(';');
                        Chambre chambre = new()
                        {
                            Id = int.Parse(champs[0]),
                            Description = champs[1],
                            PrixJournalier = int.Parse(champs[2]),
                        };
                        ListeChambres.Add(chambre);
                    }
                }
                return ListeChambres.Cast<IReservable>().ToList();
            }
            else 
            {
                ListeReservations.Clear();

                if (!File.Exists(CheminFichierReservation))
                {
                    File.Create(CheminFichierReservation).Close();
                }
                else
                {
                    var lignes = File.ReadAllLines(CheminFichierReservation);
                    foreach (var ligne in lignes)
                    {
                        var champs = ligne.Split(';');

                        Reservation reservation = new(int.Parse(champs[0]),
                            DateTime.Parse(champs[1]),
                            DateTime.Parse(champs[2]),
                            DefinirObjetReservation(
                                champs[3],
                                int.Parse(champs[4])))
                        {
                            Prix = double.Parse(champs[5])
                        };

                        ListeReservations.Add(reservation);
                    }
                }
                return ListeReservations.Cast<IReservable>().ToList(); ;
            }
        }

        public static IReservable DefinirObjetReservation(string type, int id)
        {
            if (type == "Chambre")
            {
                Chambre? chambre = ObtenirReservableParId("chambre", id) as Chambre;
                return chambre ?? throw new ArgumentException("Chambre introuvable");
            }
            else if (type == "Voiture")
            {
                Voiture? voiture = ObtenirReservableParId("voiture", id) as Voiture; ;
                return voiture ?? throw new ArgumentException("Voiture introuvable");
            }
            else
            {
                throw new ArgumentException("Type de réservation inconnu");
            }
        }

        public static void AjouterReservable(IReservable reservable)
        {
            if (reservable is Voiture voiture)
            {
                voiture.Id = GenererId("voiture");
                ListeVoitures.Add(voiture);

                SauvegarderListeReservable("voiture");
            }
            else if (reservable is Chambre chambre)
            {
                chambre.Id = GenererId("chambre");
                ListeChambres.Add(chambre);

                SauvegarderListeReservable("chambre");
            }
            else if (reservable is Reservation reservation)
            {
                reservation.Id = GenererIdAleatoire();
                ListeReservations.Add(reservation);

                SauvegarderListeReservable("reservation");
            }
            else
            {
                throw new ArgumentException("Type de réservation inconnu");
            }
        }

        public static void SupprimerReservable(IReservable reservable)
        {
            if (reservable is Voiture voiture)
            {
                Voiture voitureASupprimer = ListeVoitures.Single(v => v.Id == voiture.Id);
                ListeVoitures.Remove(voitureASupprimer);
                SauvegarderListeReservable("voiture");
            }
            else if (reservable is Chambre chambre)
            {
                Chambre chambreASupprimer = ListeChambres.Single(c => c.Id == chambre.Id);
                ListeChambres.Remove(chambreASupprimer);
                SauvegarderListeReservable("chambre");
            }
            else
            {
                Reservation reservationASupprimer = ListeReservations.Single(r => r.Id == reservable.Id);
                ListeReservations.Remove(reservationASupprimer);
                SauvegarderListeReservable("reservation");
            }
        }

        public static IReservable? ObtenirReservableParId(string type, int id)
        {
            if (type == "voiture")
            {
                return ListeVoitures.SingleOrDefault(v => v.Id == id);
            }
            else if (type == "chambre")
            {
                return ListeChambres.SingleOrDefault(c => c.Id == id);
            }
            else if (type == "reservation") 
            {
                return ListeReservations.SingleOrDefault(r => r.Id == id);
            }
            else
            {
                throw new ArgumentException("Type de réservation inconnu");
            }
        }

        public static void ModifierReservable(IReservable reservable)
        {
            if (reservable is Voiture voiture)
            {
                Voiture voitureAModifier = ListeVoitures.Single(v => v.Id == voiture.Id);
                voitureAModifier.Marque = voiture.Marque;
                voitureAModifier.PrixJournalier = voiture.PrixJournalier;
                voitureAModifier.Description = voiture.Description;
                voitureAModifier.AnneeFabrication = voiture.AnneeFabrication;

                SauvegarderListeReservable("voiture");
            }
            else if (reservable is Chambre chambre)
            {
                Chambre chambreAModifier = ListeChambres.Single(c => c.Id == chambre.Id);
                chambreAModifier.Description = chambre.Description;
                chambreAModifier.PrixJournalier = chambre.PrixJournalier;

                SauvegarderListeReservable("chambre");
            }
            else if (reservable is Reservation reservation)
            {
                Reservation reservationAModifier = ListeReservations.Single(r => r.Id == reservation.Id);

                reservationAModifier.DateDebut = reservation.DateDebut;
                reservationAModifier.DateFin = reservation.DateFin;
                reservationAModifier.ObjetDeLaReservation = reservation.ObjetDeLaReservation;
                reservationAModifier.Prix = reservation.Prix;

                SauvegarderListeReservable("reservation");
            }
        }
    }
}
    