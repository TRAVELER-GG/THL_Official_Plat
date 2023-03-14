using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace League.Api
{
    public class League_DbContext : DbContext
    {
        public static string Connection_Strings { get; set; }
        public static string Migrations_Assembly { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Connection_Strings, b => b.MigrationsAssembly(Migrations_Assembly));
            //SQL 2008R2 支持Skip and Take
            optionsBuilder.UseSqlServer(Connection_Strings, b => b.UseRowNumberForPaging());

            //查询指定为只读查询，减少消耗
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business_Dues>().Property(c => c.Dues).HasColumnType("decimal(18,4)");
            modelBuilder.Entity<Business_Coupon>().Property(c => c.Coupon_Price).HasColumnType("decimal(18,4)");
        }

        //add-migration xxxxxxx
        //update-database
        public DbSet<Alumni_Member> Alumni_Member { get; set; }
        public DbSet<Alumni_Member_Enterprise> Alumni_Member_Enterprise { get; set; }
        public DbSet<Alumni_Union_Member_Link> Alumni_Union_Member_Link { get; set; }
        public DbSet<Alumni_Union> Alumni_Union { get; set; }
        public DbSet<Alumni_Union_User> Alumni_Union_User { get; set; }
        public DbSet<Alumni_Union_User_Login_Record> Alumni_Union_User_Login_Record { get; set; }

        public DbSet<Mini_Service> Mini_Service { get; set; }
        public DbSet<Follow> Follow { get; set; }

        public DbSet<Business> Business { get; set; }
        public DbSet<Business_Dues> Business_Dues { get; set; }
        public DbSet<Business_Certificate> Business_Certificate { get; set; }
        public DbSet<Business_Album> Business_Album { get; set; }
        public DbSet<Business_Coupon> Business_Coupon { get; set; }
        public DbSet<Business_Coupon_Member> Business_Coupon_Member { get; set; }


        public DbSet<Activity> Activity { get; set; }
        public DbSet<Activity_Apply> Activity_Apply { get; set; }
        public DbSet<Information> Information { get; set; }
        public DbSet<User_Plat> User_Plat { get; set; }
        public DbSet<User_Plat_Login_Record> User_Plat_Login_Record { get; set; }
    }
}
