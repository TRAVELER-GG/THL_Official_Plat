using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace League.Api
{
    public class User_Plat
    {
        public User_Plat()
        {
            UID = Guid.Empty;
            Create_DT = DateTime.Now;
            Name = string.Empty;
            Name_Full = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Tel = string.Empty;
            Is_Frozen = string.Empty;
            Is_Admin = string.Empty;
            Roles_Title = string.Empty;
            Roles_Title_List = Enum.GetNames(typeof(User_Plat_Title_Enum)).ToList();
        }

        [Key]
        public Guid UID { get; set; }

        [Required]
        [DisplayName("创建日期")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("用户名")]
        public string Name { get; set; }

        [Required]
        [DisplayName("用户全称")]
        public string Name_Full { get; set; }

        [Required]
        [DisplayName("密码")]
        public string Password { get; set; }

        [Required]
        [DisplayName("电子邮箱")]
        public string Email { get; set; }

        [Required]
        [DisplayName("电话号码")]
        public string Tel { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Is_Admin { get; set; }

        [Required]
        [DisplayName("当前状态")]
        public string Is_Frozen { get; set; }

        [Required]
        [DisplayName("系统角色")]
        public string Roles_Title { get; set; }

        [NotMapped]
        public List<string> Roles_Title_List { get; set; }
    }

    public class User_Plat_Login_Record
    {
        public User_Plat_Login_Record()
        {
            UPLR_ID = Guid.Empty;
            Link_UID = Guid.Empty;
            Name = string.Empty;
            Name_Full = string.Empty;
            Last_Login_IP = string.Empty;
            Password = string.Empty;
            Last_Login_DT = DateTime.Now;
            Client_Info = string.Empty;
        }

        [Key]
        public Guid UPLR_ID { get; set; }

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

    public class User_Plat_Filter
    {
        public User_Plat_Filter()
        {
            PageIndex = 0;
            PageSize = 100;
            Keyword_Name = string.Empty;
            Roles_Title = string.Empty;
            Roles_Title_List = Enum.GetNames(typeof(User_Plat_Title_Enum)).ToList();
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword_Name { get; set; }
        public string Roles_Title { get; set; }
        public List<string> Roles_Title_List { get; set; }
    }

    public enum User_Plat_Is_Admin_Enum
    {
        Admin,
    }

    public enum User_Plat_Title_Enum
    {
        资讯专员,
        活动专员,
        商务专员,
    }

    public enum Is_Freeze_Enum
    {
        启用,
        冻结
    }
}
