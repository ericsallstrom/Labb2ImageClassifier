using AIModelTrainer.Handler;
using AIModelTrainer.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIModelTrainer.Application
{
    public class ModelTrainerApp
    {
        private readonly ImageInputHandler _handler;
        private readonly ConsoleMenu _menu;

        private int SelectedMenuOption { get; set; }

        public ModelTrainerApp(ImageInputHandler handler, ConsoleMenu menu)
        {
            _handler = handler;
            _menu = menu;
        }

        public async Task StartTrainingAsync()
        {
            Console.CursorVisible = false;

            while (true)
            {
                RunMainMenu();

                switch (SelectedMenuOption)
                {
                    case 0:
                        await _handler.HandleFileUploadAsync();
                        break;
                    case 1:
                        //await _handler.HandleFolderUploadAsync();
                        break;
                    case 2:
                        await _handler.HandleImageUrlAsync();
                        break;
                    case 3:
                        Console.WriteLine("\n\nThe model trainer application is shutting down...");
                        Thread.Sleep(2000);
                        return;
                }
            }
        }

        private void RunMainMenu()
        {
            string prompt = "Welcome to the AI trainer model for the Mushroom Classifier project." +
                "\nNavigate using the arrow keys and press enter to make your selection.";

            string[] menuOptions = { "Upload local file", "Upload folder of images (UNDER CONSTRUCTION)", "Enter image URL", "Exit" };

            DisplayMenuOptions(prompt, menuOptions);
        }

        private void DisplayMenuOptions(string prompt, string[] menuOptions)
        {
            _menu.MenuOptions = menuOptions;
            _menu.Prompt = prompt;
            SelectedMenuOption = _menu.GetMenuChoice();
        }
    }
}
