using AIModelTrainer.Navigation;
using AIModelTrainer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIModelTrainer.Handler
{
    public class UserInputHandler
    {
        private readonly CustomVisionService _customVisionService;

        public UserInputHandler(CustomVisionService customVisionService)
        {
            _customVisionService = customVisionService;
        }

        public void HandleLocalFileUpload()
        {
            Console.Clear();
            Console.CursorVisible = true;

            Console.Write("Input path to the local image file: ");
            var imagePath = GetValidatedFilePath(Console.ReadLine());
            
            if (imagePath == null)
            {
                return;
            }

            Console.CursorVisible = false;
            string prompt = "What tag would you give this image?";
            string[] menuOptions = { "Champignon", "Destroying angel", "Other" };

            var menu = Menu.Instance;
            menu.Prompt = prompt;
            menu.MenuOptions = menuOptions;

            int chosenTag = menu.GetMenuChoice();
            string imageTag = menuOptions[chosenTag];
            _customVisionService.UploadAndTrainImage(imagePath, imageTag);
        }

        //public async Task HandleImageUrl()
        //{
        //    Console.Clear();
        //    Console.WriteLine("Input the image URL:");
        //    var imageUrl = Console.ReadLine();

        //    try
        //    {
        //        using var response = await _httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine($"Unable to retrieve image from URL. Status Code: {response.StatusCode}");
        //            return;
        //        }

        //        var imageBytes = await response.Content.ReadAsByteArrayAsync();
        //        var imagePath = Path.GetTempFileName();
        //        await File.WriteAllBytesAsync(imagePath, imageBytes);

        //        ProcessImage(imagePath);                
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Failed to download image. Error: {ex.Message}");                
        //    }
        //}

        private string GetValidatedFilePath(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("\nFile path cannot be empty. Press enter to return to the menu.");
                Console.ReadLine();
                return null;
            }

            string imagePath = RemoveDoubleQuotes(input);

            if (!File.Exists(imagePath))
            {
                Console.WriteLine("\nFile does not exist. Press enter to return to the menu.");
                Console.ReadLine();
                return null;
            }

            return imagePath;
        }

        private string RemoveDoubleQuotes(string imagePath)
        {
            return imagePath.Trim('"');
        }     
    }
}
