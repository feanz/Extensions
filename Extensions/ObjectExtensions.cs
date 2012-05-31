using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        ///   Casts the specified instance as the specified type
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="instance"> The object to convert to type. </param>
        /// <returns> Object converted to supplied type. </returns>
        public static T As<T>(this object instance)
        {
            if (instance.IsNotNull())
            {
                //If its an enum
                if (typeof (T).IsEnum)
                    return (T) Enum.Parse(typeof (T), instance.ToString(), true);

                //If it is IConvertable
                if (instance.Is<IConvertible>())
                    return (T) Convert.ChangeType(instance, typeof (T));

                //If assignable from cast
                if (instance is T)
                    return (T) instance;

                //Try and get type converter from type description
                var converter = TypeDescriptor.GetConverter(instance.GetType());
                if (converter.CanConvertTo(typeof (T)))
                    return (T) converter.ConvertTo(instance, typeof (T));
            }
            return default(T);
        }

        /// <summary>
        ///   Parses the source instance as the specified enumeration
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="instance"> The object to convert to enum. </param>
        /// <param name="ignoreCase"> Flag determining whether we should ignore case. </param>
        /// <returns> An Enum of supplied type. </returns>
        public static T AsEnum<T>(this object instance, bool ignoreCase = true)
        {
            if (instance.IsNotNull())
            {
                return (T) Enum.Parse(typeof (T), instance.ToString(), ignoreCase);
            }
            return default(T);
        }

        /// <summary>
        ///   Determines if the specified instance is of the specified type
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="instance"> The object to check. </param>
        /// <returns> <c>true</c> If object is instance of type. <c>false</c> If object is not instance of type. </returns>
        public static bool Is<T>(this object instance)
        {
            var checkType = typeof (T);
            var instanceType = (instance is Type ? (Type) instance : instance.GetType());
            if (checkType.IsInterface)
            {
                return instanceType == checkType || instanceType.GetInterface(checkType.Name).IsNotNull();
            }
            return instanceType == checkType || instanceType.IsSubclassOf(checkType);
        }

        /// <summary>
        ///   Determines if the object is null.
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="instance"> The object to check. </param>
        /// <returns> <c>True</c> If it is not default value <c>False</c> if it is default value. </returns>
        public static bool IsDefault<T>(this T instance)
        {
            return instance.ValidateNotNull("Object").Equals(default(T));
        }

        /// <summary>
        ///   Determines if the specified instance is not of the specified type
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="instance"> The object to check. </param>
        /// <returns> <c>true</c> If object is instance of type. <c>false</c> If object is not instance of type. </returns>
        public static bool IsNot<T>(this object instance)
        {
            return !Is<T>(instance);
        }

        /// <summary>
        ///   Determines if the object is not default value.
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="instance"> The object to check. </param>
        /// <returns> <c>True</c> If it is the default value <c>False</c> if it is not default value. </returns>
        public static bool IsNotDefault<T>(this T instance)
        {
            return !instance.ValidateNotNull("Object").Equals(default(T));
        }

        /// <summary>
        ///   Determines if the specified instance is not null
        /// </summary>
        /// <param name="instance"> Object to check. </param>
        /// <returns> <c>true</c> If object is not null. <c>false</c> If object is null. </returns>
        public static bool IsNotNull(this object instance)
        {
            return !instance.IsNull();
        }

        /// <summary>
        ///   Determines if the source instance is not equal to zero
        /// </summary>
        /// <param name="instance"> Object to check. </param>
        /// <returns> <c>true</c> If object is not zero. <c>false</c> If object is zero. </returns>
        public static bool IsNotZero(this object instance)
        {
            return !instance.IsZero();
        }

        /// <summary>
        ///   Determines if the specified instance is null
        /// </summary>
        /// <param name="instance"> Object to check. </param>
        /// <returns> <c>true</c> If object is null. <c>false</c> If object is not null. </returns>
        public static bool IsNull(this object instance)
        {
            return instance == null;
        }

        /// <summary>
        ///   Determines whether the supplied object is of a numeric type.
        /// </summary>
        /// <param name="val"> Object to check. </param>
        /// <returns> True if the supplied object is of a numeric type. </returns>
        public static bool IsNumeric(this object val)
        {
            return TypeHelper.NumericTypes().ContainsKey(val.GetType().Name);
        }

        /// <summary>
        ///   Determines if the source instance is equal to zero
        /// </summary>
        /// <param name="instance"> Object to check. </param>
        /// <returns> <c>true</c> If object is zero. <c>false</c> If object is not zero. </returns>
        public static bool IsZero(this object instance)
        {
            return instance.IsNotNull() && instance.ToString().Equals("0");
        }

        /// <summary>
        ///   Returns a string representation using the Current culture
        /// </summary>
        /// <param name="instance"> </param>
        /// <returns> </returns>
        public static string ToStringCurrentCulture(this object instance)
        {
            return instance.ToString().ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   Returns a string representation using an invariant culture
        /// </summary>
        /// <param name="instance"> </param>
        /// <returns> </returns>
        public static string ToStringInvariantCulture(this object instance)
        {
            return instance.ToString().ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///   Serialize object to XML
        /// </summary>
        /// <param name="toBeSerialized"> </param>
        /// <returns> </returns>
        public static string ToXML(this object toBeSerialized)
        {
            var fullName = toBeSerialized.GetType().FullName;
            if (fullName != null && !fullName.Contains("AnonymousType"))
            {
                var serializer = new XmlSerializer(toBeSerialized.GetType());

                using (var sw = new StringWriter())
                {
                    serializer.Serialize(sw, toBeSerialized);
                    return sw.ToString();
                }
            }

            // Create the default root XML element
            var root = new XElement("AnonymousTypeRoot");

            // Start to build the child XML elements by recursion method
            BuildXmlElement(toBeSerialized, ref root);

            return root.ToString();
        }

        private static void BuildXmlElement(object obj, ref XElement element)
        {
            if (obj.IsNull())
                return;

            // Get all properyInfo

            var propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pi in propertyInfos)
            {
                // Get the property value
                var o = pi.GetValue(obj, null);

                if (o != null)
                {
                    Type t = o.GetType();

                    // If the property value is an array, retrieve each object value inside
                    if (t.IsArray)
                    {
                        // Build the element with the property's name
                        var newElement = new XElement(pi.Name);

                        var arrs = o as object[];

                        for (var i = 0; i < arrs.Length; i++)
                        {
                            var arrayElement = new XElement("Element");

                            // For each the array element, build the child XML elements
                            BuildXmlElement(arrs[i], ref arrayElement);
                            newElement.Add(arrayElement);
                        }

                        element.Add(newElement);
                    }

                    else
                    {
                        // For the anonymous type and other entity class type
                        if (t.IsClass && t.Name != "String")
                        {
                            var newElement = new XElement(pi.Name);
                            BuildXmlElement(o, ref newElement);
                            element.Add(newElement);
                        }

                            // For other value type and string type, build the XML element
                        else
                        {
                            var newElement = new XElement(pi.Name, o.ToString());
                            element.Add(newElement);
                        }
                    }
                }
            }
        }

        #region Validation

        /// <summary>
        ///   Validate that the supplied object is one of a items of objects.
        /// </summary>
        /// <typeparam name="T"> Type of object to check. </typeparam>
        /// <param name="obj"> Object to look for. </param>
        /// <param name="possibles"> List with possible values for object. </param>
        /// <returns> True if the object is equal to one in the supplied items. Otherwise, <see cref="ArgumentException" /> is thrown. </returns>
        public static bool ValidateIsOneOfSupplied<T>(this T obj, List<T> possibles)
        {
            return ValidateIsOneOfSupplied(obj, possibles, "The object does not have one of the supplied values.");
        }

        /// <summary>
        ///   Validate that the supplied object is one of a items of objects.
        /// </summary>
        /// <typeparam name="T"> Type of object to check. </typeparam>
        /// <param name="instance"> Object to look for. </param>
        /// <param name="possibles"> List with possible values for object. </param>
        /// <param name="errorMessage"> Message of exception to throw. </param>
        /// <returns> True if the object is equal to one in the supplied items. Otherwise, <see cref="ArgumentException" /> is thrown. </returns>
        public static bool ValidateIsOneOfSupplied<T>(this T instance, List<T> possibles, string errorMessage)
        {
            foreach (T possible in possibles)
                if (possible.Equals(instance))
                    return true;
            throw new ValidationException(errorMessage);
        }

        /// <summary>
        ///   Validates that the specified source instance is not null.
        /// </summary>
        /// <param name="instance"> Object to check. </param>
        /// <param name="paramName"> name of parameter </param>
        /// <param name="errorMessage"> Description of the object being validated that will be added to error errorMessage. </param>
        /// <returns> Object supplied </returns>
        public static T ValidateNotNull<T>(this T instance, string paramName, string errorMessage = "")
        {
            if (instance.IsNull())
            {
                if (errorMessage.IsNotNullOrEmpty())
                    throw new ValidationException(errorMessage, paramName);
                throw new ValidationException("The {0} was null and failed validation.".Format(new {paramName}),
                                              paramName);
            }
            return instance;
        }

        /// <summary>
        ///   Validates that the specified instance is not zero
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="instance"> Object to check. </param>
        /// <param name="paramName"> name of parameter </param>
        /// <param name="errorMessage"> Description of the object being validated that will be added to error errorMessage. </param>
        /// <returns> Object supplied </returns>
        public static T ValidateNotZero<T>(this T instance, string paramName, string errorMessage = "")
        {
            instance.ValidateNotNull(errorMessage);
            if (instance.ToString().Equals("0"))
            {
                if (errorMessage.IsNotNullOrEmpty())
                    throw new ValidationException(errorMessage, paramName);
                else
                    throw new ValidationException("The {0} was zero and failed validation.".Format(new {paramName}),
                                                  paramName);
            }
            return instance;
        }

        #endregion
    }
}