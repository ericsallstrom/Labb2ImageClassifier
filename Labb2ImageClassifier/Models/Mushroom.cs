using Labb2ImageClassifier.Interfaces;

namespace Labb2ImageClassifier.Models
{
    public class Mushroom : IClassificationResult
    {
        public string Name { get; set; }
        public string LatinName { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
        public bool IsPoisonous { get; set; }   
        public bool IsEdible => !IsPoisonous;
    }
}
