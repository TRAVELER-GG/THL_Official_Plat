using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

//小程序接口重构 2020.12.3
namespace League.Api
{
    public partial interface IApi_Service
    {
        Lable_List Get_Lable_List(string OpenID, string Mark);
        List<Alumnus> Get_Alumnus_Search(string OpenID, string Keyword);
        List<News> Get_News_List(int PageIndex);
        News Get_News_Item(Guid IMID);
        string Error_Logo_List(string Name);
        Member Get_Member_Other(string AMID, string UID);
        Alumni_Member Get_App_User_By_OpenID(string OpenID);
        Alumni_Member Get_App_User_By_AMID(string AMID);
        Alumnus Get_App_User_Brief(string OpenID);
        List<News> Get_Home_News_List(int PageIndex);
        App_Home Get_App_Home(string openid, int PageIndex);
        List<Enterprise> Get_Enterprise_List(string OpenID, string Keyword, int PageIndex);
        Alumni_Member_Enterprise Get_Member_Enterprise_Item(string MEID);
        List<Alu_Activity> Get_Activity_List(string OpenID, int PageIndex);
        List<Alu_Activity> Get_Event_List(string AMID, int PageIndex);
        Attention Get_Attention(string openid);
        Alu_Activity Get_Activity_Item(string AID, string AMID);
        void Create_Activity_Apply(string AID, string AMID);
        void Follow_Member_Or_Cancel(string Follow_AMID, string Fans_AMID);
        Alumnus_Wrap Get_Alumnus_Wrap_Mine(string OpenID);
        List<string> Get_Alumni_Union_Name_List();
        List<string> Get_Faculty_Name_List();
        List<string> Get_Years_List();
        List<string> Get_Degree_List();
        void Create_Alumnus_App(Alumni_Member Item, int Year);
        List<string> Get_Interest_List();
        List<Alumnus> Get_Alumnus_List_Verifty(string OpenID);
        void Change_Alumni_Verifty(string AMID);
        App_Personal Get_App_Personal(string openid);
        List<string> Get_Profession_List();
        void Update_Alumnus_Mobile(string OpenID, string Mobile);
        void Update_Alumnus_Gender(string OpenID, string Gender);
        void Update_Alumnus_Interest(string OpenID, string Interest);
        void Update_Alumnus_Year(string OpenID, int Year);
        void Update_Alumnus_Degree(string OpenID, string Degree);
        void Update_Alumnus_Faculty(string OpenID, string Faculty);
        void Update_Alumnus_Profession(string OpenID, string Profession);
        void Update_Alumnus_Company(string OpenID, string Company);
        void Update_Alumnus_Position(string OpenID, string Position);
        void Update_Alumnus_City(string OpenID, string Province, string City, string District);
        void Update_Alumnus_Native_City(string OpenID, string Native_Province, string Native_City, string Native_District);

        void Set_Page_View(string Page_View, string ID);

        App_Association Get_App_Association();
        Association Get_Association_Item(string AUID, string OpenID);
        void Create_Alumni_Union_Member_Link(string AUID, string OpenID);
    }

    public partial class Api_Service : IApi_Service
    {
        private readonly League_DbContext db = new League_DbContext();
    }

    public partial class Api_Service : IApi_Service
    {
        //搜索标签
        public Lable_List Get_Lable_List(string OpenID, string Mark)
        {
            Alumni_Member AM = this.Get_App_User_By_OpenID(OpenID);
            Lable_List Item = new Lable_List();
            if (Mark == Mark_Enum.Member.ToString()) { Item = this.Get_Member_Lable_List(AM.AMID); }
            if (Mark == Mark_Enum.Member_Company.ToString()) { Item = this.Get_Member_Enterprise_Lable_List(); }
            if (Mark == Mark_Enum.Mini.ToString()) { Item = this.Get_Mini_Service_Lable_List(); }
            if (Mark == Mark_Enum.Business.ToString()) { Item = this.Get_Business_Lable_List(); }
            return Item;
        }

        //校友搜索标签
        private Lable_List Get_Member_Lable_List(Guid AMID)
        {
            List<Alumni_Member> AM_List = db.Alumni_Member.Where(x => x.Is_Frozen == Alumni_Is_Frozen_Enum.启用.ToString()).ToList();

            foreach (var x in AM_List.Where(x => x.City == "市辖区"))
            {
                x.City = x.Province;
            }

            //按行业
            List<string> Profession_List = AM_List.Select(x => x.Profession).Distinct().ToList();
            Profession_List = Profession_List.Where(x => x != string.Empty && x != "其他").OrderBy(x => x).ToList();

            //按地区(所在地)
            List<string> Place_List = AM_List.Select(x => x.City).Distinct().ToList();
            Place_List = Place_List.Where(x => x != string.Empty && x != "请选择").OrderBy(x => x).ToList();

            //按院系
            List<string> Faculty_List = AM_List.Select(x => x.Faculty).Distinct().ToList();
            Faculty_List = Faculty_List.Where(x => x != string.Empty).OrderBy(x => x).ToList();

            //按兴趣
            List<string> Interest_List = new List<string>();
            foreach (var xx in AM_List.Where(c => c.Interest != string.Empty))
            {
                List<string> Inter_List = Common_Lib.Convert_ToString_List(xx.Interest);
                foreach (var Inter in Inter_List)
                {
                    if (Interest_List.Where(c => c == Inter).Any() == false)
                    {
                        Interest_List.Add(Inter);
                    }
                }
            }
            Interest_List = Interest_List.Where(x => x != string.Empty).OrderBy(x => x).ToList();

            Lable_List Item = new Lable_List
            {
                Faculty = Faculty_List,
                Place = Place_List,
                Interest = Interest_List,
                Profession_Member = Profession_List,
            };
            if (AMID == Guid.Empty) { Item = new Lable_List(); }
            return Item;
        }

        //校企搜索标签
        private Lable_List Get_Member_Enterprise_Lable_List()
        {
            List<string> Enter_List = db.Alumni_Member_Enterprise.Select(x => x.Profession).Distinct().ToList();
            Enter_List = Enter_List.Where(x => x != string.Empty).OrderBy(x => x.Length).ThenBy(x => x).ToList();
            Lable_List Item = new Lable_List
            {
                Enterprise = Enter_List,
            };
            return Item;
        }

        //微服坊搜索标签
        private Lable_List Get_Mini_Service_Lable_List()
        {
            List<string> Type_List = db.Mini_Service.Select(x => x.Type).Distinct().ToList();
            Type_List = Type_List.OrderBy(x => x.Length).ThenBy(x => x).ToList();
            Lable_List Item = new Lable_List
            {
                Service_Type = Type_List
            };
            return Item;
        }

        //商家搜索标签
        private Lable_List Get_Business_Lable_List()
        {
            List<Business> Bus_List = db.Business.ToList();

            //按商家区域
            List<string> Place_List = Bus_List.Select(x => x.Bus_District).Distinct().ToList();
            Place_List = Place_List.OrderBy(x => Guid.NewGuid()).Take(5).ToList();

            //按商家类型
            List<string> Type_List = Bus_List.Select(x => x.Bus_Type).Distinct().ToList();
            Type_List = Type_List.OrderBy(x => Guid.NewGuid()).Take(5).ToList();

            Lable_List Item = new Lable_List
            {
                Business_Type = Type_List,
                Business_Place = Place_List,
            };
            return Item;
        }


        //校友搜索
        public List<Alumnus> Get_Alumnus_Search(string OpenID, string Keyword)
        {
            Alumni_Member User = this.Get_App_User_By_OpenID(OpenID);
            var query = db.Alumni_Member.Where(x => x.Is_Frozen == Alumni_Is_Frozen_Enum.启用.ToString()).AsQueryable();
            query = query.Where(x => x.Member_Name.Contains(Keyword)
                                  || x.Faculty.Contains(Keyword)
                                  || x.Province.Contains(Keyword)
                                  || x.City.Contains(Keyword)
                                  || x.Interest.Contains(Keyword)
                                  || x.Profession.Contains(Keyword)).AsQueryable();

            Attention Att = this.Get_Attention(OpenID);
            Att = Att ?? new Attention();

            List<Alumnus> List = new List<Alumnus>();
            Alumnus Alu = new Alumnus();
            foreach (var x in query)
            {
                Alu = new Alumnus
                {
                    ID = x.AMID,
                    Img = x.Img,
                    Avatar_Url = x.Avatar_Url,
                    Name = x.Member_Name,
                    Gender = x.Gender,
                    Years = x.Year.ToString(),
                    Faculty = x.Faculty,
                    Status = "未关注",
                };

                if (Att.Follow_List.Where(c => c.ID == x.AMID).Any()) { Alu.Status = "已关注"; }
                List.Add(Alu);
            }
            if (User.AMID == Guid.Empty) { List = new List<Alumnus>(); }
            return List;
        }

