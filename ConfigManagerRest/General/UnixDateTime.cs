using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerRest.General
{
    public static class UnixDateTime
    {
        private static DateTime initialUnixDatTime = Convert.ToDateTime("1970-01-01 00:00:00");

        #region UnixTime
        internal static dynamic UnixTime(DateTime datetime = default(DateTime))
        {
            return (Int64)(datetime.Subtract(initialUnixDatTime)).TotalSeconds;
        }
        #endregion

        #region ClearUnixTime
        internal static dynamic ClearUnixTime(Int64 UnixTime)
        {
            return initialUnixDatTime.AddSeconds(UnixTime);
        }
        #endregion
    }
}
