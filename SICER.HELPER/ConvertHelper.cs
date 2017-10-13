using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICER.HELPER
{
    public static class ConvertHelper
    {
        public static String ToSafeString(this object val)
        {
            return (val ?? String.Empty).ToString();
        }

        #region Byte Array Helper
        public static byte[] GetBytes(this String str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        #endregion


        #region JsonParse for equality of objects
        public class JSonEqualityComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return String.Equals
                (
                    Newtonsoft.Json.JsonConvert.SerializeObject(x),
                    Newtonsoft.Json.JsonConvert.SerializeObject(y)
                );
            }

            public int GetHashCode(T obj)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj).GetHashCode();
            }
        }

        public static IEnumerable<T> ExceptUsingJSonCompare<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return first.Except(second, new JSonEqualityComparer<T>());
        }

        #endregion
    }
}
