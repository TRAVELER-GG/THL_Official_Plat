using System;
using System.Collections.Generic;

namespace System
{
    public class PageComponent
    {
        public static string MyPageNumberList(PagePar P)
        {
            string Language = "Cn";
            string En_Total = Language == "En" ? "Total" : "总计";
            string En_Total_Record = Language == "En" ? "Items" : "条记录";
            string Total = Language == "En" ? "Total" : "共";
            string Push = Language == "En" ? "Push" : "推送";
            string PageStr = Language == "En" ? "Page" : "页";
            string PageStr_One = Language == "En" ? "Each Page" : "每页";
            string PageStr_One_Record = Language == "En" ? "Records" : "条";
            string Jump = Language == "En" ? "to" : "跳至";
            string Go = Language == "En" ? "Go" : "确定";


            string HTMLStr = string.Empty;
            int PreviousPage = P.PageIndex - 1;
            int NextPage = P.PageIndex + 1;

            try
            {
                P.TotalPage = Convert.ToInt32(Math.Ceiling((double)P.TotalRecord / (double)P.PageSize));
            }
            catch
            {
                P.TotalPage = 0;
            }

            if (P.TotalRecord > 0)
            {
                HTMLStr += "<table class=\"MyPage\"><tr>";
                if (P.DataCount <= 0)
                {
                    HTMLStr += "<td style=\"width:10px;\">" + En_Total + " " + P.TotalRecord + " " + En_Total_Record + "， " + Total + " " + P.TotalPage + " " + PageStr + "</td>";
                }
                else
                {
                    HTMLStr += "<td style=\"width:10px;\">" + En_Total + " " + P.DataCount + " " + En_Total_Record + "，" + Push + P.TotalRecord + " " + En_Total_Record + "，" + Total + " " + P.TotalPage + PageStr + "</td>";
                }

                HTMLStr += "<td style=\"width:10px; padding-left:5px; padding-right:5px; \">，" + PageStr_One + " " + P.PageSize + " " + PageStr_One_Record + "</td>";

                if (P.PageIndex > 1)
                {
                    HTMLStr += "<td style=\"width:10px;\"><button type=\"button\" title =\"首页\" onclick=\"SubmitPageIndex('1')\"><i class='icon-double-angle-left'></i></button></td>";
                    HTMLStr += "<td style=\"width:10px;\"><button type=\"button\" title=\"上页\" onclick=\"SubmitPageIndex('" + PreviousPage + "')\"><i class='icon-angle-left'></i></button></td>";
                }
                else
                {
                    HTMLStr += "<td style=\"width:10px;\"><button type=\"button\" class=\"DisabledBut\" disabled title=\"首页\"><i class='icon-double-angle-left'></i></button></td>";
                    HTMLStr += "<td style=\"width:10px;\"><button type=\"button\" class=\"DisabledBut\" disabled title=\"上页\"><i class='icon-angle-left'></i></button></td>";
                }

                HTMLStr += "<td style=\"width:100px; text-align:right; \">";
                for (int i = 1; i <= P.TotalPage; i++)
                {
                    if (P.PageIndex > (i - P.LoopSize) && P.PageIndex < (i + P.LoopSize))
                    {
                        if (i == P.PageIndex)
                        {
                            HTMLStr += "<button type=\"button\" onclick=\"SubmitPageIndex('" + i + "')\" class=\"LoopButAct\">" + i + "</button>";
                        }
                        else
                        {
                            HTMLStr += "<button type=\"button\" onclick=\"SubmitPageIndex('" + i + "')\" class=\"LoopBut\">" + i + "</button>";
                        }
                    }
                }
                HTMLStr += "</td>";

                if (P.PageIndex < P.TotalPage)
                {
                    HTMLStr += "<td style=\"width:10px;\"><button type=\"button\" title=\"下页\" onclick=\"SubmitPageIndex('" + NextPage + "')\"><i class='icon-angle-right'></i></button></td>";
                    HTMLStr += "<td style=\"width:10px;\"><button type=\"button\" title=\"末页\" onclick=\"SubmitPageIndex('" + P.TotalPage + "')\"><i class='icon-double-angle-right'></i></button></td>";
                }
                else
                {
                    HTMLStr += "<td style=\"width:10px;\"><button type=\"button\" title=\"下页\" class=\"DisabledBut\" disabled><i class='icon-angle-right'></i></button></td>";
                    HTMLStr += "<td style=\"width:10px;\"><button type=\"button\" title=\"末页\" class=\"DisabledBut\" disabled><i class='icon-double-angle-right'></i></button></td>";
                }

                HTMLStr += "<td style=\"width:10px; padding-left:5px;\">";
                HTMLStr += Jump + " <input type=\"number\" onkeypress=\"if(event.keyCode=='13'){SubmitSelectIndex()}\" style=\"width:60px; text-align:center;\" min=\"1\" max=\"" + P.TotalPage + "\" id=\"FootPageIndexNo\" name=\"FootPageIndexNo\" value=\"" + P.PageIndex + "\" /> " + PageStr;
                HTMLStr += "</td>";

                HTMLStr += "<td style=\"padding-left:5px;padding-right:10px; width:10px;\">";
                HTMLStr += "<button type=\"button\" id=\"MyFootBut\" onclick=\"SubmitSelectIndex()\" type=\"button\">" + Go + "</button>";
                HTMLStr += "</td>";
                HTMLStr += "</tr></table>";
            }
            return HTMLStr;
        }

