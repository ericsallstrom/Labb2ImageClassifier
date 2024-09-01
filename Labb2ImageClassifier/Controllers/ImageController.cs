using Labb2ImageClassifier.Interfaces;
using Labb2ImageClassifier.Models;
using Labb2ImageClassifier.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Extensions.Options;

namespace Labb2ImageClassifier.Controllers
{
    public class ImageController : Controller
    {
        private readonly CustomVisionSettings _keys;
        private readonly CustomVisionPredictionClient _predictionClient;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ImageController> _logger;
        private readonly MushroomService _service;

        public ImageController(IOptions<CustomVisionSettings> customVisionSettings, HttpClient httpClient, ILogger<ImageController> logger, MushroomService service)
        {
            _keys = customVisionSettings.Value;
            _predictionClient = new CustomVisionPredictionClient(new ApiKeyServiceClientCredentials(_keys.PredictionKey))
            {
                Endpoint = _keys.PredictionEndpoint
            };
            _httpClient = httpClient;
            _logger = logger;   
            _service = service;
        }

        public IActionResult Index(ImageAnalysisViewModel model)
        {
            return View(model);
        }

        // Metod för att klassificera en bild via URL
        [HttpPost]
        public async Task<IActionResult> ClassifyImageUrl(string imageUrl)
        {
            var model = new ImageAnalysisViewModel();

            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                model.ErrorMessage = "Please upload a valid image URL.";
                return View("Index", model);
            }

            try
            {
                using var response = await _httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    model.ErrorMessage = "Unable to retrieve image from URL.";
                    return View("Index", model);
                }

                var contentType = response.Content.Headers.ContentType?.MediaType;
                var fileSize = response.Content.Headers.ContentLength;

                if (!IsValidImageType(contentType!))
                {
                    model.ErrorMessage = "This file type is not valid. Please provide a valid image URL.";
                    return View("Index", model);
                }

                if (!IsValidFileSize(fileSize))
                {
                    model.ErrorMessage = "This file size exceeds 4 MB. Please provide a smaller image URL.";
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                var result = await _predictionClient.ClassifyImageAsync(new Guid(_keys.ProjectId), _keys.ProjectName, stream);

                var topPrediction = result.Predictions.OrderByDescending(p => p.Probability).FirstOrDefault();
                var classificationResult = HandleClassification(topPrediction.TagName);

                if (classificationResult is Mushroom mushroom)
                {
                    model.Mushroom = mushroom;
                }
                else if (classificationResult is Other other)
                {
                    model.Other = other;
                }

                model.Predictions = result.Predictions;
                model.ImageUrl = imageUrl;

                return View("ImageAnalysis", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the image from URL.");
                model.ErrorMessage = "An error occurred while processing the image.";
                return View("Index", model);
            }
        }

        // Metod för att klassificera en uppladdad bildfil
        [HttpPost]
        public async Task<IActionResult> ClassifyImageFile(IFormFile imageFile)
        {
            var model = new ImageAnalysisViewModel();

            if (imageFile == null || imageFile.Length == 0)
            {
                model.ErrorMessage = "Please upload valid image file.";
                return View("Index", model);
            }

            if (!IsValidImageType(imageFile.ContentType))
            {
                model.ErrorMessage = "This file type is not valid. Please upload a valid image file.";
                return View("Index", model);
            }

            if (!IsValidFileSize(imageFile.Length))
            {
                model.ErrorMessage = "This file size exceeds 4 MB. Please upload a smaller image file.";
                return View("Index", model);
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var result = await _predictionClient.ClassifyImageAsync(new Guid(_keys.ProjectId), _keys.ProjectName, memoryStream);
                var topPrediction = result.Predictions.OrderByDescending(p => p.Probability).FirstOrDefault();
                var classificationResult = HandleClassification(topPrediction.TagName);
                model.Predictions = result.Predictions;

                if (classificationResult is Mushroom mushroom)
                {
                    model.Mushroom = mushroom;
                }
                else if (classificationResult is Other other)
                {
                    model.Other = other;
                }

                var base64Image = Convert.ToBase64String(memoryStream.ToArray());
                var imageDataUrl = $"data:{imageFile.ContentType};base64,{base64Image}";
                model.ImageUrl = imageDataUrl;

                return View("ImageAnalysis", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the uploaded image.");
                model.ErrorMessage = "An error occurred while processing the image.";
                return View("Index", model);
            }
        }

        public IActionResult ImageAnalysis(ImageAnalysisViewModel model)
        {
            return View(model);
        }

        /* FUNCTIONS */
        private bool IsValidImageType(string contentType)
        {
            return _keys.AllowedImageTypes.Contains(contentType);
        }

        private bool IsValidFileSize(long? fileSize)
        {
            return fileSize.HasValue && fileSize.Value <= _keys.MaxFileSizeInBytes;
        }

        private IClassificationResult HandleClassification(string tagName)
        {
            var mushroom = _service.GetMushroomByTagName(tagName);
            if (mushroom != null)
            {
                return mushroom;
            }
            else
            {
                return new Other
                {
                    TagName = tagName
                };
            }
        }
    }
}
