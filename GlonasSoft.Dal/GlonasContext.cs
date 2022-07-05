using GlonasSoft.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GlonasSoft.Dal;

public class GlonasContext : DbContext
{
    private readonly string _connectionString;
    public GlonasContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("GlonasDb")!;
    }
    
    public DbSet<UserRequestDal> UserRequests { get; set; }
    
    public DbSet<UserDal> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_connectionString);
    }
}