        //新鲜事列表
        public List<News> Get_News_List(int PageIndex)
        {
            var query = db.Information.Where(x => x.Infor_Status == Infor_Status_Enum.已发布.ToString()).OrderByDescending(x => x.Create_DT).AsQueryable();

            int PageSize = 10;
            List<News> List = new List<News>();
            foreach (var x in query)
            {
                List.Add(new News
                {
                    ID = x.IMID,
                    Create_DT = x.Create_DT.ToString("yyyy/MM/dd").ToString(),
                    Img = x.Cover_Img,
                    Title = x.Title,
                    Publisher = x.Source,
                });
            }

            List = List.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return List;
        }

        //新鲜事详情
        public News Get_News_Item(Guid IMID)
        {
            Information Infor = db.Information.Find(IMID);
            News Item = new News
            {
                ID = Infor.IMID,
                Create_DT = Infor.Create_DT.ToString("yyyy/MM/dd HH:mm").ToString(),
                Img = Infor.Cover_Img,
                Title = Infor.Title,
                Content = Infor.Content,
                Publisher = Infor.Source
            };

            return Item;
        }

        //调用不到图片放此logo
        public string Error_Logo_List(string Name)
        {
            string Img_Url = "Images/Error_Logo/" + Name + ".png";
            return Img_Url;
        }

