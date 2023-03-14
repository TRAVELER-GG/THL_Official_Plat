using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Linq;
using League.Api;

namespace League.Plat
{
    public partial class My_Upload
    {
        public static readonly string wwwroot = Web_Root.wwwroot;
        public static readonly string wwwroot_Upload = Web_Root.wwwroot + Web_Root.Upload_File + "\\";
        public static readonly string wwwroot_Temp_PDF = wwwroot_Upload + "Temp_PDF" + "\\";
        public static readonly string wwwroot_Temp_Excel = wwwroot_Upload + "Temp_Excel" + "\\";
        public static void Directory_Check(string Path)
        {
            if (!Directory.Exists(Path)) { Directory.CreateDirectory(Path); }
        }

        public static string File(IFormFile File_Input, string Sub_DIR, Guid Item_GUID)
        {
            string Directory_Path = wwwroot_Upload + Sub_DIR + "\\";
            Directory_Check(Directory_Path);

            var File_Name_Path = Directory_Path + Item_GUID.ToString() + "_" + File_Input.FileName;
            if (System.IO.File.Exists(File_Name_Path)) { System.IO.File.Delete(File_Name_Path); }

            var File_Stream = new FileStream(File_Name_Path, FileMode.CreateNew);
            File_Input.CopyTo(File_Stream);
            File_Stream.Flush();
            File_Stream.Dispose();

            string Virtual_Path = File_Name_Path.Replace(wwwroot, "");
            Virtual_Path = Virtual_Path.Replace(@"\", @"/");
            return Virtual_Path;
        }

        public static string Image(IFormFile File_Input, string Sub_DIR, Guid Item_GUID)
        {
            Check_Content_Type(File_Input, My_Mime_Type.Image.ToString());

            string Directory_Path = wwwroot_Upload + Sub_DIR + "\\";
            Directory_Check(Directory_Path);

            var File_Name_Path = Directory_Path + Item_GUID.ToString() + "_" + File_Input.FileName;
            if (System.IO.File.Exists(File_Name_Path)) { System.IO.File.Delete(File_Name_Path); }

            var File_Stream = new FileStream(File_Name_Path, FileMode.CreateNew);
            File_Input.CopyTo(File_Stream);
            File_Stream.Flush();
            File_Stream.Dispose();

            string Virtual_Path = File_Name_Path.Replace(wwwroot, "");
            Virtual_Path = Virtual_Path.Replace(@"\", @"/");
            return Virtual_Path;
        }
        public static Mat_Img Goods_Img(IFormFile File_Input, Guid Item_ID)
        {
            Check_Content_Type(File_Input, My_Mime_Type.Image.ToString());
            string Upload_Sub_DIR = My_Upload_Sub_DIR.Goods_Img.ToString();
            string Directory_Path = wwwroot_Upload + Upload_Sub_DIR + "\\";
            Directory_Check(Directory_Path);

            Guid Flag_ID = MyGUID.NewGuid();
            var File_Name_Path = Directory_Path + Item_ID.ToString() + "_" + Flag_ID.ToString() + ".png";
            if (System.IO.File.Exists(File_Name_Path)) { System.IO.File.Delete(File_Name_Path); }

            var File_Stream = new FileStream(File_Name_Path, FileMode.CreateNew);
            File_Input.CopyTo(File_Stream);
            File_Stream.Flush();
            File_Stream.Dispose();

            Mat_Img IMG = new Mat_Img { Img_Path_Source_Physical = File_Name_Path };
            IMG.Img_Path_Source = IMG.Img_Path_Source_Physical.Replace(wwwroot, "");
            IMG.Img_Path_Source = IMG.Img_Path_Source.Replace(@"\", @"/");

            //生成缩略图片
            Goods_Img_Thumb(IMG, Item_ID, Flag_ID, Upload_Sub_DIR);
            return IMG;
        }
        private static void Goods_Img_Thumb(Mat_Img IMG, Guid Item_ID, Guid Flag_ID, string Upload_Sub_DIR)
        {
            FileStream Img_Source = System.IO.File.Open(IMG.Img_Path_Source_Physical, FileMode.Open, FileAccess.Read, FileShare.None);
            System.Drawing.Image Image_Item = System.Drawing.Image.FromStream(Img_Source, true);

            //原始图像尺寸
            double Source_Width = Image_Item.Width;
            double Source_Height = Image_Item.Height;

            double Target_Width = (double)400;
            double Ttarget_Height = (double)400;

            //宽大于高或宽等于高（横图或正方）
            if (Image_Item.Width > Image_Item.Height || Image_Item.Width == Image_Item.Height)
            {
                //如果宽大于模版
                if (Image_Item.Width > Target_Width)
                {
                    //宽按模版，高按比例缩放
                    Source_Width = Target_Width;
                    Source_Height = Image_Item.Height * (Target_Width / Image_Item.Width);
                }
            }
            //高大于宽（竖图）
            else
            {
                //如果高大于模版
                if (Image_Item.Height > Ttarget_Height)
                {
                    //高按模版，宽按比例缩放
                    Source_Height = Ttarget_Height;
                    Source_Width = Image_Item.Width * (Ttarget_Height / Image_Item.Height);
                }
            }

            //新建一个bmp图片
            System.Drawing.Image Image_Thumb = new System.Drawing.Bitmap((int)Source_Width, (int)Source_Height);

            //新建一个画板
            System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(Image_Thumb);

            //设置质量
            newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //置背景色
            newG.Clear(System.Drawing.Color.White);
            //画图
            newG.DrawImage(Image_Item, new System.Drawing.Rectangle(0, 0, Image_Thumb.Width, Image_Thumb.Height), new System.Drawing.Rectangle(0, 0, Image_Item.Width, Image_Item.Height), System.Drawing.GraphicsUnit.Pixel);

            IMG.Img_Path_Physical = wwwroot_Upload + Upload_Sub_DIR + "\\" + Item_ID.ToString() + "_" + Flag_ID.ToString() + "_Thumb.png";
            Image_Thumb.Save(IMG.Img_Path_Physical, System.Drawing.Imaging.ImageFormat.Jpeg);
            Img_Source.Dispose();
            Image_Thumb.Dispose();

            IMG.Img_Path = IMG.Img_Path_Physical.Replace(wwwroot, "");
            IMG.Img_Path = IMG.Img_Path.Replace(@"\", @"/");
        }

        public static string Image_Thumb(IFormFile File_Input, string Sub_DIR, Guid Item_GUID)
        {
            Check_Content_Type(File_Input, My_Mime_Type.Image.ToString());

            //上传原始图片
            string Directory_Path = wwwroot_Upload + Sub_DIR + "\\";
            Directory_Check(Directory_Path);

            var File_Name_Path = Directory_Path + Item_GUID.ToString() + "_" + File_Input.FileName;
            if (System.IO.File.Exists(File_Name_Path)) { System.IO.File.Delete(File_Name_Path); }

            var File_Stream = new FileStream(File_Name_Path, FileMode.CreateNew);
            File_Input.CopyTo(File_Stream);
            File_Stream.Flush();
            File_Stream.Dispose();


            //读取原始图片
            FileStream Img_Source = System.IO.File.Open(File_Name_Path, FileMode.Open, FileAccess.Read, FileShare.None);
            System.Drawing.Image Image_Item = System.Drawing.Image.FromStream(Img_Source, true);


            //原始图像尺寸
            double Source_Width = Image_Item.Width;
            double Source_Height = Image_Item.Height;

            double Target_Width = (double)800;
            double Ttarget_Height = (double)800;

            //宽大于高或宽等于高（横图或正方）
            if (Image_Item.Width > Image_Item.Height || Image_Item.Width == Image_Item.Height)
            {
                //如果宽大于模版
                if (Image_Item.Width > Target_Width)
                {
                    //宽按模版，高按比例缩放
                    Source_Width = Target_Width;
                    Source_Height = Image_Item.Height * (Target_Width / Image_Item.Width);
                }
            }
            //高大于宽（竖图）
            else
            {
                //如果高大于模版
                if (Image_Item.Height > Ttarget_Height)
                {
                    //高按模版，宽按比例缩放
                    Source_Height = Ttarget_Height;
                    Source_Width = Image_Item.Width * (Ttarget_Height / Image_Item.Height);
                }
            }

            //新建一个bmp图片
            System.Drawing.Image Image_Thumb = new System.Drawing.Bitmap((int)Source_Width, (int)Source_Height);

            //新建一个画板
            System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(Image_Thumb);

            //设置质量
            newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //置背景色
            newG.Clear(System.Drawing.Color.White);
            //画图
            newG.DrawImage(Image_Item, new System.Drawing.Rectangle(0, 0, Image_Thumb.Width, Image_Thumb.Height), new System.Drawing.Rectangle(0, 0, Image_Item.Width, Image_Item.Height), System.Drawing.GraphicsUnit.Pixel);

            string Thumb_Path_Physical = Directory_Path + Item_GUID.ToString() + "_" + MyGUID.NewGuid() + "_Thumb.png";
            Image_Thumb.Save(Thumb_Path_Physical, System.Drawing.Imaging.ImageFormat.Jpeg);
            Img_Source.Dispose();
            Image_Thumb.Dispose();

            //删除原图
            if (System.IO.File.Exists(File_Name_Path))
            {
                //如果存在则删除
                System.IO.File.Delete(File_Name_Path);
            }

            string Virtual_Path = Thumb_Path_Physical.Replace(wwwroot, "");
            Virtual_Path = Virtual_Path.Replace(@"\", @"/");
            return Virtual_Path;
        }


        private static void Check_Content_Type(IFormFile File, string Check_Type)
        {
            if (File.Length <= 0) { throw new Exception("Did not choose to upload file"); }
            string _ContentType = File.ContentType;
            if (Check_Type == My_Mime_Type.Image.ToString())
            {
                if (Mime_Map_Img().Where(x => x.Mime_Type == _ContentType).Any() == false)
                {
                    throw new Exception("File type is not valid - " + _ContentType);
                }
            }
            else if (Check_Type == My_Mime_Type.Excel.ToString())
            {
                if (Mime_Map_Excel().Where(x => x.Mime_Type == _ContentType).Any() == false)
                {
                    throw new Exception("File type is not valid - " + _ContentType);
                }
            }
        }

        private static List<Mime_Map> Mime_Map_Img()
        {
            List<Mime_Map> List = new List<Mime_Map>
            {
                new Mime_Map { File_Extension = ".bmp", Mime_Type = "image/bmp" },
                new Mime_Map { File_Extension = ".jpe", Mime_Type = "image/jpeg" },
                new Mime_Map { File_Extension = ".jpeg", Mime_Type = "image/jpeg" },
                new Mime_Map { File_Extension = ".jpg", Mime_Type = "image/jpeg" },
                new Mime_Map { File_Extension = ".jpg", Mime_Type = "image/jpg" },
                new Mime_Map { File_Extension = ".png", Mime_Type = "image/png" },
                new Mime_Map { File_Extension = ".pnz", Mime_Type = "image/png" },

                //用于IE兼容的 MIME图像格式
                new Mime_Map { File_Extension = ".png", Mime_Type = "image/x-png" },
                new Mime_Map { File_Extension = ".jpg", Mime_Type = "image/pjpeg" },
                new Mime_Map { File_Extension = ".jpeg", Mime_Type = "image/pjpeg" }
            };

            return List;
        }

        private static List<Mime_Map> Mime_Map_Excel()
        {
            List<Mime_Map> List = new List<Mime_Map>
            {
                new Mime_Map { File_Extension = ".xls", Mime_Type = "application/vnd.ms-excel" },
                new Mime_Map { File_Extension = ".xlsx", Mime_Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
            };
            return List;
        }

        private static List<Mime_Map> Mime_Map_File()
        {
            List<Mime_Map> List = new List<Mime_Map>();
            List.AddRange(Mime_Map_Img());
            return List;
        }

        public static void Delete_File(string Virtual_Path)
        {
            string Physical_Path = wwwroot + Virtual_Path;
            Physical_Path = Physical_Path.Replace(@"/", @"\");
            if (System.IO.File.Exists(Physical_Path))
            {
                //如果存在则删除
                System.IO.File.Delete(Physical_Path);
            }
        }
    }

    public enum My_Upload_Sub_DIR
    {
        Goods_Img,
        Business_Certificate,
        Business_Album,
    }

    public enum My_Mime_Type
    {
        Image,
        Excel,
        PDF,
        Nonmal,
    }

    public class Mime_Map
    {
        public Mime_Map()
        {
            File_Extension = string.Empty;
            Mime_Type = string.Empty;
        }
        public string File_Extension { get; set; }
        public string Mime_Type { get; set; }
    }

    public class Mat_Img
    {
        public Mat_Img()
        {
            Img_Path_Source = string.Empty;
            Img_Path_Source_Physical = string.Empty;
            Img_Path = string.Empty;
            Img_Path_Physical = string.Empty;
        }
        public string Img_Path_Source { get; set; }
        public string Img_Path_Source_Physical { get; set; }
        public string Img_Path { get; set; }
        public string Img_Path_Physical { get; set; }
    }
}
