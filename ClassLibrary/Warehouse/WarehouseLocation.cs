﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    /// <summary>
    /// Keeps the data for all the single-locations of boxes. instantiated with the limitations of the location
    /// </summary>
    public class WarehouseLocation : IEnumerable
    {
        private List<Box> boxes = new List<Box>();
        private int height;
        private int width;
        private int depth;
        private int maxVolume;
        private double maxWeight;
        private int currentVolume;
        private double currentWeight;
        private bool containsFragileBox = false;
        internal int Height { get => height; }
        internal int Width { get => width; }
        internal int Depth { get => depth; }
        internal int MaxVolume { get => maxVolume; }
        internal double MaxWeight { get => maxWeight; }
        public List<Box> Boxes { get => boxes; }
        /// <summary>
        /// Constructor for WarehouseLocation, the different parameters should represent the actual locations limitations
        /// </summary>
        /// <param name="height">the height of the location</param>
        /// <param name="width">width of the location</param>
        /// <param name="depth">depth of the location</param>
        /// <param name="maxWeight">the maximal weight the location can handle</param>
        internal WarehouseLocation(int height, int width, int depth, double maxWeight)
        {
            this.height = height;
            this.width = width;
            this.depth = depth;
            this.maxWeight = maxWeight;
            this.maxVolume = height * width * depth;
        }

        /// <summary>
        /// Checks if given box can be added in the current WarehouseLocation
        /// Returns true if it can, else returns false
        /// </summary>
        /// <param name="box">The box that will be tested</param>
        /// <returns>Returns True if it can be added, else returns false</returns>
        private bool BoxCanBeAdded(Box box)
        {
            if (containsFragileBox)
            {
                return false;
            }
            //If the box that is to be added is fragile, it has to be empty
            if(currentVolume != 0 && box.IsFragile)
            {
                return false;
            }
            if(currentVolume + box.Volume > maxVolume)
            {
                return false;
            }
            if(currentWeight + box.Weight > maxWeight)
            {
                return false;
            }
            //If none of the above, return true to indicate that the new box can be added.
            return true;
        }
        /// <summary>
        /// Uses BoxCanBeAdded() to check if the current box can be added.
        /// If it can, adds it to the current WarehouseLocation, and returns true.
        /// Else, returns false.
        /// </summary>
        /// <param name="box">The box to try to add</param>
        /// <returns>True if box has been added, else false</returns>
        internal bool AddBox(Box box)
        {
            if (BoxCanBeAdded(box))
            {
                boxes.Add(box);
                currentVolume += box.Volume;
                currentWeight += box.Weight;
                if (box.IsFragile)
                {
                    containsFragileBox = true;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if given ID-number is present in the WarehouseLocation. If it is, returns true, else false.
        /// </summary>
        /// <param name="id">the ID-number to search for</param>
        /// <returns>True if given ID is found, else false</returns>
        internal bool IDIsPresent(int id)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].ID == id)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Tries to find a box, by given ID-number, does the proper calculations and corrections to the WarehouseLocations, 
        /// returns the box, and removes it from the current WarehouseLocation.
        /// If box can't be found, returns null.
        /// </summary>
        /// <param name="id">The ID-number to search for</param>
        /// <returns>The removed box if successful, else null</returns>
        internal Box RetrieveBoxByID(int id)
        {
            for(int i=0; i<boxes.Count; i++)
            {
                if(boxes[i].ID == id)
                {
                    if (boxes[i].IsFragile)
                    {
                        containsFragileBox = false;
                    }
                    currentVolume -= boxes[i].Volume;
                    currentWeight -= boxes[i].Weight;
                    Box boxToReturn = boxes[i];
                    boxes.RemoveAt(i);
                    return boxToReturn;
                }
            }
            return null;
        }
        /// <summary>
        /// Finds and returns a copy of given box
        /// </summary>
        /// <param name="id">the id of the box to return a copy of</param>
        /// <returns>A box, copied from the box with the id input</returns>
        internal Box RetrieveCopyOfBox(int id)
        {
            for (int i = 0; i < boxes.Count; i++)
            {
                if (boxes[i].ID == id)
                {
                    Box boxToReturn = boxes[i];
                    return boxToReturn;
                }
            }
            return null;
        }
        /// <summary>
        /// Returns a list of boxes currently present in the WarehouseLocation
        /// </summary>
        /// <returns>Returns a list of boxes contained in the WarehouseLocation</returns>
        public List<Box> Content()
        {
            return boxes;
        }
        /// <summary>
        /// Clones the current WarehouseLocation, and returns the clone.
        /// </summary>
        /// <returns>Returns a clone of the current WarehouseLocation</returns>
        internal WarehouseLocation Clone()
        {
            WarehouseLocation clonedVersion = new WarehouseLocation(height, width, depth, maxWeight);
            List<Box> boxesToClone = Content();
            foreach(Box box in boxesToClone)
            {
                clonedVersion.AddBox(box);
            }
            return clonedVersion;
        }
        /// <summary>
        /// Returns an enumerator that iterates through the list of boxes found in the instance
        /// </summary>
        /// <returns>Returns an enumerator that iretates through a list of boxes</returns>
        public IEnumerator GetEnumerator()
        {
            return boxes.GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through the list of boxes found in the instance
        /// </summary>
        /// <returns>Returns an enumerator that iretates through a list of boxes</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return boxes.GetEnumerator();
        }
    }
}
