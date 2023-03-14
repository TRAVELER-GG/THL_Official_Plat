using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace League.Api
{
    public partial interface IActivity_Service
    {
        PageList<Activity> Get_Activity_PageList(Activity_Filter MF);
        Activity Get_Activity_Item(Guid AID);
        Activity Get_Activity_Empty();
        void Create_Activity(Guid Link_UID, Activity Item);
        void Save_Activity(Guid Link_UID, Activity Item);
        void Set_Activity(Guid AID, Activity Item);
        void Set_Activity_Poster(Guid AID, string Poster);
        void Del_Activity(Guid AID);
    }

    public partial class Activity_Service : IActivity_Service
    {
        private readonly League_DbContext db = new League_DbContext();
    }

    public partial class Activity_Service : IActivity_Service
    {
        public PageList<Activity> Get_Activity_PageList(Activity_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Activity.AsQueryable();

            if (!string.IsNullOrEmpty(MF.Keyword_Title))
            {
                query = query.Where(x => x.Title.Contains(MF.Keyword_Title)).AsQueryable();
            }

            List<Activity> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            List<Guid> AID_List = Row_List.Select(x => x.AID).ToList();
            List<Activity_Apply> Activity_Apply_List = db.Activity_Apply.Where(x => AID_List.Contains(x.Link_AID)).ToList();
            foreach (Activity Item in Row_List)
            {
                Item.People = Activity_Apply_List.Where(x => x.Link_AID == Item.AID).Count();
            }
            PageList<Activity> PList = new PageList<Activity>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public Activity Get_Activity_Item(Guid AID)
        {
            Activity Item = db.Activity.Find(AID);
            Item = Item ?? new Activity();
            Item.People = db.Activity_Apply.Where(x => x.Link_AID == AID).Count();
            Item.Union_List = db.Alumni_Union.ToList();
            return Item;
        }

        public Activity Get_Activity_Empty()
        {
            Activity Item = new Activity();
            Item.Union_List = db.Alumni_Union.ToList();
            return Item;
        }

        public void Create_Activity(Guid Link_UID, Activity Item)
        {
            Obj_Init.Trim(Item);
            Item.AID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;
            Item.Status = Act_Status_Enum.已发布.ToString();
            Item.Link_UID = Link_UID;

            this.Check_Activity_Info(Item);
            db.Activity.Add(Item);
            db.SaveChanges();
        }

        public void Save_Activity(Guid Link_UID, Activity Item)
        {
            Obj_Init.Trim(Item);

            Item.AID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;
            Item.Status = Act_Status_Enum.待发布.ToString();
            Item.Link_UID = Link_UID;

            if (string.IsNullOrEmpty(Item.Title)) { throw new Exception("标题未填写"); }
            db.Activity.Add(Item);
            db.SaveChanges();
        }

        private void Check_Activity_Info(Activity Item)
        {
            if (string.IsNullOrEmpty(Item.Title)) { throw new Exception("未填写活动标题"); }
            if (string.IsNullOrEmpty(Item.Location)) { throw new Exception("未填写活动地点"); }
            if (Item.Link_AUID == Guid.Empty) { throw new Exception("未选择主办方"); }
            //if (string.IsNullOrEmpty(Item.Scope)) { throw new Exception("未选择报名对象"); }
            if (Item.Content == "<p><br></p>") { throw new Exception("未填写活动内容"); }

            if (Item.Start_DT == DateTime.MinValue) { throw new Exception("未选择活动开始时间"); }
            if (Item.End_DT == DateTime.MinValue) { throw new Exception("未选择活动结束时间"); }
            if (Item.Start_DT > Item.End_DT) { throw new Exception("活动结束时间与活动开始时间冲突"); }

            bool Title = db.Activity.Where(x => x.AID != Item.AID && x.Title == Item.Title).Any();
            if (Title == true) { throw new Exception("活动标题重复"); }
        }

        public void Set_Activity(Guid AID, Activity Item)
        {
            Obj_Init.Trim(Item);
            Activity OLD_Item = db.Activity.Find(AID);

            OLD_Item.Title = Item.Title;
            OLD_Item.Start_DT = Item.Start_DT;
            OLD_Item.End_DT = Item.End_DT;
            OLD_Item.Location = Item.Location;
            OLD_Item.Num_of_people = Item.Num_of_people;
            OLD_Item.Content = Item.Content;
            OLD_Item.Scope = Item.Scope;
            OLD_Item.Status = Act_Status_Enum.已发布.ToString();
            OLD_Item.Link_AUID = Item.Link_AUID;

            this.Check_Activity_Info(OLD_Item);

            db.Entry(OLD_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Activity_Poster(Guid AID, string Poster)
        {
            Activity Item = db.Activity.Find(AID);
            Item.Poster = Poster;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Del_Activity(Guid AID)
        {
            Activity Item = db.Activity.Find(AID);
            Item ??= new Activity();
            List<Activity_Apply> List = db.Activity_Apply.Where(x => x.Link_AID == AID).ToList();

            db.Activity_Apply.RemoveRange(List);
            db.Activity.Remove(Item);
            db.SaveChanges();
        }
    }

    public partial interface IActivity_Service
    {
        PageList<Activity_Apply> Get_Activity_Apply_PageList(Activity_Filter MF);
    }

    public partial class Activity_Service : IActivity_Service
    {
        public PageList<Activity_Apply> Get_Activity_Apply_PageList(Activity_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Activity_Apply.Where(x => x.Link_AID == MF.Link_AID).AsQueryable();

            List<Activity_Apply> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            var AM_List = db.Alumni_Member.AsQueryable();
            foreach (var x in Row_List)
            {
                foreach (var xx in AM_List.Where(c => c.AMID == x.Link_AMID))
                {
                    x.Gender = xx.Gender;
                    x.Faculty = xx.Faculty;
                }
            }

            PageList<Activity_Apply> PList = new PageList<Activity_Apply>
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
