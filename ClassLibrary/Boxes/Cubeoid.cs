using System;
using System.Text;

namespace ClassLibrary
{
    public class Cubeoid : Box
    {
        private int xSide;
        private int ySide;
        private int zSide;

        public int XSide { get => xSide; }
        public int YSide { get => ySide; }
        public int ZSide { get => zSide; }

        internal Cubeoid(int id, string description, double weight, bool isFragile, int xSide, int ySide, int zSide) : base(id, description, weight, isFragile)
        {
            this.xSide = xSide;
            this.ySide = ySide;
            this.zSide = zSide;
            this.area = CalculateArea();
            this.volume = CalculateVolume();
            this.maxDimension = FindMaxDimension();
        }
        /// <summary>
        /// Calculates the biggest area of the current cubeoid, returns it in square centimeters, as an integer
        /// </summary>
        /// <returns>The cubeoid's biggest area, in square centimeters, as an int</returns>
        internal override int CalculateArea()
        {
            //Sorts the sides by size, then multiplies the biggest 2, to get the biggest possible area
            int[] measurements = new int[] { xSide, ySide, zSide };
            Array.Sort(measurements);
            int biggestArea = measurements[1] * measurements[2];
            return biggestArea;
        }
        /// <summary>
        /// Calculates and finds the biggest dimension of the cubeiod, and returns it in centimeters, as an integer
        /// </summary>
        /// <returns>an integer containing the biggest dimension of the cubeoid, in centimeters</returns>
        internal override int FindMaxDimension()
        {
            //Sorts the sides by size, then returns the biggest one
            int[] measurements = new int[] { xSide, ySide, zSide };
            Array.Sort(measurements);
            return measurements[2];
        }
        /// <summary>
        /// Calculates the cubeoid's volume, and returns it in cubic centimeters, as an integer
        /// </summary>
        /// <returns>an integer containing the cubeoid's volume, in cubic centimeters</returns>
        internal override int CalculateVolume()
        {
            int volume = xSide * ySide * zSide;
            return volume;
        }
        /// <summary>
        /// Converts current Cubeoid to a database-writeable string
        /// </summary>
        /// <returns>a string containing the data needed to write it to the database</returns>
        internal override string ToDatabaseString()
        {
            StringBuilder cubeoidAsString = new StringBuilder();
            cubeoidAsString.Append("Cubeoid#");
            cubeoidAsString.Append(ID + "#");
            cubeoidAsString.Append(Description + "#");
            cubeoidAsString.Append(Weight + "#");
            cubeoidAsString.Append(IsFragile + "#");
            cubeoidAsString.Append(xSide + "#");
            cubeoidAsString.Append(ySide + "#");
            cubeoidAsString.Append(zSide);
            return cubeoidAsString.ToString();
        }
        /// <summary>
        /// Returns the cubeoid as a string
        /// </summary>
        /// <returns>a string containing all the information about the cubeoid</returns>
        public override string ToString()
        {
            StringBuilder returnString = new StringBuilder();
            returnString.AppendLine("Type: Cubeoid");
            returnString.AppendLine("ID: " + ID);
            returnString.AppendLine("Description: " + Description);
            returnString.AppendLine("Weight: " + Weight + " kg");
            returnString.AppendLine("Fragile: " + (IsFragile ? "Yes" : "No"));
            returnString.AppendLine("X side: " + xSide + " cm");
            returnString.AppendLine("Y side: " + ySide + " cm");
            returnString.AppendLine("Z side: " + zSide + " cm");
            returnString.AppendLine("Area: " + Area + " square cm");
            returnString.AppendLine("Volume: " + Volume + " cubic cm");
            returnString.AppendLine("Max Dimension: " + MaxDimension + " cm");
            return returnString.ToString();
        }
    }
}