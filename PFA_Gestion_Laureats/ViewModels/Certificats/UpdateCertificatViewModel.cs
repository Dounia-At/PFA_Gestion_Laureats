using PFA_Gestion_Laureats.Models;

namespace PFA_Gestion_Laureats.ViewModels.Certificats
{
    public class UpdateCertificatViewModel : AddCertificatViewModel
    {
        public int Id { get; set; }
        public UpdateCertificatViewModel() { }
        public UpdateCertificatViewModel(Certificat c)
        {
            Id = c.Id;
            Nom = c.Nom;
            Organisation = c.Organisation;
            Date_Emission = c.Date_Emission;
            Date_Expiration = c.Date_Expiration;

        }
    }
}
