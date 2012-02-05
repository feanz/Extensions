using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Extensions
{
    public static class ByteExtensions
    {
        #region CompressionType enum

        public enum CompressionType
        {
            Deflate = 0,
            GZip = 1
        }

        #endregion

        /// <summary>
        ///   Appends the specified data to the source byte array
        /// </summary>
        /// <param name="data"> Byte array to be appended </param>
        /// <param name="toAppend"> Byte array to append. </param>
        /// <returns> The new combined byte array </returns>
        public static byte[] Append(this byte[] data, byte[] toAppend)
        {
            if (data.IsNotNull())
            {
                var buffer = new byte[data.Length + toAppend.Length];
                Array.Copy(data, buffer, data.Length);
                Array.Copy(toAppend, 0, buffer, data.Length, toAppend.Length);
                return buffer;
            }
            return data;
        }

        /// <summary>
        ///   Compresses the data using the specified compression type
        /// </summary>
        /// <param name="data"> data to compress </param>
        /// <param name="compressionType"> Compression type </param>
        /// <returns> The compressed data </returns>
        public static byte[] Compress(this byte[] data, CompressionType compressionType = CompressionType.Deflate)
        {
            data.ValidateNotNull("data");
            using (var stream = new MemoryStream())
            {
                using (var zipStream = GetStream(stream, CompressionMode.Compress, compressionType))
                {
                    zipStream.Write(data, 0, data.Length);
                    zipStream.Close();
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        ///   Compresses a string of data
        /// </summary>
        /// <param name="data"> data to Compress </param>
        /// <param name="encodingUsing"> Encoding that the data uses (defaults to UTF8) </param>
        /// <param name="compressionType"> The compression type used </param>
        /// <returns> The data Compressed </returns>
        public static string Compress(this string data, Encoding encodingUsing = null,
                                      CompressionType compressionType = CompressionType.Deflate)
        {
            data.ValidateNotNull("data");
            return data.ToByteArray(encodingUsing).Compress(compressionType).ToBase64String();
        }

        /// <summary>
        ///   Decompresses the byte array that is sent in
        /// </summary>
        /// <param name="data"> data to decompress </param>
        /// <param name="compressionType"> The compression type used </param>
        /// <returns> The data decompressed </returns>
        public static byte[] Decompress(this byte[] data, CompressionType compressionType = CompressionType.Deflate)
        {
            data.ValidateNotNull("data");
            using (var stream = new MemoryStream())
            {
                using (var zipStream = GetStream(new MemoryStream(data), CompressionMode.Decompress, compressionType))
                {
                    var buffer = new byte[4096];
                    while (true)
                    {
                        int size = zipStream.Read(buffer, 0, buffer.Length);
                        if (size > 0) stream.Write(buffer, 0, size);
                        else break;
                    }
                    zipStream.Close();
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        ///   Decompresses a string of data
        /// </summary>
        /// <param name="data"> data to decompress </param>
        /// <param name="encodingUsing"> Encoding that the result should use (defaults to UTF8) </param>
        /// <param name="compressionType"> The compression type used </param>
        /// <returns> The data decompressed </returns>
        public static string Decompress(this string data, Encoding encodingUsing = null,
                                        CompressionType compressionType = CompressionType.Deflate)
        {
            data.ValidateNotNull("data");
            return data.Base64ToByteArray().Decompress(compressionType).ToEncodedString(encodingUsing);
        }

        /// <summary>
        ///   Determines if a byte array is unicode
        /// </summary>
        /// <param name="data"> Input array </param>
        /// <returns> True if it's unicode, false otherwise </returns>
        public static bool IsUnicode(this byte[] data)
        {
            return data.IsNull() || data.ToEncodedString(new UnicodeEncoding()).IsUnicode();
        }

        /// <summary>
        ///   Converts a byte array into a base 64 string
        /// </summary>
        /// <param name="data"> Input array </param>
        /// <returns> The equivalent byte array in a base 64 string </returns>
        public static string ToBase64String(this byte[] data)
        {
            return data.IsNull() ? "" : Convert.ToBase64String(data);
        }

        /// <summary>
        ///   Convert byte array to a binary representation as a string
        /// </summary>
        /// <param name="data"> The byte array to be converted to a string </param>
        /// <returns> A binary string that represents the byte array </returns>
        public static string ToBinary(this byte[] data)
        {
            var bits = new BitArray(data);
            var output = string.Empty;

            var octets = bits.Length/8;
            for (var o = 0; o < octets; o++)
            {
                for (var b = 7; b >= 0; b--)
                {
                    var index = (o*8) + b;
                    output += (bits[index] ? "1" : "0");
                }
            }

            return output;
        }

        /// <summary>
        ///   Converts a byte array to a string
        /// </summary>
        /// <param name="data"> input array </param>
        /// <param name="encodingUsing"> The type of encoding the string is using (defaults to UTF8) </param>
        /// <param name="index"> The index of the first byte to decode </param>
        /// <param name="count"> Number of bytes starting at the index to convert (use -1 for the entire array starting at the index) </param>
        /// <returns> string of the byte array </returns>
        public static string ToEncodedString(this byte[] data, Encoding encodingUsing = null, int index = 0,
                                             int count = -1)
        {
            if (data.IsNull())
                return "";
            if (count == -1)
                count = data.Length - index;
            encodingUsing = encodingUsing ?? new UTF8Encoding();
            return encodingUsing.GetString(data, index, count);
        }

        /// <summary>
        ///   Converts a byte array to a hexadecimal string representation.
        /// </summary>
        /// <param name="data"> Byte array to convert to hexadecimal string. </param>
        /// <returns> String representation of byte array. </returns>
        public static string ToHex(this byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "");
        }

        /// <summary>
        ///   Convert byte to nibble (4 bit value)
        /// </summary>
        /// <param name="data"> the byte to be converted to a nibble </param>
        /// <returns> A nibble of the byte supplied </returns>
        public static byte ToHexNibble(this byte data)
        {
            if (data >= 48 && data <= 57)
                return (byte) (data & 15);
            if (data >= 65 && data <= 70)
                return (byte) (data - 55);
            if (data >= 97 && data <= 102)
                return (byte) (data - 87);

            return 0;
        }

        /// <summary>
        ///   Convert byte array to a 32 bit int
        /// </summary>
        /// <param name="data"> The byte array to be converted </param>
        /// <returns> An int representation of the byte array </returns>
        public static int ToInt32(this byte[] data)
        {
            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        ///   Convert a byte array into a string with each byte represented as nibbles
        /// </summary>
        /// <param name="data"> The byte array to be converted to a string of nibbles </param>
        /// <returns> A string made up of nibbles for each byte </returns>
        public static string ToNibbleString(this byte[] data)
        {
            var temp = string.Empty;
            if (data.IsNotNull())
            {
                for (var i = 0; i < data.Length; i++)
                {
                    temp += data[i].ToHexNibble();
                }
            }
            return temp;
        }

        private static Stream GetStream(MemoryStream stream, CompressionMode mode, CompressionType compressionType)
        {
            if (compressionType == CompressionType.Deflate)
                return new DeflateStream(stream, mode, true);
            return new GZipStream(stream, mode, true);
        }
    }
}