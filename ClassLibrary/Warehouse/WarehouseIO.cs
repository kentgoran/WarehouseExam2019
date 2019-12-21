using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClassLibrary
{
    internal class WarehouseIO
    {
        private readonly string filename;
        
        internal WarehouseIO(string filename)
        {
            this.filename = filename;
        }
        /// <summary>
        /// Takes in a 2d-array of WarehouseLocations, and writes it to the database.
        /// </summary>
        /// <param name="storage">the 2d-array of WarehouseLocations to write to database</param>
        /// <returns>True if successful, false if something went wrong</returns>
        internal bool WriteToDatabase(WarehouseLocation[,] storage)
        {
            using(StreamWriter databaseWriter = new StreamWriter(filename, false))
            {
                try
                {
                    databaseWriter.WriteLine("Length of database #{0}#{1}", storage.GetLength(0), storage.GetLength(1));
                    for(int i=1; i<storage.GetLength(0); i++)
                    {
                        for(int j=1; j<storage.GetLength(1); j++)
                        {
                            databaseWriter.WriteLine("{0}#{1}", i, j);
                            foreach(Box box in storage[i, j])
                            {
                                databaseWriter.WriteLine(box.ToDatabaseString());
                            }
                        }
                    }
                }
                catch(Exception exception)
                {
                    ExceptionLogger(exception);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Takes in data from the database, and returns it as a WarehouseLocation[,].
        /// </summary>
        /// <returns>a 2D WarehouseLocation, containing the data found in the database</returns>
        /// <exception cref="InvalidDataException">Thrown when the database is incorrect, corrupt or empty.</exception>
        internal WarehouseLocation[,] ReadFromDatabase()
        {
            WarehouseLocation[,] storageFromDatabase;
            using(StreamReader databaseReader = new StreamReader(filename))
            {
                try
                {
                    //The first line in the database contains information about the
                    //length of the storage. Here it's read, and the storage is formed accordingly
                    string[] lengthOfDatabase = databaseReader.ReadLine().Split('#');
                    bool foundLocation = (int.TryParse(lengthOfDatabase[1], out int location));
                    bool foundFloor = (int.TryParse(lengthOfDatabase[2], out int floor));
                    if (foundLocation && foundFloor)
                    {
                        storageFromDatabase = new WarehouseLocation[location, floor];
                    }
                    else
                    {
                        throw new InvalidDataException("Database is corrupt or empty");
                    }
                    //storageFromDatabase needs to be instanciated before filling it with database-content
                    for (int i = 0; i < storageFromDatabase.GetLength(0); i++)
                    {
                        for (int j = 0; j < storageFromDatabase.GetLength(1); j++)
                        {
                            storageFromDatabase[i, j] = new WarehouseLocation(150, 250, 200, 1000);
                        }
                    }

                    //The rest of the database is read here, and handled accordingly
                    //Some lines contains only numbers, and those indicate going to a new
                    //WarehouseLocation. Other contain boxes found in said WarehouseLocation
                    string lineFromDatabase = databaseReader.ReadLine();
                    int currentLocation = 0;
                    int currentFloor = 0;
                    do
                    {
                        string[] lineSplit = lineFromDatabase.Split('#');
                        //lineIndicatesNewLocation, this alters the current WarehouseLocation to the next one
                        bool lineIndicatesNewLocation = int.TryParse(lineSplit[0], out int newLocation);
                        if (lineIndicatesNewLocation)
                        {
                            int.TryParse(lineSplit[1], out int newFloor);
                            currentLocation = newLocation;
                            currentFloor = newFloor;
                        }
                        else
                        {
                            Box parsedBox = ParseReadLineToBox(lineSplit);
                            //If ParseReadLineToBox is null, then it's neither a new WarehouseLocation, or box
                            //Hence the database has become corrupted.
                            if (parsedBox == null)
                            {
                                throw new Exception("Database is corrupt");
                            }
                            storageFromDatabase[currentLocation, currentFloor].AddBox(parsedBox);
                        }

                        //Lastly, read the next line in database.txt
                        lineFromDatabase = databaseReader.ReadLine();
                    } while (lineFromDatabase != null);
                }
                catch(InvalidDataException invalidDataException)
                {
                    ExceptionLogger(invalidDataException);
                    Console.WriteLine("While reading from file '{0}', came across empty, false or corrupt data. Couldn't read from file.", filename);
                    return null;
                }
                catch(Exception exception)
                {
                    ExceptionLogger(exception);
                    Console.WriteLine("Critical error. Contact support. Did not read in data from file '{0}'.", filename);
                    return null;
                }
                return storageFromDatabase;
            }
        }
        /// <summary>
        /// Takes a line read from the database, split into an array, and turns it into an appropriate box
        /// </summary>
        /// <param name="lineSplit">A line read from the database, split into an array</param>
        /// <returns>an appropriate box, given the input string-array</returns>
        private Box ParseReadLineToBox(string[] lineSplit)
        {
            Box boxToReturn = null;
            if (lineSplit[0].Equals("Blob"))
            {
                int id = int.Parse(lineSplit[1]);
                string description = lineSplit[2];
                double weight = double.Parse(lineSplit[3]);
                int side = int.Parse(lineSplit[4]);
                boxToReturn = new Blob(id, description, weight, side);
            }
            else if (lineSplit[0].Equals("Cube"))
            {
                int id = int.Parse(lineSplit[1]);
                string description = lineSplit[2];
                double weight = double.Parse(lineSplit[3]);
                bool isFragile = lineSplit[4].Equals("True") ? true : false;
                int side = int.Parse(lineSplit[5]);
                boxToReturn = new Cube(id, description, weight, isFragile, side);
            }
            else if (lineSplit[0].Equals("Cubeoid"))
            {
                int id = int.Parse(lineSplit[1]);
                string description = lineSplit[2];
                double weight = double.Parse(lineSplit[3]);
                bool isFragile = lineSplit[4].Equals("True") ? true : false;
                int xSide = int.Parse(lineSplit[5]);
                int ySide = int.Parse(lineSplit[6]);
                int zSide = int.Parse(lineSplit[7]);
                boxToReturn = new Cubeoid(id, description, weight, isFragile, xSide, ySide, zSide);
            }
            else if (lineSplit[0].Equals("Sphere"))
            {
                int id = int.Parse(lineSplit[1]);
                string description = lineSplit[2];
                double weight = double.Parse(lineSplit[3]);
                bool isFragile = lineSplit[4].Equals("True") ? true : false;
                int side = int.Parse(lineSplit[5]);
                boxToReturn = new Sphere(id, description, weight, isFragile, side);
            }
            return boxToReturn;
        }
        /// <summary>
        /// Logs any occuring exceptions in exceptionlog.txt.
        /// </summary>
        /// <param name="exception">The exception to be logged.</param>
        private void ExceptionLogger(Exception exception)
        {
            string exceptionFile = "exceptionlog.txt";
            using (StreamWriter exceptionWriter = new StreamWriter(exceptionFile, true))
            {
                exceptionWriter.WriteLine(DateTime.Now.ToString());
                exceptionWriter.WriteLine(exception.GetType().ToString());
                exceptionWriter.WriteLine(exception.Message);
                exceptionWriter.WriteLine("---------------------------------");
                exceptionWriter.WriteLine();
            }
        }
    }
}
