using PFA_Gestion_Laureats.Services;
using PFA_Gestion_Laureats.ViewModels.Annonces;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFA_Gestion_Laureats.Models
{
    public class Annonce
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string ?Photo { get; set; }
        public string Email_Reception { get; set; }
        public DateTime Date_limite_Deposer { get; set; }
        public DateTime Date_Creation { get; set; }

        public string Nature { get; set; }
        public bool Remuniration { get; set; }
        public IList<Postulation>? postulations { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur? utilisateur { get; set; }
        public int EntrepriseId { get; set; }
        public Entreprise? entreprise { get; set; }
        public IList<AnnonceTechnologie>? AnnonceTechnologies { get; set; }
        public Annonce() { }
        public Annonce(AddAnnonceViewModel amv) { 
            this.Titre = amv.Titre;
            
            this.Description= amv.Description;
            this.Email_Reception=amv.Email_Reception;
            this.Date_limite_Deposer=amv.Date_limite_Deposer;
            this.Date_Creation=DateTime.Now;
            this.EntrepriseId= amv.EntrepriseId;
            this.Remuniration = amv.Remuniration;
            this.Nature = amv.Nature;
        }

        public List<Annonce> FilterAnnonces(List<Annonce> annonces, string SearchString, string Nature, string remunerer, string Technologie, string orderBy)
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                annonces = annonces.Where(a => a.entreprise.Nom.ToLower().Contains(SearchString.ToLower()) || a.entreprise.Ville.ToLower().Contains(SearchString.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(Nature))
            {
                annonces = annonces.Where(a => a.Nature == Nature).ToList();
            }
            if (!string.IsNullOrEmpty(remunerer))
            {
                if (remunerer == "Remunerer")
                {
                    annonces = annonces.Where(a => a.Remuniration == true).ToList();
                }
                else if (remunerer == "NonRemunerer")
                {
                    annonces = annonces.Where(a => a.Remuniration == false).ToList();
                }

            }

            if (!string.IsNullOrEmpty(Technologie))
            {
                List<Annonce> annonces1 = new List<Annonce>();
                foreach (Annonce annonce in annonces.ToList())
                {
                    foreach (AnnonceTechnologie technologie in annonce.AnnonceTechnologies)
                    {
                        if (technologie.Technologie.Libelle.Equals(Technologie))
                        {
                            annonces1.Add(annonce);
                        }
                    }
                }
                annonces = annonces1;
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy)
                {
                    case "plusConsultes":

                        annonces = annonces.OrderByDescending(a => a.postulations.Count).ToList();
                        break;

                    case "plusPostules":

                        annonces = annonces.OrderByDescending(a => a.postulations.Count(p => p.Date_Postulation != null)).ToList();
                        break;
                    case "moinsConsultes":

                        annonces = annonces.OrderBy(a => a.postulations.Count).ToList();
                        break;
                    case "moinsPostules":

                        annonces = annonces.OrderBy(a => a.postulations.Count(p => p.Date_Postulation != null)).ToList();
                        break;


                }
            }

            return annonces;
        }

    }
}