        //查看其他校友资料
        public Member Get_Member_Other(string AMID, string UID)
        {
            Guid Guid_AMID = Common_Lib.Convert_GUID(AMID);
            Guid Guid_UID = Common_Lib.Convert_GUID(UID);
            Alumni_Member AM = db.Alumni_Member.Find(Guid_AMID);

            Alumni_Member_Enterprise Enter = db.Alumni_Member_Enterprise.Where(x => x.Link_AMID == AM.AMID).FirstOrDefault();
            Enter = Enter ?? new Alumni_Member_Enterprise();

            Member Item = new Member
            {
                ID = AM.AMID,
                Img = AM.Img,
                Name = AM.Member_Name,
                Gender = AM.Gender,
                College = "西安交通大学",
                Faculty = AM.Faculty,
                Years = AM.Year.ToString(),
                Degree = AM.Degree,
                Phone = AM.Mobile,
                Phone_verifty = AM.Mobile,
                Profession = AM.Profession,
                Interest = AM.Interest.Replace(",", " | "),
                Company = AM.Company,
                Position = AM.Position,
                Enter_MEID = Enter.MEID,
                Enter_Name = Enter.Name,
                Enter_Duty = Enter.Duty,

                Province = AM.Province,
                City = AM.City,
                District = AM.District,

                Native_Province = AM.Native_Province,
                Native_City = AM.Native_City,
                Native_District = AM.Native_District,

                Link_AUID = AM.Link_AUID,
                Audit_Status = AM.Status,
                Avatar_Url = AM.Avatar_Url,
                Status = Member_Status_Enum.未关注.ToString(),
            };

            bool Is_Follow = db.Follow.Where(c => c.Follow_AMID == Guid_AMID && c.Fans_AMID == Guid_UID).Any();
            if (Is_Follow == true)
            {
                Item.Status = Member_Status_Enum.已关注.ToString();
            }

            //互关判断,互关显示手机号码
            if (db.Follow.Where(x => x.Fans_AMID == Guid_UID && x.Follow_AMID == Guid_AMID).Any() &&
                db.Follow.Where(x => x.Fans_AMID == Guid_AMID && x.Follow_AMID == Guid_UID).Any())
            {
                Item.Status = Member_Status_Enum.互关.ToString();
            }

            if (Item.Status != Member_Status_Enum.互关.ToString())
            {
                Item.Phone = Regex.Replace(Item.Phone, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
            }
            return Item;
        }

        //获取用户信息by openid
        public Alumni_Member Get_App_User_By_OpenID(string OpenID)
        {
            Alumni_Member Member = new Alumni_Member();
            if (db.Alumni_Member.Where(x => x.OpenID == OpenID).Any())
            {
                Member = db.Alumni_Member.Where(x => x.OpenID == OpenID).FirstOrDefault();
                Member = Member ?? new Alumni_Member();
            }
            Member.Logo = "Upload_File/Logo_College/20191202-1338-416a-bdd6-69a2cf88b511_西安交通大学校徽.png";
            Member.OpenID = OpenID;
            Member.Sign_DT = Member.Create_DT.ToString("yyyy-MM-dd");
            return Member;
        }

        //获取用户信息by AMID
        public Alumni_Member Get_App_User_By_AMID(string AMID)
        {
            if (string.IsNullOrEmpty(AMID)) { throw new Exception("UID为空"); }
            Guid Guid_AMID = new Guid(AMID);
            Alumni_Member User = db.Alumni_Member.Find(Guid_AMID);
            return User;
        }

        //获取用户简略信息
        public Alumnus Get_App_User_Brief(string OpenID)
        {
            Alumnus Item = new Alumnus();
            if (db.Alumni_Member.Where(x => x.OpenID == OpenID).Any())
            {
                Alumni_Member M = db.Alumni_Member.Where(x => x.OpenID == OpenID).FirstOrDefault();
                Alumni_Union AU = db.Alumni_Union.Find(M.Link_AUID);
                AU = AU ?? new Alumni_Union();

                //所有过期的商家券
                List<Guid> Ignore_Coupon_ID_List = db.Business_Coupon.Where(x => x.Expiry_DT < DateTime.Now).Select(x => x.Coupon_ID).ToList();
                //所有已领未使用的券(不含过期券)
                List<Business_Coupon_Member> Cou_Member_List = db.Business_Coupon_Member.Where(x => x.Link_AMID == M.AMID
                                                                                                 && x.Cancel_DT == string.Empty
                                                                                                 && Ignore_Coupon_ID_List.Contains(x.Link_Coupon_ID) == false).ToList();
                Item.ID = M.AMID;
                Item.Img = M.Img;
                Item.Avatar_Url = M.Avatar_Url;
                Item.College_Logo = "Upload_File/Logo_College/20191202-1338-416a-bdd6-69a2cf88b511_西安交通大学校徽.png";
                Item.College_Short = "西安交通大学";
                Item.Name = M.Member_Name;
                Item.Faculty = M.Faculty;
                Item.Years = M.Year.ToString();
                Item.Degree = M.Degree;
                Item.Status = M.Status;
                Item.Authority_Type = M.Authority_Type;

                Item.Coupon_Count = Cou_Member_List.Count();
                Item.Fans_Count = db.Follow.Where(x => x.Follow_AMID == M.AMID).Count();
                if (M.Authority_Type == Authority_Type_Enum.超级权限.ToString())
                {
                    Item.Verify_Count = db.Alumni_Member.Where(x => x.Status == Status_Enum.待认证.ToString()).Count();
                }
                else if (M.Authority_Type == Authority_Type_Enum.区域权限.ToString())
                {
                    Item.Verify_Count = db.Alumni_Member.Where(x => x.Status == Status_Enum.待认证.ToString() && x.Province == M.Authority_Province && x.City == M.Authority_City).Count();
                }
                else
                {
                    Item.Verify_Count = 0;
                }
            }
            return Item;
        }

        //首页展示新鲜事
        public List<News> Get_Home_News_List(int PageIndex)
        {
            var query = db.Information.Where(x => x.Infor_Status == Infor_Status_Enum.已发布.ToString()).OrderByDescending(x => x.Create_DT).AsQueryable();
            Information Info = new Information();
            int PageSize = 10;

            List<News> List = new List<News>();
            foreach (var x in query)
            {
                List.Add(new News
                {
                    ID = x.IMID,
                    Create_DT = x.Create_DT.ToString("yyyy/MM/dd").ToString(),
                    Img = x.Cover_Img,
                    Title = x.Title,
                    Publisher = x.Source,
                });
            }
            List = List.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return List;
        }

        //获取首页内容
        public App_Home Get_App_Home(string openid, int PageIndex)
        {
            App_Home Home = new App_Home();
            Guid AUID = Guid.Empty;
            if (!string.IsNullOrEmpty(openid))
            {
                Home.Home_Img = "Upload_File/Background/20191202-1338-416a-bdd6-69a2cf88b511_20201218-1103-44b8-80a2-0dcc6df92fa8_Thumb.png";
            }
            Home.News_List = this.Get_Home_News_List(PageIndex);
            return Home;
        }

        //校友企业列表
        public List<Enterprise> Get_Enterprise_List(string OpenID, string Keyword, int PageIndex)
        {
            Alumni_Member AM = this.Get_App_User_By_OpenID(OpenID);
            var query = db.Alumni_Member_Enterprise.Where(x => x.Link_AUID == AM.Link_AUID).AsQueryable();

            if (!string.IsNullOrEmpty(Keyword)) { query = query.Where(x => x.Name.Contains(Keyword) || x.Profession.Contains(Keyword)).AsQueryable(); }
            query = query.OrderBy(x => x.Create_DT).AsQueryable();

            List<Guid> AMID_List = query.Select(x => x.Link_AMID).Distinct().ToList();
            List<Alumni_Member> AM_List = db.Alumni_Member.Where(x => AMID_List.Contains(x.AMID)).ToList();
            Alumni_Member Temp_AM = new Alumni_Member();

            List<Guid> AUID_List = query.Select(x => x.Link_AUID).Distinct().ToList();
            List<Alumni_Union> AU_List = db.Alumni_Union.Where(x => AUID_List.Contains(x.AUID)).ToList();
            Alumni_Union Temp_AU = new Alumni_Union();

            List<Enterprise> List = new List<Enterprise>();
            foreach (var x in query)
            {
                Temp_AM = AM_List.Where(c => c.AMID == x.Link_AMID).FirstOrDefault();
                Temp_AM = Temp_AM ?? new Alumni_Member();

                Temp_AU = AU_List.Where(c => c.AUID == x.Link_AUID).FirstOrDefault();
                Temp_AU = Temp_AU ?? new Alumni_Union();

                List.Add(new Enterprise
                {
                    ID = x.MEID,
                    Img = x.Logo,
                    Name = x.Name,
                    Member_Name = Temp_AM.Member_Name,
                    Alumni_Name = Temp_AU.Name_Short,
                    Year = Temp_AM.Year.ToString(),

                    Profession = x.Profession,
                    Duty = x.Duty,

                    Province = x.Province,
                    City = x.City,
                });
            }
            int PageSize = 10;
            PageIndex = PageIndex <= 0 ? 1 : PageIndex;
            List = List.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return List;
        }

        //查看企业详情
        public Alumni_Member_Enterprise Get_Member_Enterprise_Item(string MEID)
        {
            Guid Guid_MEID = Common_Lib.Convert_GUID(MEID);
            Alumni_Member_Enterprise Enter = db.Alumni_Member_Enterprise.Find(Guid_MEID);
            Enter = Enter ?? new Alumni_Member_Enterprise();

            Alumni_Member AM = db.Alumni_Member.Find(Enter.Link_AMID);
            Enter.Member_Name = AM.Member_Name;
            return Enter;
        }

        //本校活动列表
        public List<Alu_Activity> Get_Activity_List(string OpenID, int PageIndex)
        {
            Alumni_Member AM = this.Get_App_User_By_OpenID(OpenID);
            // var query = db.Activity.Where(x => x.Status == Act_Status_Enum.已发布.ToString() && x.Link_AUID == AM.Link_AUID && x.End_DT > DateTime.Now).AsQueryable();
            var query = db.Activity.Where(x => x.Status == Act_Status_Enum.已发布.ToString() && x.End_DT > DateTime.Now).AsQueryable();
            List<Alu_Activity> List = new List<Alu_Activity>();
            int PageSize = 10;
            foreach (var x in query.OrderByDescending(x => x.Create_DT))
            {
                List.Add(new Alu_Activity
                {
                    ID = x.AID,
                    Poster = x.Poster,
                    Title = x.Title,
                    Start_DT = x.Start_DT.ToString("yyyy/MM/dd").ToString(),
                    Location = x.Location,
                    Sponsor = x.Sponsor,
                });
            }

            List = List.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return List;
        }

        //我参与的活动
        public List<Alu_Activity> Get_Event_List(string AMID, int PageIndex)
        {
            Guid Guid_AMID = Common_Lib.Convert_GUID(AMID);
            List<Guid> AID_List = db.Activity_Apply.Where(x => x.Link_AMID == Guid_AMID).Select(x => x.Link_AID).ToList();
            var query = db.Activity.Where(x => AID_List.Contains(x.AID)).AsQueryable();
            List<Alu_Activity> List = new List<Alu_Activity>();
            int PageSize = 10;
            foreach (var x in query.OrderByDescending(x => x.Create_DT))
            {
                List.Add(new Alu_Activity
                {
                    ID = x.AID,
                    Poster = x.Poster,
                    Title = x.Title,
                    Start_DT = x.Start_DT.ToString("yyyy-MM/dd").ToString(),
                    Location = x.Location,
                });
            }
            List = List.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            return List;
        }

        //我的关注、粉丝、互关
        public Attention Get_Attention(string openid)
        {
            Alumni_Member M = this.Get_App_User_By_OpenID(openid);
            Attention Att = new Attention();

            //我的关注
            List<Guid> Follow_AMID_List = db.Follow.Where(x => x.Fans_AMID == M.AMID).Select(x => x.Follow_AMID).ToList();
            List<Alumni_Member> Follow_Member_List = new List<Alumni_Member>();
            if (Follow_AMID_List.Any())
            {
                Follow_Member_List = db.Alumni_Member.Where(x => Follow_AMID_List.Contains(x.AMID)).ToList();
                foreach (var x in Follow_Member_List)
                {
                    Att.Follow_List.Add(new Alumnus
                    {
                        ID = x.AMID,
                        Avatar_Url = x.Avatar_Url,
                        Name = x.Member_Name,
                        Years = x.Year.ToString(),
                        Degree = x.Degree,
                        Faculty = x.Faculty,
                    });
                }
            }

            //我的粉丝
            List<Guid> Fans_AMID_List = db.Follow.Where(x => x.Follow_AMID == M.AMID).Select(x => x.Fans_AMID).ToList();
            List<Alumni_Member> Fans_Member_List = new List<Alumni_Member>();
            if (Fans_AMID_List.Any())
            {
                Fans_Member_List = db.Alumni_Member.Where(x => Fans_AMID_List.Contains(x.AMID)).ToList();
                foreach (var x in Fans_Member_List)
                {
                    Att.Fans_List.Add(new Alumnus
                    {
                        ID = x.AMID,
                        Avatar_Url = x.Avatar_Url,
                        Name = x.Member_Name,
                        Years = x.Year.ToString(),
                        Degree = x.Degree,
                        Faculty = x.Faculty,
                    });
                }
            }

            //互关
            var Each_AMID_List = Fans_AMID_List.Intersect(Follow_AMID_List).ToList(); //求出两者交集
            List<Alumni_Member> Each_Member_List = new List<Alumni_Member>();
            if (Each_AMID_List.Any())
            {
                Each_Member_List = db.Alumni_Member.Where(x => Each_AMID_List.Contains(x.AMID)).ToList();
                foreach (var x in Fans_Member_List)
                {
                    Att.Each_List.Add(new Alumnus
                    {
                        ID = x.AMID,
                        Avatar_Url = x.Avatar_Url,
                        Name = x.Member_Name,
                        Years = x.Year.ToString(),
                        Degree = x.Degree,
                        Faculty = x.Faculty,
                    });
                }
            }
            return Att;
        }

        //活动详情
        public Alu_Activity Get_Activity_Item(string AID, string AMID)
        {
            Guid Guid_AID = Common_Lib.Convert_GUID(AID);
            Guid Guid_AMID = Common_Lib.Convert_GUID(AMID);
            Activity Activity = db.Activity.Find(Guid_AID);
            Alumni_Union AU = db.Alumni_Union.Find(Activity.Link_AUID);
            AU = AU ?? new Alumni_Union();
            Alu_Activity Item = new Alu_Activity
            {
                ID = Activity.AID,
                Title = Activity.Title,
                Poster = Activity.Poster,
                Sponsor = Activity.Sponsor,
                Union = AU,
                Start_DT = Activity.Start_DT.ToString("yyyy/MM/dd HH:mm").ToString(),
                End_DT = Activity.End_DT.ToString("yyyy/MM/dd HH:mm").ToString(),
                Location = Activity.Location,
                Num_of_people = Activity.Num_of_people,
                Content = Activity.Content,
                Is_Attend = Is_Attend_Enum.未参与.ToString(),
                People = this.Get_Activity_People_List(Guid_AID).Count(),
                People_List = this.Get_Activity_People_List(Guid_AID),
            };

            if (Guid_AMID != Guid.Empty)
            {
                if (db.Activity_Apply.Where(c => c.Link_AID == Guid_AID && c.Link_AMID == Guid_AMID).Any())
                {
                    Item.Is_Attend = Is_Attend_Enum.已参与.ToString();
                }
            }
            return Item;
        }

        private List<Activity_People> Get_Activity_People_List(Guid AID)
        {
            var Activity_Apply_List = db.Activity_Apply.Where(x => x.Link_AID == AID).AsQueryable();
            List<Guid> AMID_List = Activity_Apply_List.Select(x => x.Link_AMID).ToList();
            var Member_List = db.Alumni_Member.Where(x => AMID_List.Contains(x.AMID)).AsQueryable();
            List<Activity_People> List = new List<Activity_People>();
            if (Member_List.Any())
            {
                foreach (var x in Member_List)
                {
                    List.Add(new Activity_People
                    {
                        Img = x.Img,
                        Name = x.Member_Name,
                        Phone = x.Mobile
                    });
                }
            }
            return List;
        }




        //一键报名
        public void Create_Activity_Apply(string AID, string AMID)
        {
            Guid Guid_AID = Common_Lib.Convert_GUID(AID);
            Guid Guid_AMID = Common_Lib.Convert_GUID(AMID);
            Alumni_Member AM = db.Alumni_Member.Find(Guid_AMID);
            if (AM.AMID == Guid.Empty) { throw new Exception("游客无法参与活动"); }
            if (AM.Status == Status_Enum.待认证.ToString()) { throw new Exception("待认证状态，无法参与"); }
            if (db.Activity_Apply.Where(x => x.Link_AID == Guid_AID && x.Link_AMID == Guid_AMID).Any()) { throw new Exception("已报名成功，请勿重复报名"); }
            Activity Activity = db.Activity.Find(Guid_AID);
            Activity = Activity ?? new Activity();
            if (Activity.Num_of_people == db.Activity_Apply.Where(x => x.Link_AID == Guid_AID).Count())
            {
                throw new Exception("报名人数已满");
            }
            Activity_Apply Item = new Activity_Apply
            {
                AAID = MyGUID.NewGuid(),
                Name = AM.Member_Name,
                Phone = AM.Mobile,
                Link_AMID = Guid_AMID,
                Link_AID = Guid_AID,
            };
            db.Activity_Apply.Add(Item);
            db.SaveChanges();
        }

        //关注/取关校友
        public void Follow_Member_Or_Cancel(string Follow_AMID, string Fans_AMID)
        {
            Guid Guid_Follow_AMID = Common_Lib.Convert_GUID(Follow_AMID);
            Guid Guid_Fans_AMID = Common_Lib.Convert_GUID(Fans_AMID);

            if (Guid_Follow_AMID == Guid_Fans_AMID) { throw new Exception("无法关注自己"); }

            bool Is_Follow = db.Follow.Where(x => x.Follow_AMID == Guid_Follow_AMID && x.Fans_AMID == Guid_Fans_AMID).Any();
            if (Is_Follow == true)
            {
                Follow F = db.Follow.Where(x => x.Follow_AMID == Guid_Follow_AMID && x.Fans_AMID == Guid_Fans_AMID).FirstOrDefault();
                db.Follow.Remove(F);
            }
            else
            {
                Follow Item = new Follow()
                {
                    FID = MyGUID.NewGuid(),
                    Create_DT = DateTime.Now,
                    Follow_AMID = Guid_Follow_AMID,
                    Follow_MEID = Guid.Empty,
                    Follow_MSID = Guid.Empty,
                    Fans_AMID = Guid_Fans_AMID,
                };
                db.Follow.Add(Item);
            }
            if (db.Alumni_Member.Where(x => x.Status == Status_Enum.待认证.ToString() && x.AMID == Guid_Fans_AMID).Any())
            {
                throw new Exception("当前未认证，无法关注校友");
            }
            db.SaveChanges();
        }

        //搜校友
        public Alumnus_Wrap Get_Alumnus_Wrap_Mine(string OpenID)
        {
            Alumni_Member AM = this.Get_App_User_By_OpenID(OpenID);
            Alumnus_Wrap Item = new Alumnus_Wrap();
            if (AM.AMID != Guid.Empty)
            {
                Item = new Alumnus_Wrap
                {
                    Faculty = this.Get_Member_Faculty_Mine(AM.AMID),
                    Interest = this.Get_Member_Interest_Mine(AM.AMID),
                    Profession = this.Get_Member_Profession_Mine(AM.AMID),
                    Village = this.Get_Member_Village_Mine(AM.AMID),
                    Year = this.Get_Member_Year(AM.AMID),
                };
            }
            return Item;
        }

        private List<Member> Get_Member_Faculty_Mine(Guid AMID)
        {
            Alumni_Member AM = db.Alumni_Member.Find(AMID);

            List<Alumni_Member> Member_List = new List<Alumni_Member>();
            if (!string.IsNullOrEmpty(AM.Faculty))
            {
                var query = db.Alumni_Member.Where(x => x.AMID != AM.AMID).AsQueryable();
                Member_List = query.Where(x => x.Faculty == AM.Faculty).ToList();
            }

            Attention Att = this.Get_Attention(AM.OpenID);

            List<Member> List = new List<Member>();
            Member Item = new Member();
            foreach (var x in Member_List)
            {
                Item = new Member
                {
                    ID = x.AMID,
                    Img = x.Img,
                    Avatar_Url = x.Avatar_Url,
                    Name = x.Member_Name,
                    Faculty = x.Faculty,
                    Gender = x.Gender,
                    Status = "未关注",
                };
                if (Att.Follow_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "已关注"; }
                if (Att.Each_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "互关"; }
                List.Add(Item);
            }
            List = List.Take(20).ToList();
            return List;
        }

        private List<Member> Get_Member_Interest_Mine(Guid AMID)
        {
            Alumni_Member AM = db.Alumni_Member.Find(AMID);
            List<string> Interest_List = Common_Lib.Convert_ToString_List(AM.Interest);

            List<Alumni_Member> AM_List = new List<Alumni_Member>();
            if (!string.IsNullOrEmpty(AM.Interest))
            {
                AM_List = db.Alumni_Member.Where(x => x.AMID != AMID).ToList();

                foreach (var x in AM_List)
                {
                    List<string> AM_Interest_List = Common_Lib.Convert_ToString_List(x.Interest);
                    foreach (var Intere in Interest_List)
                    {
                        foreach (var xxx in AM_Interest_List.Where(c => c == Intere))
                        {
                            x.Flag += 1;
                        }
                    }
                }
                AM_List = AM_List.Where(x => x.Flag != 0).OrderByDescending(x => x.Flag).ToList();
            }

            Attention Att = this.Get_Attention(AM.OpenID);

            List<Member> List = new List<Member>();
            Member Item = new Member();
            foreach (var x in AM_List)
            {
                Item = new Member
                {
                    ID = x.AMID,
                    Img = x.Img,
                    Avatar_Url = x.Avatar_Url,
                    Name = x.Member_Name,
                    Faculty = x.Faculty,
                    Gender = x.Gender,
                    Status = "未关注",
                };
                if (Att.Follow_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "已关注"; }
                if (Att.Each_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "互关"; }

                List.Add(Item);
            }
            List = List.Take(20).ToList();
            return List;
        }

        private List<Member> Get_Member_Profession_Mine(Guid AMID)
        {
            Alumni_Member AM = db.Alumni_Member.Find(AMID);

            List<Alumni_Member> Member_List = new List<Alumni_Member>();
            if (!string.IsNullOrEmpty(AM.Profession))
            {
                var query = db.Alumni_Member.Where(x => x.AMID != AM.AMID).AsQueryable();
                Member_List = query.Where(x => x.Profession == AM.Profession).ToList();
            }

            Attention Att = this.Get_Attention(AM.OpenID);

            List<Member> List = new List<Member>();
            Member Item = new Member();
            foreach (var x in Member_List)
            {
                Item = new Member
                {
                    ID = x.AMID,
                    Img = x.Img,
                    Avatar_Url = x.Avatar_Url,
                    Name = x.Member_Name,
                    Faculty = x.Faculty,
                    Gender = x.Gender,
                    Status = "未关注",
                };
                if (Att.Follow_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "已关注"; }
                if (Att.Each_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "互关"; }

                List.Add(Item);
            }

            List = List.Take(10).ToList();
            return List;
        }

        private List<Member> Get_Member_Village_Mine(Guid AMID)
        {
            Alumni_Member AM = db.Alumni_Member.Find(AMID);

            List<Alumni_Member> Member_List = new List<Alumni_Member>();
            if (!string.IsNullOrEmpty(AM.Native_City))
            {
                var query = db.Alumni_Member.Where(x => x.AMID != AM.AMID).AsQueryable();
                Member_List = query.Where(x => AM.Native_City == x.Native_City).ToList();
            }

            Attention Att = this.Get_Attention(AM.OpenID);

            List<Member> List = new List<Member>();
            Member Item = new Member();
            foreach (var x in Member_List)
            {
                Item = new Member
                {
                    ID = x.AMID,
                    Img = x.Img,
                    Avatar_Url = x.Avatar_Url,
                    Name = x.Member_Name,
                    Faculty = x.Faculty,
                    Gender = x.Gender,
                    Status = "未关注",
                };
                if (Att.Follow_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "已关注"; }
                if (Att.Each_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "互关"; }

                List.Add(Item);
            }
            List = List.Take(20).ToList();
            return List;
        }

        private List<Member> Get_Member_Year(Guid AMID)
        {
            Alumni_Member AM = db.Alumni_Member.Find(AMID);

            List<Alumni_Member> Member_List = new List<Alumni_Member>();

            var query = db.Alumni_Member.Where(x => x.AMID != AM.AMID).AsQueryable();
            Member_List = query.Where(x => x.Year == AM.Year).ToList();

            Attention Att = this.Get_Attention(AM.OpenID);

            List<Member> List = new List<Member>();
            Member Item = new Member();
            foreach (var x in Member_List)
            {
                Item = new Member
                {
                    ID = x.AMID,
                    Img = x.Img,
                    Avatar_Url = x.Avatar_Url,
                    Name = x.Member_Name,
                    Faculty = x.Faculty,
                    Gender = x.Gender,
                    Status = "未关注",
                };
                if (Att.Follow_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "已关注"; }
                if (Att.Each_List.Where(c => c.ID == x.AMID).Any()) { Item.Status = "互关"; }
                List.Add(Item);
            }
            List = List.OrderBy(x => Guid.NewGuid()).Take(20).ToList();
            return List;
        }


        public List<string> Get_Alumni_Union_Name_List()
        {
            List<string> List = db.Alumni_Union.OrderBy(x => x.Name).Select(x => x.Name).ToList();
            return List;
        }

        public List<string> Get_Faculty_Name_List()
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

        public List<string> Get_Degree_List()
        {
            List<string> List = Enum.GetNames(typeof(Degree_Enum)).ToList();
            return List;
        }

        public List<string> Get_Years_List()
        {
            List<string> List = new List<string>();
            int Year = DateTime.Now.Year;
            for (int i = 1980; i <= Year; i++)
            {
                List.Add(i.ToString());
            }
            return List;
        }

        public void Create_Alumnus_App(Alumni_Member Item, int Year)
        {
            Obj_Init.Trim(Item);
            if (string.IsNullOrEmpty(Item.OpenID)) { throw new Exception("openid为空"); }
            if (string.IsNullOrEmpty(Item.Member_Name)) { throw new Exception("真实姓名未填写"); }
            if (string.IsNullOrEmpty(Item.Mobile)) { throw new Exception("手机号码未填写"); }
            if (Year == 0) { throw new Exception("毕业年份未选择"); }
            if (string.IsNullOrEmpty(Item.Faculty)) { throw new Exception("院系名称未填写"); }
            if (string.IsNullOrEmpty(Item.Degree)) { throw new Exception("所获学历未填写"); }
            if (Item.Province == "请选择") { throw new Exception("所在城市未选择"); }
            if (string.IsNullOrEmpty(Item.Gender)) { throw new Exception("性别未选择"); }

            Item.AMID = MyGUID.NewGuid();
            Item.Create_DT = DateTime.Now;
            Item.Img = string.Empty;
            Item.Is_Frozen = Alumni_Is_Frozen_Enum.启用.ToString();
            Item.Status = Status_Enum.待认证.ToString();
            Item.Year = Year;
            Item.Link_AUID = Guid.Empty;

            if (db.Alumni_Member.Where(x => x.OpenID == Item.OpenID).Any()) { throw new Exception("请勿重复注册"); }
            db.Alumni_Member.Add(Item);
            db.SaveChanges();
        }

        //获取所有兴趣列表
        public List<string> Get_Interest_List()
        {
            List<string> List = new List<string>
            {
                new string("爬山"),
                new string("攀岩"),
                new string("冲浪"),
                new string("跳伞"),
                new string("露营"),
                new string("骑行"),
                new string("马拉松"),
                new string("足球"),
                new string("篮球"),
                new string("羽毛球"),
                new string("网球"),
                new string("乒乓球"),
                new string("保龄球"),
                new string("高尔夫"),
                new string("游泳"),
                new string("瑜伽"),
                new string("跆拳道"),
                new string("拳击"),
                new string("太极"),
                new string("柔道"),
                new string("健美"),
                new string("舞蹈"),
                new string("收藏"),
                new string("炒股"),
                new string("投资"),
                new string("理财"),
                new string("读书"),
                new string("摄影"),
                new string("书法"),
                new string("绘画"),
                new string("掼蛋"),
                new string("钓鱼"),
                new string("唱歌"),
                new string("桥牌"),
            };
            return List;
        }

        //待认证校友列表
        public List<Alumnus> Get_Alumnus_List_Verifty(string OpenID)
        {
            Alumni_Member AM = this.Get_App_User_By_OpenID(OpenID);

            List<Alumni_Member> AM_List = db.Alumni_Member.Where(x => x.Link_AUID == AM.Link_AUID && x.Status == Status_Enum.待认证.ToString()).OrderByDescending(x => x.Create_DT).ToList();
            if (AM.Authority_Type == Authority_Type_Enum.区域权限.ToString())
            {
                AM_List = AM_List.Where(x => x.Province == AM.Authority_Province && x.City == AM.Authority_City).ToList();
            }

            List<Alumnus> List = new List<Alumnus>();
            foreach (var x in AM_List)
            {
                List.Add(new Alumnus
                {
                    ID = x.AMID,
                    Name = x.Member_Name,
                    Img = x.Avatar_Url,
                    Degree = x.Degree,
                    Years = x.Year.ToString(),
                    Faculty = x.Faculty,
                });
            }
            return List;
        }

        public void Change_Alumni_Verifty(string AMID)
        {
            Guid Guid_AMID = Common_Lib.Convert_GUID(AMID);
            Alumni_Member Item = db.Alumni_Member.Find(Guid_AMID);
            Item.Status = Status_Enum.已认证.ToString();
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public App_Personal Get_App_Personal(string openid)
        {
            App_Personal Data = new App_Personal()
            {
                Years_List = this.Get_Years_List(),
                Interest_List = this.Get_Interest_List(),
                Degree_List = Enum.GetNames(typeof(Degree_Enum)).ToList(),
                Faculty_List = this.Get_Faculty_Name_List(),
                Profession_First = this.Get_Profession_List(),
                Profession_Second = this.Get_Profession_Second_List(),
            };

            return Data;
        }

        public List<string> Get_Profession_List()
        {
            List<string> List = new List<string>()
            {
                new string("商务服务"),
                new string("科技服务"),
                new string("互联网"),
                new string("文娱"),
                new string("大消费"),
                new string("科技"),
                new string("文娱"),
                new string("大健康"),
                new string("生产制造"),
                new string("能源环保"),
                new string("农林牧渔"),
                new string("政府民生"),
                new string("其他"),
            };
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

        //修改个人信息-手机号码
        public void Update_Alumnus_Mobile(string OpenID, string Mobile)
        {
            Alumni_Member Old_Itme = this.Get_App_User_By_OpenID(OpenID);
            Old_Itme.Mobile = Mobile;
            db.Entry(Old_Itme).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-性别
        public void Update_Alumnus_Gender(string OpenID, string Gender)
        {
            Alumni_Member Old_Itme = this.Get_App_User_By_OpenID(OpenID);
            Old_Itme.Gender = Gender;
            db.Entry(Old_Itme).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-兴趣爱好
        public void Update_Alumnus_Interest(string OpenID, string Interest)
        {
            Alumni_Member Old_Itme = this.Get_App_User_By_OpenID(OpenID);
            Old_Itme.Interest = Common_Lib.Trim_End(Interest);
            db.Entry(Old_Itme).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-毕业年份
        public void Update_Alumnus_Year(string OpenID, int Year)
        {
            Alumni_Member Old_Item = this.Get_App_User_By_OpenID(OpenID);
            Old_Item.Year = Year;
            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-所获学位
        public void Update_Alumnus_Degree(string OpenID, string Degree)
        {
            Alumni_Member Old_Item = this.Get_App_User_By_OpenID(OpenID);
            Old_Item.Degree = Degree;
            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-所属院系
        public void Update_Alumnus_Faculty(string OpenID, string Faculty)
        {
            Alumni_Member Old_Item = this.Get_App_User_By_OpenID(OpenID);
            //if (db.Alumni_Union_Faculty.Where(x => x.Link_AUID == Old_Item.Link_AUID && x.Name == Faculty).Any() == false)
            //{
            //    throw new Exception("院系有误");
            //}
            Old_Item.Faculty = Faculty;
            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-所处行业
        public void Update_Alumnus_Profession(string OpenID, string Profession)
        {
            Alumni_Member Old_Item = this.Get_App_User_By_OpenID(OpenID);
            Old_Item.Profession = Profession;
            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-所在公司
        public void Update_Alumnus_Company(string OpenID, string Company)
        {
            Alumni_Member Old_Item = this.Get_App_User_By_OpenID(OpenID);

            if (string.IsNullOrEmpty(Company)) { throw new Exception("公司名称未填写"); }
            Old_Item.Company = Company.Trim();

            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-当前职位
        public void Update_Alumnus_Position(string OpenID, string Position)
        {
            Alumni_Member Old_Item = this.Get_App_User_By_OpenID(OpenID);

            if (string.IsNullOrEmpty(Position)) { throw new Exception("职位名称未填写"); }
            Old_Item.Position = Position.Trim();

            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-所在城市
        public void Update_Alumnus_City(string OpenID, string Province, string City, string District)
        {
            Alumni_Member Old_Item = this.Get_App_User_By_OpenID(OpenID);
            Old_Item.Province = Province;
            Old_Item.City = City;
            Old_Item.District = District;
            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //修改个人信息-出生籍贯
        public void Update_Alumnus_Native_City(string OpenID, string Native_Province, string Native_City, string Native_District)
        {
            Alumni_Member Old_Item = this.Get_App_User_By_OpenID(OpenID);
            Old_Item.Native_Province = Native_Province;
            Old_Item.Native_City = Native_City;
            Old_Item.Native_District = Native_District;
            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Set_Page_View(string Page_View, string ID)
        {
            if (Page_View == PV_Enum.Information.ToString()) { this.Set_Information_PV(ID); }
            if (Page_View == PV_Enum.Activity.ToString()) { this.Set_Activity_PV(ID); }
            if (Page_View == PV_Enum.Business.ToString()) { this.Set_Business_PV(ID); }
            if (Page_View == PV_Enum.Alumni_Union.ToString()) { this.Set_Alumni_Union_PV(ID); }
        }

        public enum PV_Enum
        {
            Information,
            Activity,
            Business,
            Alumni_Union,
        }

        //资讯浏览量+1
        private void Set_Information_PV(string ID)
        {
            Guid Info_ID = Common_Lib.Convert_GUID(ID);
            Information Item = db.Information.Find(Info_ID);
            Item = Item ?? new Information();

            Item.Infor_PV += 1;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //活动浏览量+1
        private void Set_Activity_PV(string ID)
        {
            Guid AID = Common_Lib.Convert_GUID(ID);
            Activity Item = db.Activity.Find(AID);
            Item = Item ?? new Activity();

            Item.Activity_PV += 1;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //商家浏览量+1
        private void Set_Business_PV(string ID)
        {
            Guid BID = Common_Lib.Convert_GUID(ID);
            Business Item = db.Business.Find(BID);
            Item = Item ?? new Business();

            Item.Bus_PV += 1;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }

        //分会浏览量+1
        private void Set_Alumni_Union_PV(string ID)
        {
            Guid AUID = Common_Lib.Convert_GUID(ID);
            Alumni_Union Item = db.Alumni_Union.Find(AUID);
            Item = Item ?? new Alumni_Union();

            Item.Union_PV += 1;
            db.Entry(Item).State = EntityState.Modified;
            db.SaveChanges();
        }


        //获取校友分会列表
        public App_Association Get_App_Association()
        {
            List<Alumni_Union> Alumni_Union_List = db.Alumni_Union.OrderBy(x => x.Union_Type).ToList();
            Alumni_Union Alumni_Union = new Alumni_Union();

            List<Guid> AUID_List = Alumni_Union_List.Select(x => x.AUID).ToList();
            var Alumni_Member_Count = db.Alumni_Union_Member_Link.Where(x => AUID_List.Contains(x.Link_AUID)).AsQueryable();

            List<Association> List = new List<Association>();
            Association Item = new Association();
            foreach (var x in Alumni_Union_List)
            {
                Item = new Association
                {
                    ID = x.AUID,
                    Img = x.Logo,
                    Name = x.Name,
                    Name_Short = x.Name_Short,
                    Establish_DT = x.Establish_DT,
                    Union_Type = x.Union_Type,
                    Count = Alumni_Member_Count.Where(c => c.Link_AUID == x.AUID).Count().ToString(),
                };

                List.Add(Item);
            }

            App_Association App_Association = new App_Association()
            {
                Part1_List = List.Where(c => c.Union_Type == Union_Type_Enum.地区.ToString()).ToList(),
                Part2_List = List.Where(c => c.Union_Type == Union_Type_Enum.行业.ToString()).ToList(),
            };
            return App_Association;
        }

        //获取分会详情
        public Association Get_Association_Item(string AUID, string OpenID)
        {
            Alumni_Member Alumni_Member = this.Get_App_User_By_OpenID(OpenID);
            Guid LinK_AUID = Common_Lib.Convert_GUID(AUID);
            Alumni_Union Alumni_Union = db.Alumni_Union.Find(LinK_AUID);

            Association Item = new Association()
            {
                ID = Alumni_Union.AUID,
                Contact = Alumni_Union.Contact,
                Phone = Alumni_Union.Phone,
                Img = Alumni_Union.Logo,
                Content = Alumni_Union.Content,
                Name = Alumni_Union.Name,
                Name_Short = Alumni_Union.Name_Short,
                Is_Join = db.Alumni_Union_Member_Link.Where(x => x.Link_AUID == LinK_AUID && x.Link_AMID == Alumni_Member.AMID).Any() ? Is_Join_Enum.已加入.ToString() : Is_Join_Enum.未加入.ToString(),
            };
            return Item;
        }

        public Alumni_Union Get_Alumni_Union_By_AUID(string AUID)
        {
            Guid Guid_AUID = Common_Lib.Convert_GUID(AUID);
            Alumni_Union Item = db.Alumni_Union.Find(Guid_AUID);
            Item = Item ?? new Alumni_Union();
            return Item;
        }

        //加入校友分会
        public void Create_Alumni_Union_Member_Link(string AUID, string OpenID)
        {
            Alumni_Union Alumni_Union = this.Get_Alumni_Union_By_AUID(AUID);
            Alumni_Member Alumni_Member = this.Get_App_User_By_OpenID(OpenID);

            Alumni_Union_Member_Link Item = new Alumni_Union_Member_Link
            {
                Link_ID = MyGUID.NewGuid(),
                Create_DT = DateTime.Now,
                Link_AMID = Alumni_Member.AMID,
                Link_AUID = Alumni_Union.AUID,
            };

            if (string.IsNullOrEmpty(OpenID)) { throw new Exception("请于首页注册仙交校友会"); }
            if (Alumni_Member.Status == Status_Enum.待认证.ToString()) { throw new Exception("请认证后再来加入"); }

            db.Alumni_Union_Member_Link.Add(Item);
            db.SaveChanges();
        }
    }
}

//商家接口
namespace League.Api
{
    public partial interface IApi_Service
    {
        App_Business Get_App_Business(string Keyword, string Keyword_Area, string Keyword_Type, int PageIndex);
        List<Business> Get_Business_List(string Keyword, string Keyword_Area, string Keyword_Type, int PageIndex);
        Business Get_Business_Info(string BID, string OpenID);
        List<Business_Coupon> Get_Business_Coupon_List(int PageIndex);
        void Create_Business_Coupon_Member(string Coupon_ID, string OpenID);

        App_Business_Coupon Get_App_Business_Coupon(string Keyword_Area, string Keyword_Name, string OpenID);
        Business_Coupon_Member Get_Business_Coupon_Member_Item(string Coupon_ID, string OpenID);
        void Cancel_Business_Coupon_Member(string Cou_Mem_ID);
    }

    public partial class Api_Service : IApi_Service
    {
        //吃住行数据
        public App_Business Get_App_Business(string Keyword, string Keyword_Area, string Keyword_Type, int PageIndex)
        {
            App_Business Item = new App_Business
            {
                Business_List = this.Get_Business_List(Keyword, Keyword_Area, Keyword_Type, PageIndex),
                Business_Type_List = this.Get_Business_Type_List(),
                Business_Area_List = this.Get_App_Business_Area_List(),
            };
            return Item;
        }

        private List<Submenu> Get_Business_Type_List()
        {
            List<Submenu> List = new List<Submenu>();
            List.Add(new Submenu
            {
                Name = "全部分类",
                Value = "",
            });
            foreach (var Type in Enum.GetNames(typeof(Bus_Type_Enum)))
            {
                List.Add(new Submenu
                {
                    Name = Type,
                    Value = Type,
                });
            }
            return List;
        }

        private List<Business_Area> Get_App_Business_Area_List()
        {
            var query = db.Business.AsQueryable();
            List<string> City_List = query.Select(x => x.Bus_City).Distinct().ToList();
            List<string> District_List = new List<string>();

            List<Business_Area> Area_List = new List<Business_Area>();
            Business_Area Area = new Business_Area();
            List<Submenu> Sub_List = new List<Submenu>();
            Sub_List.Add(new Submenu
            {
                Name = "所有区",
                Value = "",
            });
            Area_List.Add(new Business_Area
            {
                Name = "所有市",
                Value = "所有市",
                Submenu = Sub_List
            });


            foreach (var City in City_List)
            {
                Area = new Business_Area
                {
                    Name = City,
                    Value = City,
                    Submenu = new List<Submenu>()
                };
                Area_List.Add(Area);
            }

            Submenu Submenu = new Submenu();
            foreach (var x in Area_List)
            {
                District_List = new List<string>();
                District_List = query.Where(c => c.Bus_City == x.Name).Select(c => c.Bus_District).Distinct().ToList();

                foreach (var District in District_List)
                {
                    Submenu = new Submenu
                    {
                        Name = District,
                        Value = District,
                    };
                    x.Submenu.Add(Submenu);
                }
            }
            return Area_List;
        }

        //获取商家列表
        public List<Business> Get_Business_List(string Keyword, string Keyword_Area, string Keyword_Type, int PageIndex)
        {
            var query = db.Business.AsQueryable();
            int PageSize = 10;

            if (!string.IsNullOrEmpty(Keyword))
            {
                query = query.Where(x => x.Bus_Name.Contains(Keyword) || x.Bus_Type.Contains(Keyword) || x.Bus_City.Contains(Keyword) || x.Bus_District.Contains(Keyword)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(Keyword_Area))
            {
                query = query.Where(x => x.Bus_District.Contains(Keyword_Area)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(Keyword_Type))
            {
                query = query.Where(x => x.Bus_Type.Contains(Keyword_Type)).AsQueryable();
            }

            List<Business> List = new List<Business>();
            List = query.OrderByDescending(x => x.Create_DT).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            List<Guid> BID_List = List.Select(x => x.BID).ToList();
            List<Business_Coupon> Coup_List = db.Business_Coupon.Where(x => BID_List.Contains(x.Link_BID) && x.Expiry_DT > DateTime.Now).OrderByDescending(x => x.Create_DT).Take(2).ToList();

            List<Business_Coupon_Member> CM_list = db.Business_Coupon_Member.Where(x => BID_List.Contains(x.Link_BID)).ToList();
            //卡券余量
            foreach (var x in Coup_List)
            {
                x.Coupon_Quantity_Surplus = x.Coupon_Quantity - CM_list.Where(c => c.Link_BID == x.Link_BID && c.Link_Coupon_ID == x.Coupon_ID).Count();
            }

            List<Business_Album> Alb_list = db.Business_Album.Where(x => BID_List.Contains(x.Link_BID) && x.Album_Type == Album_Type_Enum.封面.ToString()).ToList();

            List<Business_Coupon> Coup_List_Sub = new List<Business_Coupon>();
            foreach (var x in List)
            {
                Business_Album Album = Alb_list.Where(c => c.Link_BID == x.BID).FirstOrDefault();
                Album = Album ?? new Business_Album();

                Coup_List_Sub = Coup_List.Where(c => c.Link_BID == x.BID).ToList();
                x.Coupon_List = Coup_List_Sub;
                x.Bus_Img = Album.Album_Img;
            }
            return List;
        }

        //获取商家详情
        public Business Get_Business_Info(string BID, string OpenID)
        {
            Guid Guid_BID = Common_Lib.Convert_GUID(BID);
            Business Item = db.Business.Find(Guid_BID);
            Item = Item ?? new Business();

            Item.Album_List = db.Business_Album.Where(x => x.Link_BID == Item.BID && x.Album_Type == Album_Type_Enum.相册.ToString()).OrderByDescending(x => x.Create_DT).ToList();
            Item.Album_List_Count = Item.Album_List.Count();

            Alumni_Member Member = this.Get_App_User_By_OpenID(OpenID);

            Item.Coupon_List = db.Business_Coupon.Where(x => x.Coupon_Status == Coupon_Status_Enum.启用.ToString() && x.Expiry_DT > DateTime.Now && x.Link_BID == Guid_BID).ToList();
            List<Business_Coupon_Member> CM_list = db.Business_Coupon_Member.Where(x => x.Link_BID == Guid_BID).ToList();
            //卡券余量
            foreach (var x in Item.Coupon_List)
            {
                x.Coupon_Quantity_Surplus = x.Coupon_Quantity - CM_list.Where(c => c.Link_BID == x.Link_BID && c.Link_Coupon_ID == x.Coupon_ID).Count();
                x.Coupon_Receive = CM_list.Where(c => c.Link_Coupon_ID == x.Coupon_ID && c.Link_AMID == Member.AMID).Any() ? "已领" : "领取";
                x.Expiry_DT_Show = x.Expiry_DT.ToString("yyyy/MM/dd");
            }
            Item.Coupon_List = Item.Coupon_List.OrderBy(x => x.Coupon_Receive).ToList();
            return Item;
        }

        //领券中心(所有优惠券)
        public List<Business_Coupon> Get_Business_Coupon_List(int PageIndex)
        {
            var query = db.Business_Coupon.Where(x => x.Coupon_Status == Coupon_Status_Enum.启用.ToString() && x.Expiry_DT > DateTime.Now).AsQueryable();
            int PageSize = 10;
            List<Business_Coupon> List = new List<Business_Coupon>();
            List = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            List<Guid> Coupon_ID_List = List.Select(x => x.Coupon_ID).ToList();
            List<Business_Coupon_Member> CM_list = db.Business_Coupon_Member.Where(x => Coupon_ID_List.Contains(x.Link_Coupon_ID)).ToList();
            //卡券余量
            foreach (var x in List)
            {
                x.Coupon_Quantity_Surplus = x.Coupon_Quantity - CM_list.Where(c => c.Link_BID == x.Link_BID && c.Link_Coupon_ID == x.Coupon_ID).Count();
            }
            return List;
        }

        //本商家所有卡券
        public List<Business_Coupon> Get_Business_Coupon_List_By_BID(string BID, string OpenID, int PageIndex)
        {
            Guid Guid_BID = Common_Lib.Convert_GUID(BID);
            var query = db.Business_Coupon.Where(x => x.Coupon_Status == Coupon_Status_Enum.启用.ToString() && x.Expiry_DT > DateTime.Now && x.Link_BID == Guid_BID).AsQueryable();
            int PageSize = 10;

            Alumni_Member Member = this.Get_App_User_By_OpenID(OpenID);

            List<Business_Coupon> List = new List<Business_Coupon>();
            List = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            List<Business_Coupon_Member> CM_list = db.Business_Coupon_Member.Where(x => x.Link_BID == Guid_BID).ToList();
            //卡券余量
            foreach (var x in List)
            {
                x.Coupon_Quantity_Surplus = x.Coupon_Quantity - CM_list.Where(c => c.Link_BID == x.Link_BID && c.Link_Coupon_ID == x.Coupon_ID).Count();
                x.Coupon_Receive = CM_list.Where(c => c.Link_Coupon_ID == x.Coupon_ID && c.Link_AMID == Member.AMID).Any() ? "已领" : "领取";
            }
            List = List.OrderBy(x => x.Coupon_Receive).ToList();
            return List;
        }

        //领取卡券
        public void Create_Business_Coupon_Member(string Coupon_ID, string OpenID)
        {
            Guid Link_Coupon_ID = Common_Lib.Convert_GUID(Coupon_ID);
            Business_Coupon Coupon = db.Business_Coupon.Find(Link_Coupon_ID);
            Coupon = Coupon ?? new Business_Coupon();

            Alumni_Member Member = this.Get_App_User_By_OpenID(OpenID);

            var Cou_Member_Query = db.Business_Coupon_Member.Where(x => x.Link_Coupon_ID == Coupon.Coupon_ID).AsQueryable();
            Coupon.Coupon_Quantity_Surplus = Coupon.Coupon_Quantity - Cou_Member_Query.Count();

            if (Member.AMID == Guid.Empty) { throw new Exception("请于首页注册校友汇"); }
            if (Member.Status == Status_Enum.待认证.ToString()) { throw new Exception("等待认证中"); }
            if (Cou_Member_Query.Where(c => c.Link_AMID == Member.AMID).Any()) { throw new Exception("无法重复领取"); }
            if (Coupon.Coupon_Quantity_Surplus <= 0) { throw new Exception("优惠卡券余量不足"); }

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


        //我的优惠券
        public App_Business_Coupon Get_App_Business_Coupon(string Keyword_Area, string Keyword_Name, string OpenID)
        {
            App_Business_Coupon Item = new App_Business_Coupon
            {
                Business_Area_List = this.Get_App_Business_Coupon_Area_List(OpenID),
                Business_Name_List = this.Get_App_Business_Coupon_Name_List(OpenID),
                Coupon_List_A = this.Get_Business_Coupon_List_By_OpenID(Keyword_Area, Keyword_Name, OpenID, Business_Coupon_Status_Enum.待使用.ToString()),
                Coupon_List_B = this.Get_Business_Coupon_List_By_OpenID(Keyword_Area, Keyword_Name, OpenID, Business_Coupon_Status_Enum.已使用.ToString()),
                Coupon_List_C = this.Get_Business_Coupon_List_By_OpenID(Keyword_Area, Keyword_Name, OpenID, Business_Coupon_Status_Enum.已失效.ToString()),
            };
            return Item;
        }

        private List<Business_Area> Get_App_Business_Coupon_Area_List(string OpenID)
        {
            Alumni_Member Member = this.Get_App_User_By_OpenID(OpenID);
            List<Business_Coupon_Member> Cou_Member_List = db.Business_Coupon_Member.Where(x => x.Link_AMID == Member.AMID).ToList();
            List<Guid> BID_List = Cou_Member_List.Select(x => x.Link_BID).Distinct().ToList();
            var query = db.Business.Where(x => BID_List.Contains(x.BID)).AsQueryable();

            List<string> City_List = query.Select(x => x.Bus_City).Distinct().ToList();
            List<string> District_List = new List<string>();

            List<Business_Area> Area_List = new List<Business_Area>();
            Business_Area Area = new Business_Area();
            List<Submenu> Sub_List = new List<Submenu>();
            Sub_List.Add(new Submenu
            {
                Name = "所有区",
                Value = "",
            });
            Area_List.Add(new Business_Area
            {
                Name = "所有市",
                Value = "所有市",
                Submenu = Sub_List
            });
            foreach (var City in City_List)
            {
                Area = new Business_Area
                {
                    Name = City,
                    Value = City,
                    Submenu = new List<Submenu>()
                };
                Area_List.Add(Area);
            }

            Submenu Submenu = new Submenu();
            foreach (var x in Area_List)
            {
                District_List = new List<string>();
                District_List = query.Where(c => c.Bus_City == x.Name).Select(c => c.Bus_District).Distinct().ToList();

                foreach (var District in District_List)
                {
                    Submenu = new Submenu
                    {
                        Name = District,
                        Value = District,
                    };
                    x.Submenu.Add(Submenu);
                }
            }
            return Area_List;
        }

        private List<Submenu> Get_App_Business_Coupon_Name_List(string OpenID)
        {
            Alumni_Member Member = this.Get_App_User_By_OpenID(OpenID);
            List<Business_Coupon_Member> Cou_Member_List = db.Business_Coupon_Member.Where(x => x.Link_AMID == Member.AMID).ToList();
            List<Guid> BID_List = Cou_Member_List.Select(x => x.Link_BID).Distinct().ToList();
            List<Business> Business_List = db.Business.Where(x => BID_List.Contains(x.BID)).ToList();

            List<Submenu> List = new List<Submenu>();
            List.Add(new Submenu
            {
                Name = "全部商家",
                Value = "",
            });
            foreach (var Bus in Business_List)
            {
                List.Add(new Submenu
                {
                    Name = Bus.Bus_Name + " / " + Bus.Bus_Type,
                    Value = Bus.Bus_Name,
                });
            }
            return List;
        }

        public List<Business_Coupon> Get_Business_Coupon_List_By_OpenID(string Keyword_Area, string Keyword_Name, string OpenID, string Status)
        {
            Alumni_Member Member = this.Get_App_User_By_OpenID(OpenID);
            List<Business_Coupon_Member> Cou_Member_List = db.Business_Coupon_Member.Where(x => x.Link_AMID == Member.AMID).ToList();
            Business_Coupon_Member Cou_Member = new Business_Coupon_Member();
            List<Guid> BID_List = Cou_Member_List.Select(x => x.Link_BID).Distinct().ToList();
            List<Business> Business_List = db.Business.Where(x => BID_List.Contains(x.BID)).ToList();
            Business Bus = new Business();
            List<Business_Coupon> Bus_Coup_List = new List<Business_Coupon>();
            //票未核销、券没过期
            if (Status == Business_Coupon_Status_Enum.待使用.ToString())
            {
                List<Guid> Coupon_ID_List = Cou_Member_List.Where(x => x.Cancel_DT == string.Empty).Select(x => x.Link_Coupon_ID).ToList();
                Bus_Coup_List = db.Business_Coupon.Where(x => Coupon_ID_List.Contains(x.Coupon_ID) && x.Expiry_DT > DateTime.Now).ToList();
                foreach (var x in Bus_Coup_List)
                {
                    Bus = Business_List.Where(c => c.BID == x.Link_BID).FirstOrDefault();
                    Bus = Bus ?? new Business();
                    x.Business = Bus;
                    x.Expiry_DT_Show = x.Expiry_DT.ToString("yyyy/MM/dd");
                }

                if (!string.IsNullOrEmpty(Keyword_Area))
                {
                    Bus_Coup_List = Bus_Coup_List.Where(x => x.Business.Bus_District.Contains(Keyword_Area)).ToList();
                }

                if (!string.IsNullOrEmpty(Keyword_Name))
                {
                    Bus_Coup_List = Bus_Coup_List.Where(x => x.Business.Bus_Name.Contains(Keyword_Name)).ToList();
                }
            }

            //票已核销
            if (Status == Business_Coupon_Status_Enum.已使用.ToString())
            {
                List<Guid> Coupon_ID_List = Cou_Member_List.Where(x => x.Cancel_DT != string.Empty).Select(x => x.Link_Coupon_ID).ToList();
                Bus_Coup_List = db.Business_Coupon.Where(x => Coupon_ID_List.Contains(x.Coupon_ID)).ToList();
                foreach (var x in Bus_Coup_List)
                {
                    Bus = Business_List.Where(c => c.BID == x.Link_BID).FirstOrDefault();
                    Bus = Bus ?? new Business();

                    Cou_Member = Cou_Member_List.Where(c => c.Link_Coupon_ID == x.Coupon_ID).FirstOrDefault();
                    Cou_Member = Cou_Member ?? new Business_Coupon_Member();

                    x.Business = Bus;
                    x.Cancel_DT_Show = Cou_Member.Cancel_DT;
                }
            }

            //券已失效
            if (Status == Business_Coupon_Status_Enum.已失效.ToString())
            {
                List<Guid> Coupon_ID_List = Cou_Member_List.Select(x => x.Link_Coupon_ID).ToList();
                Bus_Coup_List = db.Business_Coupon.Where(x => Coupon_ID_List.Contains(x.Coupon_ID) && x.Expiry_DT <= DateTime.Now).ToList();
                foreach (var x in Bus_Coup_List)
                {
                    Bus = Business_List.Where(c => c.BID == x.Link_BID).FirstOrDefault();
                    Bus = Bus ?? new Business();
                    x.Business = Bus;
                    x.Expiry_DT_Show = x.Expiry_DT.ToString("yyyy/MM/dd");
                }
            }
            return Bus_Coup_List;
        }

        public enum Business_Coupon_Status_Enum
        {
            待使用,
            已使用,
            已失效
        }

        //优惠券详情
        public Business_Coupon_Member Get_Business_Coupon_Member_Item(string Coupon_ID, string OpenID)
        {
            Guid Guid_Coupon_ID = Common_Lib.Convert_GUID(Coupon_ID);
            Alumni_Member Member = this.Get_App_User_By_OpenID(OpenID);
            Business_Coupon_Member Item = db.Business_Coupon_Member.Where(x => x.Link_Coupon_ID == Guid_Coupon_ID && x.Link_AMID == Member.AMID).FirstOrDefault();
            Item = Item ?? new Business_Coupon_Member();

            Business Bus = db.Business.Find(Item.Link_BID);
            Bus = Bus ?? new Business();
            Item.Business = Bus;
            Business_Album Album = db.Business_Album.Where(x => x.Link_BID == Item.Business.BID && x.Album_Type == Album_Type_Enum.封面.ToString()).FirstOrDefault();
            Album = Album ?? new Business_Album();
            Item.Business.Bus_Img = Album.Album_Img;

            Business_Coupon Bus_Coupon = db.Business_Coupon.Find(Item.Link_Coupon_ID);
            Bus_Coupon = Bus_Coupon ?? new Business_Coupon();
            Item.Business_Coupon = Bus_Coupon;

            Item.Business_Coupon.Expiry_DT_Show = Item.Business_Coupon.Expiry_DT.ToString("yyyy/MM/dd");
            Item.Cancel_DT_Show = Item.Cancel_DT;
            return Item;
        }

        //核销优惠券
        public void Cancel_Business_Coupon_Member(string Cou_Mem_ID)
        {
            Guid Guid_Cou_Mem_ID = Common_Lib.Convert_GUID(Cou_Mem_ID);
            Business_Coupon_Member Old_Item = db.Business_Coupon_Member.Find(Guid_Cou_Mem_ID);
            Old_Item = Old_Item ?? new Business_Coupon_Member();

            if (string.IsNullOrEmpty(Old_Item.Cancel_DT)) { Old_Item.Cancel_DT = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); }

            db.Entry(Old_Item).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}

