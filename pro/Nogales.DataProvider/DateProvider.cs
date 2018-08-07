using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider
{
    public static class DateProvider
    {
        public static DateTime StartOfWeek(this DateTime dt)
        {
            DayOfWeek startOfWeek = DayOfWeek.Sunday;
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }


        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1).Date;
        }

        public static DateTime StartOfQuarter(this DateTime dt)
        {
            int quarter = (dt.Month + 2) / 3;
            return new DateTime(dt.Year, quarter == 1 ? 1 : quarter == 2 ? 4 : quarter == 3 ? 7 : 10, 1).Date;
        }

        public static DateTime StartOfLastQuarter(this DateTime dt)
        {
            int quarter = (dt.Month + 2) / 3;

            return new DateTime(quarter == 1 ? dt.AddYears(-1).Year : dt.Year, quarter == 1 ? 10 : quarter == 2 ? 1 : quarter == 3 ? 4 : 7, 1).Date;
        }

        public static DateTime StartOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1).Date;
        }
    }
}
