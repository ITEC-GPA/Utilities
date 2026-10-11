namespace GPC.Utilities.Serialization
{
    internal static class Obsolescence
    {
        internal const string BinaryFormatter = "BinaryFormatter is unsafe with data from others and removed from .NET 9. "
            + "Use a DataContractSerializer, or ModelValues.CopyValue for the GPC Model values.";
    }
}
