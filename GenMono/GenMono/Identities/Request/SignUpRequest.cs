using System;
using System.ComponentModel.DataAnnotations;

namespace GenMono.Identities.Request
{
    public class SignUpRequest
    {
        [Required]
        public string FullName { get; set; }

        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Email is not correct format")]
        public string Email { get; set; }

        [RegularExpression(@"^0[1-9][0-9]{8}$", ErrorMessage = "PhoneNo is not correct format")]
        public string PhoneNo { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password needs to be 6 characters long")]
        public string Password { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }
    }
}
