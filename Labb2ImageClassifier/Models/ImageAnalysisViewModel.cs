using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace Labb2ImageClassifier.Models
{
    public class ImageAnalysisViewModel
    {
        public string ImageUrl { get; set; }
        public IList<PredictionModel> Predictions { get; set; }
        public string ErrorMessage { get; set; }
        public Mushroom Mushroom { get; set; }
        public Other Other { get; set; }
    }
}
