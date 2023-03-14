using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace League.Api
{
    public class Mini_Service
    {
        public Mini_Service()
        {
            MSID = Guid.Empty;
            Create_DT = DateTime.Now;
            Cover_Img = string.Empty;
            Type = string.Empty;
            Name = string.Empty;
            Address = string.Empty;
            Intro = string.Empty;
            Linkman = string.Empty;
            Contact = string.Empty;
            Main_Business = string.Empty; 
        }

        [Key]
        public Guid MSID { get; set; }

        [Required]
        [DisplayName("创建日期")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("封面图")]
        public string Cover_Img { get; set; }

        [Required]
        [DisplayName("类型")]
        public string Type { get; set; }

        [Required]
        [DisplayName("公司名称")]
        public string Name { get; set; }

        [Required]
        [DisplayName("产品服务")]
        public string Main_Business { get; set; }

        [Required]
        [DisplayName("地址")]
        public string Address { get; set; }

        [Required]
        [DisplayName("联系人")]
        public string Linkman { get; set; }

        [Required]
        [DisplayName("联系方式")]
        public string Contact { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("公司简介")]
        public string Intro { get; set; }

        [Required]
        [DisplayName("发布状态")]
        public string Status { get; set; }
    }

    [NotMapped]
    public class Mini_Service_Filter
    {
        public Mini_Service_Filter()
        {
            PageIndex = 0;
            PageSize = 100;
            Keyword_Name = string.Empty;
            Keyword_Type = string.Empty;
            Link_AID = Guid.Empty;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword_Name { get; set; }
        public string Keyword_Type { get; set; }
        public Guid Link_AID { get; set; }
    }

    public enum Type_Status_Enum
    {
        企业,
        个人,
        综合
    }
}
