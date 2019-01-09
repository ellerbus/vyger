using System;
using System.Collections.Generic;
using System.Reflection;
using Augment;

namespace Vyger.Common
{
    public static class Utilities
    {
        /// <summary>
        /// Overlays value type properties
        /// </summary>
        public static void OverlayFrom<T>(this T target, T source, params string[] excluding) where T : class
        {
            IEnumerable<PropertyInfo> properties = GetProperties<T>(excluding);

            foreach (PropertyInfo prop in properties)
            {
                object value = prop.GetValue(source);

                prop.SetValue(target, value);
            }
        }

        private static IEnumerable<PropertyInfo> GetProperties<T>(string[] excluding)
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in properties)
            {
                if (!prop.Name.IsIn(excluding))
                {
                    if (prop.CanRead && prop.CanWrite)
                    {
                        if (IsOverlable(prop.PropertyType))
                        {
                            yield return prop;
                        }
                        else if (prop.PropertyType.IsArray && IsOverlable(prop.PropertyType.GetElementType()))
                        {
                            yield return prop;
                        }
                    }
                }
            }
        }

        private static bool IsOverlable(Type type)
        {
            return type.IsValueType || type.IsEnum || type == typeof(string);
        }
    }
}