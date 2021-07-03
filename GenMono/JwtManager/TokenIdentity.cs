using System.ComponentModel.DataAnnotations;
using Extension;

namespace JwtManager
{
    public class TokenIdentity
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsValid()
        {
            if (this == null)
            {
                return false;
            }

            if (this.UserId.IsNullOrEmpty())
            {
                return false;
            }

            if (this.FullName.IsNullOrEmpty())
            {
                return false;
            }

            if (this.Gender.IsNullOrEmpty())
            {
                return false;
            }

            if (this.DateOfBirth.IsNullOrEmpty())
            {
                return false;
            }

            if (this.Role.IsNullOrEmpty())
            {
                return false;
            }

            return true;
        }
    }
}
