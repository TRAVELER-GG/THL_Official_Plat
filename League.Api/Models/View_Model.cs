using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

//小程序
namespace League.Api
{
    //找谁家
    public class App_Business
    {
        public App_Business()
        {
            Business_Type_List = new List<Submenu>();
            Business_Area_List = new List<Business_Area>();
            Business_List = new List<Business>();
        }
        //商家类型
        public List<Submenu> Business_Type_List { get; set; }
        public List<Business_Area> Business_Area_List { get; set; }
        public List<Business> Business_List { get; set; }
    }

    public class Business_Area
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<Submenu> Submenu { get; set; }
    }

    public class Submenu
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class App_Business_Coupon
    {
        public App_Business_Coupon()
        {
            Business_Area_List = new List<Business_Area>();
            Business_Name_List = new List<Submenu>();
            Coupon_List_A = new List<Business_Coupon>();
            Coupon_List_B = new List<Business_Coupon>();
            Coupon_List_C = new List<Business_Coupon>();
        }
       
        public List<Business_Area> Business_Area_List { get; set; }
        public List<Submenu> Business_Name_List { get; set; }
        public List<Business_Coupon> Coupon_List_A { get; set; }
        public List<Business_Coupon> Coupon_List_B { get; set; }
        public List<Business_Coupon> Coupon_List_C { get; set; }
    }


    public class App_Home
    {
        public App_Home()
        {
            Home_Img = string.Empty;
            News_List = new List<News>();
        }

        public string Home_Img { get; set; }
        public List<News> News_List { get; set; }
    }

    public class App_Personal
    {
        public List<string> Years_List { get; set; }
        public List<string> Interest_List { get; set; }
        public List<string> Degree_List { get; set; }
        public List<string> Faculty_List { get; set; }
        public List<string> Profession_First { get; set; }
        public List<Profession_Second> Profession_Second { get; set; }
    }

    public class College
    {
        public Guid CID { get; set; }
        public string Logo { get; set; }
        public string College_Name { get; set; }
        public List<Faculty> Faculty_List { get; set; }
    }

    public class Faculty
    {
        public Guid FID { get; set; }
        public string Faculty_Name { get; set; }
        public Guid Link_CID { get; set; }
    }

    //新鲜事
    public class News
    {
        public Guid ID { get; set; }
        public string Create_DT { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Publisher { get; set; }
        public int Page_View { get; set; }
        public Guid Link_AUID { get; set; }
        public string Status { get; set; }
    }

    public class Member
    {
        public Guid ID { get; set; }
        public string Img { get; set; }
        public string Logo_College { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string College { get; set; }
        public string College_Short { get; set; }
        public string Faculty { get; set; }
        public string Years { get; set; }
        public string Degree { get; set; }
        public string Phone { get; set; }
        public string Phone_verifty { get; set; }
        public string Profession_1st { get; set; }
        public string Profession { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Native_Province { get; set; }
        public string Native_City { get; set; }
        public string Native_District { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Interest { get; set; }
        public string Avatar_Url { get; set; }
        public string Nick_Name { get; set; }
        public string OpenID { get; set; }
        public string Session_Key { get; set; }
        public string Is_Auditor { get; set; }
        //审核状态
        public string Audit_Status { get; set; }

        public Guid Enter_MEID { get; set; }
        public string Enter_Name { get; set; }
        public string Enter_Duty { get; set; }

        //关注状态
        public string Status { get; set; }
        public Guid Link_AUID { get; set; }
    }

    public enum Member_Status_Enum
    {
        已关注,
        互关,
        未关注,
    }

    public class Lable_List
    {
        public List<string> Profession_Member { get; set; }
        public List<string> Faculty { get; set; }
        public List<string> Place { get; set; }
        public List<string> Interest { get; set; }
        public List<string> Enterprise { get; set; }
        public List<string> Service_Type { get; set; }
        public List<string> Business_Type { get; set; }
        public List<string> Business_Place { get; set; }
    }

    public enum Mark_Enum
    {
        Member, //校友
        Member_Company,  //校企
        Mini,  //微服坊
        Business,  //微服坊
    }

    public class App_Association
    {
        public List<Association> Part1_List { get; set; }
        public List<Association> Part2_List { get; set; }
    }

    //校友会
    public class Association
    {
        public Guid ID { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Name_Short { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public string Establish_DT { get; set; }
        public string Union_Type { get; set; }
        public string Count { get; set; }
        public string Is_Join { get; set; }
    }

    public enum Is_Join_Enum
    {
        已加入,
        未加入,
    }

    //校友
    public class Alumnus
    {
        public Alumnus()
        {
            ID = Guid.Empty;
            Img = string.Empty;
            Name = string.Empty;
            College = string.Empty;
            College_Short = string.Empty;
            College_Logo = string.Empty;
            Years = string.Empty;
            Faculty = string.Empty;
            Degree = string.Empty;
            Company = string.Empty;
            Status = string.Empty;
            Authority_Type = string.Empty;
            Verify_Count = 0;
            Coupon_Count = 0;
            Fans_Count = 0;
        }

        public Guid ID { get; set; }
        public string Img { get; set; }
        public string Avatar_Url { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string College { get; set; }
        public string College_Short { get; set; }
        public string College_Logo { get; set; }
        public string Years { get; set; }
        public string Faculty { get; set; }
        public string Degree { get; set; }
        public string Company { get; set; }
        public string Status { get; set; }
        public string Authority_Type { get; set; }
        public int Verify_Count { get; set; }
        public int Coupon_Count { get; set; }
        public int Fans_Count { get; set; }
    }

    public class Alumnus_Wrap
    {
        public List<Member> Faculty { get; set; }
        public List<Member> Village { get; set; }
        public List<Member> Profession { get; set; }
        public List<Member> Interest { get; set; }
        public List<Member> Year { get; set; }
    }

    public class Attention
    {
        public Attention()
        {
            Follow_List = new List<Alumnus>();
            Fans_List = new List<Alumnus>();
            Each_List = new List<Alumnus>();
        }

        public List<Alumnus> Follow_List { get; set; }
        public List<Alumnus> Fans_List { get; set; }
        public List<Alumnus> Each_List { get; set; }
    }

    public class Alu_Activity
    {
        public Guid ID { get; set; }
        public string Poster { get; set; }
        public string Title { get; set; }
        public string Sponsor { get; set; }
        public string Start_DT { get; set; }
        public string End_DT { get; set; }
        public string Location { get; set; }
        public int Num_of_people { get; set; }
        public string Content { get; set; }

        //是否报名
        public string Is_Attend { get; set; }

        //已报名人数
        public int People { get; set; }

        public Alumni_Union Union { get; set; }
        public List<Activity_People> People_List { get; set; }
    }

    public enum Is_Attend_Enum
    {
        已参与,
        未参与
    }

    public class Activity_People
    {
        public string Img { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }


    public class Enterprise
    {
        public Guid ID { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public string Member_Name { get; set; }
        public string Year { get; set; }
        public string Alumni_Name { get; set; }
        public string Duty { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
    }

    public enum Enterprise_Status_Enum
    {
        已关注,
        未关注,
    }

    public class Service
    {
        public Guid ID { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Linkman { get; set; }
        public string Contact { get; set; }
        public string Main_Business { get; set; }
        public string Intro { get; set; }
        public string Status { get; set; }
    }

    public class Service_List
    {
        public List<Service> Company { get; set; }
        public List<Service> Person { get; set; }
        public List<Service> Other { get; set; }
    }
}