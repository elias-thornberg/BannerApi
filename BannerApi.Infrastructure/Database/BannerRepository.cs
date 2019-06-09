using BannerApi.Core.Interfaces;
using BannerApi.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BannerApi.Infrastructure.Database
{
    public class BannerRepository : IBannerRepository
    {
        private readonly BannerContext _context;

        public BannerRepository(BannerContext context)
        {
            _context = context;
        }

        public async Task Add(Banner banner)
        {
            _context.Add(banner);
            await _context.SaveChangesAsync();
        }

        public async Task<Banner> Get(int id)
        {
            return await _context.Banners.FindAsync(id);
        }

        public async Task<IEnumerable<Banner>> GetAll()
        {
            return await _context.Banners.ToListAsync();
        }

        public async Task Remove(int id)
        {
            var bannerToRemove = _context.Banners.Find(id);
            _context.Remove(bannerToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Banner banner)
        {
            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();
        }
    }
}
