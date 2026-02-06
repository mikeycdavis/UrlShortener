using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlShortenerController(ILogger<UrlShortenerController> logger, IUrlMappingService urlMappingService) : ControllerBase
    {
        [HttpPost]
        [Route("shorten-url")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Shortened URL from a long URL", typeof(string))]
        public async Task<IActionResult> GetShortenedUrl([FromBody] string url)
        {
            logger.LogInformation("Received request to shorten URL: {Url}", url);

            var shortenedUrl = await urlMappingService.GetShortUrlAsync(url);

            if (string.IsNullOrWhiteSpace(shortenedUrl))
                return NotFound();

            logger.LogInformation("Shortened URL: {ShortenedUrl}", shortenedUrl);
            return new OkObjectResult(shortenedUrl);
        }
    }
}
