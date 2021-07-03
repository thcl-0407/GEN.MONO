using System.ComponentModel.DataAnnotations;

namespace GenMono.Identities.Request
{
    public class UpdateProfileRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string StateOrCity { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
