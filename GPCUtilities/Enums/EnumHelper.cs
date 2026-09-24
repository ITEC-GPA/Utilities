using System;
using System.Collections.Generic;

namespace GPC.Utilities.Enums
{
	public static class EnumHelper
	{
		/// <summary>
		/// Get a collection of KeyValuePair with the name and the value of the enum items
		/// </summary>
		/// <typeparam name="T">The enum</typeparam>
		/// <returns>The dictionaty with names and values</returns>
		public static ICollection<KeyValuePair<string, int>> GetEnumList<T>() where T : Enum
		{
			var list = new List<KeyValuePair<string, int>>();
			foreach (var e in Enum.GetValues(typeof(T)))
			{
				// Convert.ToInt32 also works with enums whose underlying type is not int (byte, short, long...)
				list.Add(new KeyValuePair<string, int>(e.ToString(), Convert.ToInt32(e)));
			}
			return list;
		}
	}
}