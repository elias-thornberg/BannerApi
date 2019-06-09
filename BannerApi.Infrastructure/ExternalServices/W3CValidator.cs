using BannerApi.Service.Interfaces;
using BannerApi.Service.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BannerApi.Infrastructure.ExternalServices
{
    public class W3CValidator : IHtmlValidator
    {
        private const string BaseUrl = "https://html5.validator.nu/";

        public async Task<ValidationResult> Validate(string html)
        {
            var validationResult = await GetValidationResult(html);
            var errorMessage = validationResult.Messages.Where(x => x.Type == "error").Select(x => x.Message);
            var isValid = !errorMessage.Any();

            return new ValidationResult(isValid, errorMessage);
        }

        private async Task<W3CResult> GetValidationResult(string html)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "BannerApi");

                var formdata = new MultipartFormDataContent();
                formdata.Add(new StringContent("json"), "out");
                formdata.Add(new StringContent(html), "content");

                var response = await client.PostAsync(BaseUrl, formdata);
                response.EnsureSuccessStatusCode();

                var jsonResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<W3CResult>(jsonResult);
            }
        }
    }
}
