using System.Collections.Generic;

namespace BannerApi.Service.Models
{
    public class SaveBannerResult
    {
        public SaveBannerResult()
        {
            Errors = new List<string>();
        }

        public bool Sucessful { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public int? Id { get; set; }
    }
}
