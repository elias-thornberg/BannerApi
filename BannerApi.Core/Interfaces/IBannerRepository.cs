using BannerApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BannerApi.Core.Interfaces
{
    public interface IBannerRepository
    {
        Task<Banner> Get(int id);

        Task<IEnumerable<Banner>> GetAll();

        Task Add(Banner banner);

        Task Update(Banner banner);

        Task Remove(int id);
    }
}
