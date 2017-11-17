using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SICER.HELPER
{
    public static class TempDataHelper
    {
        #region HasKey

        public static bool HasKey(this TempDataDictionary TempData, TempDataKey Key)
        {
            var temp = TempData.Get(Key);

            if (temp == null)
                return false;

            return true;
        }

        #endregion

        #region Get & Set

        public static object Get(this TempDataDictionary TempData, TempDataKey Key)
        {
            return TempData[Key.ToString()];
        }

        public static void Set(this TempDataDictionary TempData, TempDataKey Key, object value)
        {
            TempData[Key.ToString()] = value;
        }

        #endregion


        #region Custom Keep

        public static void Keep(this TempDataDictionary TempData, TempDataKey Key)
        {
            var value = TempData.Get(Key);

            if (value != null)
            {
                TempData.Set(Key, value);
            }
        }

        #endregion
    }
}
