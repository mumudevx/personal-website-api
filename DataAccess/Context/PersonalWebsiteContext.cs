using System;
using System.Runtime.InteropServices;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class PersonalWebsiteContext(DbContextOptions<PersonalWebsiteContext> options) : DbContext(options)
{
    private const string Server = "LiveServer";
    private const string DatabaseName = "LiveUser";
    private const string User = "LiveUser";
    private const string Password = "LivePassword";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            if (Environment.GetEnvironmentVariable("SQLSERVER_HOST_TYPE", EnvironmentVariableTarget.User) == "Docker" ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost;" +
                    "Database=PersonalWebsiteDb;" +
                    "User Id=SA;" +
                    "Password=38fwSANsaX!9XsaO;" +
                    "Trust Server Certificate=true"
                );
            }
            else
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=localhost;" +
                    "Initial Catalog=PersonalWebsiteDb;" +
                    "Integrated Security=True;" +
                    "Trust Server Certificate=true"
                );
            }
        }
        else
        {
            optionsBuilder.UseSqlServer(
                $"Server={Server};" +
                $"Database={DatabaseName};" +
                $"User Id={User};" +
                $"Password={Password};" +
                "Trust Server Certificate=true"
            );
        }
    }

    public DbSet<BlogPost> BlogPosts { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<HealthActivity> HealthActivities { get; set; } = null!;
    public DbSet<ImageFile> ImageFiles { get; set; } = null!;
    public DbSet<PhotoGallery> PhotoGalleries { get; set; } = null!;
    public DbSet<PhotoItem> PhotoItems { get; set; } = null!;
}
