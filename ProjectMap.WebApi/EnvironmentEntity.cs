using System.ComponentModel.DataAnnotations;
namespace ProjectMap.WebApi;

public class EnvironmentEntity
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Range(0, 75)]
    public float MaxLength { get; set; }

    [Range(0, 75)]
    public float MaxHeight { get; set; }
}

