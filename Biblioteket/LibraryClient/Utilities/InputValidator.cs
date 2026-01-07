using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Biblioteket.LibraryClient.Utilities
{
    public static class Inputvalidator
    {

        public static bool isValidString(string input, int maxLength = 100, bool allowNumbers = true) // Sträng validering
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleHelper.WriteError("Inmatning får inte vara tomt!");
                return false;
            }

            if (!allowNumbers && input.Any(char.IsDigit))
            {
                ConsoleHelper.WriteError("Endast bokstäver tillåts!");
                return false;
            }

            if (input.Length > maxLength)
            {
                ConsoleHelper.WriteError($"För många tecken! Max {maxLength} tecken tillåts!");
                return false;
            }

            return true;
        }

        public static bool isValidNumber(string input, int min = 0, int max = int.MaxValue) // Tal validering
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleHelper.WriteError("Numret får inte vara tomt!");
                return false;
            }

            if (!int.TryParse(input, out int number))
            {
                ConsoleHelper.WriteError("Endast siffror tillåts!");
                return false;
            }

            if (number < min || number > max)
            {
                ConsoleHelper.WriteError($"Numret måste vara mellan {min} och {max}!");
                return false;
            }

            return true;
        }

        public static bool isValidPin(string input, int length = 4) // Pin validering
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleHelper.WriteError("PIN får inte vara tomt!");
                return false;
            }

            if (input.Length != length || !input.All(char.IsDigit))
            {
                ConsoleHelper.WriteError($"PIN måste bestå av exakt {length} siffror!");
                return false;
            }

            return true;
        }

        public static bool isValidPersonNumber(string input) // Personnummer validering
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleHelper.WriteError("Personnumret får inte vara tomt!");
                return false;
            }

            if (input.Length != 13 || input[8] != '-' || !input.Replace("-", "").All(char.IsDigit))
            {
                ConsoleHelper.WriteError("Personnumret måste vara i formatet YYYYMMDD-XXXX!");
                return false;
            }

            return true;
        }

        public static bool isValidPhoneNumber(string phoneNumber) // Telefonnummer validering
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                ConsoleHelper.WriteError("Telefonnummer får inte vara tomt!");
                return false;
            }

            if (!phoneNumber.All(char.IsDigit))
            {
                ConsoleHelper.WriteError("Endast siffror tillåts i telefonnummer!");
                return false;
            }

            if (phoneNumber.Length != 10)
            {
                ConsoleHelper.WriteError("Telefonnummer måste bestå av exakt 10 siffror!");
                return false;
            }

            return true;
        }

        public static bool isValidEmail(string input) // E-post validering
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleHelper.WriteError("E-postadressen får inte vara tomt!");
                return false;
            }

            if (!input.Contains("@") || !input.Contains("."))
            {
                ConsoleHelper.WriteError("Ogiltig e-postadress!");
                return false;
            }

            return true;
        }

        public static bool isValidIsbn(string input) // ISBN validering 10st = gamla böcker, 13st = nya böcker
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleHelper.WriteError("ISBN får inte vara tomt!");
                return false;
            }

            if (!(input.Length == 10 || input.Length == 13) || !input.All(char.IsDigit))
            {
                ConsoleHelper.WriteError("ISBN måste vara mellan 10 eller 13 siffror långt!");
                return false;
            }

            return true;
        }

        public static bool isNotEmpty<T>(ICollection<T> collection) // Lista/array (böcker,medlemmar) inte får vara tomma
        {
            if (collection == null || collection.Count == 0)
            {
                ConsoleHelper.WriteError("Listan får inte vara tom!");
                return false;
            }

            return true;
        }

        public static bool isValidDate(string input, out DateTime date) // Datum validering
        {
            if (!DateTime.TryParse(input, out date))
            {
                ConsoleHelper.WriteError("Ogiltigt datumformat! Måste vara YYYY-MM-DD!");
                return false;
            }

            return true;
        }

        public static bool isValid<T>(T data) // Validerar olika typer automatiskt
        {
            if (data == null)
            {
                ConsoleHelper.WriteError("Inmatningen får inte vara null!");
                return false;
            }

            if (data is string s) return isValidString(s);
            if (data is int i) return i >= 0;
            if (data is decimal d) return d >= 0m;
            if (data is System.Collections.ICollection c) return isNotEmpty(c.Cast<object>().ToList());

            return true;
        }
    }
}

