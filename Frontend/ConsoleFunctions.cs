using ClassLibrary;
using System;
using System.Collections.Generic;

namespace Frontend
{
    /// <summary>
    /// This class is used to handle the console-operations
    /// </summary>
    class ConsoleFunctions
    {
        Warehouse warehouse = new Warehouse(101, 4);
        internal enum BoxType
        {
            Blob, Cube, Cubeoid, Sphere, NotSpecified
        }
        /// <summary>
        /// Creates and adds a box, also puts it into storage.
        /// </summary>
        /// <param name="boxType">An enum declaring the type of box to make</param>
        internal void AddBox(BoxType boxType)
        {
            Console.Clear();
            string description = GetDescription(boxType);
            double weight = GetWeight();
            //If it's a blob, don't ask for fragile, else, ask.
            bool isFragile = boxType == BoxType.Blob ? true : GetFragileStatement();

            Box newBox;
            if(boxType == BoxType.Blob)
            {
                int side = GetMeasurement();
                newBox = warehouse.CreateBlob(description, weight, side);
            }
            else if(boxType == BoxType.Cube)
            {
                int side = GetMeasurement();
                newBox = warehouse.CreateCube(description, weight, isFragile, side);
            }
            else if(boxType == BoxType.Cubeoid)
            {
                int xSide = GetMeasurement("x-side");
                int ySide = GetMeasurement("y-side");
                int zSide = GetMeasurement("z-side");
                newBox = warehouse.CreateCubeoid(description, weight, isFragile, xSide, ySide, zSide);
            }
            //if else gets triggered, it's a sphere
            else
            {
                int radius = GetMeasurement("radius");
                newBox = warehouse.CreateSphere(description, weight, isFragile, radius);
            }

            Console.WriteLine("Your box has been created, and it has ID-{0}.", newBox.ID);
            PlaceBoxInStorage(newBox);
        }
        /// <summary>
        /// Tries to move a box, chosen by user, to a new place (also chosen by user)
        /// </summary>
        internal void MoveBox()
        {
            int IDToMove = GetBoxID();
            int newLocation = RetrieveIndexFromUser("location", warehouse.NumberOfLocations - 1);
            int newFloor = RetrieveIndexFromUser("floor", warehouse.NumberOfFloors - 1);
            bool hasMoved = warehouse.MoveBox(IDToMove, newLocation, newFloor);
            if (hasMoved)
            {
                Console.WriteLine("The box with ID: {0} has now moved to location: {1}, floor: {2}.", IDToMove, newLocation, newFloor);
            }
            else
            {
                Console.WriteLine("The box with ID: {0} couldn't move to location: {1}, floor: {2}.\n" +
                    "It's still in it's old place.", IDToMove, newLocation, newFloor);
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Prompts user for an ID, then removes the box with that ID, if found
        /// </summary>
        internal void RemoveBox()
        {
            Console.Clear();
            Console.WriteLine("Remove box:");
            int boxID = GetBoxID();
            bool isRemoved = warehouse.RemoveBox(boxID);
            if (isRemoved)
            {
                Console.WriteLine("Box with ID {0} was removed successfully.", boxID);
            }
            else
            {
                Console.WriteLine("There was no box present with ID {0}. Nothing removed.", boxID);
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Searches for a box, if found, shows all it's data to the user
        /// </summary>
        internal void SearchBoxByID()
        {
            Console.Clear();
            Console.WriteLine("Find box by ID:");
            int boxID = GetBoxID();
            bool boxFound = warehouse.FindBox(boxID, out int location, out int floor);
            if (boxFound)
            {
                Box box = warehouse.RetrieveCopyOfBox(boxID, location, floor);
                Console.WriteLine("The box is found at location {1}, floor {2}.", boxID, location, floor);
                Console.WriteLine(box.ToString());
            }
            else
            {
                Console.WriteLine("There is no box present with the id:{0}. Sorry!", boxID);
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Prints all the information about the boxes found in the given location and floor
        /// </summary>
        internal void ListCertainSpot()
        {
            int location = RetrieveIndexFromUser("location", warehouse.NumberOfLocations - 1);
            int floor = RetrieveIndexFromUser("floor", warehouse.NumberOfFloors - 1);
            List<Box> listOfBoxes = warehouse.CopyBoxesFromLocation(location, floor);
            if(listOfBoxes.Count < 1)
            {
                Console.WriteLine("There are no boxes present in location: {0} floor: {1}.", location, floor);
            }
            else
            {
                Console.WriteLine("Boxes found in location: {0} floor: {1}:", location, floor);
                foreach (Box box in listOfBoxes)
                {
                    Console.WriteLine(box.ToString());
                }
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Saves the data to the database
        /// </summary>
        internal void ManuallySaveToFile()
        {
            bool success = warehouse.WriteToDatabase();
            if (success)
            {
                Console.WriteLine("Data successfully saved to database.");
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Reads in data from the database.
        /// </summary>
        internal void ManuallyReadFromFile()
        {
            bool success = warehouse.ReadFromDatabase();
            if (success)
            {
                Console.WriteLine("Data successfully read from database.");
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Reads in data from the testdatabase.txt
        /// </summary>
        internal void ReadTestData()
        {
            bool success = warehouse.ReadFromDatabase("testdatabase.txt");
            if (success)
            {
                Console.WriteLine("Test data successfully read from test-database.");
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Prompts user if it wants to quit, and if so, if the user wants to save the data to the database
        /// </summary>
        internal void ExitQuestion()
        {
            Console.Clear();
            Console.Write("Are you sure you want to quit? Y/N ");
            ConsoleKey exitYN = Console.ReadKey().Key;
            Console.WriteLine();
            if (exitYN == ConsoleKey.Y || exitYN == ConsoleKey.Escape)
            {
                Console.Write("Do you want to save your data to the database? Y/N ");
                ConsoleKey saveFileYN = Console.ReadKey().Key;
                Console.WriteLine();
                if(saveFileYN == ConsoleKey.Y)
                {
                    warehouse.WriteToDatabase();
                }
                System.Environment.Exit(0);
            }
        }
        /// <summary>
        /// Prompts user for a description, and makes sure it's 1-100 characters, and no # are in the description(it's used in the database)
        /// </summary>
        /// <param name="boxType">The type of box to describe</param>
        /// <returns>a string containing the box's description</returns>
        private string GetDescription(BoxType boxType)
        {
            Console.Write("Please enter a description for the {0}: ", boxType);
            string description = Console.ReadLine();
            while(description.Length < 1 || description.Length > 100 || description.Contains("#"))
            {
                if(description.Length < 1)
                {
                    Console.WriteLine("The description can't be left empty.");
                }
                if(description.Length > 100)
                {
                    Console.WriteLine("No more than 100 characters is allowed.");
                }
                if (description.Contains("#"))
                {
                    Console.WriteLine("Description can't contain the character '#'.");
                }
                Console.Write("Please enter a proper description for the {0}: ", boxType);
                description = Console.ReadLine();
            }
            return description;
        }
        /// <summary>
        /// Prompts user and gets the weight of the box, as a double, in kilograms.
        /// </summary>
        /// <returns>A double containing the box's weight</returns>
        private double GetWeight()
        {
            Console.Write("Please enter a weight (in kg), decimals allowed: ");
            string weightInString = Console.ReadLine();
            bool weightParseSuccess = Double.TryParse(weightInString, out double weight);
            while (!weightParseSuccess)
            {
                Console.Write("Please only enter numbers and '.'. Try entering the weight again, in kg: ");
                weightInString = Console.ReadLine();
                weightParseSuccess = Double.TryParse(weightInString, out weight);
            }
            return weight;
        }
        /// <summary>
        /// Prompts user to enter a measurement, and returns it as an int (with type-safe checking)
        /// </summary>
        /// <param name="sideName">the name of the side wanted to get (i.e radius, side, xSide etc). Default is "side".</param>
        /// <returns>an integer containing the measurement, in centimeters</returns>
        private int GetMeasurement(string sideName = "side")
        {
            Console.Write("Please enter it's {0}, in whole centimeters: ", sideName);
            string sideInString = Console.ReadLine();
            bool sideParseSuccess = int.TryParse(sideInString, out int side);
            while (!sideParseSuccess)
            {
                Console.Write("Please only enter numbers. Try entering the {0} again, in whole centimeters: ", sideName);
                sideInString = Console.ReadLine();
                sideParseSuccess = int.TryParse(sideInString, out side);
            }
            return side;
        }
        /// <summary>
        /// Prompts user about if the box is fragile or not
        /// </summary>
        /// <returns>a boolean, with true for fragile, and false for non-fragile</returns>
        private bool GetFragileStatement()
        {
            Console.Write("Is the box fragile? Y/N: ");
            ConsoleKey isFragile = Console.ReadKey().Key;
            Console.WriteLine();
            while(!(isFragile == ConsoleKey.Y || isFragile == ConsoleKey.N))
            {
                Console.Write("Please only enter Y or N. Is the box fragile? Y/N: ");
                isFragile = Console.ReadKey().Key;
                Console.WriteLine();
            }
            return isFragile == ConsoleKey.Y ? true : false;
        }
        /// <summary>
        /// Prompts user about manual or automatic storage, and carries it out
        /// </summary>
        /// <param name="box">The input box to be put in storage</param>
        private void PlaceBoxInStorage(Box box)
        {
            Console.Write("Do you want to choose the location for the box? Y/N ");
            ConsoleKey chooseLocation = Console.ReadKey().Key;
            Console.WriteLine();
            while (!(chooseLocation == ConsoleKey.Y || chooseLocation == ConsoleKey.N))
            {
                Console.Write("Please only enter Y or N. Do you want to choose the location to put the box? Y/N: ");
                chooseLocation = Console.ReadKey().Key;
            }
            if(chooseLocation == ConsoleKey.Y)
            {
                int location = RetrieveIndexFromUser("location", warehouse.NumberOfLocations - 1);
                int floor = RetrieveIndexFromUser("floor", warehouse.NumberOfFloors - 1);
                bool successfullyPlaced = warehouse.StoreBoxManually(box, location, floor);
                if (successfullyPlaced)
                {
                    Console.WriteLine("Box successfully placed at location {0}, floor {0}.", location, floor);
                    Console.WriteLine("The box's ID-number is: {0}.", box.ID);
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("The box can't be placed there, try again.");
                    Console.ReadLine();
                    PlaceBoxInStorage(box);
                }
            }
            else
            {
                bool successfullyPlaced = warehouse.StoreBoxAutomatically(box, out int location, out int floor);
                if (successfullyPlaced)
                {
                    Console.WriteLine("Box successfully placed at location {0}, floor {1}.", location, floor);
                    Console.WriteLine("The box's ID-number is: {0}.", box.ID);
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("The box can't be placed at the storage, there is no room for it.");
                    Console.ReadLine();
                }
            }
        }
        /// <summary>
        /// Prompts user for the index of input type(indexType). returns it when it's a number and not to high
        /// </summary>
        /// <param name="indexType">The type of index to prompt for</param>
        /// <param name="maxNumber">The highest number the given location/floor can be</param>
        /// <returns>an integer containing a number usable as index</returns>
        private int RetrieveIndexFromUser(string indexType, int maxNumber)
        {
            Console.Write("What {0}? (1-{1}) ", indexType, warehouse.NumberOfLocations - 1);
            string locationString = Console.ReadLine();
            bool successfulParse = int.TryParse(locationString, out int place);
            while (!successfulParse || place > maxNumber)
            {
                if (!successfulParse)
                {
                    Console.WriteLine("Only numbers, please.");
                }
                else
                {
                    Console.WriteLine("Only numbers between 1-{0}, please.", maxNumber);
                }
                locationString = Console.ReadLine();
                successfulParse = int.TryParse(locationString, out place);
            }
            return place;
        }
        /// <summary>
        /// Prompts the user for a box's ID
        /// </summary>
        /// <returns>an integer containing the ID the user has input</returns>
        private int GetBoxID()
        {
            Console.Write("Enter the ID of the box (only numbers): ");
            string boxIDAsString = Console.ReadLine();
            bool successfulParse = int.TryParse(boxIDAsString, out int id);
            while (!successfulParse)
            {
                Console.Write("Only numbers in the ID, please. Try again: ");
                boxIDAsString = Console.ReadLine();
                successfulParse = int.TryParse(boxIDAsString, out id);
            }
            return id;
        }
        
    }
}
