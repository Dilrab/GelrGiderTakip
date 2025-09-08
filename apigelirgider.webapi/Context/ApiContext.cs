using Microsoft.EntityFrameworkCore;
using IncomeExpenseTracker.Entities;

namespace ApiGelirGider.WebApi.Context
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        // Your existing tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense>   Expenses   { get; set; }
        public DbSet<Income>    Incomes    { get; set; }
        public DbSet<User>      Users      { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User tablosu yapılandırması
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.UserEmail)
                      .IsRequired()
                      .HasMaxLength(256);

   
                entity.Property(u => u.UserName)
                      .HasMaxLength(100);

               
            });
        }
    }

}


