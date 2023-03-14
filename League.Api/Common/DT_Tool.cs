using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public class DT_SD_ED
    {
        public DateTime SD { get; set; }
        public DateTime ED { get; set; }
    }

    public class DT_Tool
    {
        //Last week（上周）
        public static DT_SD_ED Last_Week()
        {
            DateTime Now_Date = DateTime.Now;
            DateTime Start_Date = Now_Date.AddDays(1 - Convert.ToInt32(Now_Date.DayOfWeek) - 7); //上周一
            DateTime End_Date = Now_Date.AddDays(1 - Convert.ToInt32(Now_Date.DayOfWeek) - 7).AddDays(6); //上周周末

            DT_SD_ED DT = new DT_SD_ED
            {
                SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
            };
            return DT;
        }

        //This week（本周）
        public static DT_SD_ED This_Week()
        {
            DateTime Now_Date = DateTime.Now;
            DateTime Start_Date = Now_Date.AddDays(1 - Convert.ToInt32(Now_Date.DayOfWeek.ToString("d"))); //本周一
            DateTime End_Date = Start_Date.AddDays(6); //本周末

            DT_SD_ED DT = new DT_SD_ED
            {
                SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
            };
            return DT;
        }

        //Next week（下周）
        public static DT_SD_ED Next_Week()
        {
            DateTime Now_Date = DateTime.Now;
            DateTime Start_Date = Now_Date.AddDays(1 - Convert.ToInt32(Now_Date.DayOfWeek) + 7); //下周一
            DateTime End_Date = Now_Date.AddDays(1 - Convert.ToInt32(Now_Date.DayOfWeek) + 7).AddDays(6); //下周周末

            DT_SD_ED DT = new DT_SD_ED
            {
                SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
            };
            return DT;
        }

        //Last Month(上上上月)
        public static DT_SD_ED Last_Last_Last_Month()
        {
            DateTime Now_Date = DateTime.Now;
            DateTime Start_Date = Convert.ToDateTime(Now_Date.AddMonths(-3).ToString("yyyy-MM-01")); //上上上月一日
            DateTime End_Date = Start_Date.AddMonths(1).AddDays(-1); //上上上月最后一日
            DT_SD_ED DT = new DT_SD_ED
            {
                SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
            };
            return DT;
        }

        //Last Month(上上月)
        public static DT_SD_ED Last_Last_Month()
        {
            DateTime Now_Date = DateTime.Now;
            DateTime Start_Date = Convert.ToDateTime(Now_Date.AddMonths(-2).ToString("yyyy-MM-01")); //上上月一日
            DateTime End_Date = Start_Date.AddMonths(1).AddDays(-1); //上上月最后一日
            DT_SD_ED DT = new DT_SD_ED
            {
                SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
            };
            return DT;
        }

        //Last Month(上月)
        public static DT_SD_ED Last_Month()
        {
            DateTime Now_Date = DateTime.Now;
            DateTime Start_Date = Convert.ToDateTime(Now_Date.AddMonths(-1).ToString("yyyy-MM-01")); //上月一日
            DateTime End_Date = Start_Date.AddMonths(1).AddDays(-1); //上月最后一日
            DT_SD_ED DT = new DT_SD_ED
            {
                SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
            };
            return DT;
        }

        //This Month(本月)
        public static DT_SD_ED This_Month()
        {
            DateTime Now_Date = DateTime.Now;
            DateTime Start_Date = Convert.ToDateTime(Now_Date.ToString("yyyy-MM-01")); //本月一日
            DateTime End_Date = Start_Date.AddMonths(1).AddDays(-1); //本月最后一日
            DT_SD_ED DT = new DT_SD_ED
            {
                SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
            };
            return DT;
        }

        //Next Month(下月)
        public static DT_SD_ED Next_Month()
        {
            DateTime Now_Date = DateTime.Now;
            DateTime Start_Date = Convert.ToDateTime(Now_Date.AddMonths(1).ToString("yyyy-MM-01")); //下月一日
            DateTime End_Date = Start_Date.AddMonths(1).AddDays(-1); //下月最后一日
            DT_SD_ED DT = new DT_SD_ED
            {
                SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
            };
            return DT;
        }

        //This Year(今年)
        public static DT_SD_ED This_Year()
        {
            DateTime Now_Date = DateTime.Now;
            DateTime Start_Date = Convert.ToDateTime(Now_Date.ToString("yyyy-01-01")); //本年第一日
            DateTime End_Date = Convert.ToDateTime(Now_Date.ToString("yyyy-12-31")); //本年最后一日
            DT_SD_ED DT = new DT_SD_ED
            {
                SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
            };
            return DT;
        }

        //获取12个月
        public static List<DT_SD_ED> Year_All_Months(string Year)
        {
            List<DT_SD_ED> DSEL = new List<DT_SD_ED>();
            DT_SD_ED DSE = new DT_SD_ED();
            for (int i = 1; i <= 12; i++)
            {
                DateTime Start_Date = Convert.ToDateTime(Year + "-" + i + "-01 00:00:00"); //当月一日
                DateTime End_Date = Start_Date.AddMonths(1).AddDays(-1); //当月最后一日

                DSE = new DT_SD_ED
                {
                    SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                    ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
                };
                DSEL.Add(DSE);
            }
            return DSEL;
        }

        //获取时间段内月份数
        public static List<DT_SD_ED> Months_By_Time_Slot(DateTime Start, DateTime End)
        {
            int totalMonth = End.Year * 12 + End.Month - Start.Year * 12 - Start.Month;

            List<DT_SD_ED> DSEL = new List<DT_SD_ED>();
            DT_SD_ED DSE = new DT_SD_ED();

            DateTime Start_Date = Convert.ToDateTime(Start.ToString("yyyy-MM") + "-01 00:00:00"); //开始第一日

            for (int i = 0; i <= totalMonth; i++)
            {
                Start_Date = Convert.ToDateTime(Start.ToString("yyyy-MM") + "-01 00:00:00"); //开始第一日
                Start_Date = Start_Date.AddMonths(i); //当月第一日
                DateTime End_Date = Start_Date.AddMonths(1).AddDays(-1); //当月最后一日

                DSE = new DT_SD_ED
                {
                    SD = Convert.ToDateTime(Start_Date.ToString("yyyy-MM-dd 00:00:00")),
                    ED = Convert.ToDateTime(End_Date.ToString("yyyy-MM-dd 23:59:59"))
                };
                DSEL.Add(DSE);
            }
            return DSEL;
        }

        public static DateTime Get_Day_Min(string yyyy_MM_dd)
        {
            DateTime Time = DateTime.Now;
            try { Time = Convert.ToDateTime(yyyy_MM_dd + " 00:00:00"); } catch { }
            return Time;
        }

        public static DateTime Get_Day_Max(string yyyy_MM_dd)
        {
            DateTime Time = DateTime.Now;
            try { Time = Convert.ToDateTime(yyyy_MM_dd + " 23:59:59"); } catch { }
            return Time;
        }
    }
}
