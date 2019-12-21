using System;
using System.Text;

namespace ClassLibrary
{
    /// <summary>
    /// A Cube, inherited from abstract class Box, which inherits I3DStorageObject.
    /// </summary>
    public class Cube : Box
    {
        private int side;
        public int Side { get => side; }
        /// <summary>
        /// Constructs a new Cube
        /// </summary>
        /// <param name="id">The Cube's ID</param>
        /// <param name="description">A short description of the Cube</param>
        /// <param name="weight">The Cube's weight, in kilograms</param>
        /// <param name="isFragile">Boolean stating if the Cube is fragile</param>
        /// <param name="side">The Cube's side, in cm</param>
        internal Cube(int id, string description, double weight, bool isFragile, int side) : base(id, description, weight, isFragile)
        {
            this.side = side;
            this.area = CalculateArea();
            this.volume = CalculateVolume();
            this.maxDimension = FindMaxDimension();
        }

        /// <summary>
        /// Calculates and returns the cubes area in square centimeters, as an integer
        /// </summary>
        /// <returns>an integer containing the cubes area in cubic centimeters</returns>
        internal override int CalculateArea()
        {
            return Side * Side;
        }
        /// <summary>
        /// Returns the cubes volume, in cubic centimeters, as an integer
        /// </summary>
        /// <returns>an integer containing the volume in cubic centimeters</returns>
        internal override int CalculateVolume()
        {
            int volume = Side * Side * Side;
            return volume;
        }
        /// <summary>
        /// Returns the maxDimension of the cube in centimeters, as an integer
        /// </summary>
        /// <returns>an integer containing the maxDimension, in centimeters</returns>
        internal override int FindMaxDimension()
        {
            return Side;
        }
        /// <summary>
        /// Converts current Cube to a database-writeable string
        /// </summary>
        /// <returns>a string containing the data needed to write it to the database</returns>
        internal override string ToDatabaseString()
        {
            StringBuilder cubeAsString = new StringBuilder();
            cubeAsString.Append("Cube#");
            cubeAsString.Append(ID + "#");
            cubeAsString.Append(Description + "#");
            cubeAsString.Append(Weight + "#");
            cubeAsString.Append(IsFragile + "#");
            cubeAsString.Append(Side);
            return cubeAsString.ToString();
        }
        /// <summary>
        /// Returns the cube as a string
        /// </summary>
        /// <returns>a string containing all the information about the cube</returns>
        public override string ToString()
        {
            StringBuilder returnString = new StringBuilder();
            returnString.AppendLine("Type: Cube");
            returnString.AppendLine("ID: " + ID);
            returnString.AppendLine("Description: " + Description);
            returnString.AppendLine("Weight: " + Weight + " kg");
            returnString.AppendLine("Fragile: " + (IsFragile ? "Yes" : "No"));
            returnString.AppendLine("Side: " + Side + " cm");
            returnString.AppendLine("Area: " + Area + " square cm");
            returnString.AppendLine("Volume: " + Volume + " cubic cm");
            returnString.AppendLine("Max Dimension: " + MaxDimension + " cm");
            return returnString.ToString();
        }
        /// <summary>
        /// A method that returns a string containing type and ID of current box
        /// </summary>
        /// <returns>A string like this "Type: Blob, ID: 14823"</returns>
        public override string ToShortString()
        {
            string shortDescription = "Type: Cube, ID: " + ID;
            return shortDescription;
        }
    }
}