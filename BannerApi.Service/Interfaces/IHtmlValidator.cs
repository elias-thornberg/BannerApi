using BannerApi.Service.Models;
using System.Threading.Tasks;

namespace BannerApi.Service.Interfaces
{
    public interface IHtmlValidator
    {
        Task<ValidationResult> Validate(string html);
    }
}
