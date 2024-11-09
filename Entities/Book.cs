using Core.Models;

namespace Entities;

public class Book : Entity
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string? Publisher { get; set; }
    public string? Isbn { get; set; }
    public int? Year { get; set; }
    public int Pages { get; set; }
    public string? Comment { get; set; }
    public float? Rating { get; set; }
    
    public Guid FeaturedImageId { get; set; }
    public ImageFile FeaturedImage { get; set; } = null!;

    public DateTime StartReadingDate { get; set; }
    public DateTime? EndReadingDate { get; set; }

    public int? DaysToComplete => EndReadingDate.HasValue
        ? (EndReadingDate.Value - StartReadingDate).Days
        : null;

    public List<string> Tags { get; set; } = [];
}