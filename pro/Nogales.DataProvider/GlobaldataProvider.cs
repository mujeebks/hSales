using Nogales.BusinessModel;
using Nogales.DataProvider.ENUM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider
{
    public static class GlobaldataProvider
    {
        public static List<GlobalFilter> GetFilterWithPeriods()
        {
            List<GlobalFilter> data = new List<GlobalFilter>()
                                            {
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisWeek), Id=(int)GlobalFilterEnum.ThisWeek,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.ThisWeek,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisWeekToDate), Id=(int)GlobalFilterEnum.ThisWeekToDate,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.ThisWeekToDate,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisMonth), Id=(int)GlobalFilterEnum.ThisMonth,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.ThisMonth,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisMonthToDate), Id=(int)GlobalFilterEnum.ThisMonthToDate,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.ThisMonthToDate,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisQuarter), Id=(int)GlobalFilterEnum.ThisQuarter,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.ThisQuarter,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisQuarterToDate), Id=(int)GlobalFilterEnum.ThisQuarterToDate,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.ThisQuarterToDate,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisYear), Id=(int)GlobalFilterEnum.ThisYear,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.ThisYear,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisYearToDate), Id=(int)GlobalFilterEnum.ThisYearToDate,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.ThisYearToDate,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisYearToLastMonth), Id=(int)GlobalFilterEnum.ThisYearToLastMonth,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.ThisYearToLastMonth,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastWeekToDate), Id=(int)GlobalFilterEnum.LastWeekToDate,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.LastWeekToDate,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastMonth), Id=(int)GlobalFilterEnum.LastMonth,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.LastMonth,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastMonthToDate), Id=(int)GlobalFilterEnum.LastMonthToDate,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.LastMonthToDate,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastQuarter), Id=(int)GlobalFilterEnum.LastQuarter,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.LastQuarter,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastQuarterToDate), Id=(int)GlobalFilterEnum.LastQuarterToDate,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.LastQuarterToDate,DateTime.Now)},
                                                    //new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastYear), Id=(int)GlobalFilterEnum.LastYear,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.LastYear)},
                                                    //new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastYearToDate), Id=(int)GlobalFilterEnum.LastYearToDate,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.LastYearToDate)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.Since30days), Id=(int)GlobalFilterEnum.Since30days,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.Since30days,DateTime.Now)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.Since60days), Id=(int)GlobalFilterEnum.Since60days,Periods=ENUMClass.GetPeriod(GlobalFilterEnum.Since60days,DateTime.Now)},

                                            };
            return data;
        }

        public static List<GlobalFilter> GetFilterWithPeriodsByDate(DateTime date)
        {
            date = date.Date.Add(new TimeSpan(0, 0, 0));
            List<GlobalFilter> data = new List<GlobalFilter>()
                                            {
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisWeek), Id=(int)GlobalFilterEnum.ThisWeek,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.ThisWeek,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisWeekToDate), Id=(int)GlobalFilterEnum.ThisWeekToDate,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.ThisWeekToDate,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisMonth), Id=(int)GlobalFilterEnum.ThisMonth,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.ThisMonth,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisMonthToDate), Id=(int)GlobalFilterEnum.ThisMonthToDate,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.ThisMonthToDate,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisQuarter), Id=(int)GlobalFilterEnum.ThisQuarter,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.ThisQuarter,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisQuarterToDate), Id=(int)GlobalFilterEnum.ThisQuarterToDate,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.ThisQuarterToDate,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisYear), Id=(int)GlobalFilterEnum.ThisYear,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.ThisYear,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisYearToDate), Id=(int)GlobalFilterEnum.ThisYearToDate,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.ThisYearToDate,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.ThisYearToLastMonth), Id=(int)GlobalFilterEnum.ThisYearToLastMonth,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.ThisYearToLastMonth,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastWeekToDate), Id=(int)GlobalFilterEnum.LastWeekToDate,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.LastWeekToDate,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastMonth), Id=(int)GlobalFilterEnum.LastMonth,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.LastMonth,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastMonthToDate), Id=(int)GlobalFilterEnum.LastMonthToDate,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.LastMonthToDate,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastQuarter), Id=(int)GlobalFilterEnum.LastQuarter,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.LastQuarter,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastQuarterToDate), Id=(int)GlobalFilterEnum.LastQuarterToDate,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.LastQuarterToDate,date)},
                                                    //new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastYear), Id=(int)GlobalFilterEnum.LastYear,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.LastYear)},
                                                    //new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.LastYearToDate), Id=(int)GlobalFilterEnum.LastYearToDate,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.LastYearToDate)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.Since30days), Id=(int)GlobalFilterEnum.Since30days,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.Since30days,date)},
                                                    new GlobalFilter { Description=StringEnum.GetStringValue(GlobalFilterEnum.Since60days), Id=(int)GlobalFilterEnum.Since60days,Periods=ENUMClass.GetPeriodForReport(GlobalFilterEnum.Since60days,date)},

                                            };
            return data;
        }

        public static class ENUMClass
        {


            public static string GetWeekOfMonth(DateTime date)
            {

                while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                    date = date.AddDays(1);
                DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);
                int weekNo = (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
                return "Week " + weekNo + " " + date.ToString("MMM yy");

            }

            public static string GetLastWeektoDate(DateTime date)
            {
                DateTime lastWeekDate = date.AddDays(-7);

                while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                    date = date.AddDays(1);
                while (lastWeekDate.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                    lastWeekDate = lastWeekDate.AddDays(1);
                DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

                DateTime beginningOflastWeekMonth = new DateTime(lastWeekDate.Year, lastWeekDate.Month, 1);
                int weekNo = (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
                int LastweekNo = (int)Math.Truncate((double)lastWeekDate.Subtract(beginningOflastWeekMonth).TotalDays / 7f) + 1;
                return "Wk " + LastweekNo + " " + lastWeekDate.ToString("MMM yy") + " - To Wk " + weekNo + " " + date.ToString("MMM yy");

            }



            public static string GetMonth(DateTime date)
            {
                return date.ToString("MMM yy");
            }

            public static string GetLastMonthToDate(DateTime date)
            {
                return date.AddMonths(-1).ToString("MMM yy") + " To " + date.ToString("MMM yy");
            }

            public static string GetQuarter(DateTime date)
            {
                return "Qtr " + (date.Month + 2) / 3 + " " + date.ToString("yy");
            }


            public static string GetYear(DateTime date)
            {
                return "Jan " + date.ToString("MMM") + " " + date.ToString("yy");
            }

            public static string GetSinceFromDays(DateTime date, int days)
            {
                return date.ToString("dd/MM/yy") + " - " + date.AddDays(-days).ToString("dd/MM/yy");
            }
            private static DateTime GetDate(DateTime dt)
            {

                return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            }

            public static Period GetPeriod(GlobalFilterEnum value, DateTime date)
            {

                Period period = new Period();
                if (value == GlobalFilterEnum.ThisWeek)
                {
                    //period.Current = new DateRange() { Start = date.StartOfWeek(), End = date.StartOfWeek().AddDays(6).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.StartOfWeek().AddDays(6)) };
                    //period.Prior = new DateRange() { Start = date.AddDays(-7).StartOfWeek(), End = date.AddDays(-7).StartOfWeek().AddDays(6).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.AddDays(-7).StartOfWeek().AddDays(6)) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfWeek(), End = date.AddYears(-1).StartOfWeek().AddDays(6).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.AddYears(-1).StartOfWeek().AddDays(6)) };
                    period.Current = new DateRange() { Start = date.StartOfWeek(), End = GetDate(date.StartOfWeek().AddDays(6)), Label = GetWeekOfMonth(date.StartOfWeek().AddDays(6)) };
                    period.Prior = new DateRange() { Start = date.AddDays(-7).StartOfWeek(), End = GetDate(date.AddDays(-7).StartOfWeek().AddDays(6)), Label = GetWeekOfMonth(date.AddDays(-7).StartOfWeek().AddDays(6)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfWeek(), End = GetDate(date.AddYears(-1).StartOfWeek().AddDays(6)), Label = GetWeekOfMonth(date.AddYears(-1).StartOfWeek().AddDays(6)) };
                }
                if (value == GlobalFilterEnum.ThisWeekToDate)
                {
                    //period.Current = new DateRange() { Start = date.StartOfWeek(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date) };
                    //period.Prior = new DateRange() { Start = date.AddDays(-7).StartOfWeek(), End = date.AddDays(-7).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.AddDays(-7)) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfWeek(), End = date.AddYears(-1).StartOfWeek().AddDays((date - date.StartOfWeek()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.AddYears(-1).StartOfWeek().AddDays((date - date.StartOfWeek()).TotalDays)) };
                    period.Current = new DateRange() { Start = date.StartOfWeek(), End = GetDate(date), Label = GetWeekOfMonth(date) };
                    period.Prior = new DateRange() { Start = date.AddDays(-7).StartOfWeek(), End = GetDate(date.AddDays(-7)), Label = GetWeekOfMonth(date.AddDays(-7)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfWeek(), End = GetDate(date.AddYears(-1).StartOfWeek().AddDays((date - date.StartOfWeek()).TotalDays)), Label = GetWeekOfMonth(date.AddYears(-1).StartOfWeek().AddDays((date - date.StartOfWeek()).TotalDays)) };

                }
                if (value == GlobalFilterEnum.ThisMonth)
                {
                    //period.Current = new DateRange() { Start = date.StartOfMonth(), End = date.StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(0, 0, 0)).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.StartOfMonth().AddMonths(1).AddDays(-1)) };
                    //period.Prior = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(0, 0, 0)).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1)) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfMonth(), End = date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(0, 0, 0)).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1)) };
                    //period.Current = new DateRange() { Start = date.StartOfMonth(), End = new DateTime(date.Year, date.StartOfMonth().AddMonths(1).AddDays(-1).Month, date.StartOfMonth().AddMonths(1).AddDays(-1).Day, 23, 59, 59), Label = GetMonth(date.StartOfMonth().AddMonths(1).AddDays(-1)) };
                    //period.Prior = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = new DateTime(date.Year, date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1).Month, date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1).Day, 23, 59, 59), Label = GetMonth(date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1)) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfMonth(), End = new DateTime(date.AddYears(-1).Year, date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1).Month, date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1).Day, 23, 59, 59), Label = GetMonth(date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1)) };
                    period.Current = new DateRange() { Start = date.StartOfMonth(), End = GetDate(date.StartOfMonth().AddMonths(1).AddDays(-1)), Label = GetMonth(date.StartOfMonth().AddMonths(1).AddDays(-1)) };
                    period.Prior = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = GetDate(date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1)), Label = GetMonth(date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfMonth(), End = GetDate(date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1)), Label = GetMonth(date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1)) };
                }
                if (value == GlobalFilterEnum.ThisMonthToDate)
                {
                    //period.Current = new DateRange() { Start = date.StartOfMonth(), End = date.Add(new TimeSpan(0, 0, 0)).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date) };
                    //period.Prior = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = date.AddMonths(-1).Add(new TimeSpan(0, 0, 0)).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddMonths(-1)) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfMonth(), End = date.AddYears(-1).Add(new TimeSpan(0, 0, 0)).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddYears(-1)) };
                    //period.Current = new DateRange() { Start = date.StartOfMonth(), End = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59), Label = GetMonth(date) };
                    //period.Prior = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = new DateTime(date.Year, date.AddMonths(-1).Month, date.AddMonths(-1).Day, 23, 59, 59), Label = GetMonth(date.AddMonths(-1)) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfMonth(), End = new DateTime(date.AddYears(-1).Year, date.Month, date.Day, 23, 59, 59), Label = GetMonth(date.AddYears(-1)) };
                    period.Current = new DateRange() { Start = date.StartOfMonth(), End = GetDate(date), Label = GetMonth(date) };
                    period.Prior = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = GetDate(date.AddMonths(-1)), Label = GetMonth(date.AddMonths(-1)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfMonth(), End = GetDate(date.AddYears(-1)), Label = GetMonth(date.AddYears(-1)) };
                }
                if (value == GlobalFilterEnum.ThisQuarter)
                {
                    period.Current = new DateRange() { Start = date.StartOfQuarter(), End = date.StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfQuarter().AddMonths(3).AddDays(-1)) };
                    period.Prior = new DateRange() { Start = date.StartOfQuarter().AddDays(-1).StartOfQuarter(), End = date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfQuarter(), End = date.AddYears(-1).StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddYears(-1).StartOfQuarter().AddMonths(3).AddDays(-1)) };
                    period.Current = new DateRange() { Start = date.StartOfQuarter(), End = GetDate(date.StartOfQuarter().AddMonths(3).AddDays(-1)), Label = GetQuarter(date.StartOfQuarter().AddMonths(3).AddDays(-1)) };
                    period.Prior = new DateRange() { Start = date.StartOfQuarter().AddDays(-1).StartOfQuarter(), End = GetDate(date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1)), Label = GetQuarter(date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfQuarter(), End = GetDate(date.AddYears(-1).StartOfQuarter().AddMonths(3).AddDays(-1)), Label = GetQuarter(date.AddYears(-1).StartOfQuarter().AddMonths(3).AddDays(-1)) };
                }

                if (value == GlobalFilterEnum.ThisQuarterToDate)
                {
                    //period.Current = new DateRange() { Start = date.StartOfQuarter(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date) };
                    //period.Prior = new DateRange() { Start = date.StartOfQuarter().AddDays(-1).StartOfQuarter(), End = date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays)) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfQuarter(), End = date.AddYears(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddYears(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays)) };
                    period.Current = new DateRange() { Start = date.StartOfQuarter(), End = GetDate(date), Label = GetQuarter(date) };
                    period.Prior = new DateRange() { Start = date.StartOfQuarter().AddDays(-1).StartOfQuarter(), End = GetDate(date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays)), Label = GetQuarter(date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfQuarter(), End = GetDate(date.AddYears(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays)), Label = GetQuarter(date.AddYears(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays)) };
                }

                if (value == GlobalFilterEnum.ThisYear)
                {
                    //period.Current = new DateRange() { Start = date.StartOfYear(), End = date.StartOfYear().AddYears(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.StartOfYear().AddYears(1).AddDays(-1)) };
                    ////period.Prior = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-2).StartOfYear().AddYears(1).AddDays(-1), Label = GetYear(date.AddYears(-2).StartOfYear().AddYears(1).AddDays(-1)) };
                    //period.Prior = new DateRange() { Start = Convert.ToDateTime("1900-01-01"), End = Convert.ToDateTime("1900-01-01").Add(new TimeSpan(23, 59, 59)), Label = "" };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date.AddYears(-1).StartOfYear().AddYears(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.AddYears(-1).StartOfYear().AddYears(1).AddDays(-1)) };
                    period.Current = new DateRange() { Start = date.StartOfYear(), End = GetDate(date.StartOfYear().AddYears(1).AddDays(-1)), Label = GetYear(date.StartOfYear().AddYears(1).AddDays(-1)) };
                    //period.Prior = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-2).StartOfYear().AddYears(1).AddDays(-1), Label = GetYear(date.AddYears(-2).StartOfYear().AddYears(1).AddDays(-1)) };
                    period.Prior = new DateRange() { Start = Convert.ToDateTime("1900-01-01"), End = GetDate(Convert.ToDateTime("1900-01-01")), Label = "" };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = GetDate(date.AddYears(-1).StartOfYear().AddYears(1).AddDays(-1)), Label = GetYear(date.AddYears(-1).StartOfYear().AddYears(1).AddDays(-1)) };
                }

                if (value == GlobalFilterEnum.ThisYearToDate)
                {
                    //period.Current = new DateRange() { Start = date.StartOfYear(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetYear(date) };
                    ////period.Prior = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-2), Label = GetYear(date.AddYears(-2)) };
                    //period.Prior = new DateRange() { Start = Convert.ToDateTime("1900-01-01"), End = Convert.ToDateTime("1900-01-01").Add(new TimeSpan(23, 59, 59)), Label = "" };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date.AddYears(-1).Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.AddYears(-1)) };
                    period.Current = new DateRange() { Start = date.StartOfYear(), End = GetDate(date), Label = GetYear(date) };
                    //period.Prior = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-2), Label = GetYear(date.AddYears(-2)) };
                    period.Prior = new DateRange() { Start = Convert.ToDateTime("1900-01-01"), End = GetDate(Convert.ToDateTime("1900-01-01")), Label = "" };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = GetDate(date.AddYears(-1)), Label = GetYear(date.AddYears(-1)) };
                }
                if (value == GlobalFilterEnum.ThisYearToLastMonth)
                {
                    //period.Current = new DateRange() { Start = date.AddMonths(-1).StartOfYear(), End = date.StartOfMonth().AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.StartOfMonth().AddDays(-1)) };
                    //// period.Prior = new DateRange() { Start = date.AddMonths(-1).AddYears(-2).StartOfYear(), End = date.StartOfMonth().AddDays(-1).AddYears(-2), Label = GetYear(date.StartOfMonth().AddDays(-1).AddYears(-2)) };
                    //period.Prior = new DateRange() { Start = Convert.ToDateTime("1900-01-01"), End = Convert.ToDateTime("1900-01-01").Add(new TimeSpan(23, 59, 59)), Label = "" };
                    //period.Historical = new DateRange() { Start = date.AddMonths(-1).AddYears(-1).StartOfYear(), End = date.StartOfMonth().AddDays(-1).AddYears(-1).Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.StartOfMonth().AddDays(-1).AddYears(-1)) };
                    period.Current = new DateRange() { Start = date.AddMonths(-1).StartOfYear(), End = GetDate(date.StartOfMonth().AddDays(-1)), Label = GetYear(date.StartOfMonth().AddDays(-1)) };
                    // period.Prior = new DateRange() { Start = date.AddMonths(-1).AddYears(-2).StartOfYear(), End = date.StartOfMonth().AddDays(-1).AddYears(-2), Label = GetYear(date.StartOfMonth().AddDays(-1).AddYears(-2)) };
                    period.Prior = new DateRange() { Start = Convert.ToDateTime("1900-01-01"), End = GetDate(Convert.ToDateTime("1900-01-01")), Label = "" };
                    period.Historical = new DateRange() { Start = date.AddMonths(-1).AddYears(-1).StartOfYear(), End = GetDate(date.StartOfMonth().AddDays(-1).AddYears(-1)), Label = GetYear(date.StartOfMonth().AddDays(-1).AddYears(-1)) };
                }
                if (value == GlobalFilterEnum.LastWeekToDate)
                {
                    //period.Current = new DateRange() { Start = date.AddDays(-7).StartOfWeek(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetLastWeektoDate(date) };
                    //period.Prior = new DateRange() { Start = date.AddDays(-21).StartOfWeek(), End = date.AddDays(-14).Add(new TimeSpan(23, 59, 59)), Label = GetLastWeektoDate(date.AddDays(-14)) };
                    //period.Historical = new DateRange() { Start = date.AddDays(-7).AddYears(-1).StartOfWeek(), End = date.AddDays(-7).AddYears(-1).StartOfWeek().AddDays((date - date.AddDays(-7).StartOfWeek()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetLastWeektoDate(date.AddDays(-7).AddYears(-1).StartOfWeek().AddDays((date - date.AddDays(-7).StartOfWeek()).TotalDays)) };
                    period.Current = new DateRange() { Start = date.AddDays(-7).StartOfWeek(), End = GetDate(date), Label = GetLastWeektoDate(date) };
                    period.Prior = new DateRange() { Start = date.AddDays(-21).StartOfWeek(), End = GetDate(date.AddDays(-14)), Label = GetLastWeektoDate(date.AddDays(-14)) };
                    period.Historical = new DateRange() { Start = date.AddDays(-7).AddYears(-1).StartOfWeek(), End = GetDate(date.AddDays(-7).AddYears(-1).StartOfWeek().AddDays((date - date.AddDays(-7).StartOfWeek()).TotalDays)), Label = GetLastWeektoDate(date.AddDays(-7).AddYears(-1).StartOfWeek().AddDays((date - date.AddDays(-7).StartOfWeek()).TotalDays)) };
                }
                if (value == GlobalFilterEnum.LastMonth)
                {
                    //period.Current = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddMonths(-1)) };
                    //period.Prior = new DateRange() { Start = date.AddMonths(-2).StartOfMonth(), End = date.AddMonths(-2).StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddMonths(-2)) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).AddMonths(-1).StartOfMonth(), End = date.AddYears(-1).AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddYears(-1).AddMonths(-1)) };
                    period.Current = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = GetDate(date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1)), Label = GetMonth(date.AddMonths(-1)) };
                    period.Prior = new DateRange() { Start = date.AddMonths(-2).StartOfMonth(), End = GetDate(date.AddMonths(-2).StartOfMonth().AddMonths(1).AddDays(-1)), Label = GetMonth(date.AddMonths(-2)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).AddMonths(-1).StartOfMonth(), End = GetDate(date.AddYears(-1).AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1)), Label = GetMonth(date.AddYears(-1).AddMonths(-1)) };
                }
                if (value == GlobalFilterEnum.LastMonthToDate)
                {
                    //period.Current = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetLastMonthToDate(date) };
                    //period.Prior = new DateRange() { Start = date.AddMonths(-2).StartOfMonth(), End = date.AddMonths(-1).Add(new TimeSpan(23, 59, 59)), Label = GetLastMonthToDate(date.AddMonths(-1)) };
                    //period.Historical = new DateRange() { Start = date.AddMonths(-1).AddYears(-1).StartOfMonth(), End = date.AddYears(-1).Add(new TimeSpan(23, 59, 59)), Label = GetLastMonthToDate(date.AddYears(-1)) };
                    period.Current = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = GetDate(date), Label = GetLastMonthToDate(date) };
                    period.Prior = new DateRange() { Start = date.AddMonths(-2).StartOfMonth(), End = GetDate(date.AddMonths(-1)), Label = GetLastMonthToDate(date.AddMonths(-1)) };
                    period.Historical = new DateRange() { Start = date.AddMonths(-1).AddYears(-1).StartOfMonth(), End = GetDate(date.AddYears(-1)), Label = GetLastMonthToDate(date.AddYears(-1)) };
                }
                if (value == GlobalFilterEnum.LastQuarter)
                {
                    //period.Current = new DateRange() { Start = date.StartOfLastQuarter(), End = date.StartOfLastQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfLastQuarter().AddMonths(3).AddDays(-1)/*.AddMonths(-3)*/) };
                    //period.Prior = new DateRange() { Start = date.StartOfLastQuarter().AddDays(-1).StartOfQuarter(), End = date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1)/*.AddMonths(-3)*/) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfLastQuarter(), End = date.AddYears(-1).StartOfLastQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddYears(-1).StartOfLastQuarter().AddMonths(3).AddDays(-1)/*.AddMonths(-3)*/) };
                    period.Current = new DateRange() { Start = date.StartOfLastQuarter(), End = GetDate(date.StartOfLastQuarter().AddMonths(3).AddDays(-1)), Label = GetQuarter(date.StartOfLastQuarter().AddMonths(3).AddDays(-1)/*.AddMonths(-3)*/) };
                    period.Prior = new DateRange() { Start = date.StartOfLastQuarter().AddDays(-1).StartOfQuarter(), End = GetDate(date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1)), Label = GetQuarter(date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1)/*.AddMonths(-3)*/) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfLastQuarter(), End = GetDate(date.AddYears(-1).StartOfLastQuarter().AddMonths(3).AddDays(-1)), Label = GetQuarter(date.AddYears(-1).StartOfLastQuarter().AddMonths(3).AddDays(-1)/*.AddMonths(-3)*/) };
                }
                if (value == GlobalFilterEnum.LastQuarterToDate)
                {
                    //period.Current = new DateRange() { Start = date.StartOfLastQuarter(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddMonths(-3)) };
                    //period.Prior = new DateRange() { Start = date.StartOfLastQuarter().AddDays(-1).StartOfQuarter(), End = date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).AddMonths(-3)) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfLastQuarter(), End = date.AddYears(-1).StartOfLastQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddYears(-1).StartOfLastQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).AddMonths(-3)) };
                    period.Current = new DateRange() { Start = date.StartOfLastQuarter(), End = GetDate(date), Label = GetQuarter(date.AddMonths(-3)) };
                    period.Prior = new DateRange() { Start = date.StartOfLastQuarter().AddDays(-1).StartOfQuarter(), End = GetDate(date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays)), Label = GetQuarter(date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).AddMonths(-3)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfLastQuarter(), End = GetDate(date.AddYears(-1).StartOfLastQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays)), Label = GetQuarter(date.AddYears(-1).StartOfLastQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).AddMonths(-3)) };
                }
                //if (value == GlobalFilterEnum.LastYear)
                //{
                //    period.Current = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date.AddYears(-1).StartOfYear().AddYears(1).AddDays(-1) };
                //    period.Prior = new DateRange() { Start = date.AddYears(-3).StartOfYear(), End = date.AddYears(-3).StartOfYear().AddYears(1).AddDays(-1) };
                //    period.Historical = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-2).StartOfYear().AddYears(1).AddDays(-1) };
                //}
                //if (value == GlobalFilterEnum.LastYearToDate)
                //{
                //    period.Current = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date };
                //    period.Prior = new DateRange() { Start = date.AddYears(-3).StartOfYear(), End = date.AddYears(-2) };
                //    period.Historical = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-1) };
                //}

                if (value == GlobalFilterEnum.Since30days)
                {
                    //period.Current = new DateRange() { Start = date.AddDays(-30).Date, End = date.Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date, 30) };
                    //period.Prior = new DateRange() { Start = date.AddDays(-60).Date, End = date.AddDays(-30).Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date.AddDays(-30), 30) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).AddDays(-30).Date, End = date.AddYears(-1).Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date.AddYears(-1), 30) };
                    period.Current = new DateRange() { Start = date.AddDays(-30).Date, End = GetDate(date), Label = GetSinceFromDays(date, 30) };
                    period.Prior = new DateRange() { Start = date.AddDays(-60).Date, End = GetDate(date.AddDays(-30).Date), Label = GetSinceFromDays(date.AddDays(-30), 30) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).AddDays(-30).Date, End = GetDate(date.AddYears(-1).Date), Label = GetSinceFromDays(date.AddYears(-1), 30) };
                }

                if (value == GlobalFilterEnum.Since60days)
                {
                    //period.Current = new DateRange() { Start = date.AddDays(-60).Date, End = date.Date.Add(new TimeSpan(0, 0, 0)).Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date, 60) };
                    //period.Prior = new DateRange() { Start = date.AddDays(-120).Date, End = date.AddDays(-60).Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date.AddDays(-60), 60) };
                    //period.Historical = new DateRange() { Start = date.AddYears(-1).AddDays(-60).Date, End = date.AddYears(-1).Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date.AddYears(-1), 60) };
                    period.Current = new DateRange() { Start = date.AddDays(-60).Date, End = GetDate(date.Date), Label = GetSinceFromDays(date, 60) };
                    period.Prior = new DateRange() { Start = date.AddDays(-120).Date, End = GetDate(date.AddDays(-60).Date), Label = GetSinceFromDays(date.AddDays(-60), 60) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).AddDays(-60).Date, End = GetDate(date.AddYears(-1).Date), Label = GetSinceFromDays(date.AddYears(-1), 60) };
                }
                return period;
            }

            public static Period GetPeriodForReport(GlobalFilterEnum value, DateTime date)
            {

                Period period = new Period();
                if (value == GlobalFilterEnum.ThisWeek)
                {
                    period.Current = new DateRange() { Start = date.StartOfWeek(), End = date.StartOfWeek().AddDays(6).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.StartOfWeek().AddDays(6)) };
                    period.Prior = new DateRange() { Start = date.AddDays(-7).StartOfWeek(), End = date.AddDays(-7).StartOfWeek().AddDays(6).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.AddDays(-7).StartOfWeek().AddDays(6)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfWeek(), End = date.AddYears(-1).StartOfWeek().AddDays(6).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.AddYears(-1).StartOfWeek().AddDays(6)) };
                }
                if (value == GlobalFilterEnum.ThisWeekToDate)
                {
                    period.Current = new DateRange() { Start = date.StartOfWeek(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date) };
                    period.Prior = new DateRange() { Start = date.AddDays(-7).StartOfWeek(), End = date.AddDays(-7).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.AddDays(-7)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfWeek(), End = date.AddYears(-1).StartOfWeek().AddDays((date - date.StartOfWeek()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetWeekOfMonth(date.AddYears(-1).StartOfWeek().AddDays((date - date.StartOfWeek()).TotalDays)) };
                }
                if (value == GlobalFilterEnum.ThisMonth)
                {
                    period.Current = new DateRange() { Start = date.StartOfMonth(), End = date.StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.StartOfMonth().AddMonths(1).AddDays(-1)) };
                    period.Prior = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddMonths(-1).StartOfMonth().AddMonths(1).AddDays(-1)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfMonth(), End = date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1)) };
                }
                if (value == GlobalFilterEnum.ThisMonthToDate)
                {
                    period.Current = new DateRange() { Start = date.StartOfMonth(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date) };
                    period.Prior = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = date.AddMonths(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddMonths(-1)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfMonth(), End = date.AddYears(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddYears(-1)) };
                }
                if (value == GlobalFilterEnum.ThisQuarter)
                {
                    period.Current = new DateRange() { Start = date.StartOfQuarter(), End = date.StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfQuarter().AddMonths(3).AddDays(-1)) };
                    period.Prior = new DateRange() { Start = date.StartOfQuarter().AddDays(-1).StartOfQuarter(), End = date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfQuarter(), End = date.AddYears(-1).StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddYears(-1).StartOfQuarter().AddMonths(3).AddDays(-1)) };
                }

                if (value == GlobalFilterEnum.ThisQuarterToDate)
                {
                    period.Current = new DateRange() { Start = date.StartOfQuarter(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date) };
                    period.Prior = new DateRange() { Start = date.StartOfQuarter().AddDays(-1).StartOfQuarter(), End = date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfQuarter(), End = date.AddYears(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddYears(-1).StartOfQuarter().AddDays((date - date.StartOfQuarter()).TotalDays)) };
                }

                if (value == GlobalFilterEnum.ThisYear)
                {
                    period.Current = new DateRange() { Start = date.StartOfYear(), End = date.StartOfYear().AddYears(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.StartOfYear().AddYears(1).AddDays(-1)) };
                    //period.Prior = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-2).StartOfYear().AddYears(1).AddDays(-1), Label = GetYear(date.AddYears(-2).StartOfYear().AddYears(1).AddDays(-1)) };
                    period.Prior = new DateRange() { Start = Convert.ToDateTime("1900-01-01"), End = Convert.ToDateTime("1900-01-01").Add(new TimeSpan(23, 59, 59)), Label = "" };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date.AddYears(-1).StartOfYear().AddYears(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.AddYears(-1).StartOfYear().AddYears(1).AddDays(-1)) };
                }

                if (value == GlobalFilterEnum.ThisYearToDate)
                {
                    period.Current = new DateRange() { Start = date.StartOfYear(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetYear(date) };
                    //period.Prior = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-2), Label = GetYear(date.AddYears(-2)) };
                    period.Prior = new DateRange() { Start = Convert.ToDateTime("1900-01-01"), End = Convert.ToDateTime("1900-01-01").Add(new TimeSpan(23, 59, 59)), Label = "" };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date.AddYears(-1).Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.AddYears(-1)) };
                }
                if (value == GlobalFilterEnum.ThisYearToLastMonth)
                {
                    period.Current = new DateRange() { Start = date.StartOfYear(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.StartOfMonth()) };
                    // period.Prior = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date.AddYears(-1), Label = GetYear(date.StartOfMonth().AddYears(-1)) };
                    period.Prior = new DateRange() { Start = Convert.ToDateTime("1900-01-01"), End = Convert.ToDateTime("1900-01-01").Add(new TimeSpan(23, 59, 59)), Label = "" };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date.AddYears(-1).Add(new TimeSpan(23, 59, 59)), Label = GetYear(date.StartOfMonth().AddYears(-1)) };
                }
                if (value == GlobalFilterEnum.LastWeekToDate)
                {
                    period.Current = new DateRange() { Start = date.AddDays(-7).StartOfWeek(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetLastWeektoDate(date) };
                    period.Prior = new DateRange() { Start = date.AddDays(-21).StartOfWeek(), End = date.AddDays(-14).Add(new TimeSpan(23, 59, 59)), Label = GetLastWeektoDate(date.AddDays(-14)) };
                    period.Historical = new DateRange() { Start = date.AddDays(-7).AddYears(-1).StartOfWeek(), End = date.AddDays(-7).AddYears(-1).StartOfWeek().AddDays((date - date.AddDays(-7).StartOfWeek()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetLastWeektoDate(date.AddDays(-7).AddYears(-1).StartOfWeek().AddDays((date - date.AddDays(-7).StartOfWeek()).TotalDays)) };
                }
                if (value == GlobalFilterEnum.LastMonth)
                {
                    period.Current = new DateRange() { Start = date.StartOfMonth(), End = date.StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date) };
                    period.Prior = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = date.AddMonths(-1).StartOfMonth().AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddMonths(-1)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfMonth(), End = date.AddYears(-1).StartOfMonth().AddMonths(1).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetMonth(date.AddYears(-1)) };
                }
                if (value == GlobalFilterEnum.LastMonthToDate)
                {
                    period.Current = new DateRange() { Start = date.AddMonths(-1).StartOfMonth(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetLastMonthToDate(date) };
                    period.Prior = new DateRange() { Start = date.AddMonths(-2).StartOfMonth(), End = date.AddMonths(-1).Add(new TimeSpan(23, 59, 59)), Label = GetLastMonthToDate(date.AddMonths(-1)) };
                    period.Historical = new DateRange() { Start = date.AddMonths(-1).AddYears(-1).StartOfMonth(), End = date.AddYears(-1).Add(new TimeSpan(23, 59, 59)), Label = GetLastMonthToDate(date.AddYears(-1)) };
                }
                if (value == GlobalFilterEnum.LastQuarter)
                {
                    period.Current = new DateRange() { Start = date.StartOfQuarter(), End = date.StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfQuarter().AddMonths(3).AddDays(-1)/*.AddMonths(-3)*/) };
                    period.Prior = new DateRange() { Start = date.StartOfQuarter().AddDays(-1).StartOfQuarter(), End = date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfQuarter().AddDays(-1).StartOfQuarter().AddMonths(3).AddDays(-1)/*.AddMonths(-3)*/) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfQuarter(), End = date.AddYears(-1).StartOfQuarter().AddMonths(3).AddDays(-1).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddYears(-1).StartOfQuarter().AddMonths(3).AddDays(-1)/*.AddMonths(-3)*/) };
                }
                if (value == GlobalFilterEnum.LastQuarterToDate)
                {
                    period.Current = new DateRange() { Start = date.StartOfLastQuarter(), End = date.Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddMonths(-3)) };
                    period.Prior = new DateRange() { Start = date.StartOfLastQuarter().AddDays(-1).StartOfQuarter(), End = date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.StartOfLastQuarter().AddDays(-1).StartOfQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).AddMonths(-3)) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).StartOfLastQuarter(), End = date.AddYears(-1).StartOfLastQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).Add(new TimeSpan(23, 59, 59)), Label = GetQuarter(date.AddYears(-1).StartOfLastQuarter().AddDays((date - date.StartOfLastQuarter()).TotalDays).AddMonths(-3)) };
                }
                //if (value == GlobalFilterEnum.LastYear)
                //{
                //    period.Current = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date.AddYears(-1).StartOfYear().AddYears(1).AddDays(-1) };
                //    period.Prior = new DateRange() { Start = date.AddYears(-3).StartOfYear(), End = date.AddYears(-3).StartOfYear().AddYears(1).AddDays(-1) };
                //    period.Historical = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-2).StartOfYear().AddYears(1).AddDays(-1) };
                //}
                //if (value == GlobalFilterEnum.LastYearToDate)
                //{
                //    period.Current = new DateRange() { Start = date.AddYears(-1).StartOfYear(), End = date };
                //    period.Prior = new DateRange() { Start = date.AddYears(-3).StartOfYear(), End = date.AddYears(-2) };
                //    period.Historical = new DateRange() { Start = date.AddYears(-2).StartOfYear(), End = date.AddYears(-1) };
                //}

                if (value == GlobalFilterEnum.Since30days)
                {
                    period.Current = new DateRange() { Start = date.AddDays(-30).Date, End = date.Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date, 30) };
                    period.Prior = new DateRange() { Start = date.AddDays(-60).Date, End = date.AddDays(-30).Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date.AddDays(-30), 30) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).AddDays(-30).Date, End = date.AddYears(-1).Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date.AddYears(-1), 30) };
                }

                if (value == GlobalFilterEnum.Since60days)
                {
                    period.Current = new DateRange() { Start = date.AddDays(-60).Date, End = date.Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date, 60) };
                    period.Prior = new DateRange() { Start = date.AddDays(-120).Date, End = date.AddDays(-60).Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date.AddDays(-60), 60) };
                    period.Historical = new DateRange() { Start = date.AddYears(-1).AddDays(-60).Date, End = date.AddYears(-1).Date.Add(new TimeSpan(23, 59, 59)), Label = GetSinceFromDays(date.AddYears(-1), 60) };
                }
                return period;
            }
        }

    }
}
