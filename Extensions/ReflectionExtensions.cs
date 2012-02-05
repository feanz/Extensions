using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        ///   Gets the specified attributeToFind from the source property
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="property"> The property info who's attributeToFind we are fetching. </param>
        /// <param name="inherit"> Specifies whether to search this member's inheritance chain to find the attributes. </param>
        /// <returns> The attributes of supplied property info. </returns>
        public static T GetAttribute<T>(this PropertyInfo property, bool inherit = true) where T : Attribute
        {
            if (property.IsNotNull())
            {
                return
                    property.GetCustomAttributes(typeof (T), inherit).ValidateNotEmpty("attributeToFind").First().As<T>();
            }
            return null;
        }

        /// <summary>
        ///   Get the DisplayName of the provided property info item if it has one else return the property name
        /// </summary>
        /// <param name="type"> Type of items to use. </param>
        /// <returns> The DisplayName attributeToFind of type or TyneName if no display name. </returns>
        public static string GetName(this Type type)
        {
            var name = type.Name;
            var displayName = type.GetCustomAttributes(typeof (DisplayNameAttribute), true);
            if (displayName.Any())
                name = ((DisplayNameAttribute) displayName.First()).DisplayName;

            return name;
        }

        /// <summary>
        ///   Get the DisplayName of the provided property info item if it has one else return the property name
        /// </summary>
        /// <returns> The DisplayName attributeToFind of property or property name if no display name. </returns>
        public static string GetName(this PropertyInfo pi)
        {
            var name = pi.Name;
            var displayName = pi.GetCustomAttributes(typeof (DisplayNameAttribute), true);
            if (displayName.Any())
                name = ((DisplayNameAttribute) displayName.First()).DisplayName;

            return name;
        }

        /// <summary>
        ///   Get the properties of a type which have an attributeToFind
        /// </summary>
        /// <param name="t"> A class type. </param>
        /// <returns> List of properties </returns>
        public static List<PropertyInfo> GetPropertyWithAttribute<T>(this Type t) where T : Attribute
        {
            return t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttributes(typeof (T), false).Count() == 1).ToList();
        }

        /// <summary>
        ///   Gets the specified property value from the source object
        /// </summary>
        /// <param name="instance"> Object which we want to get property value for. </param>
        /// <param name="propertyName"> The name of the property we want to get. </param>
        /// <returns> </returns>
        public static object GetValue(this object instance, string propertyName)
        {
            if (instance.IsNotNull())
            {
                return instance
                    .GetType()
                    .GetProperty(propertyName)
                    .ValidateNotNull("property")
                    .GetValue(instance, null);
            }
            return null;
        }

        /// <summary>
        ///   Gets the specified property value from the source object as the specified type
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="instance"> Object which we want to get property value for. </param>
        /// <param name="propertyName"> The name of the property we want to get. </param>
        /// <returns> </returns>
        public static T GetValue<T>(this object instance, string propertyName)
        {
            if (instance.IsNotNull())
            {
                return instance
                    .GetType()
                    .GetProperty(propertyName)
                    .ValidateNotNull("property")
                    .GetValue(instance, null)
                    .As<T>();
            }
            return default(T);
        }

        /// <summary>
        ///   Verifies that the source property has the specified generic attributeToFind
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="property"> The property to check. </param>
        /// <param name="inhert"> Specifies whether to search this member's inheritance chain to find the attributes. </param>
        /// <returns> </returns>
        public static bool HasAttribute<T>(this PropertyInfo property, bool inhert = true) where T : Attribute
        {
            return (property.GetCustomAttributes(typeof (T), true).Length > 0);
        }

        /// <summary>
        ///   Checks if type is nullable
        /// </summary>
        /// <param name="type"> Type to check </param>
        /// <returns> </returns>
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType
                   && type.GetGenericTypeDefinition() == typeof (Nullable<>);
        }

        /// <summary>
        ///   Checks if property is nullable
        /// </summary>
        /// <param name="propertyInfo"> Property to check </param>
        /// <returns> </returns>
        public static bool IsNullableType(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsGenericType
                   && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>);
        }

        /// <summary>
        ///   Determines whether objects of the supplied type are numeric.
        /// </summary>
        /// <param name="type"> Type to check. </param>
        /// <returns> True if objects of the supplied type are numeric. </returns>
        public static bool IsNumeric(this Type type)
        {
            return TypeHelper.NumericTypes().ContainsKey(type.Name);
        }

        /// <summary>
        ///   Sets the specified property to the specified value on the source object
        /// </summary>
        /// <param name="instance"> Object whose property will be set. </param>
        /// <param name="propertyName"> Property name to set. </param>
        /// <param name="value"> Property value to set. </param>
        /// <returns> </returns>
        public static object SetProperty(this object instance, string propertyName, object value)
        {
            if (instance.IsNotNull())
            {
                instance
                    .GetType()
                    .GetProperty(propertyName)
                    .ValidateNotNull("property")
                    .SetValue(instance, value, null);
            }
            return instance;
        }

        /// <summary>
        ///   Sets the specified property to the specified value on the source object
        /// </summary>
        /// <param name="instance"> Object whose property will be set. </param>
        /// <param name="propertyName"> Property name to set. </param>
        /// <param name="value"> Property value to set. </param>
        /// <returns> </returns>
        public static object SetProperty(this object instance, string propertyName, string value)
        {
            if (instance.IsNotNull())
            {
                // Remove spaces.
                propertyName = propertyName.Trim();
                propertyName.ValidateNotNullOrEmpty("Property Name");

                Type type = instance.GetType();
                PropertyInfo propertyInfo = type.GetProperty(propertyName);

                // Correct property with write access 
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    // Validate same Type
                    if (ReflectionTypeChecker.CanConvertToCorrectType(propertyInfo, value))
                    {
                        object convertedVal = ReflectionTypeChecker.ConvertToSameType(propertyInfo, value);
                        propertyInfo.SetValue(instance, convertedVal, null);
                    }
                }

                return instance;
            }
            throw new ArgumentException("Can't set property of null object");
        }

        /// <summary>
        ///   Set the object properties using the prop name and value.
        /// </summary>
        /// <typeparam name="T"> A class type. </typeparam>
        /// <param name="instance"> Object whose property will be set. </param>
        /// <param name="propertyName"> Property name to set. </param>
        /// <param name="value"> Property value to set. </param>
        public static object SetProperty<T>(this object instance, string propertyName, object value) where T : class
        {
            if (instance.IsNotNull())
            {
                // Remove spaces.
                propertyName = propertyName.Trim();
                propertyName.ValidateNotNullOrEmpty("Property Name");

                var type = instance.GetType();
                var propertyInfo = type.GetProperty(propertyName);

                // Correct property with write access 
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    // Validate same Type
                    if (ReflectionTypeChecker.CanConvertToCorrectType(propertyInfo, value))
                    {
                        var convertedVal = ReflectionTypeChecker.ConvertToSameType(propertyInfo, value);
                        propertyInfo.SetValue(instance, convertedVal, null);
                    }
                }

                return instance;
            }
            throw new ArgumentException("Can't set property of null object");
        }
    }
}