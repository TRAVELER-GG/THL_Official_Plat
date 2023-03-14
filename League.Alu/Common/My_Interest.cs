using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace League.Alu
{
    public static class My_Interest
    {
        static public List<Interest> Get_Interest_By_XML()
        {
            string XMLPath = Web_Root.wwwroot + "XML_Config\\Interest_Label.xml";
            XDocument XMLDOC = XDocument.Load(XMLPath);
            var Query_Result = (from c in XMLDOC.Descendants() where c.Name == "Label" select c).ToList();

            List<Interest> IL = new List<Interest>();
            Interest I = new Interest();
            Label_Sub C = new Label_Sub();
            foreach (var x in Query_Result)
            {
                I = new Interest
                {
                    Label = x.Attribute("Name").Value,
                    Label_Sub_List = new List<Label_Sub>()
                };
                foreach (var y in (from c in x.Descendants() where c.Name == "Label_Sub" select c).ToList())
                {
                    C = new Label_Sub
                    {
                        Label_Sub_Name = y.Attribute("Name").Value
                    };
                    I.Label_Sub_List.Add(C);
                }
                IL.Add(I);
            }
            return IL;
        }
    }

    public class Interest
    {
        public Interest()
        {
            Label = string.Empty;
            Label_Sub_List = new List<Label_Sub>();
        }
        public string Label { get; set; }
        public List<Label_Sub> Label_Sub_List { get; set; }
    }

    public class Label_Sub
    {
        public Label_Sub()
        {
            Label_Sub_Name = string.Empty;
        }
        public string Label_Sub_Name { get; set; }
    }
}
