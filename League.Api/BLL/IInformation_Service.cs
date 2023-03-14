using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace League.Api
{
    public interface IInformation_Service
    {
        PageList<Information> Get_Information_PageList(Information_Filter MF);
        Information Get_Information_Item(Guid IMID);
        Information Get_Information_Empty();

        void Create_Information(Guid UID, Information Item);
        void Set_Information(Guid IMID, Information Item);
        void Set_Information_Cover_Img(Guid IMID, string Cover_Img);
        void Del_Information(Guid IMID);

        void Save_Information(Guid UID, Information Item);
    }

    public partial class Information_Service : IInformation_Service
    {
        private readonly League_DbContext db = new League_DbContext();
    }

    public partial class Information_Service : IInformation_Service
    {
        public PageList<Information> Get_Information_PageList(Information_Filter MF)
        {
            Obj_Init.Trim_Filter(MF);
            var query = db.Information.AsQueryable();

            Guid ALL_Info = new Guid("00000000-0000-0000-0000-000000000001");

            //资讯来源
            //if (MF.Link_AUID == ALL_Info)
            //{
            //    query = query.Where(x => x.Link_AUID != MF.Link_AUID).AsQueryable();
            //}

            if (MF.Link_AUID == Guid.Empty)
            {
                query = query.Where(x => x.Link_AUID == Guid.Empty).AsQueryable();
            }

            if (MF.Link_AUID != Guid.Empty && MF.Link_AUID != ALL_Info)
            {
                query = query.Where(x => x.Link_AUID == MF.Link_AUID).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Keyword_Title))
            {
                query = query.Where(x => x.Title.Contains(MF.Keyword_Title)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Classify))
            {
                query = query.Where(x => x.Classify.Contains(MF.Classify)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(MF.Infor_Status))
            {
                query = query.Where(x => x.Infor_Status == MF.Infor_Status).AsQueryable();
            }

            List<Guid> AUID_List = db.Information.Select(x => x.Link_AUID).Distinct().ToList();
            List<Alumni_Union> AU_List = db.Alumni_Union.Where(x => AUID_List.Contains(x.AUID)).ToList();

            MF.Alumni_Union_List.Add(new Alumni_Union
            {
                AUID = Guid.Empty,
                Name_Short = Publisher_Enum.校友总会.ToString(),
            });
            MF.Alumni_Union_List.AddRange(AU_List);

            List<Information> Row_List = query.OrderByDescending(s => s.Create_DT).Skip((MF.PageIndex - 1) * MF.PageSize).Take(MF.PageSize).ToList();
            PageList<Information> PList = new PageList<Information>
            {
                PageIndex = MF.PageIndex,
                PageSize = MF.PageSize,
                TotalRecord = query.Count(),
                Rows = Row_List
            };
            return PList;
        }

        public Information Get_Information_Item(Guid IMID)
        {
            Information Item = db.Information.Find(IMID);
            Item ??= new Information();

            Item.Alumni_Union_List.Add(new Alumni_Union
            {
                AUID = Guid.Empty,
                Name_Short = Publisher_Enum.校友总会.ToString(),
            });
            Item.Alumni_Union_List.AddRange(db.Alumni_Union.ToList());
            return Item;
        }

        public Information Get_Information_Empty()
        {
            Information Item = new Information();
            Item.Alumni_Union_List.Add(new Alumni_Union
            {
                AUID = Guid.Empty,
                Name_Short = Publisher_Enum.校友总会.ToString(),
            });
            Item.Alumni_Union_List.AddRange(db.Alumni_Union.ToList());
            return Item;
        }

        public void Create_Information(Guid UID, Information Item)
        {
            Obj_Init.Trim(Item);
            Item.IMID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;
            Item.Infor_Status = Infor_Status_Enum.已发布.ToString();

            if (Item.Link_AUID == Guid.Empty)
            {
                Item.Source = Publisher_Enum.校友总会.ToString();
            }
            else
            {
                Alumni_Union AU = db.Alumni_Union.Find(Item.Link_AUID);
                AU = AU ?? new Alumni_Union();
                Item.Source = AU.Name_Short;
            }

            this.Check_Information_Info(Item);
            db.Information.Add(Item);
            db.SaveChanges();
        }

        public void Save_Information(Guid UID, Information Item)
        {
            Obj_Init.Trim(Item);
            Item.IMID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;
            Item.Infor_Status = Infor_Status_Enum.待发布.ToString();

            if (Item.Link_AUID == Guid.Empty)
            {
                Item.Source = Publisher_Enum.校友总会.ToString();
            }
            else
            {
                Alumni_Union AU = db.Alumni_Union.Find(Item.Link_AUID);
                AU = AU ?? new Alumni_Union();
                Item.Source = AU.Name_Short;
            }

            if (string.IsNullOrEmpty(Item.Title)) { throw new Exception("标题未填写！"); }
            db.Information.Add(Item);
            db.SaveChanges();
        }

        public void Set_Information(Guid IMID, Information Item)
        {
            Obj_Init.Trim(Item);
            Information OLD_Item = db.Information.Find(IMID);

            OLD_Item.Title = Item.Title;
            OLD_Item.Classify = Item.Classify;
            OLD_Item.Content = Item.Content;
            OLD_Item.Publisher = Item.Publisher;
            OLD_Item.Type = Item.Type;
            OLD_Item.Infor_Status = Infor_Status_Enum.已发布.ToString();
            OLD_Item.Link_AUID = Item.Link_AUID;

            Alumni_Union AU = db.Alumni_Union.Find(Item.Link_AUID);
            AU = AU ?? new Alumni_Union();
            if (AU.AUID != Guid.Empty)
            {
                OLD_Item.Source = AU.Name_Short;
            }
            else
            {
                OLD_Item.Source = Publisher_Enum.校友总会.ToString();
            }

            this.Check_Information_Info(OLD_Item);
            db.Entry(OLD_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Information_Cover_Img(Guid IMID, string Cover_Img)
        {
            Information Item = db.Information.Find(IMID);
            Item.Cover_Img = Cover_Img;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void Check_Information_Info(Information Item)
        {
            if (string.IsNullOrEmpty(Item.Title)) { throw new Exception("标题未填写！"); }
            if (Item.Content == "<p><br></p>") { throw new Exception("内容未填写！"); }
        }

        public void Del_Information(Guid IMID)
        {
            Information Item = db.Information.Find(IMID);
            Item ??= new Information();

            db.Information.Remove(Item);
            db.SaveChanges();
        }
    }
}
