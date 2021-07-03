using System.ComponentModel.DataAnnotations;

namespace GenMonoAdmin.Models.Requests
{
    public class GenerateHashRequest
    {
        [Required]
        public string value { get; set; }
    }
}
