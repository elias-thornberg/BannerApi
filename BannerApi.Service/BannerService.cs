using BannerApi.Core.Interfaces;
using BannerApi.Core.Models;
using BannerApi.Service.Interfaces;
using BannerApi.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BannerApi.Service
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _repository;
        private readonly IHtmlValidator _validator;

        public BannerService(IHtmlValidator validator, IBannerRepository repository)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<SaveBannerResult> Add(CreateBannerModel banner)
        {
            var validationResult = await _validator.Validate(banner.Html);
            if(!validationResult.IsValid)
            {
                return ValidationFailed(validationResult);
            }

            var newBanner = new Banner
            {
                Html = banner.Html,
                Created = DateTime.Now
            };

            await _repository.Add(newBanner);

            return SaveSucessful(newBanner);
        }

        public async Task<Banner> Get(int id)
        {
            return await _repository.Get(id);
        }

        public async Task<IEnumerable<Banner>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<string> GetHtml(int id)
        {
            var banner = await _repository.Get(id);
            return banner.Html;
        }

        public async Task Remove(int id)
        {
            await _repository.Remove(id);
        }

        public async Task<SaveBannerResult> Update(UpdateBannerModel banner)
        {
            var validationResult = await _validator.Validate(banner.Html);
            if(!validationResult.IsValid)
            {
                return ValidationFailed(validationResult);
            }

            var bannerToUpdate = await _repository.Get(banner.Id);
            if(bannerToUpdate == null)
            {
                return BannerNotFound();
            }

            bannerToUpdate.Html = banner.Html;
            bannerToUpdate.Modified = DateTime.Now;

            await _repository.Update(bannerToUpdate);

            return SaveSucessful(bannerToUpdate);
        }

        private SaveBannerResult ValidationFailed(ValidationResult result)
        {
            return new SaveBannerResult
            {
                Sucessful = false,
                Errors = result.Errors
            };
        }

        private SaveBannerResult SaveSucessful(Banner banner)
        {
            return new SaveBannerResult
            {
                Id = banner.Id,
                Sucessful = true
            };
        }

        private SaveBannerResult BannerNotFound()
        {
            return new SaveBannerResult
            {
                Sucessful = false,
                Errors = new List<string> { "Banner not found" }
            };
        }
    }
}
