using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using League.Api;

namespace League.Alu
{
    /// <summary>
    /// 应用功能树形结构
    /// </summary>
    public static class FunTree
    {
        public static List<Top_Menu> Get_Top_Menu_List(Alumni_Union_User U, string MOD_Name, string Left_Menu_Name)
        {
            MOD MOD_Item = Get_Tree_List(U).MOD_List.Where(x => x.Name == MOD_Name).FirstOrDefault();
            MOD_Item ??= new MOD();

            Left_Menu Menu = MOD_Item.Left_Menu_List.Where(x => x.Name == Left_Menu_Name).FirstOrDefault();
            Menu ??= new Left_Menu();

            List<Top_Menu> Top_Menu_List = Menu.Top_Menu_List.Where(x => x.Active == 1).ToList();
            return Top_Menu_List;
        }

        public static Function_Tree Get_Tree_List(Alumni_Union_User U)
        {
            Function_Tree Item = Get_List_XML();
            List<Top_Menu> Top_Menu_List = new List<Top_Menu>();
            foreach (var x in Item.MOD_List)
            {
                foreach (var xx in x.Left_Menu_List)
                {
                    foreach (var xxx in xx.Top_Menu_List)
                    {
                        xxx.Active = 1;
                    }
                    xx.Active = xx.Top_Menu_List.Where(c => c.Active == 1).Any() ? 1 : 0;
                    if (xx.Active == 1)
                    {
                        xx.URL = xx.Top_Menu_List.Where(c => c.Active == 1).FirstOrDefault().URL;
                    }
                }
                x.Active = x.Left_Menu_List.Where(c => c.Active == 1).Any() ? 1 : 0;
            }
            return Item;
        }

        private static Function_Tree Get_List_XML()
        {
            string XMLPath = Web_Root.wwwroot + "XML_Config\\Function_Tree.xml";
            XDocument XMLDOC = XDocument.Load(XMLPath);
            var query = XMLDOC.Descendants("Function_Tree").ToList();

            List<MOD> MOD_List = new List<MOD>();
            MOD MOD_Item;

            Left_Menu Left_Menu_Item;
            foreach (var x in query)
            {
                MOD_Item = new MOD
                {
                    Name = x.Attribute("MOD_Name").Value,
                    Icon = x.Attribute("Icon").Value,
                    Left_Menu_List = new List<Left_Menu>()
                };
                foreach (var xx in x.Descendants("Left_Menu").ToList())
                {
                    Left_Menu_Item = new Left_Menu { Name = xx.Attribute("Name").Value };
                    Left_Menu_Item.Top_Menu_List = new List<Top_Menu>();
                    foreach (var xxx in xx.Descendants("Top_Menu").ToList())
                    {
                        Left_Menu_Item.Top_Menu_List.Add(new Top_Menu
                        {
                            Name = xxx.Attribute("Name").Value,
                            URL = xxx.Attribute("URL").Value,
                            Role_Titel = xxx.Attribute("Role_Titel").Value,
                            Role_Titel_List = Common_Lib.Convert_ToString_List(xxx.Attribute("Role_Titel").Value)
                        });
                    }
                    Left_Menu_Item.URL = Left_Menu_Item.Top_Menu_List.FirstOrDefault().URL;
                    MOD_Item.Left_Menu_List.Add(Left_Menu_Item);
                }

                MOD_Item.URL = MOD_Item.Left_Menu_List.FirstOrDefault().URL;
                MOD_List.Add(MOD_Item);
            }

            Function_Tree Item = new Function_Tree
            {
                MOD_List = MOD_List
            };
            return Item;
        }
    }

    public class Function_Tree
    {
        public Function_Tree()
        {
            MOD_List = new List<MOD>();
        }
        public List<MOD> MOD_List { get; set; }
    }

    public class MOD
    {
        public MOD()
        {
            Name = string.Empty;
            URL = string.Empty;
            Icon = string.Empty;
            Active = 0;
            Left_Menu_List = new List<Left_Menu>();
        }

        public string Name { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }
        public int Active { get; set; }
        public List<Left_Menu> Left_Menu_List { get; set; }
    }

    public class Left_Menu
    {
        public Left_Menu()
        {
            Name = string.Empty;
            URL = string.Empty;
            Active = 0;
            Top_Menu_List = new List<Top_Menu>();
        }

        public string Name { get; set; }
        public string URL { get; set; }
        public int Active { get; set; }
        public List<Top_Menu> Top_Menu_List { get; set; }
    }

    public class Top_Menu
    {
        public Top_Menu()
        {
            Name = string.Empty;
            URL = string.Empty;
            Role_Titel = string.Empty;
            Role_Titel_List = new List<string>();
            Active = 0;
        }

        public string Name { get; set; }
        public string URL { get; set; }
        public string Role_Titel { get; set; }
        public List<string> Role_Titel_List { get; set; }
        public int Active { get; set; }
    }
}
