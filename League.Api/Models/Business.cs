using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace League.Api
{
    //入驻商家
    public class Business
    {
        public Business()
        {
            BID = Guid.Empty;
            Create_DT = DateTime.Now;
            Bus_Img = string.Empty;
            Bus_Name = string.Empty;
            Bus_Name_Register = string.Empty;
            Bus_Type = string.Empty;
            Bus_Type_List = Enum.GetNames(typeof(Bus_Type_Enum)).ToList();
            Bus_Recommend = string.Empty;
            Bus_Hours = string.Empty;
            Bus_Province = string.Empty;
            Bus_City = string.Empty;
            Bus_District = string.Empty;
            Bus_Address = string.Empty;
            Bus_Contact = string.Empty;
            Bus_Phone = string.Empty;
            Bus_Intro = string.Empty;
            Bus_Is_Freeze = string.Empty;
            Bus_PV = 0;
        }

        [Key]
        public Guid BID { get; set; }

        [Required]
        [DisplayName("创建时间")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("商家店面图")]
        public string Bus_Img { get; set; }

        [Required]
        [DisplayName("商家店名")]
        public string Bus_Name { get; set; }

        [Required]
        [DisplayName("注册名称")]
        public string Bus_Name_Register { get; set; }

        [Required]
        [DisplayName("商家类别")]
        public string Bus_Type { get; set; }

        [NotMapped]
        [DisplayName("商家类别")]
        public List<string> Bus_Type_List { get; set; }

        [NotMapped]
        [DisplayName("会费状态")]
        public List<string> Bus_Dues_Status_List { get; set; }

        [Required]
        [DisplayName("所在区域-省")]
        public string Bus_Province { get; set; }

        [Required]
        [DisplayName("所在区域-市")]
        public string Bus_City { get; set; }

        [Required]
        [DisplayName("所在区域-区")]
        public string Bus_District { get; set; }

        [Required]
        [DisplayName("商家地址")]
        public string Bus_Address { get; set; }

        [Required]
        [DisplayName("联系人")]
        public string Bus_Contact { get; set; }

        [Required]
        [DisplayName("联系电话")]
        public string Bus_Phone { get; set; }

        [Required]
        [DisplayName("推荐语")]
        public string Bus_Recommend { get; set; }

        [Required]
        [DisplayName("营业时间")]
        public string Bus_Hours { get; set; }

        [Required]
        [DisplayName("商家简介")]
        public string Bus_Intro { get; set; }

        [Required]
        [DisplayName("启用状态")]
        public string Bus_Is_Freeze { get; set; }

        [Required]
        [DisplayName("浏览量")]
        public int Bus_PV { get; set; }

        [NotMapped]
        [DisplayName("证书执照")]
        public List<Business_Certificate> Cert_List { get; set; }

        [NotMapped]
        [DisplayName("商家相册")]
        public List<Business_Album> Album_List { get; set; }

        [NotMapped]
        public int Album_List_Count { get; set; }

        

        [NotMapped]
        [DisplayName("优惠优惠卡券")]
        public List<Business_Coupon> Coupon_List { get; set; }
    }

    //缴费记录
    public class Business_Dues
    {
        public Business_Dues()
        {
            Dues_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Dues_Start_DT = DateTime.Now;
            Dues_End_DT = DateTime.Now;
            Dues = 0;
            Dues_Remark = string.Empty;
            Link_BID = Guid.Empty;
        }

        [Key]
        public Guid Dues_ID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("会费起止")]
        public DateTime Dues_Start_DT { get; set; }

        [Required]
        [DisplayName("会费起止")]
        public DateTime Dues_End_DT { get; set; }

        [Required]
        [DisplayName("缴纳会费")]
        public decimal Dues { get; set; }

        [Required]
        [DisplayName("备注")]
        public string Dues_Remark { get; set; }

        [Required]
        [DisplayName("关联商家")]
        public Guid Link_BID { get; set; }
    }

    //证书执照
    public class Business_Certificate
    {
        public Business_Certificate()
        {
            Cert_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Cert_Type = string.Empty;
            Cert_Img = string.Empty;
            Cert_Type_List = Enum.GetNames(typeof(Cert_Type_Enum)).ToList();
            Link_BID = Guid.Empty;
        }

        [Key]
        public Guid Cert_ID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("证书类型")]
        public string Cert_Type { get; set; }

        [NotMapped]
        public List<string> Cert_Type_List { get; set; }

        [Required]
        public string Cert_Img { get; set; }

        [Required]
        [DisplayName("关联商家")]
        public Guid Link_BID { get; set; }
    }

    //商家相册
    public class Business_Album
    {
        public Business_Album()
        {
            Album_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Album_Type = string.Empty;
            Album_Describe = string.Empty;
            Album_Img = string.Empty;
            Link_BID = Guid.Empty;
        }

        [Key]
        public Guid Album_ID { get; set; }

        [Required]
        [DisplayName("上传时间")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("类型")]
        public string Album_Type { get; set; }

        [Required]
        [DisplayName("相册描述")]
        public string Album_Describe { get; set; }

        [Required]
        public string Album_Img { get; set; }

        [Required]
        [DisplayName("关联商家")]
        public Guid Link_BID { get; set; }
    }

    //优惠卡券
    public class Business_Coupon
    {
        public Business_Coupon()
        {
            Coupon_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Coupon_Img = string.Empty;
            Coupon_NO = string.Empty;
            Coupon_Name = string.Empty;
            Coupon_Quantity = 0;
            Coupon_Quantity_Surplus = 0;
            Expiry_DT = DateTime.Now;
            Expiry_DT_Show = string.Empty;
            Cancel_DT_Show = string.Empty;
            Coupon_Province = string.Empty;
            Coupon_City = string.Empty;
            Coupon_Status = string.Empty;
            Coupon_Status_List = Enum.GetNames(typeof(Coupon_Status_Enum)).ToList();
            Coupon_Type = string.Empty;
            Coupon_Type_List = Enum.GetNames(typeof(Coupon_Type_Enum)).ToList();
            Coupon_Price = 0;
            Coupon_Remark = string.Empty;
            Link_BID = Guid.Empty;
            Business = new Business();
            Coupon_Member_List = new List<Business_Coupon_Member>();
        }

        [Key]
        public Guid Coupon_ID { get; set; }

        [Required]
        [DisplayName("创建日期")]
        public DateTime Create_DT { get; set; }

        [Required]
        public string Coupon_Img { get; set; }

        [Required]
        [DisplayName("优惠卡券编号")]
        public string Coupon_NO { get; set; }

        [Required]
        [DisplayName("优惠卡券名称")]
        public string Coupon_Name { get; set; }

        [Required]
        [DisplayName("发放数量")]
        public int Coupon_Quantity { get; set; }

        [NotMapped]
        [DisplayName("优惠卡券余量")]
        public int Coupon_Quantity_Surplus { get; set; }

        [Required]
        [DisplayName("失效日期")]
        public DateTime Expiry_DT { get; set; }

        [NotMapped]
        [DisplayName("失效日期")]
        public string Expiry_DT_Show { get; set; }

        [NotMapped]
        [DisplayName("核销日期")]
        public string Cancel_DT_Show { get; set; }

        [Required]
        [DisplayName("所在区域-省")]
        public string Coupon_Province { get; set; }

        [Required]
        [DisplayName("所在区域-市")]
        public string Coupon_City { get; set; }

        [Required]
        [DisplayName("优惠卡券状态")]
        public string Coupon_Status { get; set; }

        [NotMapped]
        [DisplayName("优惠卡券状态")]
        public List<string> Coupon_Status_List { get; set; }

        [Required]
        [DisplayName("优惠类型")]
        public string Coupon_Type { get; set; }

        [NotMapped]
        [DisplayName("优惠类型")]
        public List<string> Coupon_Type_List { get; set; }

        [Required]
        [DisplayName("优惠数额")]
        public decimal Coupon_Price { get; set; }

        [NotMapped]
        [DisplayName("是否领取")]
        public string Coupon_Receive { get; set; }

        [Required]
        [DisplayName("优惠卡券备注")]
        public string Coupon_Remark { get; set; }

        [Required]
        [DisplayName("关联商家")]
        public Guid Link_BID { get; set; }

        [NotMapped]
        [DisplayName("关联商家")]
        public Business Business { get; set; }

        [NotMapped]
        [DisplayName("领取会员")]
        public List<Business_Coupon_Member> Coupon_Member_List { get; set; }
    }

    //领取会员
    public class Business_Coupon_Member
    {
        public Business_Coupon_Member()
        {
            Cou_Mem_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Cancel_DT = string.Empty;
            Cancel_DT_Show = string.Empty;
            Link_AMID = Guid.Empty;
            Link_AMID_Name = string.Empty;
            Link_Coupon_ID = Guid.Empty;
            Link_Coupon_ID_Name = string.Empty;
            Business_Coupon = new Business_Coupon();
            Link_BID = Guid.Empty;
            Business = new Business();
        }

        [Key]
        public Guid Cou_Mem_ID { get; set; }

        [Required]
        [DisplayName("领取时间")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("核销时间")]
        public string Cancel_DT { get; set; }

        [NotMapped]
        [DisplayName("核销时间")]
        public string Cancel_DT_Show { get; set; }

        [Required]
        [DisplayName("领取会员")]
        public Guid Link_AMID { get; set; }

        [Required]
        [DisplayName("领取会员")]
        public string Link_AMID_Name { get; set; }

        [Required]
        [DisplayName("关联优惠卡券")]
        public Guid Link_Coupon_ID { get; set; }

        [Required]
        [DisplayName("券名")]
        public string Link_Coupon_ID_Name { get; set; }

        [NotMapped]
        [DisplayName("关联卡券")]
        public Business_Coupon Business_Coupon { get; set; }

        [Required]
        [DisplayName("关联商家")]
        public Guid Link_BID { get; set; }

        [NotMapped]
        [DisplayName("关联商家")]
        public Business Business { get; set; }
    }

    //评论区
    public class Business_Comment
    {
        public Business_Comment()
        {
            Bus_Com_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Satisfaction = 0;
            Comment = string.Empty;
            Comment_Type = string.Empty;
            Link_AMID = Guid.Empty;
            Link_AMID_Name = string.Empty;
            Link_BID = Guid.Empty;
        }

        [Key]
        public Guid Bus_Com_ID { get; set; }

        [Required]
        [DisplayName("评论时间")]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("满意度")]
        public decimal Satisfaction { get; set; }

        [Required]
        [DisplayName("评论内容")]
        public string Comment { get; set; }

        [Required]
        [DisplayName("评论方式")]
        public string Comment_Type { get; set; }

        [Required]
        [DisplayName("评论人")]
        public Guid Link_AMID { get; set; }

        [Required]
        [DisplayName("评论人")]
        public string Link_AMID_Name { get; set; }

        [Required]
        [DisplayName("关联商家")]
        public Guid Link_BID { get; set; }
    }

    //评论图片
    public class Business_Comment_Img
    {
        public Business_Comment_Img()
        {
            Com_Img_ID = Guid.Empty;
            Create_DT = DateTime.Now;
            Img = string.Empty;
            Link_AMID = Guid.Empty;
            Link_Bus_Com_ID = Guid.Empty;
            Link_BID = Guid.Empty;
        }

        [Key]
        public Guid Com_Img_ID { get; set; }

        [Required]
        public DateTime Create_DT { get; set; }

        [Required]
        [DisplayName("评论图片")]
        public string Img { get; set; }

        [Required]
        [DisplayName("评论人")]
        public Guid Link_AMID { get; set; }

        [Required]
        [DisplayName("关联评论")]
        public Guid Link_Bus_Com_ID { get; set; }

        [Required]
        [DisplayName("关联商家")]
        public Guid Link_BID { get; set; }
    }

    [NotMapped]
    public class Business_Filter
    {
        public Business_Filter()
        {
            PageIndex = 0;
            PageSize = 100;
            Keyword_Bus_Name = string.Empty;

            Bus_Province = string.Empty;
            Bus_Province_List = new List<string>();
            Bus_City = string.Empty;
            Bus_City_List = new List<string>();
            Bus_District = string.Empty;
            Bus_District_List = new List<string>();

            Bus_Type = string.Empty;
            Bus_Type_List = new List<string>();
            Coupon_Status = string.Empty;
            Coupon_Status_List = Enum.GetNames(typeof(Coupon_Status_Enum)).ToList();
            Link_BID = Guid.Empty;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword_Bus_Name { get; set; }
        public string Keyword_Coupon_Name { get; set; }

        public string Bus_Province { get; set; }
        public List<string> Bus_Province_List { get; set; }

        public string Bus_City { get; set; }
        public List<string> Bus_City_List { get; set; }

        public string Bus_District { get; set; }
        public List<string> Bus_District_List { get; set; }

        public string Bus_Type { get; set; }
        public List<string> Bus_Type_List { get; set; }
        public string Coupon_Status { get; set; }
        public List<string> Coupon_Status_List { get; set; }
        public Guid Link_BID { get; set; }
    }

    public enum Comment_Type_Enum
    {
        实名,
        匿名
    }

    public enum Coupon_Type_Enum
    {
        折扣,
        金额,
    }

    public enum Coupon_Status_Enum
    {
        关闭,
        启用
    }

    public enum Album_Type_Enum
    {
        相册,
        封面,
    }

    public enum Cert_Type_Enum
    {
        营业执照,
        资质证书,
        其他证书,
    }

    public enum Bus_Dues_Status_Enum
    {
        已缴纳,
        待缴纳
    }

    public enum Bus_Is_Freeze_Enum
    {
        启用,
        冻结
    }

    public enum Bus_Type_Enum
    {
        餐饮会所,
        精品民宿,
        网红茶社,
        亲子农场,
    }
}
