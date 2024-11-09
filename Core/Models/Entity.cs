using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public interface IEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class Entity : IEntity
{
    [Key] public Guid Id { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}