        public static string MyPageNumberList_Sim(PagePar_Sim P)
        {
            try { P.TotalPage = Convert.ToInt32(Math.Ceiling((double)P.TotalRecord / (double)P.PageSize)); } catch { }
            P.Page_Previous = P.PageIndex - 1;
            P.Page_Previous = P.Page_Previous <= 0 ? 0 : P.Page_Previous;
            P.Page_Next = P.PageIndex + 1;

            string HTMLStr = string.Empty;
            int PreviousPage = P.PageIndex - 1;
            int NextPage = P.PageIndex + 1;


            HTMLStr += "<table class=\"Small_Page_Tab\">";
            HTMLStr += "<tr>";

            HTMLStr += "<td>Total：<strong style=\"font-size:14px;\">" + P.TotalRecord + "</strong> Items</td>";
            HTMLStr += "<td style=\"width:auto; text-align:right; padding-right:10px;\">" + P.PageIndex + " / " + P.TotalPage + "</td>";

            if (P.Page_Previous <= 0)
            {
                HTMLStr += "<td title=\"首页\"><button type=\"button\" class=\"PageBtn PageBtn_Not_Act\"><span class=\"icon-double-angle-left\"></span></button></td>";
                HTMLStr += "<td title=\"上页\"><button type=\"button\" class=\"PageBtn PageBtn_Not_Act\"><span class=\"icon-caret-left\"></span></button></td>";
            }
            else
            {
                HTMLStr += "<td title=\"首页\"><button type=\"button\" class=\"PageBtn PageBtn_Act\" onclick=\"SubmitPageIndex_Sim('1')\"><span class=\"icon-double-angle-left\"></span></button></td>";
                HTMLStr += "<td title=\"上页\"><button type=\"button\" class=\"PageBtn PageBtn_Act\" onclick=\"SubmitPageIndex_Sim('" + P.Page_Previous + "')\"><span class=\"icon-caret-left\"></span></button></td>";
            }

            HTMLStr += "<td style=\"width: 5px;\">&nbsp;</td>";

            if (P.Page_Next > P.TotalPage)
            {
                HTMLStr += "<td title=\"下页\"><button type=\"button\" class=\"PageBtn PageBtn_Not_Act\"><span class=\"icon-caret-right\"></span></button></td>";
                HTMLStr += "<td title=\"末页\"><button type=\"button\" class=\"PageBtn PageBtn_Not_Act\"><span class=\"icon-double-angle-right\"></span></button></td>";
            }
            else
            {
                HTMLStr += "<td title=\"下页\"><button type=\"button\" class=\"PageBtn PageBtn_Act\" onclick=\"SubmitPageIndex_Sim('" + P.Page_Next + "')\"><span class=\"icon-caret-right\"></span></button></td>";
                HTMLStr += "<td title=\"末页\"><button type=\"button\" class=\"PageBtn PageBtn_Act\" onclick=\"SubmitPageIndex_Sim('" + P.TotalPage + "')\"><span class=\"icon-double-angle-right\"></span></button></td>";
            }

            HTMLStr += "</tr></table>";
            return HTMLStr;
        }

        public static void Set_PagePar_Sim(PagePar_Sim P)
        {
            try { P.TotalPage = Convert.ToInt32(Math.Ceiling((double)P.TotalRecord / (double)P.PageSize)); } catch { }
            P.Page_Previous = P.PageIndex - 1;
            P.Page_Previous = P.Page_Previous <= 0 ? 0 : P.Page_Previous;
            P.Page_Next = P.PageIndex + 1;
        }
    }

    public class PagePar
    {
        public PagePar()
        {
            LoopSize = 5;
            PageSizeList = "20,50,100";
        }
        public int DataCount { get; set; }
        public int TotalRecord { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string PageSizeList { get; set; }
        public int TotalPage { get; set; }
        public int LoopSize { get; set; }
    }

    public class PagePar_Sim
    {
        public PagePar_Sim()
        {
            TotalRecord = 0;
            PageIndex = 0;
            PageSize = 0;
            TotalPage = 0;
            Page_Previous = 0;
            Page_Next = 0;
        }
        public int TotalRecord { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Page_Previous { get; set; }
        public int Page_Next { get; set; }
        public int TotalPage { get; set; }
    }

    public class PageList<T> where T : class
    {
        public PageList()
        {
            DurationMsec = 0;
            Rows = new List<T>();
            DataCount = 0;
            MaxTake = 50000;
        }

        //最大读取数
        public int MaxTake { get; set; }

        //数据表总记录数
        public int DataCount { get; set; }

        //记录数
        public int TotalRecord { get; set; }

        //页码
        public int PageIndex { get; set; }

        //每页记录数
        public int PageSize { get; set; }

        //总页数
        public int TotalPage { get; set; }

        //耗时毫秒
        public long DurationMsec { get; set; }

        //分页数据
        public List<T> Rows { get; set; }

    }
}
