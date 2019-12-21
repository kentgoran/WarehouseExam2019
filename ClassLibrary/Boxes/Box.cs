using System;

namespace ClassLibrary
{
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
