using System.ComponentModel.DataAnnotations;

namespace GenMono.Identities.Request
{
    public class GetUserRequest
    {
        [Required]
        public string Payload { get; set; }
    }
}
