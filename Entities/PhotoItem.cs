using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace Entities;

public class PhotoItem : Entity
{
    [MaxLength(150)] public string Title { get; set; } = null!;
    [MaxLength(500)] public string? Description { get; set; }
    public int Order { get; set; }

    public Guid PhotoGalleryId { get; set; }
    public PhotoGallery PhotoGallery { get; set; } = null!;

    public Guid ImageFileId { get; set; }
    public ImageFile ImageFile { get; set; } = null!;

    public List<string> Tags { get; set; } = [];
}