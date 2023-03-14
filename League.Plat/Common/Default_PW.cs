using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace League.Plat
{
    public class Default_PW
    {
        public static string DEF_PW()
        {
            string XMLPath = Web_Root.wwwroot.ToString() + "XML_Config\\Default_PW.xml";
            XDocument XMLDOC = XDocument.Load(XMLPath);
            var query = XMLDOC.Descendants("Default_Password").ToList();
            string URL_Path = string.Empty;
            foreach (var x in query)
            {
                URL_Path = x.Attribute("VAL").Value;
                break;
            }
            return URL_Path;
        }
    }
}
