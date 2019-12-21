using System;
using System.Text;

namespace ClassLibrary
{
    public class Blob : Box
    {
        private int side;
        public int Side { get => side; }

        internal Blob(int id, string description, double weight, int side) : base(id, description, weight, true)
        {
            this.side = side;
            this.area = CalculateArea();
            this.volume = CalculateVolume();
            this.maxDimension = FindMaxDimension();
        }
        /// <summary>
        /// Calculates the area of given sphere, and returns it as square centimeters in an int.
        /// NOTE: It is counted as if it was a cube, for this exam.
        /// </summary>
        /// <returns>an integer containing area as square centimeters</returns>
        internal override int CalculateArea()
        {
            return Side * Side;
        }
        /// <summary>
        /// Calculates the maxDimension of given blob
        /// </summary>
        /// <returns>an integer containing the max dimension, in centimeters</returns>
        internal override int FindMaxDimension()
        {
            return Side;
        }
        /// <summary>
        /// Calculates given blob's volume, and returns it as in cubic centimeters, in an int.
        /// NOTE: It is counted as if it was a cube, for this exam.
        /// </summary>
        /// <returns>an integer containing the volume as cubic centimeters</returns>
        internal override int CalculateVolume()
        {
            return Side * Side * Side;
        }
        /// <summary>
        /// Converts current Blob to a database-writeable string
        /// </summary>
        /// <returns>a string containing the data needed to write it to the database</returns>
        internal override string ToDatabaseString()
        {
            StringBuilder blobAsString = new StringBuilder();
            blobAsString.Append("Blob#");
            blobAsString.Append(ID + "#");
            blobAsString.Append(Description + "#");
            blobAsString.Append(Weight + "#");
            blobAsString.Append(Side);
            return blobAsString.ToString();
        }
        /// <summary>
        /// Returns the blob as a string
        /// </summary>
        /// <returns>a string containing all the information about the blob</returns>
        public override string ToString()
        {
            StringBuilder returnString = new StringBuilder();
            returnString.AppendLine("Type: Blob");
            returnString.AppendLine("ID: " + ID);
            returnString.AppendLine("Description: " + Description);
            returnString.AppendLine("Weight: " + Weight + " kg");
            returnString.AppendLine("Fragile: Yes");
            returnString.AppendLine("Side: " + Side + " cm");
            returnString.AppendLine("Area: " + Area + " square cm");
            returnString.AppendLine("Volume: " + Volume + " cubic cm");
            returnString.AppendLine("Max Dimension: " + MaxDimension + " cm");
            return returnString.ToString();
        }
    }
}