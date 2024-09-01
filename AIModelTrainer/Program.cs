using AIModelTrainer.MushroomCustomVision;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AIModelTrainer.Application;
using AIModelTrainer.Services;
using AIModelTrainer.Handler;

namespace AIModelTrainer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configurationService = new ConfigurationService();
            var keys = configurationService.GetApiKeys();

            var customVisionService = new CustomVisionService(keys);
            var userInputHandler = new UserInputHandler(customVisionService);

            var app = new TrainerApp(userInputHandler);
            app.RunTrainer();

            //var keys = GetApiKeys();

            //TrainerApp app = new TrainerApp();
            //app.RunTrainer();

            //var trainingApi = AuthenticateTraining(keys.TrainingEndpoint, keys.TrainingKey);

            //var project = trainingApi.GetProject(new Guid(keys.ProjectId));
            //if (project == null)
            //{
            //    Console.WriteLine("Project not found. Exiting.");
            //    return;
            //}

            //Console.WriteLine("Input path to image to train model with:");
            //var imagePath = Console.ReadLine();

            //Console.WriteLine("What tag would you give this image? Champignon, Destroying angel or Other?");
            //var imageTag = Console.ReadLine();

            //var capitilizedTag = char.ToUpper(imageTag.First()) + imageTag.Substring(1).ToLower();

            //if (!File.Exists(imagePath))
            //{
            //    Console.WriteLine("File does not exist. Press enter to exit.");
            //    Console.ReadLine();
            //    return;
            //}

            //var imageFile = File.OpenRead(imagePath);
            //var tags = trainingApi.GetTags(project.Id);
            //var matchedTag = tags.FirstOrDefault(t => t.Name == capitilizedTag);

            //if (matchedTag == null)
            //{
            //    Console.WriteLine($"Tag '{capitilizedTag}' not found.");
            //    return;
            //}

            //var memoryStream = new MemoryStream();
            //imageFile.CopyTo(memoryStream);

            //var fileCreateEntry = new ImageFileCreateEntry(imageFile.Name, memoryStream.ToArray());
            //var fileCreateBatch = new ImageFileCreateBatch { Images = new List<ImageFileCreateEntry> { fileCreateEntry }, TagIds = new List<Guid> { matchedTag.Id } };

            //try
            //{
            //    var result = trainingApi.CreateImagesFromFiles(project.Id, fileCreateBatch);

            //    var resultImage = result.Images.FirstOrDefault();

            //    switch (resultImage.Status)
            //    {
            //        case "OKDuplicate":
            //            Console.WriteLine("Image is already used for training. Please use another to train with.");
            //            Console.ReadLine();
            //            break;
            //        default:
            //            break;
            //    }

            //    var iteration = trainingApi.TrainProject(project.Id);
            //    Console.WriteLine("Training model...");

            //    while (iteration.Status != "Completed")
            //    {
            //        Thread.Sleep(1000);
            //        iteration = trainingApi.GetIteration(project.Id, iteration.Id);
            //    }

            //    trainingApi.UpdateIteration(project.Id, iteration.Id, iteration);
            //    Console.WriteLine("Done training!");
            //}
            //catch (CustomVisionErrorException ex)
            //{
            //    Console.WriteLine($"Error: {ex.Body.Message}");
            //    Console.WriteLine($"Status Code: {ex.Response.StatusCode}");
            //    return;
            //}
        }

        //private static CustomVisionTrainingClient AuthenticateTraining(string endpoint, string trainingKey)
        //{
        //    var trainingApi = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(trainingKey))
        //    {
        //        Endpoint = endpoint
        //    };
        //    return trainingApi;
        //}
    }
}
