using System.ComponentModel.DataAnnotations;
using Core.Models;

namespace Entities;

public class ImageFile : Entity
{
    [MaxLength(150)] public string Name { get; set; } = null!;
    [MaxLength(350)] public string Alt { get; set; } = null!;
    [MaxLength(500)] public string Path { get; set; } = null!;
}