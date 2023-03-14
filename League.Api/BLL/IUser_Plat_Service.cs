using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace League.Api
{
    public interface IUser_Plat_Service
    {
        User_Plat Get_User_Plat_Login(User_Plat U, string Last_Login_IP, string Client_Info);
        User_Plat Get_User_Plat_Item(Guid UID);
        User_Plat Get_User_Plat_By_Controller(string Identity_UID);

        PageList<User_Plat> Get_User_Plat_PageList(User_Plat_Filter MF);
        PageList<User_Plat_Login_Record> Get_User_Plat_Login_Record_PageList(User_Plat_Filter MF);
        Guid Create_User(User_Plat U);
        void Set_User_Item(Guid UID, User_Plat U);
        void Delete_User_Item(Guid UID);
    }

    public partial class User_Plat_Service : IUser_Plat_Service
    {
        private readonly League_DbContext db = new League_DbContext();
    }

    public partial class User_Plat_Service : IUser_Plat_Service
    {
        public User_Plat Get_User_Plat_Login(User_Plat U, string Last_Login_IP, string Client_Info)
        {
            Last_Login_IP = Last_Login_IP == null ? string.Empty : Last_Login_IP.Trim();
            Client_Info = Client_Info == null ? string.Empty : Client_Info.Trim();

            U.Name = U.Name == null ? string.Empty : U.Name.Trim();
            U.Password = U.Password == null ? string.Empty : U.Password.Trim();
            User_Plat OLD_U = db.User_Plat.Where(x => x.Name == U.Name && x.Password == U.Password).FirstOrDefault();
            if (OLD_U == null) { throw new Exception("用户名或密码不正确"); }

            if (OLD_U.Is_Frozen != Alumni_Is_Frozen_Enum.启用.ToString())
            {
                throw new Exception("当前用户已被『" + OLD_U.Is_Frozen + "』");
            }

            //写入最后一次登陆记录
            User_Plat_Login_Record UR = new User_Plat_Login_Record
            {
                UPLR_ID = MyGUID.NewGuid(),
                Link_UID = OLD_U.UID,
                Name = OLD_U.Name,
                Name_Full = OLD_U.Name_Full,
                Password = OLD_U.Password,
                Last_Login_IP = Last_Login_IP,
                Client_Info = Client_Info,
                Last_Login_DT = DateTime.Now,
            };
            db.User_Plat_Login_Record.Add(UR);
            db.SaveChanges();
            return OLD_U;
        }

        public User_Plat Get_User_Plat_Item(Guid UID)
        {
            User_Plat Item = db.User_Plat.Find(UID);
            Item.Roles_Title_List = Common_Lib.Convert_ToString_List(Item.Roles_Title);
            return Item;
        }

        public User_Plat Get_User_Plat_By_Controller(string Identity_UID)
        {
            Guid UID = Common_Lib.Convert_GUID(Identity_UID);
            try { UID = new Guid(Identity_UID); } catch { UID = Guid.Empty; }
            User_Plat Item = this.Get_User_Plat_Item(UID);
            return Item;
        }
    }

    public partial class User_Plat_Service : IUser_Plat_Service
    {
        public PageList<User_Plat> Get_User_Plat_PageList(User_Plat_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);

            var query = db.User_Plat.AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Name_Full.Contains(MF.Keyword_Name) || x.Name.Contains(MF.Keyword_Name)).AsQueryable();
            }

            List<User_Plat> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();

            PageList<User_Plat> PList = new PageList<User_Plat>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public PageList<User_Plat_Login_Record> Get_User_Plat_Login_Record_PageList(User_Plat_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);

            var query = db.User_Plat_Login_Record.AsQueryable();
            if (!string.IsNullOrEmpty(MF.Keyword_Name))
            {
                query = query.Where(x => x.Name.Contains(MF.Keyword_Name) || x.Name_Full.Contains(MF.Keyword_Name)).AsQueryable();
            }

            PageList<User_Plat_Login_Record> PList = new PageList<User_Plat_Login_Record>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = query.OrderByDescending(s => s.Last_Login_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList()
            };
            return PList;
        }

        //创建新用户
        public Guid Create_User(User_Plat U)
        {
            Obj_Init.Trim(U);
            U.UID = MyGUID.NewGuid();
            U.Create_DT = DateTime.Now;
            U.Is_Admin = string.Empty;
            U.Is_Frozen = Is_Freeze_Enum.启用.ToString();

            if (string.IsNullOrEmpty(U.Password)) { throw new Exception("密码不能为空"); }
            this.Check_User_Info(U);

            db.User_Plat.Add(U);
            db.SaveChanges();
            return U.UID;
        }

        //最高权限用户更新
        public void Set_User_Item(Guid UID, User_Plat Item)
        {
            Obj_Init.Trim(Item);
            User_Plat OLD_Item = db.User_Plat.Find(UID);

            OLD_Item.Name = Item.Name;
            OLD_Item.Name_Full = Item.Name_Full;
            OLD_Item.Email = Item.Email;
            OLD_Item.Tel = Item.Tel;
            
            if (!string.IsNullOrEmpty(Item.Password)) { OLD_Item.Password = Item.Password.Trim(); }
            if (OLD_Item.Roles_Title != Alumni_Is_Admin_Enum.Admin.ToString()) { OLD_Item.Roles_Title = Item.Roles_Title; }
            this.Check_User_Info(OLD_Item);
            db.Entry(OLD_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //删除用户
        public void Delete_User_Item(Guid UID)
        {
            User_Plat OLD_Item = db.User_Plat.Find(UID);
            if (OLD_Item.Is_Admin == Alumni_Is_Admin_Enum.Admin.ToString()) { throw new Exception("Admin账号无法删除"); }
            if (db.Activity.Where(x => x.Link_UID == UID).Any()) { throw new Exception("已被历史数据关联，无法删除"); }
            db.User_Plat.Remove(OLD_Item);
            db.SaveChanges();
        }

        //校验用户信息
        private void Check_User_Info(User_Plat U)
        {
            if (string.IsNullOrEmpty(U.Name)) { throw new Exception("用户名未填写"); }
            if (string.IsNullOrEmpty(U.Name_Full)) { throw new Exception("用户全称未填写"); }

            bool Check_Name = db.User_Plat.Where(x => x.UID != U.UID && x.Name == U.Name).Any();
            if (Check_Name) { throw new Exception("此用户名已被占用"); }
        }
    }
}
