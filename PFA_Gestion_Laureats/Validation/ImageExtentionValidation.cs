using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace PFA_Gestion_Laureats.Validation
{
    public class ImageExtentionValidation : ValidationAttribute
    {
        string[] AllowedExt;
        public ImageExtentionValidation(string[] AllowedExt)
        {
            this.AllowedExt = AllowedExt;
        }
        public override bool IsValid(object value)
            {
                if(value != null)
            {
                IFormFile image = (IFormFile)value;
                string FileExt = Path.GetExtension(image.FileName);
                if (AllowedExt.Contains(FileExt.ToLower()))
                {
                    return true;
                }
                return false;
            }
               
                return true;
            }
    }
}
