using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace League.Api
{
    public interface IBusiness_Service
    {
        PageList<Business> Get_Business_PageList(Business_Filter MF);
        Business Get_Business_Item(Guid BID);
        void Create_Business(Business Item);
        void Set_Business(Guid BID, Business Item);
        void Set_Business_Img(Guid BID, string Cover_Img);
        void Del_Business(Guid BID);

        PageList<Business_Dues> Get_Business_Dues_PageList(Business_Filter MF);
        Business_Dues Get_Business_Dues_Item(Guid Dues_ID);
        void Create_Business_Dues(Business_Dues Item);
        void Set_Business_Dues(Business_Dues Item);
        void Del_Business_Dues(Guid Dues_ID);

        List<Business_Certificate> Get_Business_Certificate_List(Guid BID);
        Business_Certificate Get_Business_Certificate_Item(Guid Cert_ID);
        void Upload_Business_Certificate(Business_Certificate Item);
        void Delete_Business_Certificate(Guid Cert_ID);

        List<Business_Album> Get_Business_Album_List(Guid BID);
        Business_Album Get_Business_Album_Item(Guid Album_ID);
        void Upload_Business_Album(Business_Album Item);
        void Delete_Business_Album(Guid Album_ID);

        PageList<Business_Coupon> Get_Business_Coupon_PageList(Business_Filter MF);
        Business_Coupon Get_Business_Coupon_Item(Guid Coupon_ID);
        void Create_Business_Coupon(Business_Coupon Item);
        void Set_Business_Coupon(Business_Coupon Item);
        void Del_Business_Coupon(Guid Coupon_ID);
        void Create_Business_Coupon_Member(Guid Link_Coupon_ID, Guid Link_AMID);
    }

    public partial class Business_Service : IBusiness_Service
    {
        private readonly League_DbContext db = new League_DbContext();
    }

    //商家信息
    public partial class Business_Service : IBusiness_Service
    {
        public PageList<Business> Get_Business_PageList(Business_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Business.AsQueryable();

            MF.Bus_Type_List = query.Select(x => x.Bus_Type).Distinct().ToList();
            MF.Bus_Province_List = query.Select(x => x.Bus_Province).Distinct().ToList();
            if (!string.IsNullOrEmpty(MF.Bus_Province))
            {
                query = query.Where(x => x.Bus_Province.Contains(MF.Bus_Province)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Bus_City))
            {
                query = query.Where(x => x.Bus_City.Contains(MF.Bus_City)).AsQueryable();
            }
            MF.Bus_City_List = query.Select(x => x.Bus_City).Distinct().ToList();

            if (!string.IsNullOrEmpty(MF.Bus_District))
            {
                query = query.Where(x => x.Bus_District.Contains(MF.Bus_District)).AsQueryable();
            }
            MF.Bus_District_List = query.Select(x => x.Bus_District).Distinct().ToList();


            if (!string.IsNullOrEmpty(MF.Bus_Type))
            {
                query = query.Where(x => x.Bus_Type == MF.Bus_Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Bus_Name))
            {
                query = query.Where(x => x.Bus_Name.Contains(MF.Keyword_Bus_Name)).AsQueryable();
            }

            List<Business> Row_List = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<Business> PList = new PageList<Business>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public Business Get_Business_Item(Guid BID)
        {
            Business Item = db.Business.Find(BID);
            Item = Item ?? new Business();
            Item.Cert_List = db.Business_Certificate.Where(x => x.Link_BID == BID).OrderByDescending(x => x.Create_DT).ToList();
            Item.Album_List = db.Business_Album.Where(x => x.Link_BID == BID).OrderByDescending(x => x.Create_DT).ToList();
            Item.Coupon_List = db.Business_Coupon.Where(x => x.Link_BID == BID).OrderByDescending(x => x.Create_DT).ToList();
            return Item;
        }

        public void Create_Business(Business Item)
        {
            Obj_Init.Trim(Item);
            Item.BID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;
            Item.Bus_Is_Freeze = Bus_Is_Freeze_Enum.启用.ToString();
            if (Item.Bus_Province != "江苏省") { throw new Exception("当前仅支持江苏省区域"); }

            this.Check_Business_Info(Item);
            db.Business.Add(Item);
            db.SaveChanges();
        }

        public void Set_Business(Guid BID, Business Item)
        {
            Obj_Init.Trim(Item);
            Business OLD_Item = db.Business.Find(BID);

            OLD_Item.Bus_Name = Item.Bus_Name;
            OLD_Item.Bus_Name_Register = Item.Bus_Name_Register;
            OLD_Item.Bus_Province = Item.Bus_Province;
            OLD_Item.Bus_City = Item.Bus_City;
            OLD_Item.Bus_District = Item.Bus_District;
            OLD_Item.Bus_Address = Item.Bus_Address;
            OLD_Item.Bus_Contact = Item.Bus_Contact;
            OLD_Item.Bus_Phone = Item.Bus_Phone;
            OLD_Item.Bus_Type = Item.Bus_Type;
            OLD_Item.Bus_Hours = Item.Bus_Hours;
            OLD_Item.Bus_Intro = Item.Bus_Intro;
            OLD_Item.Bus_Recommend = Item.Bus_Recommend;

            if (OLD_Item.Bus_Province != "江苏省") { throw new Exception("当前仅支持江苏省区域"); }

            this.Check_Business_Info(OLD_Item);
            db.Entry(OLD_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Business_Img(Guid BID, string Cover_Img)
        {
            Business Item = db.Business.Find(BID);
            Item.Bus_Img = Cover_Img;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void Check_Business_Info(Business Item)
        {
            if (string.IsNullOrEmpty(Item.Bus_Name)) { throw new Exception("请填写商家店名！"); }
            if (string.IsNullOrEmpty(Item.Bus_Name_Register)) { throw new Exception("请填写注册名称！"); }
            if (string.IsNullOrEmpty(Item.Bus_Province)) { throw new Exception("请选择所在省份！"); }
            if (string.IsNullOrEmpty(Item.Bus_City)) { throw new Exception("请选择所在市！"); }
            if (string.IsNullOrEmpty(Item.Bus_District)) { throw new Exception("请选择所在区！"); }
            if (string.IsNullOrEmpty(Item.Bus_Address)) { throw new Exception("请填写店铺地址！"); }
            if (string.IsNullOrEmpty(Item.Bus_Contact)) { throw new Exception("请填写联系人！"); }
            if (string.IsNullOrEmpty(Item.Bus_Phone)) { throw new Exception("请填写联系电话！"); }
            if (string.IsNullOrEmpty(Item.Bus_Type)) { throw new Exception("请选择商家类型！"); }
        }

        public void Del_Business(Guid BID)
        {
            Business Item = db.Business.Find(BID);
            Item ??= new Business();

            //会费记录、证书执照、商家相册、优惠卡券、领取会员
            if (db.Business_Dues.Where(x => x.Link_BID == BID).Any()
             || db.Business_Certificate.Where(x => x.Link_BID == BID).Any()
             || db.Business_Album.Where(x => x.Link_BID == BID).Any()
             || db.Business_Coupon.Where(x => x.Link_BID == BID).Any()
             || db.Business_Coupon_Member.Where(x => x.Link_BID == BID).Any())
            {
                Item.Bus_Is_Freeze = Bus_Is_Freeze_Enum.冻结.ToString();
                db.Entry(Item).State = EntityState.Modified;
            }
            else
            {
                db.Business.Remove(Item);
            }
            db.SaveChanges();
        }
    }

    //商家缴纳会费记录
    public partial class Business_Service : IBusiness_Service
    {
        public PageList<Business_Dues> Get_Business_Dues_PageList(Business_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Business_Dues.Where(x => x.Link_BID == MF.Link_BID).AsQueryable();
            List<Business_Dues> Row_List = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<Business_Dues> PList = new PageList<Business_Dues>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public Business_Dues Get_Business_Dues_Item(Guid Dues_ID)
        {
            Business_Dues Item = db.Business_Dues.Find(Dues_ID);
            Item = Item ?? new Business_Dues();

            Item.Dues = Item.Dues <= 0 ? 0 : Item.Dues;
            return Item;
        }

        public void Create_Business_Dues(Business_Dues Item)
        {
            Obj_Init.Trim(Item);
            Item.Dues_ID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;

            Business Bus = this.Get_Business_Item(Item.Link_BID);
            Item.Link_BID = Bus.BID;
            db.Business_Dues.Add(Item);
            db.SaveChanges();
        }

        public void Set_Business_Dues(Business_Dues Item)
        {
            Obj_Init.Trim(Item);
            Business_Dues Old_Item = db.Business_Dues.Find(Item.Dues_ID);
            Old_Item.Dues = Item.Dues <= 0 ? 0 : Item.Dues;
            Old_Item.Dues_Start_DT = Item.Dues_Start_DT;
            Old_Item.Dues_End_DT = Item.Dues_End_DT;
            Old_Item.Dues_Remark = Item.Dues_Remark;

            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Del_Business_Dues(Guid Dues_ID)
        {
            Business_Dues Item = db.Business_Dues.Find(Dues_ID);
            Item ??= new Business_Dues();
            db.Business_Dues.Remove(Item);
            db.SaveChanges();
        }
    }


    //商家证书
    public partial class Business_Service : IBusiness_Service
    {
        public List<Business_Certificate> Get_Business_Certificate_List(Guid BID)
        {
            List<Business_Certificate> List = db.Business_Certificate.Where(x => x.Link_BID == BID).OrderBy(x => x.Create_DT).ToList();
            return List;
        }

        public Business_Certificate Get_Business_Certificate_Item(Guid Cert_ID)
        {
            Business_Certificate Item = db.Business_Certificate.Find(Cert_ID);
            Item = Item ?? new Business_Certificate();
            return Item;
        }

        public void Upload_Business_Certificate(Business_Certificate Item)
        {
            Business Head = db.Business.Find(Item.Link_BID);
            Item.Link_BID = Head.BID;
            Item.Create_DT = DateTime.Now;
            Item.Cert_Img = Item.Cert_Img == null ? string.Empty : Item.Cert_Img.Trim();

            if (string.IsNullOrEmpty(Item.Cert_Type)) { throw new Exception("证书类型未选择"); }
            db.Business_Certificate.Add(Item);
            db.SaveChanges();
        }

        public void Delete_Business_Certificate(Guid Cert_ID)
        {
            Business_Certificate Item = db.Business_Certificate.Find(Cert_ID);
            db.Business_Certificate.Remove(Item);
            db.SaveChanges();
        }
    }

    //商家相册
    public partial class Business_Service : IBusiness_Service
    {
        public List<Business_Album> Get_Business_Album_List(Guid BID)
        {
            List<Business_Album> List = db.Business_Album.Where(x => x.Link_BID == BID).OrderBy(x => x.Create_DT).ToList();
            return List;
        }

        public Business_Album Get_Business_Album_Item(Guid Album_ID)
        {
            Business_Album Item = db.Business_Album.Find(Album_ID);
            Item = Item ?? new Business_Album();
            return Item;
        }

        public void Upload_Business_Album(Business_Album Item)
        {
            Business Head = db.Business.Find(Item.Link_BID);
            Item.Link_BID = Head.BID;
            Item.Create_DT = DateTime.Now;
            Item.Album_Img = Item.Album_Img == null ? string.Empty : Item.Album_Img.Trim();

            db.Business_Album.Add(Item);
            db.SaveChanges();
        }

        public void Delete_Business_Album(Guid Album_ID)
        {
            Business_Album Item = db.Business_Album.Find(Album_ID);
            db.Business_Album.Remove(Item);
            db.SaveChanges();
        }
    }

    //商家优惠卡券
    public partial class Business_Service : IBusiness_Service
    {
        public PageList<Business_Coupon> Get_Business_Coupon_PageList(Business_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Business_Coupon.Where(x => x.Link_BID == MF.Link_BID).AsQueryable();

            MF.Bus_Province_List = query.Select(x => x.Coupon_Province).Distinct().ToList();
            MF.Bus_City_List = query.Select(x => x.Coupon_City).Distinct().ToList();
            if (!string.IsNullOrEmpty(MF.Coupon_Status))
            {
                query = query.Where(x => x.Coupon_Status.Contains(MF.Coupon_Status)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Coupon_Name))
            {
                query = query.Where(x => x.Coupon_Name.Contains(MF.Keyword_Coupon_Name)).AsQueryable();
            }

            List<Business_Coupon> Row_List = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<Business_Coupon> PList = new PageList<Business_Coupon>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public Business_Coupon Get_Business_Coupon_Item(Guid Coupon_ID)
        {
            Business_Coupon Item = db.Business_Coupon.Find(Coupon_ID);
            Item = Item ?? new Business_Coupon();

            var Cou_Member_Query = db.Business_Coupon_Member.Where(x => x.Link_Coupon_ID == Item.Coupon_ID).AsQueryable();
            Item.Coupon_Quantity_Surplus = Item.Coupon_Quantity - Cou_Member_Query.Count();

            return Item;
        }

        public void Create_Business_Coupon(Business_Coupon Item)
        {
            Obj_Init.Trim(Item);
            Item.Coupon_ID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;

            Business Bus = this.Get_Business_Item(Item.Link_BID);
            Item.Link_BID = Bus.BID;

            Item.Coupon_Quantity = Item.Coupon_Quantity <= 0 ? 0 : Item.Coupon_Quantity;
            if (Item.Coupon_Type == Coupon_Type_Enum.折扣.ToString())
            {
                if (Item.Coupon_Price > 10 && Item.Coupon_Price < 0) { throw new Exception("请填写有效折扣"); }
            }

            if (Item.Coupon_Type == Coupon_Type_Enum.金额.ToString())
            {
                if (Item.Coupon_Price < 0) { throw new Exception("请填写有效金额"); }
            }
            this.Check_Business_Coupon_Info(Item);
            db.Business_Coupon.Add(Item);
            db.SaveChanges();
        }

        public void Set_Business_Coupon(Business_Coupon Item)
        {
            Obj_Init.Trim(Item);
            Business_Coupon Old_Item = db.Business_Coupon.Find(Item.Coupon_ID);
            Old_Item.Coupon_Name = Item.Coupon_Name;
            //Old_Item.Coupon_Province = Item.Coupon_Province;
            //Old_Item.Coupon_City = Item.Coupon_City;
            Old_Item.Coupon_Type = Item.Coupon_Type;
            Old_Item.Coupon_Price = Item.Coupon_Price;
            Old_Item.Expiry_DT = Item.Expiry_DT;
            Old_Item.Coupon_Quantity = Item.Coupon_Quantity;
            Old_Item.Coupon_Remark = Item.Coupon_Remark;
            Old_Item.Coupon_Status = Item.Coupon_Status;
            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void Check_Business_Coupon_Info(Business_Coupon Item)
        {
            if (string.IsNullOrEmpty(Item.Coupon_Name)) { throw new Exception("请填写优惠卡券名称！"); }
            //if (string.IsNullOrEmpty(Item.Coupon_Province) || Item.Coupon_Province == "省份") { throw new Exception("请选择所在省份！"); }
            //if (string.IsNullOrEmpty(Item.Coupon_City) || Item.Coupon_Province == "城市") { throw new Exception("请选择所在市！"); }
            if (string.IsNullOrEmpty(Item.Coupon_Type)) { throw new Exception("请选择优惠类型！"); }
        }

        public void Del_Business_Coupon(Guid Coupon_ID)
        {
            Business_Coupon Item = db.Business_Coupon.Find(Coupon_ID);
            Item ??= new Business_Coupon();

            if (db.Business_Coupon_Member.Where(x => x.Link_Coupon_ID == Item.Coupon_ID).Any())
            {
                throw new Exception("已有用户领取，无法删除");
            }

            db.Business_Coupon.Remove(Item);
            db.SaveChanges();
        }

        //会员领券
        public void Create_Business_Coupon_Member(Guid Link_Coupon_ID, Guid Link_AMID)
        {
            Business_Coupon Coupon = this.Get_Business_Coupon_Item(Link_Coupon_ID);
            if (Coupon.Coupon_Quantity_Surplus <= 0) { throw new Exception("优惠卡券余量不足"); }

            Alumni_Member Member = db.Alumni_Member.Find(Link_AMID);
            Member = Member ?? new Alumni_Member();
            Business_Coupon_Member Item = new Business_Coupon_Member
            {
                Cou_Mem_ID = MyGUID.NewGuid(),
                Create_DT = DateTime.Now,
                Link_Coupon_ID = Link_Coupon_ID,
                Link_Coupon_ID_Name = Coupon.Coupon_Name,
                Link_AMID = Member.AMID,
                Link_AMID_Name = Member.Member_Name,
                Link_BID = Coupon.Link_BID,
            };
            db.Business_Coupon_Member.Add(Item);
            db.SaveChanges();
        }
    }
}
