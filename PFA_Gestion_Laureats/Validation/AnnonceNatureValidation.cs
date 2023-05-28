using System.ComponentModel.DataAnnotations;

namespace PFA_Gestion_Laureats.Validation
{
    public class AnnonceNatureValidation : ValidationAttribute
    {
        string[] AllowedNatures;
        public AnnonceNatureValidation(string[] AllowedNatures)
        {
            this.AllowedNatures = AllowedNatures;
        }
        public override bool IsValid(object value)
        {
            if (value != null)
            {                
                if (AllowedNatures.Contains(value))
                {
                    return true;
                }
                return false;
            }

            return true;
        }
    }
}
