using AIModelTrainer.MushroomCustomVision;
using AIModelTrainer.Navigation;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIModelTrainer.Services
{
    public class CustomVisionService
    {
        private readonly CustomVisionTrainingClient _trainingClient;
        private readonly Guid _projectId;

        public CustomVisionService(Keys keys)
        {
            _trainingClient = AuthenticateTraining(keys.TrainingEndpoint, keys.TrainingKey);
            _projectId = new Guid(keys.ProjectId);
        }

        private CustomVisionTrainingClient AuthenticateTraining(string endpoint, string key)
        {
            return new CustomVisionTrainingClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        public Project GetProject()
        {
            return _trainingClient.GetProject(_projectId);
        }

        public IList<Tag> GetTags()
        {
            return _trainingClient.GetTags(_projectId);
        }

        public async Task UploadAndTrainImageAsync(string imagePath, string imageTag)
        {
            var project = GetProject();
            if (project == null)
            {
                ShowErrorAndReturnToMenu("Project not found.");                                
                return;
            }

            var tags = GetTags();
            var matchedTag = tags.FirstOrDefault(t => t.Name.Equals(imageTag, StringComparison.OrdinalIgnoreCase));
            if (matchedTag == null)
            {
                ShowErrorAndReturnToMenu($"Tag '{imageTag}' not found.");                                
                return;
            }

            using (var imageFile = File.OpenRead(imagePath))
            {
                var memoryStream = new MemoryStream();
                imageFile.CopyTo(memoryStream);

                var fileCreateEntry = new ImageFileCreateEntry(imageFile.Name, memoryStream.ToArray());
                var fileCreateBatch = new ImageFileCreateBatch { Images = new List<ImageFileCreateEntry> { fileCreateEntry }, TagIds = new List<Guid> { matchedTag.Id } };

                try
                {
                    var result = await _trainingClient.CreateImagesFromFilesAsync(_projectId, fileCreateBatch);
                    var resultImage = result.Images.FirstOrDefault();

                    if (resultImage.Status == "OKDuplicate")
                    {
                        ShowErrorAndReturnToMenu("Image is already used for training. Please use another to train with.");                                                
                        return;
                    }

                    await TrainModelAsync();
                }
                catch (CustomVisionErrorException ex)
                {
                    Console.WriteLine($"Error: {ex.Body.Message}");
                    Console.WriteLine($"Status Code: {ex.Response.StatusCode}");                    
                }
            }
        }

        private async Task TrainModelAsync()
        {
            var iteration = _trainingClient.TrainProject(_projectId);
            Console.WriteLine("\nTraining model... please be patient.");

            while (iteration.Status != "Completed")
            {
                Thread.Sleep(500);
                iteration = await _trainingClient.GetIterationAsync(_projectId, iteration.Id);
            }

            await _trainingClient.UpdateIterationAsync(_projectId, iteration.Id, iteration);
            Console.WriteLine("\nTraining completed!");
            GetBackToMainMenu();
        }

        private void GetBackToMainMenu()
        {
            Console.WriteLine("Press enter to return back to main menu.");
            Console.ReadLine();
        }

        private void ShowErrorAndReturnToMenu(string message)
        {
            Console.WriteLine($"\n{message}");
            GetBackToMainMenu();
        }
    }
}
