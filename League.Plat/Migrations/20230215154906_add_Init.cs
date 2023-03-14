using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace League.Plat.Migrations
{
    public partial class add_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    AID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Start_DT = table.Column<DateTime>(nullable: false),
                    End_DT = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    Poster = table.Column<string>(nullable: false),
                    Sponsor = table.Column<string>(nullable: false),
                    Num_of_people = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Scope = table.Column<string>(nullable: false),
                    Activity_PV = table.Column<int>(nullable: false),
                    Link_UID = table.Column<Guid>(nullable: false),
                    Link_AUID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.AID);
                });

            migrationBuilder.CreateTable(
                name: "Activity_Apply",
                columns: table => new
                {
                    AAID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Link_AMID = table.Column<Guid>(nullable: false),
                    Link_AID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity_Apply", x => x.AAID);
                });

            migrationBuilder.CreateTable(
                name: "Alumni_Member",
                columns: table => new
                {
                    AMID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Img = table.Column<string>(nullable: false),
                    Member_Name = table.Column<string>(nullable: false),
                    Mobile = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    Is_Frozen = table.Column<string>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Faculty = table.Column<string>(nullable: false),
                    Degree = table.Column<string>(nullable: false),
                    Profession = table.Column<string>(nullable: false),
                    Interest = table.Column<string>(nullable: false),
                    Company = table.Column<string>(nullable: false),
                    Position = table.Column<string>(nullable: false),
                    Native_Province = table.Column<string>(nullable: false),
                    Native_City = table.Column<string>(nullable: false),
                    Native_District = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Avatar_Url = table.Column<string>(nullable: false),
                    Nick_Name = table.Column<string>(nullable: false),
                    OpenID = table.Column<string>(nullable: false),
                    Session_Key = table.Column<string>(nullable: false),
                    Authority_Type = table.Column<string>(nullable: false),
                    Authority_Province = table.Column<string>(nullable: false),
                    Authority_City = table.Column<string>(nullable: false),
                    Link_AUID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumni_Member", x => x.AMID);
                });

            migrationBuilder.CreateTable(
                name: "Alumni_Member_Enterprise",
                columns: table => new
                {
                    MEID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Logo = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Duty = table.Column<string>(nullable: false),
                    Contact = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Profession = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    District = table.Column<string>(nullable: false),
                    Detial = table.Column<string>(nullable: false),
                    Main_Business = table.Column<string>(nullable: false),
                    Link_AMID = table.Column<Guid>(nullable: false),
                    Link_AUID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumni_Member_Enterprise", x => x.MEID);
                });

            migrationBuilder.CreateTable(
                name: "Alumni_Union",
                columns: table => new
                {
                    AUID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Establish_DT = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Name_Short = table.Column<string>(nullable: false),
                    Union_Type = table.Column<string>(nullable: false),
                    Logo = table.Column<string>(nullable: false),
                    Logo_College = table.Column<string>(nullable: false),
                    Background = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Permission = table.Column<string>(nullable: false),
                    Contact = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    Union_PV = table.Column<int>(nullable: false),
                    PinCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumni_Union", x => x.AUID);
                });

            migrationBuilder.CreateTable(
                name: "Alumni_Union_Member_Link",
                columns: table => new
                {
                    Link_ID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Link_AUID = table.Column<Guid>(nullable: false),
                    Link_AMID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumni_Union_Member_Link", x => x.Link_ID);
                });

            migrationBuilder.CreateTable(
                name: "Alumni_Union_User",
                columns: table => new
                {
                    UID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    User_Name = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Tel = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Img_Path = table.Column<string>(nullable: false),
                    Is_Frozen = table.Column<string>(nullable: false),
                    Is_Admin = table.Column<string>(nullable: false),
                    Roles_Title = table.Column<string>(nullable: false),
                    Link_AUID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumni_Union_User", x => x.UID);
                });

            migrationBuilder.CreateTable(
                name: "Alumni_Union_User_Login_Record",
                columns: table => new
                {
                    ULR_ID = table.Column<Guid>(nullable: false),
                    Link_UID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    Name_Full = table.Column<string>(maxLength: 256, nullable: true),
                    Password = table.Column<string>(maxLength: 256, nullable: true),
                    Last_Login_IP = table.Column<string>(maxLength: 256, nullable: true),
                    Last_Login_DT = table.Column<DateTime>(nullable: false),
                    Client_Info = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumni_Union_User_Login_Record", x => x.ULR_ID);
                });

            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    BID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Bus_Img = table.Column<string>(nullable: false),
                    Bus_Name = table.Column<string>(nullable: false),
                    Bus_Name_Register = table.Column<string>(nullable: false),
                    Bus_Type = table.Column<string>(nullable: false),
                    Bus_Province = table.Column<string>(nullable: false),
                    Bus_City = table.Column<string>(nullable: false),
                    Bus_District = table.Column<string>(nullable: false),
                    Bus_Address = table.Column<string>(nullable: false),
                    Bus_Contact = table.Column<string>(nullable: false),
                    Bus_Phone = table.Column<string>(nullable: false),
                    Bus_Recommend = table.Column<string>(nullable: false),
                    Bus_Hours = table.Column<string>(nullable: false),
                    Bus_Intro = table.Column<string>(nullable: false),
                    Bus_Is_Freeze = table.Column<string>(nullable: false),
                    Bus_PV = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.BID);
                });

            migrationBuilder.CreateTable(
                name: "Business_Album",
                columns: table => new
                {
                    Album_ID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Album_Type = table.Column<string>(nullable: false),
                    Album_Describe = table.Column<string>(nullable: false),
                    Album_Img = table.Column<string>(nullable: false),
                    Link_BID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Album", x => x.Album_ID);
                });

            migrationBuilder.CreateTable(
                name: "Business_Certificate",
                columns: table => new
                {
                    Cert_ID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Cert_Type = table.Column<string>(nullable: false),
                    Cert_Img = table.Column<string>(nullable: false),
                    Link_BID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Certificate", x => x.Cert_ID);
                });

            migrationBuilder.CreateTable(
                name: "Business_Coupon",
                columns: table => new
                {
                    Coupon_ID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Coupon_Img = table.Column<string>(nullable: false),
                    Coupon_NO = table.Column<string>(nullable: false),
                    Coupon_Name = table.Column<string>(nullable: false),
                    Coupon_Quantity = table.Column<int>(nullable: false),
                    Expiry_DT = table.Column<DateTime>(nullable: false),
                    Coupon_Province = table.Column<string>(nullable: false),
                    Coupon_City = table.Column<string>(nullable: false),
                    Coupon_Status = table.Column<string>(nullable: false),
                    Coupon_Type = table.Column<string>(nullable: false),
                    Coupon_Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Coupon_Remark = table.Column<string>(nullable: false),
                    Link_BID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Coupon", x => x.Coupon_ID);
                });

            migrationBuilder.CreateTable(
                name: "Business_Coupon_Member",
                columns: table => new
                {
                    Cou_Mem_ID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Cancel_DT = table.Column<string>(nullable: false),
                    Link_AMID = table.Column<Guid>(nullable: false),
                    Link_AMID_Name = table.Column<string>(nullable: false),
                    Link_Coupon_ID = table.Column<Guid>(nullable: false),
                    Link_Coupon_ID_Name = table.Column<string>(nullable: false),
                    Link_BID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Coupon_Member", x => x.Cou_Mem_ID);
                });

            migrationBuilder.CreateTable(
                name: "Business_Dues",
                columns: table => new
                {
                    Dues_ID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Dues_Start_DT = table.Column<DateTime>(nullable: false),
                    Dues_End_DT = table.Column<DateTime>(nullable: false),
                    Dues = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Dues_Remark = table.Column<string>(nullable: false),
                    Link_BID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Dues", x => x.Dues_ID);
                });

            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    FID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Follow_AMID = table.Column<Guid>(nullable: false),
                    Follow_MEID = table.Column<Guid>(nullable: false),
                    Follow_MSID = table.Column<Guid>(nullable: false),
                    Fans_AMID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follow", x => x.FID);
                });

            migrationBuilder.CreateTable(
                name: "Information",
                columns: table => new
                {
                    IMID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Classify = table.Column<string>(nullable: false),
                    Cover_Img = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Publisher = table.Column<string>(nullable: false),
                    Infor_Status = table.Column<string>(nullable: false),
                    Source = table.Column<string>(nullable: false),
                    Infor_PV = table.Column<int>(nullable: false),
                    Link_AUID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Information", x => x.IMID);
                });

            migrationBuilder.CreateTable(
                name: "Mini_Service",
                columns: table => new
                {
                    MSID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Cover_Img = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Main_Business = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    Linkman = table.Column<string>(nullable: false),
                    Contact = table.Column<string>(nullable: false),
                    Intro = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mini_Service", x => x.MSID);
                });

            migrationBuilder.CreateTable(
                name: "User_Plat",
                columns: table => new
                {
                    UID = table.Column<Guid>(nullable: false),
                    Create_DT = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Name_Full = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Tel = table.Column<string>(nullable: false),
                    Is_Admin = table.Column<string>(nullable: false),
                    Is_Frozen = table.Column<string>(nullable: false),
                    Roles_Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Plat", x => x.UID);
                });

            migrationBuilder.CreateTable(
                name: "User_Plat_Login_Record",
                columns: table => new
                {
                    UPLR_ID = table.Column<Guid>(nullable: false),
                    Link_UID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    Name_Full = table.Column<string>(maxLength: 256, nullable: true),
                    Password = table.Column<string>(maxLength: 256, nullable: true),
                    Last_Login_IP = table.Column<string>(maxLength: 256, nullable: true),
                    Last_Login_DT = table.Column<DateTime>(nullable: false),
                    Client_Info = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Plat_Login_Record", x => x.UPLR_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "Activity_Apply");

            migrationBuilder.DropTable(
                name: "Alumni_Member");

            migrationBuilder.DropTable(
                name: "Alumni_Member_Enterprise");

            migrationBuilder.DropTable(
                name: "Alumni_Union");

            migrationBuilder.DropTable(
                name: "Alumni_Union_Member_Link");

            migrationBuilder.DropTable(
                name: "Alumni_Union_User");

            migrationBuilder.DropTable(
                name: "Alumni_Union_User_Login_Record");

            migrationBuilder.DropTable(
                name: "Business");

            migrationBuilder.DropTable(
                name: "Business_Album");

            migrationBuilder.DropTable(
                name: "Business_Certificate");

            migrationBuilder.DropTable(
                name: "Business_Coupon");

            migrationBuilder.DropTable(
                name: "Business_Coupon_Member");

            migrationBuilder.DropTable(
                name: "Business_Dues");

            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.DropTable(
                name: "Information");

            migrationBuilder.DropTable(
                name: "Mini_Service");

            migrationBuilder.DropTable(
                name: "User_Plat");

            migrationBuilder.DropTable(
                name: "User_Plat_Login_Record");
        }
    }
}
