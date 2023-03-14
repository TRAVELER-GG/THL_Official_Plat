using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace League.Api
{
    public partial interface IDiscovery_Service
    {
        PageList<Mini_Service> Get_Mini_Service_PageList(Mini_Service_Filter MF);
        Mini_Service Get_Mini_Service_Item(Guid MSID);
        List<string> Type_List();

        void Create_Mini_Service(Mini_Service Item);
        void Save_Mini_Service(Mini_Service Item);
        void Set_Mini_Service(Guid MSID, Mini_Service Item);
        void Set_Mini_Service_Cover_Img(Guid MSID, string Cover_Img);
        void Del_Mini_Service(Guid MSID);
    }

    public partial class Discovery_Service : IDiscovery_Service
    {
        private readonly League_DbContext db = new League_DbContext();
    }

    public partial class Discovery_Service : IDiscovery_Service
    {
        public PageList<Mini_Service> Get_Mini_Service_PageList(Mini_Service_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Mini_Service.AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Name.Contains(MF.Keyword_Name)).AsQueryable();
            }

            List<Mini_Service> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<Mini_Service> PList = new PageList<Mini_Service>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public List<string> Type_List()
        {
            List<string> List = new List<string>();
            List.AddRange(this.Person_List());
            List.AddRange(this.Com_List());
            List.AddRange(this.Other_List());
            return List;
        }

        private List<string> Person_List()
        {
            List<string> Person_Str = new List<string>()
            {
                new string("亲子"),
                new string("教育"),
                new string("健康"),
                new string("保险"),
                new string("理财"),
                new string("家政"),
                new string("婚庆"),
                new string("装饰"),
            };
            return Person_Str;
        }

        private List<string> Com_List()
        {
            List<string> Com_Str = new List<string>()
            {
                new string("信息技术"),
                new string("知识产权"),
                new string("注册代账"),
                new string("管理咨询"),
                new string("维修维护"),
                new string("采购外包"),
                new string("仓储物流"),
                new string("设备租赁"),
            };
            return Com_Str;
        }

        private List<string> Other_List()
        {
            List<string> Other_Str = new List<string>()
            {
                new string("创业孵化"),
                new string("法务咨询"),
                new string("工厂搬迁"),
                new string("环境检测"),
                new string("展会相关"),
                new string("广告宣传"),
                new string("团建培训"),
                new string("会务策划"),
            };
            return Other_Str;
        }

        public Mini_Service Get_Mini_Service_Item(Guid MSID)
        {
            Mini_Service Item = db.Mini_Service.Find(MSID);
            Item ??= new Mini_Service();
            return Item;
        }

        public void Create_Mini_Service(Mini_Service Item)
        {
            Obj_Init.Trim(Item);
            Item.MSID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;
            Item.Status = Infor_Status_Enum.已发布.ToString();
            this.Check_Mini_Service_Info(Item);
            db.Mini_Service.Add(Item);
            db.SaveChanges();
        }

        public void Save_Mini_Service(Mini_Service Item)
        {
            Obj_Init.Trim(Item);
            Item.MSID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;
            Item.Status = Infor_Status_Enum.待发布.ToString();

            if (string.IsNullOrEmpty(Item.Name)) { throw new Exception("公司名称未填写！"); }
            bool Name = db.Mini_Service.Where(x => x.MSID != Item.MSID && x.Name == Item.Name).Any();
            if (Name == true) { throw new Exception("公司名称重复"); }
            db.Mini_Service.Add(Item);
            db.SaveChanges();
        }

        private void Check_Mini_Service_Info(Mini_Service Item)
        {
            if (string.IsNullOrEmpty(Item.Name)) { throw new Exception("公司名称未填写！"); }
            if (string.IsNullOrEmpty(Item.Address)) { throw new Exception("地址未填写！"); }
            if (string.IsNullOrEmpty(Item.Linkman)) { throw new Exception("联系人未填写！"); }
            if (string.IsNullOrEmpty(Item.Contact)) { throw new Exception("联系方式未填写！"); }
            if (string.IsNullOrEmpty(Item.Type)) { throw new Exception("类型未选择！"); }

            if (Item.Main_Business == "<p><br></p>") { throw new Exception("产品服务未填写！"); }
            if (Item.Intro == "<p><br></p>") { throw new Exception("公司简介未填写！"); }

            bool Name = db.Mini_Service.Where(x => x.MSID != Item.MSID && x.Name == Item.Name).Any();
            if (Name == true) { throw new Exception("公司名称重复"); }
        }

        public void Set_Mini_Service(Guid MSID, Mini_Service Item)
        {
            Obj_Init.Trim(Item);
            Mini_Service OLD_Item = db.Mini_Service.Find(MSID);

            OLD_Item.Name = Item.Name;
            OLD_Item.Type = Item.Type;
            OLD_Item.Main_Business = Item.Main_Business;
            OLD_Item.Address = Item.Address;
            OLD_Item.Linkman = Item.Linkman;
            OLD_Item.Contact = Item.Contact;
            OLD_Item.Intro = Item.Intro;
            OLD_Item.Status = Infor_Status_Enum.已发布.ToString();

            this.Check_Mini_Service_Info(OLD_Item);
            db.Entry(OLD_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Mini_Service_Cover_Img(Guid MSID, string Cover_Img)
        {
            Mini_Service Item = db.Mini_Service.Find(MSID);
            Item.Cover_Img = Cover_Img;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Del_Mini_Service(Guid MSID)
        {
            Mini_Service Item = db.Mini_Service.Find(MSID);
            Item ??= new Mini_Service();
            db.Mini_Service.Remove(Item);
            db.SaveChanges();
        }
    }

    public partial interface IDiscovery_Service
    {
        List<Service> Get_Service(string Keyword_Type);
        Service_List Get_Service_List();
        Service Get_Service_Item(string MSID, string UID);

        List<Service> Get_Follow_Service_List(Guid AMID);
        string Follow_Service_Or_Cancel(Guid Follow_MSID, Guid Fans_AMID);
    }

    public partial class Discovery_Service : IDiscovery_Service
    {
        //微服坊搜索
        public List<Service> Get_Service(string Keyword)
        {
            var query = db.Mini_Service.Where(x => x.Status == Act_Status_Enum.已发布.ToString()).AsQueryable();
            if (!string.IsNullOrEmpty(Keyword)) { query = query.Where(x => x.Type.Contains(Keyword)).AsQueryable(); }
            query = query.OrderByDescending(x => x.Create_DT).AsQueryable();
            List<Service> List = new List<Service>();
            foreach (var x in query)
            {
                List.Add(new Service
                {
                    ID = x.MSID,
                    Img = x.Cover_Img,
                    Name = x.Name,
                    Type = x.Type,
                    Address = x.Address,
                });
            }
            return List;
        }

        //关注/取关微服坊
        public string Follow_Service_Or_Cancel(Guid Follow_MSID, Guid Fans_AMID)
        {
            string result = string.Empty;
            bool Is_Follow = db.Follow.Where(x => x.Follow_MSID == Follow_MSID && x.Fans_AMID == Fans_AMID).Any();
            if (Is_Follow == true)
            {
                Follow F = db.Follow.Where(x => x.Follow_MSID == Follow_MSID && x.Fans_AMID == Fans_AMID).FirstOrDefault();
                db.Follow.Remove(F);
                result = "取消关注";
            }
            else
            {
                Follow Item = new Follow()
                {
                    FID = MyGUID.NewGuid(),
                    Create_DT = DateTime.Now,
                    Follow_AMID = Guid.Empty,
                    Follow_MEID = Guid.Empty,
                    Follow_MSID = Follow_MSID,
                    Fans_AMID = Fans_AMID,
                };
                db.Follow.Add(Item);
                result = "关注成功";
            }
            db.SaveChanges();
            return result;
        }

        //已关注的微服坊列表
        public List<Service> Get_Follow_Service_List(Guid AMID)
        {
            List<Guid> Guid_List = db.Follow.Where(x => x.Fans_AMID == AMID).Select(x => x.Follow_MSID).ToList();
            List<Mini_Service> Mini_List = db.Mini_Service.Where(x => Guid_List.Contains(x.MSID)).ToList();
            Mini_List ??= new List<Mini_Service>();
            List<Service> List = new List<Service>();
            foreach (var x in Mini_List)
            {
                List.Add(new Service
                {
                    ID = x.MSID,
                    Img = x.Cover_Img,
                    Name = x.Name,
                    Address = x.Address,
                    Type = x.Type,
                });
            }
            return List;
        }

        //微服坊列表
        public Service_List Get_Service_List()
        {
            Service_List Item = new Service_List
            {
                Company = this.Get_Service_List_By_Company(),
                Person = this.Get_Service_List_By_Person(),
                Other = this.Get_Service_List_By_Other(),
            };
            return Item;
        }

        //企业
        private List<Service> Get_Service_List_By_Company()
        {
            var query = db.Mini_Service.Where(x => x.Status == Act_Status_Enum.已发布.ToString()).AsQueryable();
            List<Mini_Service> Company_List = query.Where(x => this.Com_List().Contains(x.Type)).ToList();
            List<Service> Com_List = new List<Service>();
            foreach (var x in Company_List)
            {
                Com_List.Add(new Service
                {
                    ID = x.MSID,
                    Img = x.Cover_Img,
                    Name = x.Name,
                    Address = x.Address,
                    Type = x.Type,
                });
            }
            return Com_List;
        }

        //个人
        private List<Service> Get_Service_List_By_Person()
        {
            var query = db.Mini_Service.Where(x => x.Status == Act_Status_Enum.已发布.ToString()).AsQueryable();
            List<Mini_Service> Per_List = query.Where(x => this.Person_List().Contains(x.Type)).ToList();
            List<Service> Person_List = new List<Service>();
            foreach (var x in Per_List)
            {
                Person_List.Add(new Service
                {
                    ID = x.MSID,
                    Img = x.Cover_Img,
                    Name = x.Name,
                    Address = x.Address,
                    Type = x.Type,
                });
            }
            return Person_List;
        }

        //综合
        private List<Service> Get_Service_List_By_Other()
        {
            var query = db.Mini_Service.Where(x => x.Status == Act_Status_Enum.已发布.ToString()).AsQueryable();
            List<Mini_Service> Other_List = query.Where(x => this.Other_List().Contains(x.Type)).ToList();
            List<Service> Person_List = new List<Service>();
            List<Service> Oth_List = new List<Service>();
            foreach (var x in Other_List)
            {
                Oth_List.Add(new Service
                {
                    ID = x.MSID,
                    Img = x.Cover_Img,
                    Name = x.Name,
                    Address = x.Address,
                    Type = x.Type,
                });
            }
            return Oth_List;
        }

        //微服坊详情
        public Service Get_Service_Item(string MSID, string UID)
        {
            Guid Guid_MSID = Common_Lib.Convert_GUID(MSID);
            Guid Guid_UID = Common_Lib.Convert_GUID(UID);

            Mini_Service MS = db.Mini_Service.Find(Guid_MSID);
            MS ??= new Mini_Service();

            Service Item = new Service
            {
                ID = MS.MSID,
                Img = MS.Cover_Img,
                Name = MS.Name,
                Address = MS.Address,
                Type = MS.Type,
                Linkman = MS.Linkman,
                Contact = MS.Contact,
                Intro = MS.Intro,
                Main_Business = MS.Main_Business,
                Status = Enterprise_Status_Enum.未关注.ToString(),
            };

            if (!string.IsNullOrEmpty(UID))
            {
                bool Is_Follow = db.Follow.Where(c => c.Follow_MSID == Guid_MSID && c.Fans_AMID == Guid_UID).Any();
                if (Is_Follow == true) { Item.Status = Enterprise_Status_Enum.已关注.ToString(); }
            }

            return Item;
        }
    }
}
