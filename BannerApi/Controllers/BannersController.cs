using BannerApi.Service.Interfaces;
using BannerApi.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace BannerApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Banners")]
    public class BannersController : Controller
    {
        private readonly IBannerService _service;

        public BannersController(IBannerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var banners = await _service.GetAll();
            return Ok(banners);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var banner = await _service.Get(id);
            if(banner == null)
            {
                return NotFound();
            }

            return Ok(banner);
        }

        [HttpGet("{id}/html")]
        [Produces("text/html")]
        public async Task<ContentResult> Html(int id)
        {
            var html = await _service.GetHtml(id);
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateBannerModel banner)
        {
            if(banner == null)
            {
                return BadRequest();
            }

            var result = await _service.Add(banner);
            if(!result.Sucessful)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]UpdateBannerModel banner)
        {
            if(banner == null)
            {
                return BadRequest();
            }

            var result = await _service.Update(banner);
            if(!result.Sucessful)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _service.Remove(id);
        }
    }
}