namespace Core.Models;

public interface IUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string HashedPassword { get; set; }
    public string? LastBrowserAgentInfo { get; set; }
    public string? LastIpAddress { get; set; }
}