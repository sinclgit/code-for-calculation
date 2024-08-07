using Microsoft.EntityFrameworkCore;
using Entities;
using Configurations;

namespace Data
{
    public class CalculatorDbContext : DbContext
    {
        public CalculatorDbContext(DbContextOptions<CalculatorDbContext> options) 
            : base(options)
        {
        }

        public virtual DbSet<Calculation> Calculations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CalculationConfiguration());

        }
    }
}