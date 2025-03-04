using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required double LengthInKm { get; set; }

        public string? WalkImgUrl { get; set; }

        public required DifficultyDto Difficulty { get; set; }

        public required RegionDto Region { get; set; }

    }
}
