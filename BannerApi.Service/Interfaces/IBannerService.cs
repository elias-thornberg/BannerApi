using BannerApi.Core.Models;
using BannerApi.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BannerApi.Service.Interfaces
{
    public interface IBannerService
    {
        Task<Banner> Get(int id);

        Task<IEnumerable<Banner>> GetAll();

        Task<SaveBannerResult> Add(CreateBannerModel banner);

        Task<SaveBannerResult> Update(UpdateBannerModel banner);

        Task Remove(int id);

        Task<string> GetHtml(int id);
    }
}
