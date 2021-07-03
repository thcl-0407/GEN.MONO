using System.ComponentModel.DataAnnotations;

namespace GenMono.Identities.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string EmailOrPhone { get; set; }
    }
}