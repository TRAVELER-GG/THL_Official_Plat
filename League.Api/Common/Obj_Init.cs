using System;
namespace System
{
    public static class Obj_Init
    {
        /// <summary>
        /// 用于新增、更新持久化前对字符串类型进行Trim处理
        /// </summary>
        public static void Trim(object Item)
        {
            foreach (System.Reflection.PropertyInfo P in Item.GetType().GetProperties())
            {
                if (P.PropertyType.Name == typeof(String).Name)
                {
                    if (P.GetValue(Item) == null)
                    {
                        P.SetValue(Item, string.Empty);
                    }
                    else
                    {
                        P.SetValue(Item, P.GetValue(Item).ToString().Trim());
                    }
                }
            }
        }

        /// <summary>
        /// 用于过滤器对字符串类型进行Trim处理
        /// </summary>
        public static void Trim_Filter(object Item)
        {
            foreach (System.Reflection.PropertyInfo P in Item.GetType().GetProperties())
            {
                if (P.PropertyType.Name == typeof(String).Name)
                {
                    if (P.GetValue(Item) == null)
                    {
                        P.SetValue(Item, string.Empty);
                    }
                    else
                    {
                        P.SetValue(Item, P.GetValue(Item).ToString().Trim());
                    }
                }

                if (P.PropertyType.Name == typeof(int).Name)
                {
                    if (P.GetValue(Item) == null && P.Name == "PageIndex")
                    {
                        P.SetValue(Item, 1);
                    }
                    else if (P.Name == "PageIndex" && (int)P.GetValue(Item) <= 0)
                    {
                        P.SetValue(Item, 1);
                    }
                    else if (P.GetValue(Item) == null)
                    {
                        P.SetValue(Item, 0);
                    }
                }
            }
        }
    }
}
