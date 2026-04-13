using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CyberBot.Classes
{
    internal class UIManager
    {
        internal static void DisplayDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n" + new string('─', 60));
            Console.ResetColor();
        }

        internal void DisplayHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔──────────────────────────────────────╗
           CYBERSECURITY BOT                      
|Your Personal Online Safety Guard     |                                           
╚──────────────────────────────────────╝
            ");
            Console.ResetColor();
            Thread.Sleep(1000);
        }

        internal string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        internal string GetUserName()
        {
            DisplayDivider();
            TypeText("May I have your name? ", ConsoleColor.Yellow);

            string? name = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(name))
            {
                TypeText("Please enter a valid name: ", ConsoleColor.Red);
                name = Console.ReadLine();
            }

            return name.Trim();
        }

        internal void TypeText(string message, ConsoleColor color, int delay = 40)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.ResetColor();
        }

        internal void TypeText(string message, ConsoleColor color)
        {
            TypeText(message, color, 40); // Call the main method with default delay
        }
    }
}

namespace CybersecurityBot
{
    [SupportedOSPlatform("windows")]
    public class UIManager
    {
        public void DisplayHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
╔──────────────────────────────────────╗
           CYBERSECURITY BOT                      
|Your Personal Online Safety Guard     |                                                 
╚──────────────────────────────────────╝
            ");
            Console.ResetColor();
            Thread.Sleep(1000);
        }

        public string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        public string GetUserName()
        {
            DisplayDivider();
            TypeText("May I have your name? ", ConsoleColor.Yellow);

            string? name = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(name))
            {
                TypeText("Please enter a valid name: ", ConsoleColor.Red);
                name = Console.ReadLine();
            }

            return name.Trim();
        }

        public void TypeText(string message, ConsoleColor color, int delay = 40)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.ResetColor();
        }

        public void DisplayDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n" + new string('─', 60));
            Console.ResetColor();
        }

        public void DisplaySectionHeader(string title)
        {
            DisplayDivider();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"  {title}");
            Console.ResetColor();
            DisplayDivider();
        }
    }
}