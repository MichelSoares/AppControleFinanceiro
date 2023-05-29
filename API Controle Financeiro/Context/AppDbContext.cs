using Microsoft.EntityFrameworkCore;
using ControleFinanceiroAPI.Models;

namespace ControleFinanceiroAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction>? Transactions { get; set; }
    }
}
