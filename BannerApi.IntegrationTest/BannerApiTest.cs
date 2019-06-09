using BannerApi.Core.Models;
using BannerApi.Service.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BannerApi.IntegrationTest
{
    public class BannerApiTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public BannerApiTest(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task AddNewBanner_WithInvalidHtml_ShouldReturnBadRequestsWithDetails()
        {
            var banner = new CreateBannerModel();
            banner.Html = "ABC123";

            var httpResponse = await _client.PostAsJsonAsync("/api/banners", banner);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SaveBannerResult>(stringResponse);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.False(result.Sucessful);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public async Task AddNewBanner_WithValidHtml_ShouldReturnOkWithNewId()
        {
            var banner = new CreateBannerModel();
            banner.Html = ValidHtml;

            var httpResponse = await _client.PostAsJsonAsync("/api/banners", banner);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SaveBannerResult>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.Empty(result.Errors);
            Assert.True(result.Sucessful);
        }

        [Fact]
        public async Task UpdateBanner_WithInvalidHtml_ShouldReturnBadRequestsWithDetails()
        {
            var newBanner = await CreateBannerTestData();

            var banner = new UpdateBannerModel();
            banner.Id = newBanner.Id.Value;
            banner.Html = "ABC123";

            var httpResponse = await _client.PutAsJsonAsync("/api/banners/" + newBanner.Id, banner);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SaveBannerResult>(stringResponse);

            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
            Assert.False(result.Sucessful);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public async Task UpdateBanner_WithValidHtml_ShouldReturnOkWithNewId()
        {
            var newBanner = await CreateBannerTestData();

            var banner = new UpdateBannerModel();
            banner.Id = newBanner.Id.Value;
            banner.Html = ValidHtml;

            var httpResponse = await _client.PutAsJsonAsync("/api/banners/" + newBanner.Id, banner);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SaveBannerResult>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.Empty(result.Errors);
            Assert.True(result.Sucessful);
        }

        [Fact]
        public async Task GetBanner_WithIdThatExists_ShouldReturnOkWithBannerModel()
        {
            var newBanner = await CreateBannerTestData();

            var httpResponse = await _client.GetAsync("/api/banners/" + newBanner.Id);

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Banner>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.Equal(newBanner.Id, result.Id);
            Assert.Equal(ValidHtml, result.Html);
        }

        [Fact]
        public async Task GetBanner_WithIdThatDoesNotExist_ShouldReturnNotFound()
        {
            var httpResponse = await _client.GetAsync("/api/banners/55");

            Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        [Fact]
        public async Task GetHtml_ShouldReturnContentTypeHtml()
        {
            var newBanner = await CreateBannerTestData();

            var httpResponse = await _client.GetAsync($"/api/banners/{newBanner.Id}/html");
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();

            Assert.Equal("text/html", httpResponse.Content.Headers.ContentType.MediaType);
            Assert.Equal(ValidHtml, stringResponse);
        }

        [Fact]
        public async Task Delete_ShouldDeleteBanner_WithId()
        {
            var newBanner = await CreateBannerTestData();
            var bannersBeforeDelete = await GetBanners();

            await _client.DeleteAsync("/api/banners/" + newBanner.Id);

            var bannersAfterDelete = await GetBanners();

            Assert.NotEqual(bannersBeforeDelete.Count, bannersAfterDelete.Count);
            Assert.Contains(bannersBeforeDelete, x => x.Id == newBanner.Id);
            Assert.DoesNotContain(bannersAfterDelete, x => x.Id == newBanner.Id);
        }

        private async Task<List<Banner>> GetBanners()
        {
            var response = await _client.GetAsync("/api/banners");
            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Banner>>(stringResponse);
        }

        private async Task<SaveBannerResult> CreateBannerTestData()
        {
            var banner = new CreateBannerModel();
            banner.Html = ValidHtml;

            var response = await _client.PostAsJsonAsync("/api/banners", banner);
            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SaveBannerResult>(stringResponse);
        }

        public string ValidHtml => @"
        <!doctype html>
            <html lang =""en"">
            <head>
                <title>Testpage</title>
            </head>
            <body>
                <p>A paragraph</p>
            </body>
        </html>";
    }
}
