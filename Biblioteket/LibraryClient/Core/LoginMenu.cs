using Biblioteket.LibraryClient.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteket.LibraryClient.Core
{
    public static class LoginMenu
    {
        public static void Show(ref AdminUser? currentUser)
        {
            ConsoleHelper.ResetScreen();
            ShowLibraryLoginHeader();

            string personnummer = ConsoleHelper.EscapeToMenu("Personnummer");
            if (personnummer == "<ESC>") return;

            string pin = ConsoleHelper.EscapeToMenuMasked("PIN");
            if (pin == "<ESC>") return;

            currentUser = AuthenticatorService.Login(personnummer, pin);

            if (currentUser != null)
                ConsoleHelper.WriteSuccess("Inloggning lyckades");
            else
            {
                ConsoleHelper.WriteError("Fel personnummer eller PIN");
                ConsoleHelper.ReadKeyMessage();
            }
        }

        private static void ShowLibraryLoginHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
|\\\\ ////|
| \\\V/// |
|  |===|  |
|  |===|  |
|  |===|  |
|  |===|  |
 \ |===| /
  \|===|/
   '---'
");

            Console.ForegroundColor = ConsoleColor.White;
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("📚  VÄLKOMMEN TILL BIBLIOTEKET 📚\n");

            Console.ResetColor();
        }
    }
}