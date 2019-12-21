using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend
{
    /// <summary>
    /// A class that handles the menu-functions
    /// </summary>
    class Menu
    {
        ConsoleFunctions functions = new ConsoleFunctions();
        /// <summary>
        /// Prints the main menu, and let's the user decide what to do
        /// </summary>
        public void MainMenu()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Simons warehouse-system, version 1.0");
                    Console.WriteLine("***************************");
                    Console.WriteLine("[1]    Add a box");
                    Console.WriteLine("[2]    Move a box");
                    Console.WriteLine("[3]    Remove a box");
                    Console.WriteLine("[4]    Search for a box using ID-number");
                    Console.WriteLine("[5]    List boxes in a specific warehouse-location");
                    Console.WriteLine("[6]    List all locations and all boxes found within");
                    Console.WriteLine("[7]    Read/Save data");
                    Console.WriteLine("[8]    Exit");
                    ConsoleKey chosenInput = Console.ReadKey().Key;
                    switch (chosenInput)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            functions.AddBox(ChooseBoxType());
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            functions.MoveBox();
                            break;
                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            functions.RemoveBox();
                            break;
                        case ConsoleKey.D4:
                        case ConsoleKey.NumPad4:
                            functions.SearchBoxByID();
                            break;
                        case ConsoleKey.D5:
                        case ConsoleKey.NumPad5:
                            functions.ListCertainSpot();
                            break;
                        case ConsoleKey.D6:
                        case ConsoleKey.NumPad6:
                            functions.ListAllBoxesInWarehouse();
                            break;
                        case ConsoleKey.D7:
                        case ConsoleKey.NumPad7:
                            ReadSaveMenu();
                            break;
                        case ConsoleKey.D8:
                        case ConsoleKey.NumPad8:
                        case ConsoleKey.Escape:
                            functions.ExitQuestion();
                            break;
                        default:
                            break;
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.GetType().ToString());
                    Console.WriteLine(exception.Message);
                    Console.ReadLine();
                }
            }
        }
        /// <summary>
        /// Prints a small menu which lets the user decide what type of box to create
        /// </summary>
        /// <returns></returns>
        private ConsoleFunctions.BoxType ChooseBoxType()
        {
            ConsoleFunctions.BoxType boxType = ConsoleFunctions.BoxType.NotSpecified;
            while (boxType == ConsoleFunctions.BoxType.NotSpecified)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Adding a new box");
                Console.WriteLine("***************************");
                Console.WriteLine("What kind of box is it?");
                Console.WriteLine("[1]    Blob");
                Console.WriteLine("[2]    Cube");
                Console.WriteLine("[3]    Cubeoid");
                Console.WriteLine("[4]    Sphere");
                ConsoleKey chosenBoxType = Console.ReadKey().Key;
                switch (chosenBoxType)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        boxType = ConsoleFunctions.BoxType.Blob;
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        boxType = ConsoleFunctions.BoxType.Cube;
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        boxType = ConsoleFunctions.BoxType.Cubeoid;
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        boxType = ConsoleFunctions.BoxType.Sphere;
                        break;
                    default:
                        break;
                } 
            }
            return boxType;
        }
        /// <summary>
        /// A menu giving the user the possibility to read from either the normal database, or the test-database.
        /// Also let's the user write to the normal database
        /// </summary>
        private void ReadSaveMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Read/Save-menu");
            Console.WriteLine("***************************");
            Console.WriteLine("[1]    Save data to database");
            Console.WriteLine("[2]    Read data from database");
            Console.WriteLine("[3]    Read test-data from database (Note, for testing purposes only)");
            Console.WriteLine("[4]    Return to main menu");
            ConsoleKey chosenAlternative = Console.ReadKey().Key;
            Console.WriteLine();
            switch (chosenAlternative)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    functions.ManuallySaveToFile();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    functions.ManuallyReadFromFile();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    functions.ReadTestData();
                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    break;
                default:
                    break;
            }
        }
    
    }
}
