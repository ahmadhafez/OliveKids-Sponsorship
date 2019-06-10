using Microsoft.EntityFrameworkCore;
using OliveKids.Models;
namespace OliveKids.Repository
{
    public class OkSposershipContext : DbContext
    {
        public OkSposershipContext(DbContextOptions<OkSposershipContext> options)
            : base(options)
        {
        }
        public DbSet<Sponsor> Sponsors { get; set; }
        public DbSet<Kid> Kids { get; set; }

    }
}
