using System.Collections.Generic;

namespace BannerApi.Service.Models
{
    public class ValidationResult
    {
        public ValidationResult(bool valid, IEnumerable<string> errors = null)
        {
            IsValid = valid;
            Errors = errors ?? new List<string>();
        }

        public bool IsValid { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
