﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /// <summary>
    /// This class instanstiates the actual warehouse, with an 2d-array of WarehouseLocations. It also works as a factory,
    /// and is the only class able to create Boxes and WarehouseLocations.
    /// Note: I'd rather make the factory a separate class, but my exam explicitly demands the Warehouse to take care of it.
    /// </summary>
    public class Warehouse : IEnumerable
    {
        private WarehouseLocation[,] storage;
        private readonly int numberOfLocations;
        private readonly int numberOfFloors;
        private bool autoSave = true;
        //currentIDCount is used to ensure every box gets it's own unique ID-number
        private int currentIDCount = 1;
        public int NumberOfLocations { get => numberOfLocations; }
        public int NumberOfFloors { get => numberOfFloors; }
        public bool AutoSave { get => autoSave; set => autoSave = value; }
        /// <summary>
        /// Constructor for Warehouse-Class. Instanstiates it with a 2D-array of input parameters, numbersOfLocations and numbersOfFloors
        /// </summary>
        /// <param name="numberOfLocations">the amount of locations in the actual warehouse</param>
        /// <param name="numberOfFloors">the amount of floors in the actual warehouse</param>
        public Warehouse(int numberOfLocations, int numberOfFloors)
        {
            this.storage = CreateStorage(numberOfLocations, numberOfFloors);
            this.numberOfLocations = numberOfLocations;
            this.numberOfFloors = numberOfFloors;
        }
        /// <summary>
        /// An Indexer, returning the wanted WarehouseLocation.
        /// </summary>
        /// <param name="index1">The "Location"-index to look at</param>
        /// <param name="index2">The "Floor"-index to look at</param>
        /// <returns>A WarehouseLocation, found at index [index1(Location),index2(Floor)]</returns>
        public WarehouseLocation this[int index1, int index2]
        {
            get
            {
                return storage[index1, index2];
            }
        }

        //Factory-methods
        /// <summary>
        /// Creates a new, empty 2-dimensional WarehouseLocation-array, given the amount of floors, and locations per floor
        /// </summary>
        /// <param name="amountOfLocations">the amount of locations for each floor</param>
        /// <param name="amountOfFloors">the number of floors in the new array</param>
        /// <returns>An empty 2-dimensional WarehouseLocation-array, with input floors and locations per floor. ([locations,floors])</returns>
        private WarehouseLocation[,] CreateStorage(int amountOfLocations, int amountOfFloors)
        {
            WarehouseLocation[,] newStorage = new WarehouseLocation[amountOfLocations, amountOfFloors];
            //Initializing the storage with empty WarehouseLocations
            for (int i = 0; i < newStorage.GetLength(0); i++)
            {
                for (int j = 0; j < newStorage.GetLength(1); j++)
                {
                    newStorage[i, j] = CreateEmptyWarehouseLocation();
                }
            }
            return newStorage;
        }
        /// <summary>
        /// Creates and returns a new, empty, WarehouseLocation
        /// </summary>
        /// <returns>an empty WarehouseLocation</returns>
        public WarehouseLocation CreateEmptyWarehouseLocation()
        {
            return new WarehouseLocation(150, 250, 200, 1000);
        }
        /// <summary>
        /// Creates a new Blob, given the input parameters, and a unique ID-number
        /// </summary>
        /// <param name="description">Describes the content</param>
        /// <param name="weight">weight of the package</param>
        /// <param name="side">an integer describing it's side, in centimeters</param>
        /// <returns>a Blob, containing the given data</returns>
        public Blob CreateBlob(string description, double weight, int side)
        {
            return new Blob(currentIDCount++, description, weight, side);
        }
        /// <summary>
        /// Creates a new Cube, given the input parameters, and a unique ID-number
        /// </summary>
        /// <param name="description">Describes the content</param>
        /// <param name="weight">weight of the package</param>
        /// <param name="isFragile">Boolean stating if the package is fragile or not</param>
        /// <param name="side">an integer describing it's side, in centimeters</param>
        /// <returns>a Cube, containing the given data</returns>
        public Cube CreateCube(string description, double weight, bool isFragile, int side)
        {
            return new Cube(currentIDCount++, description, weight, isFragile, side);
        }
        /// <summary>
        /// Creates a new Cubeoid, given the input parameters, and a unique ID-number
        /// </summary>
        /// <param name="description">Describes the content</param>
        /// <param name="weight">weight of the package</param>
        /// <param name="isFragile">Boolean stating if the package is fragile or not</param>
        /// <param name="xSide">an integer describing it's xSide, in centimeters</param>
        /// <param name="ySide">an integer describing it's ySide, in centimeters</param>
        /// <param name="zSide">an integer describing it's zSide, in centimeters</param>
        /// <returns>a cubeoid, containing the given data</returns>
        public Cubeoid CreateCubeoid(string description, double weight, bool isFragile, int xSide, int ySide, int zSide)
        {
            return new Cubeoid(currentIDCount++, description, weight, isFragile, xSide, ySide, zSide);
        }
        /// <summary>
        /// Creates a new Sphere, given the input parameters, and a unique ID-number
        /// </summary>
        /// <param name="description">Describes the content</param>
        /// <param name="weight">weight of the package</param>
        /// <param name="isFragile">Boolean stating if the package is fragile or not</param>
        /// <param name="radius">an integer describing it's radius, in centimeters</param>
        /// <returns>a Sphere, containing the given data</returns>
        public Sphere CreateSphere(string description, double weight, bool isFragile, int radius)
        {
            return new Sphere(currentIDCount++, description, weight, isFragile, radius);
        }

        //Storing-methods
        /// <summary>
        /// Tries to store given box in the warehouse, at the first available spot
        /// </summary>
        /// <param name="box">The box supposed to be put in storage</param>
        /// <param name="placedLocation">an integer containing the location-number of the box (or -1 if no place was found)</param>
        /// <param name="placedFloor">an integer containing the floor-number of the box (or -1 if no place was found)</param>
        /// <returns>True, if succeeded. Else false</returns>
        public bool StoreBoxAutomatically(Box box, out int placedLocation, out int placedFloor)
        {
            for (int i = 1; i < storage.GetLength(0); i++)
            {
                for (int j = 1; j < storage.GetLength(1); j++)
                {
                    if (storage[i, j].AddBox(box))
                    {
                        placedLocation = i;
                        placedFloor = j;
                        if (autoSave)
                        {
                            WriteToDatabase();
                        }
                        return true;
                    }
                }
            }
            placedLocation = -1;
            placedFloor = -1;
            return false;
        }
        /// <summary>
        /// Tries to store a box manually, at the given warehouseLocation(location,floor)
        /// </summary>
        /// <param name="box">The box to put in storage</param>
        /// <param name="location">the location to try in</param>
        /// <param name="floor">the floor to try in</param>
        /// <returns>True if the action was successful, else false.</returns>
        public bool StoreBoxManually(Box box, int location, int floor)
        {
            if (storage[location, floor].AddBox(box))
            {
                if (autoSave)
                {
                    WriteToDatabase();
                }
                return true;
            }
            return false;
        }

        //Find/Move/Remove
        /// <summary>
        /// Tries to find a box by given ID-number. If found, out-parameters show the WarehouseLocation of the box.
        /// Else, returns false, and out-parameters are assigned value -1.
        /// </summary>
        /// <param name="id">The ID-number to search for</param>
        /// <param name="location">out-parameter where given box resides</param>
        /// <param name="floor">out-parameter where given box resides</param>
        /// <returns>True, if ID-number correlates with a box in the system, else false.</returns>
        public bool FindBox(int id, out int location, out int floor)
        {
            for(int i=1; i<storage.GetLength(0); i++)
            {
                for(int j=1; j<storage.GetLength(1); j++)
                {
                    if (storage[i, j].IDIsPresent(id))
                    {
                        location = i;
                        floor = j;
                        return true;
                    }
                }
            }
            location = -1;
            floor = -1;
            return false;
        }
        /// <summary>
        /// Retrieves and returns a copy of given box, by entering ID, location and floor
        /// </summary>
        /// <param name="id">id of given box</param>
        /// <param name="location">location of given box</param>
        /// <param name="floor">floor of given box</param>
        /// <returns>A box</returns>
        public Box RetrieveCopyOfBox(int id, int location, int floor)
        {
            Box boxToReturn = storage[location, floor].RetrieveCopyOfBox(id);
            return boxToReturn;
        }
        /// <summary>
        /// Attempts to move a box, by given id, to the new given location.
        /// If successful, returns true.
        /// Else, it keeps the box at it's old place, and returns false.
        /// </summary>
        /// <param name="id">ID of the box wanting to move</param>
        /// <param name="newLocation">the new location for the box</param>
        /// <param name="newFloor">the new floor for the box</param>
        /// <returns>True if successful, else false</returns>
        public bool MoveBox(int id, int newLocation, int newFloor)
        {
            bool boxIsFound = FindBox(id, out int location, out int floor);
            if (boxIsFound)
            {
                Box boxToMove = storage[location, floor].RetrieveBoxByID(id);
                if (storage[newLocation, newFloor].AddBox(boxToMove))
                {
                    if (autoSave)
                    {
                        WriteToDatabase();
                    }
                    return true;
                }
                //If the box can't fit in the new WarehouseLocation, put it back in it's earlier place
                //And return false.
                storage[location, floor].AddBox(boxToMove);
            }
            return false;
        }
        /// <summary>
        /// Attempts to remove a box by given ID. If it's found, removes it and returns true.
        /// Else, returns false.
        /// </summary>
        /// <param name="id">The ID-number to be searched for.</param>
        /// <returns>True if box is found and removed, else false.</returns>
        public bool RemoveBox(int id)
        {
            bool boxIsFound = FindBox(id, out int location, out int floor);
            if (boxIsFound)
            {
                storage[location, floor].RetrieveBoxByID(id);
                if (autoSave)
                {
                    WriteToDatabase();
                }
                return true;
            }
            return false;
        }

        //Clone
        /// <summary>
        /// Retrieves a clone of the WarehouseLocation at input location and floor.
        /// Note: Required by the exam-demands.
        /// </summary>
        /// <param name="location">The location where to Clone</param>
        /// <param name="floor">The floor where to Clone</param>
        /// <returns>A WarehouseLocation clone of the given location and floor</returns>
        public WarehouseLocation RetrieveCloneOfWarehouseLocation(int location, int floor)
        {
            return storage[location, floor].Clone();
        }

        //Read/Write
        /// <summary>
        /// Tells WarehouseIO to write to given file, or "database.txt" if no filename is given
        /// </summary>
        /// <param name="filename">the filename to where the data will be written</param>
        /// <returns>True if success, else false</returns>
        public bool WriteToDatabase(string filename = "database.txt")
        {
            WarehouseIO databaseWriter = new WarehouseIO(filename);
            bool success = databaseWriter.WriteToDatabase(storage);
            return success;
        }
        /// <summary>
        /// Tells WarehouseIO to read from input file. if no file is input, reads from database.txt
        /// </summary>
        /// <param name="filename">the file from where to read</param>
        /// <returns>True if success, else false</returns>
        public bool ReadFromDatabase(string filename = "database.txt")
        {
            WarehouseIO databaseReader = new WarehouseIO(filename);
            WarehouseLocation[,] storageReadFromDatabase = databaseReader.ReadFromDatabase();
            if(storageReadFromDatabase == null)
            {
                return false;
            }
            storage = storageReadFromDatabase;
            currentIDCount = FindHighestID() + 1;
            return true;
        }

        //Enumerator
        /// <summary>
        /// Returns an enumerator that iterates through the WarehouseLocations found in the Warehouse
        /// </summary>
        /// <returns>Returns an enumerator iterating through WarehouseLocations</returns>
        public IEnumerator GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        //Private methods below
        /// <summary>
        /// Finds the highest current ID-number, and returns it as an integer
        /// </summary>
        /// <returns>an integer containing the current highest integer</returns>
        private int FindHighestID()
        {
            int highestID = 0;
            for (int i = 1; i < storage.GetLength(0); i++)
            {
                for (int j = 1; j < storage.GetLength(1); j++)
                {
                    foreach (Box box in storage[i, j])
                    {
                        highestID = highestID > box.ID ? highestID : box.ID;
                    }
                }
            }
            return highestID;
        }
    }
}
