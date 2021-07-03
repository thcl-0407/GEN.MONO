using System;
using System.ComponentModel.DataAnnotations;

namespace GenMono.Identities.Request
{
    public class VerifyEmailRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
