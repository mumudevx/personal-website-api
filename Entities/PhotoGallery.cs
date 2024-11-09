using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace Entities;

public class PhotoGallery : Entity
{
    [MaxLength(150)] public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    [MaxLength(500)] public string? Description { get; set; }

    public List<PhotoItem> Photos { get; set; } = [];
}