using Microsoft.EntityFrameworkCore;
using Entities;

namespace Data{

public class TransientDataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public TransientDataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity, change to a real db for production applications
        options.UseInMemoryDatabase("CalcDb");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Calculation> Calculations {get; set;}
    
}
}