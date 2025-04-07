//using System.ComponentModel.DataAnnotations;
//namespace ProjectMap.WebApi;

//public class EnvironmentEntity
//{
//    public const int MAX_NUMBER_OF_USER_ENVIRONMENTS = 5;

//    public Guid Id { get; set; }

//    [Required]
//    [MinLength(1)]
//    [MaxLength(25)]
//    public string? Name { get; set; }

//    public string? OwnerUserId { get; set; }

//    [Range(20, 200)]
//    public float MaxLength { get; set; } = 20;

//    [Range(10, 100)]
//    public float MaxHeight { get; set; } = 10;

//    public bool IsValidPosition(float positionX, float positionY)
//    {
//        return positionX <= MaxLength && positionY <= MaxHeight;
//    }
//}

