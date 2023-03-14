using System;
namespace System
{
    public class MyGUID
    {
        public static Guid NewGuid()
        {
            string TimeStr = DateTime.Now.ToString("yyyyMMdd-HHmm");
            Guid New_G = new Guid(TimeStr + Guid.NewGuid().ToString().Substring(13, 23));
            return New_G;
        }
    }
}
