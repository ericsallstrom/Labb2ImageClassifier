using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIModelTrainer.Navigation
{
    public class Menu
    {
        private static readonly Lazy<Menu> _instance = new(() => new Menu());

        private const ConsoleKey keyUp = ConsoleKey.UpArrow;
        private const ConsoleKey keyDown = ConsoleKey.DownArrow;        
        private const ConsoleKey keySelect = ConsoleKey.Enter;

        public string Prompt { get; set; }
        public string[] MenuOptions { get; set; }
        private ConsoleKeyInfo KeyInfo { get; set; }
        private ConsoleKey KeyPressed { get; set; }
        private int SelectedOption { get; set; }

        private Menu() 
        {
            Prompt = string.Empty;
            MenuOptions = Array.Empty<string>();
            SelectedOption = 0;
        }

        public static Menu Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public int GetMenuChoice()
        {
            do
            {
                DisplayMenu();

                KeyInfo = Console.ReadKey();
                KeyPressed = KeyInfo.Key;

                switch(KeyPressed)
                {
                    case keyUp:
                        SelectedOption = (SelectedOption - 1 + MenuOptions.Length) % MenuOptions.Length;
                        break;
                    case keyDown:
                        SelectedOption = (SelectedOption + 1 + MenuOptions.Length) % MenuOptions.Length;
                        break;
                }
            } while (KeyPressed != keySelect);

            return SelectedOption;
        }

        private void DisplayMenu()
        {
            Console.Clear();
            Console.CursorVisible = false;
            StringBuilder menuBuilder = new();
            menuBuilder.AppendLine(Prompt);

            Console.WriteLine(menuBuilder);

            for (int i = 0; i < MenuOptions.Length; i++)
            {
                string currentOption = MenuOptions[i];
                OptionColor(currentOption, i);
            }

            Console.ResetColor();
        }

        private void OptionColor(string currentOption, int selectedOption)
        {
            string prefix;

            if (SelectedOption == selectedOption)
            {
                prefix = ">";
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                prefix = " ";
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.WriteLine($"{prefix} {currentOption}");
        }
    }
}
