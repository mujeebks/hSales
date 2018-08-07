using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nogales.DataProvider.Utilities
{
    public static class UtilityExtensions
    {

        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public static string ToShortMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }


        public static double ToRoundTwoDigits(this double amount)
        {
            return Math.Round(amount, 2);
        }
        public static double ToRoundAndFormat(this decimal amount)
        {
            return Convert.ToDouble(Math.Round(amount));
        }

        public static double ToScaleDownAndRoundTwoDigits(this double amount)
        {
            return Math.Round(amount * (5 / 3), 2);
        }


        public static double ToRoundTwoDecimalDigits(this decimal amount)
        {
            return (double)Decimal.Round(amount, 2);
        }

        public static double ToRoundDigits(this decimal amount)
        {
            return (double)Decimal.Round(amount);
        }

        public static decimal ToRoundTwoDecimalDigits(this decimal? amount)
        {
            return (decimal)Decimal.Round(amount ?? 0, 2);
        }

        public static string ToColumnLabelName(this DateTime filterDate, string type, string key = "", bool isPrevious = true)
        {
            string result = string.Empty;

            if (isPrevious)
            {
                if (type == Constants.ByMonth)
                {
                    result = string.Format("{0}-{1:D2}-{2:D2}", key, filterDate.AddMonths(-1).Month, 01);
                    //result = string.Format("{0}-{1}", filterDate.AddMonths(-1).ToShortMonthName(),key);
                }
                if (type == Constants.ByYear)
                {
                    result = filterDate.AddYears(-1).ToString("yyyy-01-01");
                    //result = filterDate.AddYears(-1).ToString("yyyy");
                }
            }
            else
            {
                if (type == Constants.ByMonth)
                {
                    result = string.Format("{0}-{1:D2}-{2:D2}", key, filterDate.Month, 01);
                    //result = string.Format("{0}-{1}", filterDate.ToShortMonthName(), key);
                }
                if (type == Constants.ByYear)
                {
                    result = filterDate.ToString("yyyy-01-01");
                    //result = filterDate.ToString("yyyy");
                }
            }

            return Convert.ToDateTime(result).ToString("MMM yy");
        }

        public static double ToPercentageDifference(this decimal current, decimal previous)
        {

            if (previous == 0)
                return 0;
            return (double)((current - previous) * 100 / Math.Abs(previous)).ToRoundDigits();
        }
        public static int ToPercentageDifference(this int current, int previous)
        {

            if (previous == 0)
                return 0;
            return ((current - previous) * 100 / Math.Abs(previous));
        }

        public static decimal ToStripDecimal(this decimal? value)
        {
            return Math.Truncate(value ?? 0);
        }
        public static decimal ToStripDecimal(this decimal value)
        {
            return Math.Truncate(value);
        }

        public static double ToStripDecimal(this double? value)
        {
            return Math.Truncate(value ?? 0);
        }
        public static double ToStripDecimal(this double value)
        {
            return Math.Truncate(value);
        }
        public static double CalculateChange(double previous, double current)
        {
            if (previous == 0 && current==0) { return 0; }
            if (previous == 0) { return 1; }
            if (current == 0 && previous > 0) { return -1; }

            var change = current - previous;
            return (double)change / Math.Abs(previous);
        }
        public static double DoubleToPercentageString(double d)
        {
            return (Math.Round(d, 4) * 100);
        }
        public static decimal CalculateChangeInDecimal(decimal previous, decimal current)
        {
            if (previous == 0 && current == 0) { return 0; }
            if (previous == 0) { return 1; }

            var change = current - previous;
            return change / Math.Abs(previous);
        }

        public static string ToSalesCategoryDisplayName(this String category)
        {
            if (category == "OSS") { return "OS"; }
            if (category == "FOOD SERVI") { return "FOOD SERVICE"; }
            return category;
        }

        public static String ObjectToXMLGeneric<T>(T filter)
        {

            string xml = null;
            using (StringWriter sw = new StringWriter())
            {

                XmlSerializer xs = new XmlSerializer(typeof(T));
                xs.Serialize(sw, filter);
                try
                {
                    xml = sw.ToString();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return xml;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> list) where T : class
        {
            try
            {
                string tableName = list.FirstOrDefault().GetType().Name;

                DataTable dtOutput = new DataTable(tableName);

                //if the list is empty, return empty data table
                if (list.Count() == 0)
                    return dtOutput;

                //get the list of  public properties and add them as columns to the
                //output table           
                PropertyInfo[] properties = list.FirstOrDefault().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in properties)
                    dtOutput.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);

                //populate rows
                DataRow dr;
                //iterate through all the objects in the list and add them
                //as rows to the table
                foreach (T t in list)
                {
                    dr = dtOutput.NewRow();
                    //iterate through all the properties of the current object
                    //and set their values to data row
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        var Value = propertyInfo.GetValue(t);

                        //if (propertyInfo.PropertyType.Name == "String")
                        //    Value = Value != null ? Value.ToString().Trim() : Value;

                        dr[propertyInfo.Name] = Value ?? DBNull.Value;
                    }
                    dtOutput.Rows.Add(dr);
                }
                return dtOutput;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    }
}
