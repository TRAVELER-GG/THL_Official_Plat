using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace League.Alu
{
    public static class My_Regional
    {
        static public List<Regional> Get_Regional_By_XML()
        {
            string XMLPath = Web_Root.wwwroot + "XML_Config\\Regional.xml";
            XDocument XMLDOC = XDocument.Load(XMLPath);
            var Query_Result = (from c in XMLDOC.Descendants() where c.Name == "Province" select c).ToList();

            List<Regional> RL = new List<Regional>();
            Regional R = new Regional();
            City C = new City();
            foreach (var x in Query_Result)
            {
                R = new Regional
                {
                    Province = x.Attribute("Name").Value,
                    City_List = new List<City>()
                };
                foreach (var y in (from c in x.Descendants() where c.Name == "City" select c).ToList())
                {
                    C = new City
                    {
                        City_Name = y.Attribute("Name").Value
                    };
                    R.City_List.Add(C);
                }
                RL.Add(R);
            }
            return RL;
        }
    }

    public class Regional
    {
        public Regional()
        {
            Province = string.Empty;
            City_List = new List<City>();
        }
        public string Province { get; set; }
        public List<City> City_List { get; set; }
    }

    public class City
    {
        public City()
        {
            City_Name = string.Empty;
        }
        public string City_Name { get; set; }
    }
}
