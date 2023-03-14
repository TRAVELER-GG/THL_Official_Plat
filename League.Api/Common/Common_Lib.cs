using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace System
{
    public class Common_Lib
    {
        //获取字符串内整数
        public static int Get_Number_Int(string str)
        {
            int result = 0;
            try
            {
                if (str != null && str != string.Empty)
                {
                    // 正则表达式剔除非数字字符（不包含小数点.） 
                    str = Regex.Replace(str, @"[^\d.\d]", "");
                    // 如果是数字，则转换为decimal类型 
                    if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                    {
                        result = int.Parse(str);
                    }
                }
            }
            catch
            {
                result = 0;
            }

            return result;
        }

        //字符串转Guid
        public static Guid Convert_GUID(string str)
        {
            Guid Item = Guid.Empty;
            try { Item = new Guid(str); } catch { Item = Guid.Empty; }
            return Item;
        }

        //字符串转List<Guid>
        public static List<Guid> Convert_GUID_List(string STR)
        {
            STR = STR == null ? string.Empty : STR.Trim();

            //以,作为分离器
            char[] separator = { ',' };

            //以,拆分字符串为字符串数组
            string[] STR_List;
            STR_List = STR.Split(separator);
            List<Guid> Guid_List = new List<Guid>();
            foreach (string Guid_STR in STR_List)
            {
                try { Guid_List.Add(new Guid(Guid_STR)); } catch { }
            }
            return Guid_List;
        }

        public static int Convert_ToInt32(string Str)
        {
            int I = 0;
            try { I = Convert.ToInt32(Str); } catch { I = 0; }
            return I;
        }

        public static decimal Convert_ToDecimal(string Str)
        {
            decimal D = 0;
            try { D = Convert.ToDecimal(Str); } catch { D = 0; }
            return D;
        }

        public static DateTime Convert_ToDateTime(string Str)
        {
            DateTime DT = DateTime.Now;
            try { DT = Convert.ToDateTime(Str); } catch { DT = DateTime.Now; }
            return DT;
        }

        public static string Convert_ToString(string Str)
        {
            try { Str = Str.Trim(); } catch { Str = string.Empty; }
            return Str;
        }

        public static List<string> Convert_ToString_List(string STR)
        {
            STR = STR == null ? string.Empty : STR.Trim();
            List<string> String_List = new List<string>();
            try
            {
                //以,作为分离器
                char[] separator = { ',' };

                //以,拆分字符串为字符串数组
                string[] StrList;
                StrList = STR.Split(separator);
                foreach (string Str in StrList) { String_List.Add(Str); }
            }
            catch
            {
                String_List = new List<string>();
            }
            String_List = String_List.Where(x => x != string.Empty).ToList();
            return String_List;
        }

        static public string Company_Name_Replace_and_ToLower(string CompanyName)
        {
            string NewComName = CompanyName;
            try
            {
                NewComName = NewComName.Replace("(", "");
                NewComName = NewComName.Replace("（", "");
                NewComName = NewComName.Replace("）", "");
                NewComName = NewComName.Replace(")", "");
                NewComName = NewComName.ToLower();
            }
            catch { }
            return NewComName;
        }

        //只允许数字和字母
        static public bool Is_Letter_Or_Number(string Input_Str)
        {
            bool Flag = Regex.IsMatch(Input_Str, "^[0-9a-zA-Z]+$");
            return Flag;
        }

        //只允许数字和字母和汉字
        static public bool Is_Letter_Or_Number_Or_Chinese(string Input_Str)
        {
            bool Flag = Regex.IsMatch(Input_Str, "^[0-9a-zA-Z\u4e00-\u9fa5]+$");
            return Flag;
        }

        //替换最后一个字符
        public static string Trim_End(string STR)
        {
            string New_STR = string.Empty;
            try { New_STR = STR.Substring(0, STR.Length - 1); } catch { New_STR = STR; }
            return New_STR;
        }

        public static List<string> Img_Check_List()
        {
            List<string> List = new List<string> {
                ".bmp",
                ".BMP",
                ".jpe",
                ".JPE",
                ".jpeg",
                ".JPEG",
                ".jpg",
                ".JPG",
                ".png",
                ".PNG",
            };
            return List;
        }
    }

    public class Select_Option
    {
        public string Key { get; set; }
        public int Key_Int { get; set; }
        public Guid Key_Guid { get; set; }
        public string Name { get; set; }
        public string Col_A { get; set; }
        public string Col_B { get; set; }
        public string Col_C { get; set; }
        public string Col_D { get; set; }
        public string Col_E { get; set; }
        public string Remark { get; set; }
    }
}
