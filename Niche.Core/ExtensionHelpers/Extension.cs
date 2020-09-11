using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Niche.Core.ExtensionHelpers
{
    public static class Extension
    {
        public static string UrlEncode(this string value)
        {
            value = value.Replace("\'", "%27");
            value = value.Replace(" ", "-");
            return value;
        }
        public static string UrlDecode(this string value)
        {
            value = value.Replace("%27", "\'");
            value = value.Replace("-", " ");
            return value;
        }

        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj);
        public static T FromJson<T>(this string value) => JsonConvert.DeserializeObject<T>(value);
    }
    public class Helpers
    {
        public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly int Base = Alphabet.Length;

        public static string Encode(int i)
        {
            if (i == 0) return Alphabet[0].ToString();

            var s = new StringBuilder();// string.Empty;

            while (i > 0)
            {
                s.Insert(0, Alphabet[i % Base]);
                i = i / Base;
            }

            return s.ToString();
        }

        public static int Decode(string s)
        {
            var i = 0;

            foreach (var c in s)
            {
                i = (i * Base) + Alphabet.IndexOf(c);
            }

            return i;
        }

        private static void Main_()
        {
            // Simple test of encode/decode operations
            for (var i = 0; i < 10000; i++)
            {
                if (Decode(Encode(i)) != i)
                {
                    System.Console.WriteLine("{0} is not {1}", Encode(i), i);
                    break;
                }
            }
        }
        public static string Zip(string value)
        {
            //Transform string into byte[]
            byte[] byteArray = new byte[value.Length];
            int indexBA = 0;
            foreach (char item in value.ToCharArray())
            {
                byteArray[indexBA++] = (byte)item;
            }

            //Prepare for compress
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Compress);

            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            //Transform byte[] zip data to string
            byteArray = ms.ToArray();
            System.Text.StringBuilder sB = new System.Text.StringBuilder(byteArray.Length);
            foreach (byte item in byteArray)
            {
                sB.Append((char)item);
            }
            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return sB.ToString();
        }
        public static string Encrypt(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        public static string encodeNumber(int num)
        {
            if (num < 0) //or throw an exception
                return "";
            string alphanums = "0123456789abcdefghijklmnopqrstuvwxyz";
            int[] nums = new int[num.ToString().Length];
            int pos = 0;

            while (!(num == 0))
            {
                nums[pos] = num % nums.Length;
                num /= alphanums.Length;
                pos += 1;
            }

            string result = "";
            for (int i = 0; i < alphanums.Length; i++)
                result = alphanums[nums[i]].ToString() + result;

            return result;
        }

        public static string Encrypt(int id)
        {
            var encodedString = Encode(id);
            return encodedString;
        }
        public static int Decrypt(string txt)
        {
            var encodedInt = Decode(txt);
            return encodedInt;
        }
        public static int GetID()
        {
            var date = DateTime.UtcNow;
            var year = date.Year;
            var day = date.Day;
            var month = date.Month;
            var hour = date.Hour;
            var min = date.Minute;
            var sec = date.Second;
            var mill = date.Millisecond;

            var newId = year + month + day + hour + min + sec + mill;
            return newId;
        }
    }
}
