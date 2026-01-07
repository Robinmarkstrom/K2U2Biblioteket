using Biblioteket.LibraryClient.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteket.LibraryClient.Core
{
    public static class MainMenu
    {
        public static void ShowMainMenu(AdminUser user)
        {
            bool running = true;

            while (running)
            {
                ConsoleHelper.ResetScreen();
                Console.WriteLine($"Inloggad som: {user.Name} ({(user.IsAdmin ? "Admin" : "User")})\n");

                ConsoleHelper.WriteMenuOption("1", "Registrera nya böcker");
                ConsoleHelper.WriteMenuOption("2", "Registrera nya medlemmar");
                ConsoleHelper.WriteMenuOption("3", "Registrera lån");
                ConsoleHelper.WriteMenuOption("4", "Registrera återlämningar");
                ConsoleHelper.WriteMenuOption("5", "Visa alla aktiva lån");
                ConsoleHelper.WriteMenuOption("6", "Sök efter böcker");
                ConsoleHelper.WriteMenuOption("0", "Avsluta");
                Console.WriteLine();

                Console.Write("Välj alternativ: ");
                var key = Console.ReadKey(true).KeyChar;

                switch (key)
                {
                    case '1': LibraryManager.RegisterBook(); break;
                    case '2': LibraryManager.RegisterMember(); break;
                    case '3': LibraryManager.RegisterLoan(); break;
                    case '4': LibraryManager.RegisterReturn(); break;
                    case '5': LibraryManager.ListAllActiveLoans(); break;
                    case '6': LibraryManager.SearchBooks(); break;
                    case '0': running = false; break;
                    default: break;
                }
            }
        }
    }
}
