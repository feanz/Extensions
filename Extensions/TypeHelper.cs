using System;
using System.Collections.Generic;

namespace Utilities
{
    public static class TypeHelper
    {
        public static IDictionary<string, bool> BasicTypes()
        {
            IDictionary<string, bool> basicTypes = new Dictionary<string, bool>();

            basicTypes[typeof (int).Name] = true;
            basicTypes[typeof (long).Name] = true;
            basicTypes[typeof (float).Name] = true;
            basicTypes[typeof (double).Name] = true;
            basicTypes[typeof (decimal).Name] = true;
            basicTypes[typeof (sbyte).Name] = true;
            basicTypes[typeof (Int16).Name] = true;
            basicTypes[typeof (Int32).Name] = true;
            basicTypes[typeof (Int64).Name] = true;
            basicTypes[typeof (Double).Name] = true;
            basicTypes[typeof (Decimal).Name] = true;
            basicTypes[typeof (bool).Name] = true;
            basicTypes[typeof (DateTime).Name] = true;
            basicTypes[typeof (string).Name] = true;

            return basicTypes;
        }

        public static IDictionary<string, bool> NumericTypes()
        {
            IDictionary<string, bool> numericTypes = new Dictionary<string, bool>();

            numericTypes[typeof (int).Name] = true;
            numericTypes[typeof (long).Name] = true;
            numericTypes[typeof (float).Name] = true;
            numericTypes[typeof (double).Name] = true;
            numericTypes[typeof (decimal).Name] = true;
            numericTypes[typeof (sbyte).Name] = true;
            numericTypes[typeof (Int16).Name] = true;
            numericTypes[typeof (Int32).Name] = true;
            numericTypes[typeof (Int64).Name] = true;
            numericTypes[typeof (Double).Name] = true;
            numericTypes[typeof (Decimal).Name] = true;

            return numericTypes;
        }
    }
}