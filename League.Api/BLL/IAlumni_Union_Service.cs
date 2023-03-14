using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

//校友会设置
namespace League.Api
{
    public partial interface IAlumni_Union_Service
    {
        PageList<Alumni_Union> Get_Alumni_Union_PageList(Alumni_Union_Filter MF);
        Alumni_Union Get_Alumni_Union_Item(Guid AUID);

        void Create_Alumni_Union_And_Alumni(Alumni_Union AU);
        void Set_Alumni_Union_Base(Guid AUID, Alumni_Union Item);
        void Reset_Alumni_Union_Password(Guid AUID, string Password);
        void Delete_Alumni_Union_Item(Guid AUID);

        void Set_Alumni_Union_Logo(Guid AUID, string Logo);
        void Set_Alumni_Union_Logo_College(Guid AUID, string Logo_College);
        void Set_Alumni_Union_Background(Guid AUID, string Background);
        void Set_Alumni_Union_Introduce(Guid AUID, Alumni_Union Item);
    }

    public partial class Alumni_Union_Service : IAlumni_Union_Service
    {
        private readonly League_DbContext db = new League_DbContext();
    }

    public partial class Alumni_Union_Service : IAlumni_Union_Service
    {
        public PageList<Alumni_Union> Get_Alumni_Union_PageList(Alumni_Union_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Alumni_Union.AsQueryable();

            if (!string.IsNullOrEmpty(MF.Union_Type))
            {
                query = query.Where(x => x.Union_Type == MF.Union_Type).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Name.Contains(MF.Keyword_Name) || x.Name_Short.Contains(MF.Keyword_Name)).AsQueryable();
            }

            List<Alumni_Union> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            List<Alumni_Union_User> Alumni_List = db.Alumni_Union_User.ToList();
            List<Guid> Link_AUID_List = Row_List.Select(x => x.AUID).ToList();
            var Member_List = db.Alumni_Union_Member_Link.Where(x => Link_AUID_List.Contains(x.Link_AUID)).AsQueryable();
            foreach (var x in Row_List)
            {
                x.Member_Count = Member_List.Where(c => c.Link_AUID == x.AUID).Count();
            }

            PageList<Alumni_Union> PList = new PageList<Alumni_Union>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public Alumni_Union Get_Alumni_Union_Item(Guid AUID)
        {
            Alumni_Union Item = db.Alumni_Union.Find(AUID);
            Item = Item ?? new Alumni_Union();
            Alumni_Union_User User = db.Alumni_Union_User.Where(x => x.Link_AUID == AUID && x.Is_Admin == Alumni_Is_Admin_Enum.Admin.ToString()).FirstOrDefault();
            User ??= new Alumni_Union_User();
            Item.Real_Name = User.Name;
            Item.User_Name = User.User_Name;
            Item.Password = User.Password;
            return Item;
        }

        //创建新用户
        public void Create_Alumni_Union_And_Alumni(Alumni_Union AU)
        {
            Obj_Init.Trim(AU);
            AU.AUID = MyGUID.NewGuid();
            AU.Create_DT = DateTime.Now;
            //AU.PinCode = "654321";

            Alumni_Union_User Item = new Alumni_Union_User
            {
                UID = MyGUID.NewGuid(),
                Create_DT = DateTime.Now,
                Name = string.Empty,
                User_Name = Alumni_Is_Admin_Enum.Admin.ToString(),
                Password = "123456",
                Email = "n.a",
                Tel = "n.a",
                Img_Path = string.Empty,
                Is_Admin = Alumni_Is_Admin_Enum.Admin.ToString(),
                Roles_Title = Alumni_Is_Admin_Enum.Admin.ToString(),
                Is_Frozen = Alumni_Is_Frozen_Enum.启用.ToString(),
                Link_AUID = AU.AUID,
            };

            this.Check_Alumni_Union_Info(AU);

            db.Alumni_Union.Add(AU);
            db.Alumni_Union_User.Add(Item);
            db.SaveChanges();
        }

        //更新
        public void Set_Alumni_Union_Base(Guid AUID, Alumni_Union Item)
        {
            Obj_Init.Trim(Item);
            Alumni_Union OLD_AU = db.Alumni_Union.Find(AUID);

            OLD_AU.Name = Item.Name;
            OLD_AU.Name_Short = Item.Name_Short;
            OLD_AU.Establish_DT = Item.Establish_DT;
            OLD_AU.Union_Type = Item.Union_Type;
            OLD_AU.Contact = Item.Contact;
            OLD_AU.Phone = Item.Phone;
            OLD_AU.Content = Item.Content;
            //OLD_AU.PinCode = Item.PinCode;

            this.Check_Alumni_Union_Info(OLD_AU);

            //if (string.IsNullOrEmpty(Item.User_Name)) { throw new Exception("用户名未填写"); }
            //if (string.IsNullOrEmpty(Item.Password)) { throw new Exception("密码未填写"); }

            //Alumni_Union_User Alumni = db.Alumni_Union_User.Where(x => x.Link_AUID == AUID && x.Is_Admin == Alumni_Is_Admin_Enum.Admin.ToString()).FirstOrDefault();
            //Alumni.Name = Item.Real_Name;
            //Alumni.User_Name = Item.User_Name;
            //Alumni.Password = Item.Password;

            //db.Entry(Alumni).State = EntityState.Modified;
            db.Entry(OLD_AU).State = EntityState.Modified;
            db.SaveChanges();
        }

