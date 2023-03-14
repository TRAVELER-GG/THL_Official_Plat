using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace League.Api
{
    public class Follow
    {
        public Follow()
        {
            FID = Guid.Empty;
            Create_DT = DateTime.Now;
            Follow_AMID = Guid.Empty;
            Follow_MEID = Guid.Empty;
            Follow_MSID = Guid.Empty;
            Fans_AMID = Guid.Empty;
        }

        [Key]
        public Guid FID { get; set; }

        [Required]
        [DisplayName("创建日期")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("关注人")]
        public Guid Follow_AMID { get; set; }
        
        [Required]
        [DisplayName("关注企业")]
        public Guid Follow_MEID { get; set; }

        [Required]
        [DisplayName("关注微服坊")]
        public Guid Follow_MSID { get; set; }

        [Required]
        [DisplayName("我自己")]
        public Guid Fans_AMID { get; set; }
    }
}
