using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace League.Alu
{
    public class Host_Domain
    {
        public static string Host()
        {
            string XMLPath = Web_Root.wwwroot.ToString() + "XML_Config\\Host_Domain.xml";
            XDocument XMLDOC = XDocument.Load(XMLPath);
            var query = XMLDOC.Descendants("API_URL").ToList();
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
