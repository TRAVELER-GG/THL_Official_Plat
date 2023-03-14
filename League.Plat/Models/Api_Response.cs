using System;

namespace League.Plat
{
    public class Api_Response
    {
        public Api_Response()
        {
            Data = new object();
            Error_Code = 0;
            Error_Msg = string.Empty;
        }
       
        public object Data { get; set; }
        public int Error_Code { get; set; }
        public string Error_Msg { get; set; }
    }
}
