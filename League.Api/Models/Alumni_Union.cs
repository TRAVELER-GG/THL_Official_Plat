using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace League.Api
{
    public class Alumni_Union
    {
        public Alumni_Union()
        {
            AUID = Guid.Empty;
            Create_DT = DateTime.Now;
            Establish_DT = string.Empty;
            Union_Type = string.Empty;
            Union_Type_List = Enum.GetNames(typeof(Union_Type_Enum)).ToList();
            Name = string.Empty;
            Name_Short = string.Empty;
            Logo = string.Empty;
            Logo_College = string.Empty;
            Background = string.Empty;
            Content = string.Empty;
            Permission = string.Empty;
            Contact = string.Empty;
            Phone = string.Empty;
            Province = string.Empty;
            City = string.Empty;
            Member_Count = 0;
            Union_PV = 0;
        }

        [Key]
        public Guid AUID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("创办时间")]
        public string Establish_DT { get; set; }

        [Required]
        [DisplayName("分会名称")]
        public string Name { get; set; }

        [Required]
        [DisplayName("分会简称")]
        public string Name_Short { get; set; }

        [Required]
        [DisplayName("分会类型")]
        public string Union_Type { get; set; }

        [NotMapped]
        [DisplayName("分会类型")]
        public List<string> Union_Type_List { get; set; }

        [Required]
        [DisplayName("Logo")]
        public string Logo { get; set; }

        [Required]
        [DisplayName("校徽")]
        public string Logo_College { get; set; }

        [Required]
        [DisplayName("形象图")]
        public string Background { get; set; }

        [Required]
        [DisplayName("校友会简介")]
        public string Content { get; set; }

        [Required]
        [DisplayName("二级权限")]
        public string Permission { get; set; }

        [Required]
        [DisplayName("联系方式")]
        public string Contact { get; set; }

        [Required]
        [DisplayName("联系方式")]
        public string Phone { get; set; }

        [Required]
        [DisplayName("分会区域-省")]
        public string Province { get; set; }

        [Required]
        [DisplayName("分会区域-市")]
        public string City { get; set; }

        [NotMapped]
        [DisplayName("会员人数")]
        public int Member_Count { get; set; }

        [Required]
        [DisplayName("分会浏览量")]
        public int Union_PV { get; set; }

        [NotMapped]
        public string Real_Name { get; set; }

        [NotMapped]
        public string User_Name { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [Required]
        [DisplayName("登陆口令")]
        public string PinCode { get; set; }
    }

    public enum Union_Type_Enum
    {
        地区,
        行业
    }

    public class Alumni_Union_User
    {
        public Alumni_Union_User()
        {
            UID = Guid.Empty;
            Create_DT = DateTime.Now;
            Name = string.Empty;
            User_Name = string.Empty;
            Password = string.Empty;
            Is_Admin = string.Empty;
        }

        [Key]
        public Guid UID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("真实姓名")]
        public string Name { get; set; }

        [Required]
        [DisplayName("管理员账号")]
        public string User_Name { get; set; }

        [Required]
        [DisplayName("密码")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("固定电话")]
        public string Tel { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("电子邮件")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("头像")]
        public string Img_Path { get; set; }

        [Required]
        [DisplayName("当前状态")]
        public string Is_Frozen { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Is_Admin { get; set; }

        [Required]
        [DisplayName("系统角色")]
        public string Roles_Title { get; set; }

        [NotMapped]
        [DisplayName("分会全称")]
        public string Name_Full { get; set; }

        [NotMapped]
        [DisplayName("校友会简称")]
        public string Name_Short { get; set; }

        [NotMapped]
        [DisplayName("Logo")]
        public string Logo { get; set; }

        [Required]
        public Guid Link_AUID { get; set; }
    }

    //分会&校友关系表
    public class Alumni_Union_Member_Link
    {
        public Alumni_Union_Member_Link()
        {
            Link_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Link_AUID = Guid.Empty;
            Link_AMID = Guid.Empty;
        }

        [Key]
        public Guid Link_ID { get; set; }

        [Required]
        [DisplayName("加入时间")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("分会")]
        public Guid Link_AUID { get; set; }

        [Required]
        [DisplayName("校友")]
        public Guid Link_AMID { get; set; }
    }

    public class Alumni_Member
    {
        public Alumni_Member()
        {
            AMID = Guid.Empty;
            Create_DT = DateTime.Now;
            Sign_DT = string.Empty;
            Img = string.Empty;
            Member_Name = string.Empty;
            Mobile = string.Empty;
            Gender = string.Empty;
            Is_Frozen = string.Empty;
            Year = 0;
            Faculty = string.Empty;
            Interest = string.Empty;
            Company = string.Empty;
            Position = string.Empty;
            Profession = string.Empty;
            Degree = string.Empty;
            Native_Province = string.Empty;
            Native_City = string.Empty;
            Native_District = string.Empty;
            Province = string.Empty;
            City = string.Empty;
            District = string.Empty;
            Status = string.Empty;
            Avatar_Url = string.Empty;
            Nick_Name = string.Empty;
            OpenID = string.Empty;
            Session_Key = string.Empty;
            Authority_Type = string.Empty;
            Authority_Type_List = Enum.GetNames(typeof(Authority_Type_Enum)).ToList();
            Authority_Province = string.Empty;
            Authority_City = string.Empty;
            Flag = 0;
            Link_AUID = Guid.Empty;
            Interest_List = new List<string>();
            Faculty_List = new List<string>();
            Alumni_Union_List = new List<Alumni_Union>();
        }

        [Key]
        public Guid AMID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [NotMapped]
        [DisplayName("注册时间")]
        public string Sign_DT { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("校友头像")]
        public string Img { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("校友姓名")]
        public string Member_Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("手机号")]
        public string Mobile { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("性别")]
        public string Gender { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("当前状态")]
        public string Is_Frozen { get; set; }

        [Required]
        [DisplayName("毕业年份")]
        public int Year { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("毕业院系")]
        public string Faculty { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("所获学位")]
        public string Degree { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("所处行业")]
        public string Profession { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("兴趣")]
        public string Interest { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("公司")]
        public string Company { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("职位")]
        public string Position { get; set; }

        //省
        [Required(AllowEmptyStrings = true)]
        [DisplayName("籍贯")]
        public string Native_Province { get; set; }

        //市
        [Required(AllowEmptyStrings = true)]
        [DisplayName("籍贯")]
        public string Native_City { get; set; }

        //区
        [Required(AllowEmptyStrings = true)]
        [DisplayName("籍贯")]
        public string Native_District { get; set; }

        //省
        [Required(AllowEmptyStrings = true)]
        [DisplayName("所属区域")]
        public string Province { get; set; }

        //市
        [Required(AllowEmptyStrings = true)]
        public string City { get; set; }

        //区
        [Required(AllowEmptyStrings = true)]
        public string District { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("身份验证")]
        public string Status { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("授权登录的头像")]
        public string Avatar_Url { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("授权登录的昵称")]
        public string Nick_Name { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("授权登录的OpenID")]
        public string OpenID { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("授权登录的密钥")]
        public string Session_Key { get; set; }

        [NotMapped]
        [DisplayName("分会全称")]
        public string Name_Full { get; set; }

        [NotMapped]
        [DisplayName("校友会简称")]
        public string Name_Short { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("权限配置")]
        public string Authority_Type { get; set; }

        [NotMapped]
        [DisplayName("权限配置")]
        public List<string> Authority_Type_List { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("区域权限-省")]
        public string Authority_Province { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("区域权限-市")]
        public string Authority_City { get; set; }

        [NotMapped]
        [DisplayName("Logo")]
        public string Logo { get; set; }

        [Required]
        public Guid Link_AUID { get; set; }

        [NotMapped]
        public int Flag { get; set; }

        [NotMapped]
        public List<string> Interest_List { get; set; }

        [NotMapped]
        public List<Alumni_Union> Alumni_Union_List { get; set; }

        [NotMapped]
        public List<string> Faculty_List { get; set; }
    }

    public enum Privacy_Enum
    {
        显示,
        隐藏
    }

    public enum Authority_Type_Enum
    {
        超级权限,
        区域权限,
    }

    public class Alumni_Member_Enterprise
    {
        public Alumni_Member_Enterprise()
        {
            MEID = Guid.Empty;
            Create_DT = DateTime.Now;
            Logo = string.Empty;
            Name = string.Empty;
            Duty = string.Empty;
            Contact = string.Empty;
            Email = string.Empty;
            Profession = string.Empty;
            Address = string.Empty;
            Province = string.Empty;
            City = string.Empty;
            District = string.Empty;
            Detial = string.Empty;
            Main_Business = string.Empty;
            Link_AMID = Guid.Empty;
        }

        [Key]
        public Guid MEID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("企业Logo")]
        public string Logo { get; set; }

        [Required]
        [DisplayName("企业全称")]
        public string Name { get; set; }

        [NotMapped]
        [DisplayName("校友名称")]
        public string Member_Name { get; set; }

        [Required]
        [DisplayName("担任职务")]
        public string Duty { get; set; }

        [Required]
        [DisplayName("联系方式")]
        public string Contact { get; set; }

        [Required]
        [DisplayName("电子邮箱")]
        public string Email { get; set; }

        [Required]
        [DisplayName("所处行业")]
        public string Profession { get; set; }

        [Required]
        [DisplayName("详细地址")]
        public string Address { get; set; }

        [Required]
        [DisplayName("所属区域")]
        public string Province { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string City { get; set; }

        //区、县
        [Required(AllowEmptyStrings = true)]
        public string District { get; set; }

        [Required]
        [DisplayName("公司简介")]
        public string Detial { get; set; }

        [Required]
        [DisplayName("产品介绍")]
        public string Main_Business { get; set; }

        [Required]
        [DisplayName("关联校友")]
        public Guid Link_AMID { get; set; }

        [Required]
        [DisplayName("关联校友会")]
        public Guid Link_AUID { get; set; }
    }

    [NotMapped]
    public class Alumni_Union_Filter
    {
        public Alumni_Union_Filter()
        {
            PageIndex = 0;
            PageSize = 100;
            Keyword_Name = string.Empty;
            Roles_Title = string.Empty;
            Roles_Title_List = Enum.GetNames(typeof(Alumni_Member_Title_Enum)).ToList();
            Is_Frozen = string.Empty;
            Is_Frozen_List = Enum.GetNames(typeof(Alumni_Is_Frozen_Enum)).ToList();
            Union_Type = string.Empty;
            Union_Type_List = Enum.GetNames(typeof(Union_Type_Enum)).ToList();
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword_Name { get; set; }
        public string Roles_Title { get; set; }
        public List<string> Roles_Title_List { get; set; }
        public string Is_Frozen { get; set; }
        public List<string> Is_Frozen_List { get; set; }
        public string Union_Type { get; set; }
        public List<string> Union_Type_List { get; set; }
    }

    [NotMapped]
    public class Alumni_Member_Filter
    {
        public Alumni_Member_Filter()
        {
            PageIndex = 0;
            PageSize = 100;
            Keyword_Name = string.Empty;
            Keyword_Faculty = string.Empty;
            Faculty_List = new List<string>();
            Keyword_Year = string.Empty;
            Year_List = new List<string>();
            Keyword_Province = string.Empty;
            Province_List = new List<string>();
            Keyword_City = string.Empty;
            City_List = new List<string>();
            Keyword_District = string.Empty;
            District_List = new List<string>();
            Status = string.Empty;
            Is_Frozen = string.Empty;
            Link_AUID = Guid.Empty;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword_Name { get; set; }

        public string Keyword_Faculty { get; set; }
        public List<string> Faculty_List { get; set; }

        public string Keyword_Year { get; set; }
        public List<string> Year_List { get; set; }

        public string Keyword_Province { get; set; }
        public List<string> Province_List { get; set; }

        public string Keyword_City { get; set; }
        public List<string> City_List { get; set; }

        public string Keyword_District { get; set; }
        public List<string> District_List { get; set; }

        public string Status { get; set; }
        public string Is_Frozen { get; set; }
        public Guid Link_AUID { get; set; }
    }

    public class Alumni_Union_User_Login_Record
    {
        public Alumni_Union_User_Login_Record()
        {
            ULR_ID = Guid.Empty;
            Link_UID = Guid.Empty;
            Name = string.Empty;
            Name_Full = string.Empty;
            Last_Login_IP = string.Empty;
            Password = string.Empty;
            Last_Login_DT = DateTime.Now;
            Client_Info = string.Empty;
        }

        [Key]
        public Guid ULR_ID { get; set; }

        [Required]
        public Guid Link_UID { get; set; }

        [MaxLength(256)]
        [DisplayName("用户名")]
        public string Name { get; set; }

        [MaxLength(256)]
        [DisplayName("用户全称")]
        public string Name_Full { get; set; }

        [MaxLength(256)]
        [DisplayName("登录密码")]
        public string Password { get; set; }

        [MaxLength(256)]
        [DisplayName("IP")]
        public string Last_Login_IP { get; set; }

        [Required]
        [DisplayName("登录时间")]
        public DateTime Last_Login_DT { get; set; }

        [DefaultValue("")]
        [DisplayName("客户端信息")]
        public string Client_Info { get; set; }
    }

    //所处行业分类
    [NotMapped]
    public class Profession_First
    {
        public Profession_First()
        {
            First_Name = string.Empty;
            Second_List = new List<Profession_Second>();
        }

        public string First_Name { get; set; }
        public List<Profession_Second> Second_List { get; set; }
    }

    public class Profession_Second
    {
        public Profession_Second()
        {
            Second_Name = string.Empty;
            Link_Name = string.Empty;
        }

        public string Second_Name { get; set; }
        public string Link_Name { get; set; }
    }

    public enum Alumni_Is_Admin_Enum
    {
        Admin,
    }

    public enum Alumni_Member_Title_Enum
    {
        文案秘书,
        商务秘书,
        会长,
        秘书长,
    }

    public enum Alumni_Is_Frozen_Enum
    {
        启用,
        冻结,
    }

    public enum Degree_Enum
    {
        博士,
        硕士,
        本科,
        专科
    }

    public enum Gender_Enum
    {
        男,
        女
    }

    public enum Status_Enum
    {
        待认证,
        已认证
    }

    public enum Is_Auditor_Enum
    {
        是,
        否
    }
}

