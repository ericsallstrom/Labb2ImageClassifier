using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIModelTrainer.MushroomCustomVision
{
    public class Keys
    {
        public string TrainingKey { get; set; } = string.Empty;
        public string TrainingEndpoint { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public List<string> AllowedImageTypes { get; set; } = [];
        public long MaxFileSizeInBytes { get; set; }
    }
}
