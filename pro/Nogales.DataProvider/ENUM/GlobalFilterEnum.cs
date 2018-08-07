using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider.ENUM
{

    public enum GlobalFilterEnum
    {
        [StringValue("This Week")]
        ThisWeek = 1,
        [StringValue("This Week To Date")]
        ThisWeekToDate = 2,
        [StringValue("This Month")]
        ThisMonth = 3,
        [StringValue("This Month To Date")]
        ThisMonthToDate = 4,
        [StringValue("This Quarter")]
        ThisQuarter = 5,
        [StringValue("This Quarter To Date")]
        ThisQuarterToDate = 6,
        [StringValue("This Year")]
        ThisYear = 7,
        [StringValue("This Year To Date")]
        ThisYearToDate = 8,
        [StringValue("This Year To Last Month")]
        ThisYearToLastMonth = 9,
        [StringValue("Last Week To Date")]
        LastWeekToDate = 10,
        [StringValue("Last Month")]
        LastMonth = 11,
        [StringValue("Last Month To Date")]
        LastMonthToDate = 12,
        [StringValue("Last Quarter")]
        LastQuarter = 13,
        [StringValue("Last Quarter To Date")]
        LastQuarterToDate = 14,
        //[StringValue("Last Year")]
        //LastYear = 15,
        //[StringValue("Last Year To Date")]
        //LastYearToDate = 16,
        [StringValue("Since 30 Days Ago")]
        Since30days = 17,
        [StringValue("Since 60 Days Ago")]
        Since60days = 18,
    }

    public enum PeriodEnum
    {
        Current = 0,
        Historical = 1,
        Prior = 2
    }

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();


            //Look for our 'StringValueAttribute' 
            //in the field's custom attributes
            FieldInfo fi = type.GetField(value.ToString());
            StringValueAttribute[] attrs =
                fi.GetCustomAttributes(typeof(StringValueAttribute),
                                        false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {

                output = attrs[0].Value;
            }


            return output;
        }
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
    }

    public class StringValueAttribute : System.Attribute
    {

        private string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
}

