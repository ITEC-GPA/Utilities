using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GPC.Utilities.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum instance)
        {
            Type genericEnumType = instance.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(instance.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return instance.ToString();
        }

        public static IEnumerable<EnumValueDescription> GetAllValuesAndDescriptions(Type t)
        {
            if (!t.IsEnum)
                throw new ArgumentException($"{nameof(t)} must be an enum type");

            return Enum.GetValues(t).Cast<Enum>().Select((e) => new EnumValueDescription() { Value = e, Description = e.GetDescription() }).ToList();
        }
    }

    public class EnumValueDescription
    {
        public object Value { get; set; }
        public object Description { get; set; }
    }
}
