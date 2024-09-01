namespace Labb2ImageClassifier.Models
{
    public class CustomVisionSettings
    {
        public required string PredictionKey { get; set; }
        public required string PredictionEndpoint { get; set; }
        public required string ProjectId { get; set; }
        public required string ProjectName { get; set; }
        public List<string> AllowedImageTypes { get; set; } = [];
        public long MaxFileSizeInBytes { get; set; }    
    }
}
