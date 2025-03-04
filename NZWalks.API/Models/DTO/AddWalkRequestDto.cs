using NZWalks.API.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
        public required string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Description has to be a maximum of 1000 characters")]
        public required string Description { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Length has to be between 0 and 100 km")]
        public required double LengthInKm { get; set; }

        public string? WalkImgUrl { get; set; }

        [Required]
        public required Guid DifficultyId { get; set; }

        [Required]
        public required Guid RegionId { get; set; }
    }
}
