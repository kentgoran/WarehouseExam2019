using System;
using System.Text;

namespace ClassLibrary
{
    /// <summary>
    /// A Sphere, inherited from abstract class Box, which inherits I3DStorageObject.
    /// In this exam, a sphere is, by volume and area, counted as a cube.
    /// </summary>
    public class Sphere : Box
    {
        private int radius;
        public int Radius { get => radius; }
        /// <summary>
        /// Creates a new Sphere
        /// </summary>
        /// <param name="id">The Sphere's ID-number</param>
        /// <param name="description">a short description of the Sphere</param>
        /// <param name="weight">The Sphere's weight, in kilograms</param>
        /// <param name="isFragile">A Boolean, stating if the Sphere is fragile</param>
        /// <param name="radius">The Sphere's radius, in cm</param>
        internal Sphere(int id, string description, double weight, bool isFragile, int radius) : base(id, description, weight, isFragile)
        {
            this.radius = radius;
            this.area = CalculateArea();
            this.volume = CalculateVolume();
            this.maxDimension = FindMaxDimension();
        }
        /// <summary>
        /// Calculates the area of given sphere, and returns it as square centimeters.
        /// NOTE: The sphere is counted as a cube, for this exam.
        /// </summary>
        /// <returns>an integer containing area as square centimeters</returns>
        internal override int CalculateArea()
        {
            //Sphere counts as a cube in regards to it's area (for this program). So radius * 2, gets it's diameter. diameter * diameter is then the area
            int diameter = radius * 2;
            int calculatedArea = diameter * diameter;
            return calculatedArea;
        }
        /// <summary>
        /// Returns the sphere's biggest dimension, aka it's diameter, in centimeters, as an integer
        /// </summary>
        /// <returns>an integer containing the biggest dimension of the sphere, as an integer</returns>
        internal override int FindMaxDimension()
        {
            int maxDimension = radius * 2;
            return maxDimension;
        }
        /// <summary>
        /// Calculates and returns the spheres volume, in cubic centimeters, as an integer.
        /// NOTE: The sphere is measured as a cube, in this exam.
        /// </summary>
        /// <returns></returns>
        internal override int CalculateVolume()
        {
            //Sphere counts as a cube in regards to it's volyme, so radius*2 to get it's diameter, and then diameter*diameter*diameter
            int diameter = radius * 2;
            int calculatedVolume = diameter * diameter * diameter;
            return calculatedVolume;
        }
        /// <summary>
        /// Converts current Sphere to a database-writeable string
        /// </summary>
        /// <returns>a string containing the data needed to write it to the database</returns>
        internal override string ToDatabaseString()
        {
            StringBuilder sphereAsString = new StringBuilder();
            sphereAsString.Append("Sphere#");
            sphereAsString.Append(ID + "#");
            sphereAsString.Append(Description + "#");
            sphereAsString.Append(Weight + "#");
            sphereAsString.Append(IsFragile + "#");
            sphereAsString.Append(Radius);
            return sphereAsString.ToString();
        }
        /// <summary>
        /// Returns the sphere as a string
        /// </summary>
        /// <returns>a string containing all the information about the sphere</returns>
        public override string ToString()
        {
            StringBuilder returnString = new StringBuilder();
            returnString.AppendLine("Type: Sphere");
            returnString.AppendLine("ID: " + ID);
            returnString.AppendLine("Description: " + Description);
            returnString.AppendLine("Weight: " + Weight + " kg");
            returnString.AppendLine("Fragile: " + (IsFragile ? "Yes" : "No"));
            returnString.AppendLine("Radius: " + Radius + " cm");
            returnString.AppendLine("Area: " + Area + " square cm");
            returnString.AppendLine("Volume: " + Volume + " cubic cm");
            returnString.AppendLine("Max Dimension: " + MaxDimension + " cm");
            return returnString.ToString();
        }
    }
}