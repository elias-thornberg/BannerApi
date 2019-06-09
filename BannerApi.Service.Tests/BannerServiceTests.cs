using BannerApi.Core.Interfaces;
using BannerApi.Service.Interfaces;
using BannerApi.Service.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BannerApi.Service.Tests
{
    public class BannerServiceTests
    {
        private readonly Mock<IHtmlValidator> _htmlValidator;
        private readonly Mock<IBannerRepository> _repository;

        public BannerServiceTests()
        {
            _htmlValidator = new Mock<IHtmlValidator>();
            _repository = new Mock<IBannerRepository>();
        }

        [Fact]
        public async Task AddBanner_WithInvalidHtml_ShouldReturnErrorWithDescription()
        {
            var validationFailed = new ValidationResult(false, new List<string> { "Error" });
            _htmlValidator.Setup(x => x.Validate(It.IsAny<string>())).Returns(Task.FromResult(validationFailed));

            var bannerService = new BannerService(_htmlValidator.Object, _repository.Object);
            var bannerToAdd = new CreateBannerModel
            {
                Html = "ABC123"
            };

            var result = await bannerService.Add(bannerToAdd);

            Assert.False(result.Sucessful);
            Assert.NotEmpty(result.Errors);
            Assert.Contains("Error", result.Errors);
        }

        [Fact]
        public async Task UpdateBanner_WithInvalidHtml_ShouldReturnErrorWithDescription()
        {
            var validationFailed = new ValidationResult(false, new List<string> { "Error" });
            _htmlValidator.Setup(x => x.Validate(It.IsAny<string>())).Returns(Task.FromResult(validationFailed));

            var bannerService = new BannerService(_htmlValidator.Object, _repository.Object);
            var bannerToUpdate = new UpdateBannerModel
            {
                Id = 1,
                Html = "ABC123"
            };

            var result = await bannerService.Update(bannerToUpdate);

            Assert.False(result.Sucessful);
            Assert.NotEmpty(result.Errors);
            Assert.Contains("Error", result.Errors);
        }

        [Fact]
        public async Task UpdateBanner_WithWrongId_ShouldReturnErrorWithDescription()
        {
            var validationSuccesful = new ValidationResult(true);
            _htmlValidator.Setup(x => x.Validate(It.IsAny<string>())).Returns(Task.FromResult(validationSuccesful));

            var bannerService = new BannerService(_htmlValidator.Object, _repository.Object);
            var bannerToUpdate = new UpdateBannerModel
            {
                Id = 1,
                Html = "ABC123"
            };

            var result = await bannerService.Update(bannerToUpdate);

            Assert.False(result.Sucessful);
            Assert.NotEmpty(result.Errors);
            Assert.Contains("Banner not found", result.Errors);
        }
    }
}
