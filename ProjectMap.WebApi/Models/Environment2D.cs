using System.ComponentModel.DataAnnotations;

namespace ProjectMap.WebApi.Models
{
    public class Environment2D
    {
        public Guid id { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid? OwnerUserId { get; set; }

        [Range(20, 200)]
        public float MaxLength { get; set; }

        [Range(10, 100)]
        public float MaxHeight { get; set; }
    }
}