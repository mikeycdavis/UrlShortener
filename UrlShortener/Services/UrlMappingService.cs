using System.Collections.Concurrent;

namespace UrlShortener.Services;

public interface IUrlMappingService
{
    Task<string?> GetUrlAsync(string code);
}

public class UrlMappingService(IWebHostEnvironment env) : IUrlMappingService
{
    private readonly ConcurrentDictionary<string, string> _mappings = new(StringComparer.OrdinalIgnoreCase);
    private readonly string _filePath = Path.Combine(env.ContentRootPath, "urls.txt");

    public async Task<string?> GetUrlAsync(string code)
    {
        await LoadFromFileAsync();
        return _mappings.GetValueOrDefault(code);
    }

    private async Task LoadFromFileAsync()
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"Mapping file not found: {_filePath}");

        foreach (var line in await File.ReadAllLinesAsync(_filePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            var longUrl = parts[0];
            var shortCode = parts[1];

            _mappings[shortCode] = longUrl;
        }
    }
}