using System;

namespace ClassLibrary
{
    /// <summary>
    /// Abstract class Box, inheriting from I3DStorageObject.
    /// This class is to be inherited by all classes that want's to be able to be placed in a WarehouseLocation
    /// </summary>
    public abstract class Box : I3DStorageObject
    {
        private int id;
        private string description;
        private double weight;
        internal int volume;
        internal int area;
        private bool isFragile;
        internal int maxDimension;
        private int insuranceValue;

        public int ID { get => id; }
        public string Description { get => description; }
        public double Weight { get => weight; }
        public int Volume { get => volume; }
        public int Area { get => area; }
        public bool IsFragile { get => isFragile; }
        public int MaxDimension { get => maxDimension; }
        public int InsuranceValue { get => insuranceValue; set => insuranceValue = value; }
        /// <summary>
        /// Constructs a new box
        /// </summary>
        /// <param name="id">The box's ID-number</param>
        /// <param name="description">A short description of the box</param>
        /// <param name="weight">The box's weight, in kilograms</param>
        /// <param name="isFragile">A boolean, describing if the box is fragile</param>
        internal Box(int id, string description, double weight, bool isFragile)
        {
            this.id = id;
            //Enforcing safety from descriptions containing #, as # is used in the database
            if (description.Contains("#"))
            {
                description.Replace("#", "%");
            }
            this.description = description;
            this.weight = weight;
            this.isFragile = isFragile;
        }

        internal abstract int CalculateVolume();
        internal abstract int CalculateArea();
        internal abstract int FindMaxDimension();
        internal abstract string ToDatabaseString();
        public abstract override string ToString();
    }
}
