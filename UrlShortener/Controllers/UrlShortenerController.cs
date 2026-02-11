using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [ApiController]
    public class UrlShortenerController(ILogger<UrlShortenerController> logger, IUrlMappingService urlMappingService) : ControllerBase
    {
        [HttpGet]
        [Route("redirect")]
        [AllowAnonymous]
        [SwaggerResponse(200, "Redirects to a url based on a short url", typeof(string))]
        public IActionResult RedirectToUrl(string code)
        {
            logger.LogInformation($"Received request to for a URL with code: {code}");

            var longUrl = urlMappingService.GetUrl(code);

            if (string.IsNullOrWhiteSpace(longUrl))
                return NotFound();

            logger.LogInformation($"URL: {longUrl}");
            return Redirect(longUrl);
        }
    }
}
