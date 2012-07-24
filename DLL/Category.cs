using System;
using System.Collections.Generic;
using System.Text;

namespace DLL
{
    public class Category
    {
        int _id;
        string _name;

        public Category()
        {
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public static string GetNameById(int id)
        {
            Dictionary<int, string> cats = AllCategory;

            foreach (int key in cats.Keys)
            {
                if (key == id) return cats[key];
            }
            return "";
        }

        public static int GetIdByName(string name)
        {
            Dictionary<int, string> cats = AllCategory;

            foreach (int key in cats.Keys)
            {
                if (cats[key] == name) return key;
            }
            return 0;
        }

        public static Dictionary<int, string> AllCategory
        {
            get
            {
                Dictionary<int, string> cats = new Dictionary<int, string>();
                cats[11] = "电脑/网络";
                cats[12] = "生活/时尚";
                cats[13] = "家庭/婚姻";
                cats[14] = "电子数码";
                cats[15] = "商业/理财";
                cats[16] = "教育/学业";
                cats[17] = "交通/旅游";
                cats[18] = "社会/文化";
                cats[19] = "人文学科";
                cats[20] = "理工学科";
                cats[21] = "休闲/娱乐";
                cats[21] = "忧愁/烦恼";
                return cats;
            }
        }
    }
}
