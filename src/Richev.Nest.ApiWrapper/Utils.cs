using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Richev.Nest.ApiWrapper
{
    /// <summary>
    /// General-purpose utility functions
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Gets the value of the EnumMember attribute for this enum value (assuming that it has an EnumMemberAttribute).
        /// </summary>
        public static string ToEnumString<T>(T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }
    }
}
