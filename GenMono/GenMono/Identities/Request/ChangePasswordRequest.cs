using System.ComponentModel.DataAnnotations;

namespace GenMono.Identities.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        [MinLength(6, ErrorMessage = "Password needs to be 6 characters long")]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password needs to be 6 characters long")]
        public string NewPassword { get; set; }
    }
}
