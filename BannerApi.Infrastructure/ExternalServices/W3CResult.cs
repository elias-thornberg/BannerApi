using System.Collections.Generic;

namespace BannerApi.Infrastructure.ExternalServices
{
    public class W3CResult
    {
        public string Url { get; set; }

        public IEnumerable<W3CMessage> Messages { get; set; }
    }

    public class W3CMessage
    {
        public string Type { get; set; }

        public string Url { get; set; }

        public string Message { get; set; }
    }
}
