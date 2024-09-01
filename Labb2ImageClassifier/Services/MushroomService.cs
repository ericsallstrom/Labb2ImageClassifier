using Labb2ImageClassifier.Models;
using System.Text.Json;

namespace Labb2ImageClassifier.Services
{
    public class MushroomService
    {
        private readonly List<Mushroom> _mushrooms;

        public MushroomService(IWebHostEnvironment env)
        {
            var filePath = Path.Combine(env.ContentRootPath, "wwwroot/data/mushrooms.json");
            var json = File.ReadAllText(filePath);
            _mushrooms = JsonSerializer.Deserialize<List<Mushroom>>(json);
        }

        public Mushroom GetMushroomByTagName(string tagName)
        {
            return _mushrooms.FirstOrDefault(m => m.TagName.Equals(tagName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
