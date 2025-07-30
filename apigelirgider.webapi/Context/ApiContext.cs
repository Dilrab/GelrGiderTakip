using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using IncomeExpenseTracker.Entities;




namespace ApiGelirGider.WebApi.Context
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DILARA\\SQLEXPRESS;initial catalog=ApiGelirGiderDB; integrated security=true;TrustServerCertificate=true;");
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
