using System;
using System.ComponentModel.DataAnnotations;

namespace GenMono.Identities.Request
{
    public class UpdateVerifyStatusRequest
    {
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public bool StatusToUpdate { get; set; }
    }
}
