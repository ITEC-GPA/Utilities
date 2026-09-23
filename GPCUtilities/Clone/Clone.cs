namespace GPC.Utilities.Clone
{
    public static class Clone
    {
        /// <summary>
        /// Clone the <paramref name="obj"/> by calling <see cref="Serialization.Serialization.SerializeToStream"/> and then deserialiazing it by <see cref="Serialization.Serialization.DeserializeFromStream"/>
        /// </summary>
        /// <returns>The deep clone of <paramref name="obj"/></returns>
        public static object CloneObjectByMemoryStream(object obj)
        {
            return Serialization.Serialization.DeserializeFromStream(Serialization.Serialization.SerializeToStream(obj));
        }
    }
}