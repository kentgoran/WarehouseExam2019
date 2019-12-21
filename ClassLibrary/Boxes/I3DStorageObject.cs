namespace ClassLibrary
{
    /// <summary>
    /// Interface used for objects that wants to be able to be placed in the warehouse
    /// </summary>
    public interface I3DStorageObject
    {
        int ID { get; }
        string Description { get; }
        double Weight { get; }
        int Volume { get; }
        int Area { get; }
        bool IsFragile { get; }
        int MaxDimension { get; }
        int InsuranceValue { get; set; }
    }
}