        //重置校友会密码
        public void Reset_Alumni_Union_Password(Guid AUID, string Password)
        {
            Alumni_Union_User Item = db.Alumni_Union_User.Where(x => x.Link_AUID == AUID && x.Is_Admin == Alumni_Is_Admin_Enum.Admin.ToString()).FirstOrDefault();
            Item.Password = Password;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void Check_Alumni_Union_Info(Alumni_Union AU)
        {
            if (string.IsNullOrEmpty(AU.Name)) { throw new Exception("分会全称未填写"); }
            if (string.IsNullOrEmpty(AU.Name_Short)) { throw new Exception("分会简称未填写"); }
            //if (string.IsNullOrEmpty(AU.PinCode)) { throw new Exception("PinCode不能为空"); }

            bool Name = db.Alumni_Union.Where(x => x.AUID != AU.AUID && x.Name == AU.Name).Any();
            if (Name == true) { throw new Exception("分会全称重复"); }

            bool Name_College_Short = db.Alumni_Union.Where(x => x.AUID != AU.AUID && x.Name_Short == AU.Name_Short).Any();
            if (Name_College_Short == true) { throw new Exception("分会简称重复"); }

            //bool PinCode = db.Alumni_Union.Where(x => x.AUID != AU.AUID && x.PinCode == AU.PinCode).Any();
            //if (PinCode == true) { throw new Exception("Pincode重复"); }
        }

        //删除校友会
        public void Delete_Alumni_Union_Item(Guid AUID)
        {
            Alumni_Union Item = db.Alumni_Union.Find(AUID);

            if (db.Alumni_Member.Where(x => x.Link_AUID == AUID).Any())
            {
                throw new Exception("已产生校友，无法删除");
            }

            Alumni_Union_User User = db.Alumni_Union_User.Where(x => x.Link_AUID == AUID).FirstOrDefault();
            User = User ?? new Alumni_Union_User();

            if (User.UID != Guid.Empty)
            {
                db.Alumni_Union_User.Remove(User);
            }

            db.Alumni_Union.Remove(Item);
            db.SaveChanges();
        }

        public void Set_Alumni_Union_Logo(Guid AUID, string Logo)
        {
            Alumni_Union Item = db.Alumni_Union.Find(AUID);
            Item.Logo = Logo;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Alumni_Union_Logo_College(Guid AUID, string Logo_College)
        {
            Alumni_Union Item = db.Alumni_Union.Find(AUID);
            Item.Logo_College = Logo_College;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Alumni_Union_Background(Guid AUID, string Background)
        {
            Alumni_Union Item = db.Alumni_Union.Find(AUID);
            Item.Background = Background;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Alumni_Union_Introduce(Guid AUID, Alumni_Union Item)
        {
            Alumni_Union OLD_Item = db.Alumni_Union.Find(AUID);
            OLD_Item.Content = Item.Content;
            db.Entry(OLD_Item).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}

//校友管理员登录
namespace League.Api
{
    public partial interface IAlumni_Union_Service
    {
        Alumni_Union_User Alumni_Union_User_Login(Alumni_Union_User U, string PinCode, string Last_Login_IP, string Client_Info);
        Alumni_Union_User Get_Alumni_Union_User_By_Controller(string Identity_UID);
        Alumni_Union_User Get_Alumni_Union_User_Item(Guid AID);
    }

    public partial class Alumni_Union_Service : IAlumni_Union_Service
    {
        public Alumni_Union_User Alumni_Union_User_Login(Alumni_Union_User U, string PinCode, string Last_Login_IP, string Client_Info)
        {
            Last_Login_IP = Last_Login_IP == null ? string.Empty : Last_Login_IP.Trim();
            Client_Info = Client_Info == null ? string.Empty : Client_Info.Trim();
            PinCode = PinCode == null ? string.Empty : PinCode.Trim();

            Alumni_Union AU = db.Alumni_Union.Where(x => x.PinCode == PinCode).FirstOrDefault();
            if (AU == null) { throw new Exception("企业代码不正确"); }

            U.User_Name = U.User_Name == null ? string.Empty : U.User_Name.Trim();
            U.Password = U.Password == null ? string.Empty : U.Password.Trim();
            Alumni_Union_User OLD_U = db.Alumni_Union_User.Where(x => x.User_Name == U.User_Name && x.Password == U.Password && x.Link_AUID == AU.AUID).FirstOrDefault();
            if (OLD_U == null) { throw new Exception("用户名或密码不正确"); }

            if (OLD_U.Is_Frozen != Alumni_Is_Frozen_Enum.启用.ToString())
            {
                throw new Exception("当前用户已被『" + OLD_U.Is_Frozen + "』");
            }

            //写入最后一次登陆记录
            Alumni_Union_User_Login_Record UR = new Alumni_Union_User_Login_Record
            {
                ULR_ID = MyGUID.NewGuid(),
                Link_UID = OLD_U.UID,
                Name = OLD_U.User_Name,
                Name_Full = OLD_U.Name_Full,
                Password = OLD_U.Password,
                Last_Login_IP = Last_Login_IP,
                Client_Info = Client_Info,
                Last_Login_DT = DateTime.Now,
            };
            db.Alumni_Union_User_Login_Record.Add(UR);
            db.SaveChanges();
            return OLD_U;
        }

        public Alumni_Union_User Get_Alumni_Union_User_By_Controller(string Identity_UID)
        {
            Guid UID = Guid.Empty;
            try { UID = new Guid(Identity_UID); } catch { UID = Guid.Empty; }
            Alumni_Union_User Item = this.Get_Alumni_Union_User_Item(UID);
            return Item;
        }

        public Alumni_Union_User Get_Alumni_Union_User_Item(Guid UID)
        {
            Alumni_Union_User Item = db.Alumni_Union_User.Find(UID);
            Item ??= new Alumni_Union_User();

            Alumni_Union AU = db.Alumni_Union.Find(Item.Link_AUID);
            AU = AU ?? new Alumni_Union();
            Item.Name_Full = AU.Name;
            Item.Name_Short = AU.Name_Short;
            Item.Logo = AU.Logo;
            return Item;
        }
    }
}

//校友
namespace League.Api
{
    public partial interface IAlumni_Union_Service
    {
        PageList<Alumni_Member> Get_Alumni_Member_PageList(Alumni_Member_Filter MF);
        List<Alumni_Member> Get_Alumni_Member_List_Sim(Alumni_Member_Filter MF);
        List<Alumni_Member> Get_Alumni_Member_List_All(Alumni_Member_Filter MF);
        List<string> Get_Faculty_List(Guid AUID);
        List<Profession_Second> Get_Profession_Second_List();
        Alumni_Member Get_Alumni_Member_Item(Guid AMID);

        void Create_Alumni_Member(Alumni_Member AM, Guid AUID);
        void Set_Alumni_Member_Base(Guid AMID, Alumni_Member Item);
        void Set_Alumni_Member_Img(Guid AMID, string Img);
        void Delete_Alumni_Member_Item(Guid AMID);
        void Change_Alumni_Member_Status(Guid AMID);

        PageList<Alumni_Member> Get_Alumni_Member_Auditor_PageList(Alumni_Member_Filter MF);
        void Set_Alumni_Member_Authority_Type(Guid AMID, Alumni_Member Item);
    }

    public partial class Alumni_Union_Service
    {
        public PageList<Alumni_Member> Get_Alumni_Member_PageList(Alumni_Member_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);

            var query = db.Alumni_Member.AsQueryable();

            if (MF.Link_AUID != Guid.Empty)
            {
                query = query.Where(x => x.Link_AUID == MF.Link_AUID).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Member_Name.Contains(MF.Keyword_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Is_Frozen))
            {
                query = query.Where(x => x.Is_Frozen == MF.Is_Frozen).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Faculty))
            {
                query = query.Where(x => x.Faculty == MF.Keyword_Faculty).AsQueryable();
            }

            List<Alumni_Member> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            PageList<Alumni_Member> PList = new PageList<Alumni_Member>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }
        public PageList<Alumni_Member> Get_Alumni_Member_Auditor_PageList(Alumni_Member_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);

            var query = db.Alumni_Member.Where(x => x.Status == Status_Enum.已认证.ToString() && x.Is_Frozen == Alumni_Is_Frozen_Enum.启用.ToString()).AsQueryable();

            if (MF.Link_AUID != Guid.Empty)
            {
                query = query.Where(x => x.Link_AUID == MF.Link_AUID).AsQueryable();
            }
            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Member_Name.Contains(MF.Keyword_Name)).AsQueryable();
            }
            if (!string.IsNullOrEmpty(MF.Keyword_Faculty))
            {
                query = query.Where(x => x.Faculty == MF.Keyword_Faculty).AsQueryable();
            }

            List<Alumni_Member> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            PageList<Alumni_Member> PList = new PageList<Alumni_Member>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }
        public List<Alumni_Member> Get_Alumni_Member_List_Sim(Alumni_Member_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Alumni_Member.Where(x => x.Link_AUID == MF.Link_AUID).AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Member_Name.Contains(MF.Keyword_Name)).AsQueryable();
            }
            List<Alumni_Member> List = query.OrderByDescending(x => x.Create_DT).Take(MF.PageSize).ToList();
            return List;
        }

