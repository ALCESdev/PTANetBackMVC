using Microsoft.EntityFrameworkCore;
using pt_alicunde_aae.Entities;

namespace pt_alicunde_aae.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Bank> Bank { get; set; }
}