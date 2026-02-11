using System.Collections.Concurrent;

namespace UrlShortener.Services;

public interface IUrlMappingService
{
    string? GetUrl(string code);
}

public class UrlMappingService : IUrlMappingService
{
    private readonly ConcurrentDictionary<string, string> _mappings;
    private readonly string _filePath;

    public UrlMappingService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "urls.txt");
        _mappings = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        LoadFromFile();
    }

    public string? GetUrl(string code)
        => _mappings.GetValueOrDefault(code);

    private void LoadFromFile()
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"Mapping file not found: {_filePath}");

        foreach (var line in File.ReadAllLines(_filePath))
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