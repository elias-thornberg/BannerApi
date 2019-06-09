using BannerApi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BannerApi.Infrastructure.Database
{
    public class BannerContext : DbContext
    {
        public BannerContext(DbContextOptions<BannerContext> options) : base(options)
        {

        }

        public DbSet<Banner> Banners { get; set; }
    }
}