        public List<Alumni_Member> Get_Alumni_Member_List_All(Alumni_Member_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);

            var query = db.Alumni_Member.AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Member_Name.Contains(MF.Keyword_Name)).AsQueryable();
            }

            List<Alumni_Member> List = query.OrderByDescending(x => x.Create_DT).Take(MF.PageSize).ToList();
            var Alumni_Union_List = db.Alumni_Union.AsQueryable();
            foreach (var x in List)
            {
                foreach (var xx in Alumni_Union_List.Where(c => c.AUID == x.Link_AUID))
                {
                    x.Name_Full = xx.Name;
                }
            }

            return List;
        }

        public List<string> Get_Faculty_List(Guid AUID)
        {
            List<string> List = new List<string>
            {
                new string("理学院"),
                new string("人文社会科学学院"),
                new string("钱学森书院"),
                new string("启德书院"),
                new string("创新创业学院"),
                new string("继续教育学院"),
                new string("能源与动力工程学院"),
                new string("微电子学院"),
                new string("法医学院"),
                new string("公共政策与管理学院"),
                new string("第一临床医学院"),
                new string("信息与通信工程学院"),
                new string("软件学院"),
                new string("计算机科学与技术学院"),
                new string("经济与金融学院"),
                new string("宗濂书院"),
                new string("人居环境与建筑工程学院"),
                new string("口腔医学院"),
                new string("人工智能学院"),
                new string("药学院"),
                new string("核科学与技术学院"),
                new string("化学工程与技术学院"),
                new string("数学与统计学院"),
                new string("励志书院"),
                new string("崇实书院"),
                new string("自动化科学与工程学院"),
                new string("电子科学与工程学院"),
                new string("管理学院"),
                new string("彭康书院"),
                new string("法学院"),
                new string("网络教育学院"),
                new string("生命科学与技术学院"),
                new string("公共卫生学院"),
                new string("新媒体学院"),
                new string("文治书院"),
                new string("航天航空学院"),
                new string("第二临床医学院"),
                new string("国际教育学院"),
                new string("网络空间安全学院"),
                new string("机械工程学院"),
                new string("外国语学院"),
                new string("材料科学与工程学院"),
                new string("电气工程学院"),
                new string("南洋书院"),
                new string("马克思主义学院"),
                new string("仲英书院"),
                new string("护理学系"),
            };
            List = List.OrderBy(x => x).ToList();
            return List;
        }

        public Alumni_Member Get_Alumni_Member_Item(Guid AMID)
        {
            Alumni_Member Item = db.Alumni_Member.Find(AMID);
            Item ??= new Alumni_Member();
            return Item;
        }

        //创建新校友
        public void Create_Alumni_Member(Alumni_Member Item, Guid AUID)
        {
            Obj_Init.Trim(Item);
            Item.AMID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;

            Item.Is_Frozen = Alumni_Is_Frozen_Enum.启用.ToString();
            Item.Status = Status_Enum.已认证.ToString();

            Item.Native_Province = Item.Native_Province == "省份" ? string.Empty : Item.Native_Province;
            Item.Native_City = Item.Native_City == "城市" ? string.Empty : Item.Native_City;
            Item.Native_District = Item.Native_District == "区县" ? string.Empty : Item.Native_District;

            Item.Province = Item.Province == "省份" ? string.Empty : Item.Province;
            Item.City = Item.City == "城市" ? string.Empty : Item.City;
            Item.District = Item.District == "区县" ? string.Empty : Item.District;
            Item.Link_AUID = AUID;

            this.Check_Alumni_Member_Info(Item);
            db.Alumni_Member.Add(Item);
            db.SaveChanges();
        }


        //更新
        public void Set_Alumni_Member_Base(Guid AMID, Alumni_Member Item)
        {
            Obj_Init.Trim(Item);
            Alumni_Member OLD_Item = db.Alumni_Member.Find(AMID);

            OLD_Item.Member_Name = Item.Member_Name;
            OLD_Item.Mobile = Item.Mobile;
            OLD_Item.Gender = Item.Gender;
            OLD_Item.Degree = Item.Degree;
            OLD_Item.Faculty = Item.Faculty;
            OLD_Item.Is_Frozen = Item.Is_Frozen;
            OLD_Item.Year = Item.Year;
            OLD_Item.Native_Province = Item.Native_Province;
            OLD_Item.Native_City = Item.Native_City;
            OLD_Item.Native_District = Item.Native_District;
            OLD_Item.Province = Item.Province;
            OLD_Item.City = Item.City;
            OLD_Item.District = Item.District;

            OLD_Item.Native_Province = OLD_Item.Native_Province == "省份" ? string.Empty : OLD_Item.Native_Province;
            OLD_Item.Native_City = OLD_Item.Native_City == "城市" ? string.Empty : OLD_Item.Native_City;
            OLD_Item.Native_District = OLD_Item.Native_District == "区县" ? string.Empty : OLD_Item.Native_District;

            OLD_Item.Province = OLD_Item.Province == "省份" ? string.Empty : OLD_Item.Province;
            OLD_Item.City = OLD_Item.City == "城市" ? string.Empty : OLD_Item.City;
            OLD_Item.District = OLD_Item.District == "区县" ? string.Empty : OLD_Item.District;


            if (OLD_Item.Is_Frozen == Alumni_Is_Frozen_Enum.冻结.ToString())
            {
                if (db.Alumni_Member_Enterprise.Where(x => x.Link_AMID == AMID).Any()) { throw new Exception("该已关联校友企业，无法冻结!"); }
            }

            this.Check_Alumni_Member_Info(OLD_Item);
            db.Entry(OLD_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Alumni_Member_Img(Guid AMID, string Img)
        {
            Alumni_Member Item = db.Alumni_Member.Find(AMID);
            Item.Img = Img;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void Check_Alumni_Member_Info(Alumni_Member AM)
        {
            if (string.IsNullOrEmpty(AM.Member_Name)) { throw new Exception("校友姓名未填写"); }
            if (string.IsNullOrEmpty(AM.Mobile)) { throw new Exception("手机号未填写"); }
            if (string.IsNullOrEmpty(AM.Gender)) { throw new Exception("性别未选择"); }
            if (AM.Year == 0) { throw new Exception("请选择毕业年份"); }

            bool Mobile = db.Alumni_Member.Where(x => x.AMID != AM.AMID && x.Mobile == AM.Mobile).Any();
            if (Mobile == true) { throw new Exception("手机号重复"); }
        }

        //删除社交校友
        public void Delete_Alumni_Member_Item(Guid AMID)
        {
            Alumni_Member Item = db.Alumni_Member.Find(AMID);
            if (db.Alumni_Member_Enterprise.Where(c => c.Link_AMID == AMID).Any()) { throw new Exception("该校友与校友企业产生关联，无法删除！"); }
            this.Delete_Alumni_Member_etc(AMID);
            db.Alumni_Member.Remove(Item);
            db.SaveChanges();
        }

        //删除校友相关所有信息
        private void Delete_Alumni_Member_etc(Guid AMID)
        {
            List<Activity_Apply> Activity_Apply_List = db.Activity_Apply.Where(x => x.Link_AMID == AMID).ToList();
            List<Follow> Follow_List = db.Follow.Where(x => x.Fans_AMID == AMID || x.Follow_AMID == AMID).ToList();

            db.Activity_Apply.RemoveRange(Activity_Apply_List);
            db.Follow.RemoveRange(Follow_List);
        }

        public void Change_Alumni_Member_Status(Guid AMID)
        {
            Alumni_Member Item = db.Alumni_Member.Find(AMID);
            if (Item.Status == Status_Enum.已认证.ToString())
            {
                Item.Status = Status_Enum.待认证.ToString();
            }
            else
            {
                Item.Status = Status_Enum.已认证.ToString();
            }
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Alumni_Member_Authority_Type(Guid AMID, Alumni_Member Item)
        {
            Alumni_Member Old_Item = db.Alumni_Member.Find(AMID);

            if (Item.Authority_Type == Authority_Type_Enum.超级权限.ToString())
            {
                Old_Item.Authority_Type = Item.Authority_Type;
            }

            if (Item.Authority_Type == Authority_Type_Enum.区域权限.ToString())
            {
                Old_Item.Authority_Type = Item.Authority_Type;
                Old_Item.Authority_Province = Item.Authority_Province;
                Old_Item.Authority_City = Item.Authority_City;
            }

            if (string.IsNullOrEmpty(Item.Authority_Type))
            {
                Old_Item.Authority_Type = string.Empty;
                Old_Item.Authority_Province = string.Empty;
                Old_Item.Authority_City = string.Empty;
            }

            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}

//校友企业
namespace League.Api
{
    public partial interface IAlumni_Union_Service
    {
        PageList<Alumni_Member_Enterprise> Get_Alumni_Member_Enterprise_PageList(Alumni_Member_Filter MF);
        Alumni_Member_Enterprise Get_Alumni_Member_Enterprise_Item(Guid MEID);
        List<Profession_First> Get_Profession_First_List();

        void Create_Alumni_Member_Enterprise(Alumni_Member_Enterprise Item, Guid AUID);
        void Set_Alumni_Member_Enterprise(Guid MEID, Alumni_Member_Enterprise Item);
        void Set_Alumni_Member_Enterprise_Logo(Guid MEID, string Logo);
        void Delete_Alumni_Member_Enterprise(Guid MEID);
    }

    public partial class Alumni_Union_Service
    {
        public PageList<Alumni_Member_Enterprise> Get_Alumni_Member_Enterprise_PageList(Alumni_Member_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);

            var query = db.Alumni_Member_Enterprise.AsQueryable();

            if (MF.Link_AUID != Guid.Empty)
            {
                query = query.Where(x => x.Link_AUID == MF.Link_AUID).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Name.Contains(MF.Keyword_Name)).AsQueryable();
            }

            List<Alumni_Member_Enterprise> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            var query_member = db.Alumni_Member.Where(x => x.Link_AUID == MF.Link_AUID).AsQueryable();
            foreach (var x in Row_List)
            {
                foreach (var xx in query_member.Where(c => c.AMID == x.Link_AMID))
                {
                    x.Member_Name = xx.Member_Name;
                }
            }

            PageList<Alumni_Member_Enterprise> PList = new PageList<Alumni_Member_Enterprise>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public Alumni_Member_Enterprise Get_Alumni_Member_Enterprise_Item(Guid MEID)
        {
            Alumni_Member_Enterprise Item = db.Alumni_Member_Enterprise.Find(MEID);
            Item ??= new Alumni_Member_Enterprise();
            Item.Member_Name = this.Get_Alumni_Member_Item(Item.Link_AMID).Member_Name;
            return Item;
        }

        public List<Profession_First> Get_Profession_First_List()
        {
            List<Profession_First> List = new List<Profession_First>
            {
                new Profession_First{ First_Name="商务服务"},
                new Profession_First{ First_Name="科技服务"},
                new Profession_First{ First_Name="互联网"},
                new Profession_First{ First_Name="文娱"},
                new Profession_First{ First_Name="大消费"},
                new Profession_First{ First_Name="科技"},
                new Profession_First{ First_Name="大健康"},
                new Profession_First{ First_Name="生产制造"},
                new Profession_First{ First_Name="能源环保"},
                new Profession_First{ First_Name="农林牧渔"},
                new Profession_First{ First_Name="政府民生"},
                new Profession_First{ First_Name="其他"},
            };

            List<Profession_Second> List_Second = this.Get_Profession_Second_List();

            List<Profession_Second> Temp_List = new List<Profession_Second>();
            foreach (var x in List)
            {
                Temp_List = List_Second.Where(c => c.Link_Name == x.First_Name).ToList();
                x.Second_List.AddRange(Temp_List);
            }
            return List;
        }

        public List<Profession_Second> Get_Profession_Second_List()
        {
            List<Profession_Second> List_Second = new List<Profession_Second>
            {
                new Profession_Second{ Second_Name = "法律服务", Link_Name="商务服务"},
                new Profession_Second{ Second_Name = "企业管理服务", Link_Name="商务服务"},
                new Profession_Second{ Second_Name = "咨询与调查", Link_Name="商务服务"},
                new Profession_Second{ Second_Name = "广告服务", Link_Name="商务服务"},
                new Profession_Second{ Second_Name = "职业中介服务", Link_Name="商务服务"},

                new Profession_Second{ Second_Name = "设备维修维护", Link_Name="科技服务"},
                new Profession_Second{ Second_Name = "数字化产线改造集成", Link_Name="科技服务"},
                new Profession_Second{ Second_Name = "工业服务网络平台", Link_Name="科技服务"},
                new Profession_Second{ Second_Name = "工业电商", Link_Name="科技服务"},
                new Profession_Second{ Second_Name = "供应链服务", Link_Name="科技服务"},
                new Profession_Second{ Second_Name = "设备运维与管理", Link_Name="科技服务"},
                new Profession_Second{ Second_Name = "工业APP", Link_Name="科技服务"},
                new Profession_Second{ Second_Name = "设备管理软件", Link_Name="科技服务"},

                new Profession_Second{ Second_Name = "平台", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "APP", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "电商", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "O2O", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "区块链", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "物联网", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "云服务", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "大数据", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "网络安全", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "软件开发", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "社交网络", Link_Name="互联网"},
                new Profession_Second{ Second_Name = "共享经济", Link_Name="互联网"},

                new Profession_Second{ Second_Name = "新媒体", Link_Name="文娱"},
                new Profession_Second{ Second_Name = "短视频", Link_Name="文娱"},
                new Profession_Second{ Second_Name = "影视文化", Link_Name="文娱"},
                new Profession_Second{ Second_Name = "游戏动漫", Link_Name="文娱"},
                new Profession_Second{ Second_Name = "文化传媒", Link_Name="文娱"},
                new Profession_Second{ Second_Name = "休闲娱乐", Link_Name="文娱"},

                new Profession_Second{ Second_Name = "餐饮", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "食品", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "住宿", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "美业", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "母婴", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "婚庆", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "会展", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "新零售", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "家居日用", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "连锁加盟", Link_Name="大消费"},
                new Profession_Second{ Second_Name = "批发零售", Link_Name="大消费"},

                new Profession_Second{ Second_Name = "人工智能", Link_Name="科技"},
                new Profession_Second{ Second_Name = "机器人", Link_Name="科技"},
                new Profession_Second{ Second_Name = "工业4.0", Link_Name="科技"},
                new Profession_Second{ Second_Name = "智能设备", Link_Name="科技"},
                new Profession_Second{ Second_Name = "国防军工", Link_Name="科技"},
                new Profession_Second{ Second_Name = "航空航天", Link_Name="科技"},
                new Profession_Second{ Second_Name = "芯片", Link_Name="科技"},
                new Profession_Second{ Second_Name = "专利", Link_Name="科技"},
                new Profession_Second{ Second_Name = "电子通信", Link_Name="科技"},

                new Profession_Second{ Second_Name = "保健", Link_Name="大健康"},
                new Profession_Second{ Second_Name = "养生", Link_Name="大健康"},
                new Profession_Second{ Second_Name = "医美", Link_Name="大健康"},
                new Profession_Second{ Second_Name = "生命制药", Link_Name="大健康"},
                new Profession_Second{ Second_Name = "医疗器械", Link_Name="大健康"},
                new Profession_Second{ Second_Name = "体育健身", Link_Name="大健康"},
                new Profession_Second{ Second_Name = "医疗服务", Link_Name="大健康"},

                new Profession_Second{ Second_Name = "化学化工", Link_Name="生产制造"},
                new Profession_Second{ Second_Name = "机械机电", Link_Name="生产制造"},
                new Profession_Second{ Second_Name = "家电数码", Link_Name="生产制造"},
                new Profession_Second{ Second_Name = "建筑建材", Link_Name="生产制造"},
                new Profession_Second{ Second_Name = "新材料", Link_Name="生产制造"},
                new Profession_Second{ Second_Name = "电气设备", Link_Name="生产制造"},
                new Profession_Second{ Second_Name = "纺织服装饰品", Link_Name="生产制造"},

                new Profession_Second{ Second_Name = "新能源", Link_Name="能源环保"},
                new Profession_Second{ Second_Name = "节能环保", Link_Name="能源环保"},
                new Profession_Second{ Second_Name = "矿产冶金", Link_Name="能源环保"},
                new Profession_Second{ Second_Name = "能源贸易", Link_Name="能源环保"},
                new Profession_Second{ Second_Name = "传统能源", Link_Name="能源环保"},

                new Profession_Second{ Second_Name = "农业", Link_Name="农林牧渔"},
                new Profession_Second{ Second_Name = "林业", Link_Name="农林牧渔"},
                new Profession_Second{ Second_Name = "牧业", Link_Name="农林牧渔"},
                new Profession_Second{ Second_Name = "渔业", Link_Name="农林牧渔"},
                new Profession_Second{ Second_Name = "深加工", Link_Name="农林牧渔"},

                new Profession_Second{ Second_Name = "旅游", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "学校", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "水务", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "电力", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "物业", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "养老", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "房地产", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "基础设施", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "交通运输", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "园林园艺", Link_Name="政府民生"},
                new Profession_Second{ Second_Name = "政府平台", Link_Name="政府民生"},


                new Profession_Second{ Second_Name = "TMT", Link_Name="其他"},
                new Profession_Second{ Second_Name = "装修", Link_Name="其他"},
                new Profession_Second{ Second_Name = "教育", Link_Name="其他"},
                new Profession_Second{ Second_Name = "金融", Link_Name="其他"},
                new Profession_Second{ Second_Name = "收藏品", Link_Name="其他"},
                new Profession_Second{ Second_Name = "商务贸易", Link_Name="其他"},
                new Profession_Second{ Second_Name = "仓储物流", Link_Name="其他"},
                new Profession_Second{ Second_Name = "采购外包", Link_Name="其他"},
                new Profession_Second{ Second_Name = "汽车后市场", Link_Name="其他"},
            };
            return List_Second;
        }

        public void Create_Alumni_Member_Enterprise(Alumni_Member_Enterprise Item, Guid AUID)
        {
            Obj_Init.Trim(Item);
            Item.MEID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;
            Item.Link_AUID = AUID;

            Item.Province = Item.Province == "省份" ? string.Empty : Item.Province;
            Item.City = Item.City == "城市" ? string.Empty : Item.City;
            Item.District = Item.District == "区县" ? string.Empty : Item.District;

            this.Check_Alumni_Member_Enterprise_Info(Item);

            db.Alumni_Member_Enterprise.Add(Item);
            db.SaveChanges();
        }

        private void Check_Alumni_Member_Enterprise_Info(Alumni_Member_Enterprise Item)
        {
            if (string.IsNullOrEmpty(Item.Name)) { throw new Exception("请填写企业全称"); }
            if (Item.Link_AMID == Guid.Empty) { throw new Exception("请正确填写或选择校友名称"); }
            if (Item.Detial == "<p><br></p>") { throw new Exception("请填写公司简介"); }
            if (Item.Main_Business == "<p><br></p>") { throw new Exception("请填写产品介绍"); }

            bool Name = db.Alumni_Member_Enterprise.Where(x => x.MEID != Item.MEID && x.Name == Item.Name).Any();
            if (Name == true) { throw new Exception("企业全称重复"); }
        }

        public void Set_Alumni_Member_Enterprise(Guid MEID, Alumni_Member_Enterprise Item)
        {
            Obj_Init.Trim(Item);
            Alumni_Member_Enterprise OLD_Item = db.Alumni_Member_Enterprise.Find(MEID);

            OLD_Item.Name = Item.Name;
            OLD_Item.Duty = Item.Duty;
            OLD_Item.Profession = Item.Profession;
            OLD_Item.Address = Item.Address;
            OLD_Item.Email = Item.Email;
            OLD_Item.Contact = Item.Contact;
            OLD_Item.Province = Item.Province;
            OLD_Item.City = Item.City;
            OLD_Item.District = Item.District;
            OLD_Item.Detial = Item.Detial;
            OLD_Item.Main_Business = Item.Main_Business;
            OLD_Item.Link_AMID = Item.Link_AMID;

            this.Check_Alumni_Member_Enterprise_Info(OLD_Item);
            db.Entry(OLD_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Alumni_Member_Enterprise_Logo(Guid MEID, string Logo)
        {
            Alumni_Member_Enterprise Item = db.Alumni_Member_Enterprise.Find(MEID);
            Item.Logo = Logo;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete_Alumni_Member_Enterprise(Guid MEID)
        {
            Alumni_Member_Enterprise Item = db.Alumni_Member_Enterprise.Find(MEID);
            db.Alumni_Member_Enterprise.Remove(Item);
            db.SaveChanges();
        }
    }
}

//校友 - 分会管理
namespace League.Api
{
    public partial interface IAlumni_Union_Service
    {
        PageList<Alumni_Member> Get_Alumni_Member_ALL_PageList(Alumni_Member_Filter MF);
        PageList<Alumni_Member> Get_Alumni_Member_Auditor_ALL_PageList(Alumni_Member_Filter MF);

    }

    public partial class Alumni_Union_Service
    {
        public PageList<Alumni_Member> Get_Alumni_Member_ALL_PageList(Alumni_Member_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Alumni_Member.AsQueryable();

            MF.Faculty_List = query.Select(x => x.Faculty).Distinct().ToList();
            if (!string.IsNullOrEmpty(MF.Keyword_Faculty))
            {
                query = query.Where(x => x.Faculty == MF.Keyword_Faculty).AsQueryable();
            }

            List<int> Year_List = query.Select(x => x.Year).OrderBy(x => x).Distinct().ToList();
            foreach (var item in Year_List)
            {
                MF.Year_List.Add(item.ToString());
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Year))
            {
                query = query.Where(x => x.Year == Common_Lib.Convert_ToInt32(MF.Keyword_Year)).AsQueryable();
            }

            MF.Province_List = query.Select(x => x.Province).Distinct().ToList();
            if (!string.IsNullOrEmpty(MF.Keyword_Province))
            {
                query = query.Where(x => x.Province == MF.Keyword_Province).AsQueryable();
            }

            MF.City_List = query.Select(x => x.City).Distinct().ToList();
            if (!string.IsNullOrEmpty(MF.Keyword_City))
            {
                query = query.Where(x => x.City == MF.Keyword_City).AsQueryable();
            }

            MF.District_List = query.Select(x => x.District).Distinct().ToList();
            if (!string.IsNullOrEmpty(MF.Keyword_District))
            {
                query = query.Where(x => x.District == MF.Keyword_District).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Member_Name.Contains(MF.Keyword_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Status))
            {
                query = query.Where(x => x.Status == MF.Status).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Is_Frozen))
            {
                query = query.Where(x => x.Is_Frozen == MF.Is_Frozen).AsQueryable();
            }

            List<Alumni_Member> Row_List = query.OrderByDescending(x => x.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<Alumni_Member> PList = new PageList<Alumni_Member>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public PageList<Alumni_Member> Get_Alumni_Member_Auditor_ALL_PageList(Alumni_Member_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Alumni_Member.Where(x => x.Status == Status_Enum.已认证.ToString() && x.Is_Frozen == Alumni_Is_Frozen_Enum.启用.ToString()).AsQueryable();
            MF.Faculty_List = query.Select(x => x.Faculty).Distinct().ToList();

            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Member_Name.Contains(MF.Keyword_Name)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Faculty))
            {
                query = query.Where(x => x.Faculty == MF.Keyword_Faculty).AsQueryable();
            }

            List<Alumni_Member> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<Alumni_Member> PList = new PageList<Alumni_Member>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }
    }
}
