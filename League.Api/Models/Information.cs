using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace League.Api
{
    public class Information
    {
        public Information()
        {
            IMID = Guid.Empty;
            Create_DT = DateTime.Now;
            Title = string.Empty;
            Classify = string.Empty;
            Cover_Img = string.Empty;
            Content = string.Empty;
            Type = string.Empty;
            Publisher = string.Empty;
            Infor_Status = string.Empty;
            Source = string.Empty;
            Infor_PV = 0;
            Alumni_Union_List = new List<Alumni_Union>();
            Link_AUID = Guid.Empty;
        }

        [Key]
        public Guid IMID { get; set; }

        [Required]
        [DisplayName("发布时间")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("标题")]
        public string Title { get; set; }

        [Required]
        [DisplayName("资讯分类")]
        public string Classify { get; set; }

        [Required]
        [DisplayName("封面")]
        public string Cover_Img { get; set; }

        [Required(AllowEmptyStrings = true)]
        [DisplayName("内容")]
        public string Content { get; set; }

        [Required]
        [DisplayName("发布类型")]
        public string Type { get; set; }

        [Required]
        [DisplayName("发布人")]
        public string Publisher { get; set; }

        [Required]
        [DisplayName("发布状态")]
        public string Infor_Status { get; set; }

        [Required]
        [DisplayName("资讯来源")]
        public string Source { get; set; }

        [NotMapped]
        [DisplayName("资讯来源")]
        public List<Alumni_Union> Alumni_Union_List { get; set; }

        [Required]
        [DisplayName("浏览量")]
        public int Infor_PV { get; set; }

        [Required]
        public Guid Link_AUID { get; set; }
    }

    [NotMapped]
    public class Information_Filter
    {
        public Information_Filter()
        {
            IMID = Guid.Empty;
            PageIndex = 0;
            PageSize = 100;
            Keyword_Title = string.Empty;
            Infor_Status = string.Empty;
            Alumni_Union_List = new List<Alumni_Union>();
            Link_AUID = new Guid("00000000-0000-0000-0000-000000000001"); 
        }
        public Guid IMID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword_Title { get; set; }
        public string Classify { get; set; }
        public string Infor_Status { get; set; }
        public List<Alumni_Union> Alumni_Union_List { get; set; }
        public Guid Link_AUID { get; set; }
    }

    public enum Infor_Status_Enum
    {
        待发布,
        已发布,
    }

    public enum Infor_Type_Enum
    {
        普通资讯,
        广告资讯,
    }

    public enum Infor_Classify_Enum
    {
        热点新闻,
        行业政策,
        校友活动,
        校友风采,
    }

    public enum Publisher_Enum
    {
        校友总会,
    }
}
