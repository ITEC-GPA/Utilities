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
				list.Add(new KeyValuePair<string, int>(e.ToString(), (int)e));
			}
			return list;
		}
	}
}