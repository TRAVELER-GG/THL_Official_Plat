using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace League.Api
{
    public class Activity
    {
        public Activity()
        {
            AID = Guid.Empty;
            Create_DT = DateTime.Now;
            Start_DT = DateTime.Now;
            End_DT = DateTime.Now;
            Title = string.Empty;
            Location = string.Empty;
            Poster = string.Empty;
            Sponsor = string.Empty;
            Num_of_people = 0;
            Content = string.Empty;
            Status = string.Empty;
            People = 0;
            Activity_PV = 0;
            Scope = string.Empty;
            Link_UID = Guid.Empty;
            Link_AUID = Guid.Empty;
            Union_List = new List<Alumni_Union>();
        }

        [Key]
        public Guid AID { get; set; }

        [Required]
        [DisplayName("创建日期")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("标题")]
        public string Title { get; set; }

        [Required]
        [DisplayName("活动开始")]
        public DateTime Start_DT { get; set; }

        [Required]
        [DisplayName("活动结束")]
        public DateTime End_DT { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("活动地址")]
        public string Location { get; set; }

        [Required]
        [DisplayName("活动海报")]
        public string Poster { get; set; }

        [Required]
        [DisplayName("主办方")]
        public string Sponsor { get; set; }

        [Required]
        [DisplayName("活动人数")]
        public int Num_of_people { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("内容")]
        public string Content { get; set; }

        [Required]
        [DisplayName("活动状态")]
        public string Status { get; set; }

        [Required]
        [DisplayName("报名对象")]
        public string Scope { get; set; }

        [NotMapped]
        [DisplayName("已参与人数")]
        public int People { get; set; }

        [Required]
        [DisplayName("浏览量")]
        public int Activity_PV { get; set; }

        [Required]
        [DisplayName("发布人")]
        public Guid Link_UID { get; set; }

        [Required]
        [DisplayName("主办方")]
        public Guid Link_AUID { get; set; }

        [NotMapped]
        [DisplayName("主办方")]
        public List<Alumni_Union> Union_List { get; set; }
    }

    public class Activity_Apply
    {
        public Activity_Apply()
        {
            AAID = Guid.Empty;
            Create_DT = DateTime.Now;
            Name = string.Empty;
            Phone = string.Empty;
            Link_AMID = Guid.Empty;
            Link_AID = Guid.Empty;
        }

        [Key]
        public Guid AAID { get; set; }

        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("姓名")]
        public string Name { get; set; }

        [Required]
        [DisplayName("手机号")]
        public string Phone { get; set; }

        [NotMapped]
        public string Gender { get; set; }

        [NotMapped]
        public string Alumni_Union { get; set; }

        [NotMapped]
        public string Faculty { get; set; }

        [Required]
        public Guid Link_AMID { get; set; }

        [Required]
        public Guid Link_AID { get; set; }

    }

    [NotMapped]
    public class Activity_Filter
    {
        public Activity_Filter()
        {
            PageIndex = 0;
            PageSize = 100;
            Keyword_Title = string.Empty;
            Keyword_Sponsor = string.Empty;
            Link_AID = Guid.Empty;
            Link_AUID = Guid.Empty;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword_Title { get; set; }
        public string Keyword_Sponsor { get; set; }
        public Guid Link_AID { get; set; }
        public Guid Link_AUID { get; set; }
    }

    public enum Act_Status_Enum
    {
        已发布,
        待发布,
        已结束,
    }

    public enum Act_Scope_Enum
    {
        分会,
        总会
    }
}
