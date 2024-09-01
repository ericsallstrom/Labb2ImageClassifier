using Labb2ImageClassifier.Interfaces;
using System.Runtime.CompilerServices;

namespace Labb2ImageClassifier.Models
{
    public class Other : IClassificationResult
    {
        public string TagName { get; set; }
        public string Description { get; set; } = "We could not classify this image as a mushroom. The object in the image might not be a mushroom or the model is unsure.";        
    }
}
