namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required double LengthInKm { get; set; }

        public string? WalkImgUrl { get; set; }

        public required Guid DifficultyId { get; set; }

        public required Guid RegionId { get; set; }

        // Navigation properties
        public required Difficulty Difficulty { get; set; }

        public required Region Region { get; set; }
    }
}
