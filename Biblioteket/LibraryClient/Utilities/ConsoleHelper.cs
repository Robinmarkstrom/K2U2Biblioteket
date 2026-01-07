using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteket.LibraryClient.Utilities
{
    public class ConsoleHelper
    {
        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"🗸 {message}");
            Console.ResetColor();
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ {message}");
            Console.ResetColor();
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"▲ {message}");
            Console.ResetColor();
        }
        public static void WriteInfo(string message) // 
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"ⓘ {message}");
            Console.ResetColor();
        }

        public static void ReadKeyMessage(string message = "Press any key to continue!") // Wait for action from the user to continue with custom colored message
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\n{message}");
            Console.ResetColor();
            Console.ReadKey();
        }

        public static void ResetScreen() // Clear console and resets the color to default
        {
            Console.Clear();
            Console.ResetColor();
        }

        public static string EscapeToMenu(string message, string escMessage = "Du har tryckt ESC - återvänder till menyn!")
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{message}: ");
            Console.ResetColor();

            var inputFromUser = new StringBuilder();

            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return inputFromUser.ToString().Trim();
                }

                else if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(escMessage);
                    Console.ResetColor();
                    return "<ESC>";
                }

                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (inputFromUser.Length > 0)
                    {
                        inputFromUser.Length--;
                        Console.Write("\b \b");
                    }
                }

                else if (!char.IsControl(key.KeyChar))
                {
                    inputFromUser.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }

        public static string EscapeToMenuMasked(string message, string escMessage = "Du har tryckt ESC - återvänder till menyn!")
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{message}: ");
            Console.ResetColor();

            var maskedInputFromUser = new StringBuilder();

            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return maskedInputFromUser.ToString().Trim();
                }

                else if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(escMessage);
                    Console.ResetColor();
                    return "<ESC>";
                }

                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (maskedInputFromUser.Length > 0)
                    {
                        maskedInputFromUser.Length--;
                        Console.Write("\b \b");
                    }
                }

                else if (!char.IsControl(key.KeyChar))
                {
                    maskedInputFromUser.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
        }
        public static void WriteMenuOption(string number, string description)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{number}. ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(description);
            Console.ResetColor();
        }

        public static void CrumbBar(params string[] path)
        {
            Console.Clear();

            var breadcrumb = new StringBuilder();
            for (int i = 0; i < path.Length; i++)
            {
                if (i < path.Length - 1)
                {
                    breadcrumb.Append(path[i].ToUpper());
                    breadcrumb.Append("  >  ");
                }
                else
                {
                    breadcrumb.Append(path[i].ToUpper());
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(breadcrumb.ToString());

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', breadcrumb.Length));

            Console.ResetColor();
        }

        public static void SectionHeader(string title)
        {
            Console.WriteLine(title);
            Console.WriteLine();
        }

        public static void PrintTable(string[] headers, List<string[]> rows, Func<string[], ConsoleColor>? getRowColor = null)
        {
            int[] colWidths = new int[headers.Length];

            for (int i = 0; i < headers.Length; i++)
            {
                int maxWidth = headers[i].Length;
                foreach (var row in rows)
                {
                    if (i < row.Length && row[i] != null)
                        maxWidth = Math.Max(maxWidth, row[i].Length);
                }
                colWidths[i] = maxWidth;
            }

            string format = "";
            for (int i = 0; i < headers.Length; i++)
            {
                format += $"{{{i},-{colWidths[i]}}}";
                if (i < headers.Length - 1) format += " | ";
            }

            Console.WriteLine(format, headers);
            Console.WriteLine(new string('-', colWidths.Sum() + 3 * (headers.Length - 1)));

            foreach (var row in rows)
            {
                if (getRowColor != null)
                    Console.ForegroundColor = getRowColor(row);

                Console.WriteLine(format, row);
                Console.ResetColor();
            }

            Console.WriteLine(new string('-', colWidths.Sum() + 3 * (headers.Length - 1)));
        }
    }
}