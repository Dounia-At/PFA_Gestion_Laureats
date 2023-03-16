using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace PFA_Gestion_Laureats.Validation
{
    public class ProfilExtentionValidation : ValidationAttribute
    {
        string[] AllowedExt;
        public ProfilExtentionValidation(string[] AllowedExt)
        {
            this.AllowedExt = AllowedExt;
        }
        public override bool IsValid(object value)
            {
            
                IFormFile image = (IFormFile)value;
                string FileExt = Path.GetExtension(image.FileName);
                if (AllowedExt.Contains(FileExt.ToLower()))
                {
                    return true;
                }
                return false;
            }
    }
}
