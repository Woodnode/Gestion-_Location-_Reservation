using System.Net.Http.Headers;
using System.Reflection;
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


        public static int GenererId(string type) 
        {
            int id = 0;
            string cheminFichier = @".\monfichier" + type + ".txt";
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
            string cheminFichier = @".\monfichier" + type + ".txt";
            using StreamWriter sw = new(cheminFichier);

            if (type == "Voiture")
            {
                foreach (var voiture in ListeVoitures)
                {
                    sw.WriteLine($"{voiture.Id};{voiture.Marque};{voiture.PrixJournalier};{voiture.Description};{voiture.AnneeFabrication}");
                }
            }
            else if (type == "Chambre")
            {
                foreach (var chambre in ListeChambres)
                {
                    sw.WriteLine($"{chambre.Id};{chambre.Description};{chambre.PrixJournalier}");
                }
            }
            else
            {
                foreach (var reservation in ListeReservations)
                {
                    sw.WriteLine($"{reservation.Id};{reservation.DateDebut};{reservation.DateFin};" +
                        $"{reservation.ObjetDeLaReservation.GetType().Name};{reservation.ObjetDeLaReservation.Id};" +
                        $"{reservation.Prix}");
                }
            }
        }

        // DIFFERENCIER OBTENIR LIST RESERVABLE DE CHARGER FICHIERS. S'INSPIRER DE OBTENIR RESERVABLE.
        public static List<IReservable> ObtenirListeReservable(string type)
        {
            string cheminFichier = @".\monfichier" + type + ".txt";

            if (type == "Voiture")
            {
                ListeVoitures.Clear();
                if (!File.Exists(cheminFichier))
                {
                    File.Create(cheminFichier).Close();
                }
                else
                {
                    var lignes = File.ReadAllLines(cheminFichier);
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
            else if (type == "Chambre")
            {
                ListeChambres.Clear();
                if (!File.Exists(cheminFichier))
                {
                    File.Create(cheminFichier).Close();
                }
                else
                {
                    var lignes = File.ReadAllLines(cheminFichier);
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

                if (!File.Exists(cheminFichier))
                {
                    File.Create(cheminFichier).Close();
                }
                else
                {
                    var lignes = File.ReadAllLines(cheminFichier);
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
                voiture.Id = GenererId(voiture.GetType().Name);
                ListeVoitures.Add(voiture);
            }
            else if (reservable is Chambre chambre)
            {
                chambre.Id = GenererId(chambre.GetType().Name);
                ListeChambres.Add(chambre);
            }
            else if (reservable is Reservation reservation)
            {
                reservation.Id = GenererIdAleatoire();
                ListeReservations.Add(reservation);
            }
            else
            {
                throw new ArgumentException("Type de réservation inconnu");
            }

            SauvegarderListeReservable(reservable.GetType().Name);
        }

        public static void SupprimerReservable(IReservable reservable)
        {
            if (reservable is Voiture voiture)
            {
                Voiture voitureASupprimer = ListeVoitures.Single(v => v.Id == voiture.Id);
                ListeVoitures.Remove(voitureASupprimer);
            }
            else if (reservable is Chambre chambre)
            {
                Chambre chambreASupprimer = ListeChambres.Single(c => c.Id == chambre.Id);
                ListeChambres.Remove(chambreASupprimer);
            }
            else
            {
                Reservation reservationASupprimer = ListeReservations.Single(r => r.Id == reservable.Id);
                ListeReservations.Remove(reservationASupprimer);
            }

            SauvegarderListeReservable(reservable.GetType().Name);
        }

        public static IReservable? ObtenirReservableParId(string type, int id)
        {
            List<IReservable> listeReservable = [];
            switch (type) 
            {
                case "Voiture":
                    listeReservable = [.. ListeVoitures.Cast<IReservable>()];
                    break;
                case "Chambre":
                    listeReservable = [.. ListeChambres.Cast<IReservable>()];
                    break;
                case "Reservation":
                    listeReservable = [.. ListeReservations.Cast<IReservable>()];
                    break;
            }

            return listeReservable.SingleOrDefault(l => l.Id == id);
        }

        public static void ModifierReservable(IReservable reservable)
        {
            if (reservable is Reservation reservation)
            {
                Reservation reservationAModifier = ListeReservations.Single(r => r.Id == reservation.Id);

                reservationAModifier.DateDebut = reservation.DateDebut;
                reservationAModifier.DateFin = reservation.DateFin;
                reservationAModifier.ObjetDeLaReservation = reservation.ObjetDeLaReservation;
                reservationAModifier.Prix = reservation.Prix;

            }
            else 
            {
                List<IReservable> listeReservable = reservable is Voiture ? [.. ListeVoitures.Cast<IReservable>()] : [.. ListeChambres.Cast<IReservable>()];
                IReservable reservableAModifier = listeReservable.Single(r => r.Id == reservable.Id);

                PropertyInfo[] props = reservable.GetType().GetProperties();

                foreach (var prop in props)
                {
                    reservableAModifier.GetType().GetProperty(prop.Name)?.SetValue(reservableAModifier, prop.GetValue(reservable));
                }
            }

            SauvegarderListeReservable(reservable.GetType().Name);
        }
    }
}
    