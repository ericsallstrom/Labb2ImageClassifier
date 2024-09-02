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
    public class ImageInputHandler
    {
        private readonly CustomVisionService _customVisionService;
        private readonly FileService _fileService;
        private readonly ConsoleMenu _menu;

        public ImageInputHandler(CustomVisionService customVisionService, FileService fileService, ConsoleMenu menu)
        {
            _customVisionService = customVisionService;
            _fileService = fileService;
            _menu = menu;
        }

        public async Task HandleFileUploadAsync()
        {
            Console.Clear();
            Console.CursorVisible = true;

            Console.WriteLine("Input path to the local image file:");
            string imagePath = _fileService.GetValidatedFilePath(Console.ReadLine());

            if (imagePath == null)
            {
                return;
            }

            if (!File.Exists(imagePath))
            {
                Console.Write("\nFile does not exist. Press enter to return to the menu.");
                Console.ReadLine();
                return;
            }

            string imageTag = GetTagChoice();
            await _customVisionService.UploadAndTrainImageAsync(imagePath, imageTag);
        }

        //public async Task HandleFolderUploadAsync()
        //{
        //    Console.Clear();
        //    Console.CursorVisible = true;

        //    Console.WriteLine("Input path to the folder containg images:");
        //    string folderPath = _fileService.GetValidatedFilePath(Console.ReadLine());

        //    if (folderPath == null)
        //    {
        //        return;
        //    }

        //    if (!Directory.Exists(folderPath))
        //    {
        //        Console.Write("\nFolder does not exist. Press enter to return to the menu.");
        //        Console.ReadLine();
        //        return;
        //    }

        //    string imageTag = GetTagChoice();

        //    var imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
        //        .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
        //            || file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
        //            || file.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
        //            || file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
        //        .ToList();

        //    if (imageFiles.Count == 0)
        //    {
        //        Console.WriteLine("No images found in the specified folder. Press enter to return to the menu.");
        //        Console.ReadLine();
        //        return;
        //    }

        //    await _customVisionService.UploadAndTrainImageAsync(imageFiles, imageTag);
        //}

        public async Task HandleImageUrlAsync()
        {
            Console.Clear();
            Console.CursorVisible = true;

            Console.WriteLine("Input image url:");
            string imageUrl = _fileService.GetValidatedFilePath(Console.ReadLine());

            if (imageUrl == null)
            {
                return;
            }

            try
            {
                string imagePath = await _fileService.DownloadImageFromUrlAsync(imageUrl);

                if (imagePath == null)
                {
                    Console.Write($"\nPress enter to return to the menu.");
                    Console.ReadLine();
                    return;
                }

                string imageTag = GetTagChoice();
                await _customVisionService.UploadAndTrainImageAsync(imagePath, imageTag);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred. Error: {ex.Message}\nPress enter to return to the menu.");
                Console.ReadLine();
            }
        }

        private string GetTagChoice()
        {
            Console.CursorVisible = false;
            string prompt = "What tag would you give this image?";
            string[] menuOptions = { "Champignon", "Destroying angel", "Other" };

            _menu.Prompt = prompt;
            _menu.MenuOptions = menuOptions;

            int chosenTagIndex = _menu.GetMenuChoice();
            return menuOptions[chosenTagIndex];
        }
    }
}
