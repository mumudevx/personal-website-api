using Core.Models;
using Entities.Enums;

namespace Entities;

public class HealthActivity : Entity
{
    public ActivityType ActivityType { get; set; }
    public int Calories { get; set; }
    public DateTime ActionStartDate { get; set; }
    public DateTime ActionEndDate { get; set; }
    public double Duration => (ActionEndDate - ActionStartDate).TotalMinutes;
}