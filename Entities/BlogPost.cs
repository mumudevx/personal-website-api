using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace Entities;

public class BlogPost : Entity
{
    [MaxLength(150)] public string Title { get; set; } = null!;
    [MaxLength(300)] public string ShortContent { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;

    public Guid FeaturedImageId { get; set; }
    public ImageFile FeaturedImage { get; set; } = null!;
    
    public List<string> Tags { get; set; } = [];
}