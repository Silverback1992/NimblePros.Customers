using Microsoft.EntityFrameworkCore;
using NimblePros.Customers.Domain;

namespace NimblePros.Customers.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    public DbSet<Customer> Customers { get; set; }
}
