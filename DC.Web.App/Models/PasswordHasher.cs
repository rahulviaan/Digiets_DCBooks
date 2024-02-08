using Microsoft.AspNet.Identity;
namespace DC.Web.App.Models
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var hashPassword = DC.Encryption.EncryptCommon(password);
            return hashPassword;
        }

        public PasswordVerificationResult VerifyHashedPassword
                      (string hashedPassword, string providedPassword)
        {
            return hashedPassword == HashPassword(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }


    }
}
 