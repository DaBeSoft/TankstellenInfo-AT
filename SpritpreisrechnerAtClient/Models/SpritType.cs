using System;
using System.Linq;
using System.Reflection;

namespace SpritpreisrechnerAtClient.Models
{
    public enum SpritType
    {
        [StringValue("DIE")]
        Diesel,
        [StringValue("SUP")]
        Super,
    }

    internal class StringValueAttribute : Attribute
    {
        private readonly string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }

    internal static class EnumEx
    {
        public static string GetStringValue(this Enum value)
        {
            string output = null;
            var type = value.GetType();

            var fi = type.GetTypeInfo().DeclaredFields.First(t => t.Name == value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs != null && attrs.Length > 0)
                output = attrs[0].Value;

            return output;
        }
    }
}
