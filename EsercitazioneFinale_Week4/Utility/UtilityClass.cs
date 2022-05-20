using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsercitazioneFinale_Week4.Utility
{
    public static class UtilityClass
    {
        public static bool Confirm(string output)
        {
            string input;
            do
            {
                Console.WriteLine(output + " (Y/N)");
                input = Console.ReadLine();
            } while (!(input.ToLower() != "y" || input.ToLower() != "n"));
            if (input == "y")
                return true;
            else
                return false;
        }
    }
}
