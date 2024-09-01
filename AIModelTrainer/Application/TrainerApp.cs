using AIModelTrainer.Handler;
using AIModelTrainer.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIModelTrainer.Application
{
    public class TrainerApp
    {
        private readonly UserInputHandler _handler;        
        private int MenuChoice { get; set; }

        public TrainerApp(UserInputHandler handler)
        {
            _handler = handler;
        }

        public void RunTrainer()
        {
            Console.CursorVisible = false;

            while (true)
            {
                RunMainMenu();

                switch (MenuChoice)
                {
                    case 0:
                        _handler.HandleLocalFileUpload();
                        break;
                    case 1:
                        //await _handler.HandleImageUrl();
                        break;
                    case 2:
                        Console.WriteLine("\n\nThe Trainer application is shutting down...");
                        Thread.Sleep(2000);
                        return;
                }
            }
        }        

        private void RunMainMenu()
        {
            string prompt = "Welcome to the AI trainer model for the Mushroom Classifier project." +
                "\nNavigate using the arrow keys and press Enter to make your selection.";

            string[] menuOptions =
            {
                "Upload local file",
                "Enter image URL",
                "Exit"
            };

            GetMenuOptions(prompt, menuOptions);
        }

        private void GetMenuOptions(string prompt, string[] menuOptions)
        {
            var menu = Menu.Instance;

            menu.MenuOptions = menuOptions;
            menu.Prompt = prompt;

            MenuChoice = menu.GetMenuChoice();
        }
    }
}
