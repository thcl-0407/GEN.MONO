using System.ComponentModel.DataAnnotations;
using DatabaseHelper.Enums;
using Extension;

namespace GenMono.Identities.Request
{
    public class SignInRequest
    {
        [Required]
        public string EmailOrPhone { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password needs to be 6 characters long")]
        public string Password { get; set; }

        public SignInType GetTypeSignIn()
        {
            if(this.EmailOrPhone.IsRightEmail())
            {
                return SignInType.EMAIL;
            }

            if (this.EmailOrPhone.IsRightPhoneNo())
            {
                return SignInType.PHONE_NO;
            }

            throw new ValidationException();
        }
    }
}
