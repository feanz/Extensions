using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        ///   AddRange of items of same type to IList.
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> The list to be added to. </param>
        /// <param name="itemsToAdd"> The items to be added. </param>
        /// <returns> The new combined list. </returns>
        public static IList<T> AddRange<T>(this IList<T> items, IList<T> itemsToAdd)
        {
            if (items.IsNull() || itemsToAdd.IsNull())
                return items;

            foreach (var item in itemsToAdd)
                items.Add(item);

            return items;
        }

        /// <summary>
        ///   Append the specified bit array to the specific bit array.
        /// </summary>
        /// <param name="current"> The array to be appended to. </param>
        /// <param name="after"> The array to be appended on. </param>
        /// <returns> The new combined bit array. </returns>
        public static BitArray Append(this BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }

        /// <summary>
        ///   Converts a generic List collection to a single string using the specified delimitter.
        /// </summary>
        /// <param name="items"> The items. </param>
        /// <param name="delimiter"> The delimiter. </param>
        /// <returns> The new delimited string. </returns>
        public static string AsDelimited<T>(this IEnumerable<T> items, string delimiter)
        {
            return items.IsNull() ? string.Empty : string.Join(delimiter, items);
        }

        /// <summary>
        ///   Creates a generic items of the specified type from the source items
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> The items to covert. </param>
        /// <returns> New list of generic items. </returns>
        public static IList<T> AsEnumerable<T>(this IEnumerable items)
        {
            return items.IsNotNull() ? (from object item in items select item.As<T>()).ToList() : null;
        }

        /// <summary>
        ///   Enumerates the specified items of items and applies the specified action
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="collection"> The items on which to iterate over. </param>
        /// <param name="action"> The action to be applied to each item </param>
        /// <returns> The list of items just iterated over. </returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection != null)
            {
                foreach (var item in collection) action(item);
                return collection;
            }

            return null;
        }

        /// <summary>
        ///   Validate for any nulls.
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> List of items. </param>
        /// <returns> True if a null is present in the items. </returns>
        public static bool HasAnyNulls<T>(this IEnumerable<T> items) where T : class
        {
            return items.IsTrueForAny(x => x == null);
        }

        /// <summary>
        ///   Indicates if the specified items are empty
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> Enumerable to test </param>
        /// <returns> <c>true</c> if the specified collection is empty; otherwise, <c>false</c> . </returns>
        public static bool IsEmpty<T>(this IEnumerable<T> items)
        {
            items.ValidateNotNull("items");

            var isEmpty = !items.GetEnumerator().MoveNext();
            /* Reset the enumerator back to the starting position in the off
             * chance that we have a very poorly implemented IEnumerable
             * that does not return a *new* enumerator with every invocation
             * of the GetEnumerator method. */
            try
            {
                items.GetEnumerator().Reset();
            }
                // If this method is not supported, just skip the operation
            catch (NotSupportedException)
            {
            }

            return isEmpty;
        }

        /// <summary>
        ///   Indicates if the specified items contains some items
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> Enumerable to test </param>
        /// <returns> </returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> items)
        {
            return !items.IsEmpty();
        }

        /// <summary>
        ///   Determines whether the specified enumerable collection is empty. Note: This method has the side effect of moving the position of the enumerator back to the starting position.
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> Enumerable to test </param>
        /// <returns> <c>true</c> if the specified collection is null or empty; otherwise, <c>false</c> . </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items.IsNull() || items.IsEmpty();
        }

        /// <summary>
        ///   Validate if all of the items in the collection satisfied by the condition.
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> List of items. </param>
        /// <param name="executor"> Function to call for each item. </param>
        /// <returns> True if the executor returned true for all items. </returns>
        public static bool IsTrueForAll<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {
            return items.Select(executor).All(result => result);
        }

        /// <summary>
        ///   Validate if any of the items in the collection satisfied by the condition.
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> List of items to use. </param>
        /// <param name="executor"> Function to call for each item. </param>
        /// <returns> True if the executor returned True for at least one item. </returns>
        public static bool IsTrueForAny<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {
            return items.Select(executor).Any(result => result);
        }

        /// <summary>
        ///   Creates a text items from the specified items.
        /// </summary>
        /// <param name="items"> The items to add to the text. </param>
        /// <param name="newLine"> The new line char to be used. Defaults to '\r\n'. </param>
        /// <returns> The new text list. </returns>
        public static string TextList(this IEnumerable<string> items, string newLine = "\r\n")
        {
            return string.Join(newLine, items.ToArray());
        }

        /// <summary>
        ///   Change items to comma seperated string.
        /// </summary>
        /// <param name="items"> The items to be converted to comma delimted list. </param>
        /// <returns> New comma delimted string of the items provided. </returns>
        public static string ToCommaSeparatedString(this IEnumerable<string> items)
        {
            return AsDelimited(items, ",");
        }

        /// <summary>
        ///   Convert IEnumerable to a datatable
        /// </summary>
        /// <param name="items"> The set of items to be converted to a datatable. </param>
        /// <returns> </returns>
        public static DataTable ToDataTable(this IEnumerable items)
        {
            var dtReturn = new DataTable();
            PropertyInfo[] oProps = null;

            foreach (var rec in items)
            {
                if (dtReturn.Columns.Count == 0)
                {
                    // Use reflection to get property names, to create table
                    // column names
                    var type = rec.GetType();
                    dtReturn.TableName = type.Name;
                    oProps = type.GetProperties();

                    foreach (PropertyInfo pi in oProps)
                    {
                        if (!pi.PropertyType.IsNullableType())
                            dtReturn.Columns.Add(pi.GetName(), pi.PropertyType);
                        else
                        {
                            var converter = new NullableConverter(pi.PropertyType);
                            dtReturn.Columns.Add(pi.GetName(), converter.UnderlyingType);
                        }
                    }
                }

                var dr = dtReturn.NewRow();
                if (oProps != null)
                    foreach (var pi in oProps)
                    {
                        if (!pi.PropertyType.IsNullableType())
                            dr[pi.GetName()] = pi.GetValue(rec, null);
                        else
                        {
                            object obj = pi.GetValue(rec, null);
                            if (obj != null)
                            {
                                var converter = new NullableConverter(pi.PropertyType);
                                dr[pi.GetName()] = Convert.ChangeType(obj, converter.UnderlyingType);
                            }
                        }
                    }
                dtReturn.Rows.Add(dr);
            }

            return (dtReturn);
        }

        /// <summary>
        ///   Converts a items of items to a dictionary with the items.
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> List of items. </param>
        /// <returns> Converted items as dictionary. </returns>
        public static IDictionary<T, T> ToDictionary<T>(this IList<T> items)
        {
            IDictionary<T, T> dict = new Dictionary<T, T>();
            foreach (T item in items)
            {
                dict[item] = item;
            }
            return dict;
        }

        /// <summary>
        ///   Converts string[] array to arry of integers.
        /// </summary>
        /// <param name="source"> The array of strings to convert to integers </param>
        /// <returns> The new integer array. </returns>
        public static int[] ToIntArray(this string[] source)
        {
            return source.Select(x => int.Parse(x, CultureInfo.CurrentCulture)).ToArray();
        }

        /// <summary>
        ///   Validates that the specified items contains at least one item.
        /// </summary>
        /// <typeparam name="T"> Type of items to use. </typeparam>
        /// <param name="items"> The items which we are checking are not empty </param>
        /// <param name="context"> A description to be used in the error errorMessage. Will default to the display name </param>
        /// <returns> The list of items provided. </returns>
        public static IList<T> ValidateNotEmpty<T>(this IList<T> items, string context = "list")
        {
            if (items.IsEmpty())
            {
                throw new Exception(string.Format("The specified {0} was empty.", context ?? items.GetType().GetName()));
            }
            return items;
        }
    }
}