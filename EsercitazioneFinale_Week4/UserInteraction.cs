using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsercitazioneFinale_Week4
{
    internal static class UserInteraction
    {
        public static void StartMain()
        {
            bool isOn = true;
            do
            {
                switch (MainMenu())
                {
                    case 0:
                        isOn = false;
                        break;
                    case 1:
                        // Inserisci nuova spesa
                        ExpensesManagerAdo.InsertExpense();
                        break;
                    case 2:
                        // Approva spesa esistente
                        ExpensesManagerAdo.ApproveExpense();
                        break;
                    case 3:
                        // Cancella spesa esistente
                        int inputId;
                        do
                        {
                            Console.WriteLine("\nInserisci l'ID della spesa che vuoi eliminare");
                        } while (!int.TryParse(Console.ReadLine(), out inputId));
                        ExpensesManagerAdo.DeleteExpenseById(inputId);
                        break;
                    case 4:
                        // Menu Mostra()
                        StartShow();

                        break;
                    defrault:
                        break;

                }
            } while (isOn);
        }

        public static void StartShow()
        {
            bool isOn = true;
            do
            {
                switch (ShowMenu())
                {
                    case 0:
                        isOn = false;
                        break;
                    case 1:
                        // Mostra l'elenco delle spese approvate
                        Console.WriteLine("*************** SPESE APPROVATE ***************");
                        foreach (var exp in ExpensesManagerAdo.ShowExpenses(true))
                        {
                            Console.WriteLine(exp);
                        }
                        break;
                    case 2:
                        // Mostra l'elenco delle spese di uno specifico utente
                        Console.WriteLine("Inserisci il nome utente di cui vuoi conoscere le spese");
                        var usr = Console.ReadLine();
                        Console.WriteLine($"*************** SPESE DI {usr.ToUpper()} ***************");

                        foreach (var exp in ExpensesManagerAdo.ShowExpensesByUser(usr))
                        {
                            Console.WriteLine(exp);
                        }
                        break;
                    case 3:
                        // Mostra il totale delle spese per categoria
                        Console.WriteLine("*************** CATEGORIE ***************");
                        foreach (var cat in ExpensesManagerAdo.ShowCategories())
                        {
                            Console.WriteLine(cat);
                        }
                        int idCat;
                        do
                        {
                            Console.WriteLine("Inserisci l'id della categoria di cui vuoi visualizzare le spese");
                        } while (!int.TryParse(Console.ReadLine(), out idCat));
                        foreach (var exp in ExpensesManagerAdo.ShowExpensesByCategory(idCat))
                        {
                            Console.WriteLine(exp);
                        }
                        
                        break;
                    defrault:
                        break;

                }
            } while (isOn);
        }

        private static int MainMenu()
        {
            int input;
            do
            {
                Console.WriteLine("************************ MAIN MENU ************************");
                Console.WriteLine("1. Inserisci una nuova spesa.");
                Console.WriteLine("2. Approva una spesa esistente.");
                Console.WriteLine("3. Cancella una spesa esistente.");
                Console.WriteLine("4. Vai al menu di visualizzazione.");
                Console.WriteLine("");
                Console.WriteLine("0. Esci.");
            } while (!int.TryParse(Console.ReadLine(), out input) && input < 0 && input > 4);
            return input;
        }
        private static int ShowMenu()
        {
            int input;
            do
            {
                Console.WriteLine("************************ SHOW MENU ************************");
                Console.WriteLine("1. Mostra l'elenco delle spese approvate.");
                Console.WriteLine("2. Mostra l'elenco delle spese di uno specifico utente.");
                Console.WriteLine("3. Mostra il totale delle spese per categoria.");
                Console.WriteLine("");
                Console.WriteLine("0. Esci.");
            } while (!int.TryParse(Console.ReadLine(), out input) && input < 0 && input > 3);
            return input;
        }

    }
}
