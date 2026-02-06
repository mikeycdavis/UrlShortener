using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlShortenerController(ILogger<UrlShortenerController> logger) : ControllerBase
    {
        private readonly ILogger<UrlShortenerController> _logger = logger;

        [HttpPost(Name = "GetShortenedUrl")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Shortened URL from a long URL", typeof(string))]
        public async Task<IActionResult> GetShortenedUrl([FromBody] string url)
        {
            _logger.LogInformation("Received request to shorten URL: {Url}", url);

            var shortenedUrl = url;

            _logger.LogInformation("Shortened URL: {ShortenedUrl}", shortenedUrl);
            return new OkObjectResult(shortenedUrl);
        }
    }
}
