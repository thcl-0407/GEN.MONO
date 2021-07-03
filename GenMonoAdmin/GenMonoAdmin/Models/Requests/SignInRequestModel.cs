using System.ComponentModel.DataAnnotations;

namespace GenMonoAdmin.Models.Requests
{
    public class SignInRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = ("Password must be 6 characters long"))]
        public string Password { get; set; }
    }
}
