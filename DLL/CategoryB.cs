using System;
using System.Collections.Generic;
using System.Text;

namespace DLL
{
    public class CategoryB
    {
        int _id;
        string _name;

        public CategoryB()
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

        /// <summary>
        /// category_b --> category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Category ToCategory(int id)
        {
            int cid = 10;
            if (id == 101 || id == 108 || id == 115 || id == 116 || id == 117 || id == 118 || id == 119 || id == 121) cid = 12;
            else if (id == 102 || id == 106 || id == 123) cid = 21;
            else if (id == 103 || id == 110 || id == 111) cid = 15;
            else if (id == 104 || id == 109) cid = 18;
            else if (id == 105) cid = 11;
            else if (id == 107) cid = 21;
            else if (id == 112 || id == 113) cid = 16;
            else if (id == 114) cid = 17;
            else if (id == 120) cid = 13;
            else if (id == 122) cid = 18;
            else if (id == 124) cid = 10;

            Category cat = new Category();
            cat.ID = cid;
            if (cid > 10) cat.Name = Category.AllCategory[cid];
            else cat.Name = "所有类别";

            return cat;
        }

        public static Dictionary<int, string> AllCategory
        {
            get
            {
                Dictionary<int, string> cats = new Dictionary<int, string>();
                cats[101] = "生活";
                cats[102] = "娱乐";
                cats[103] = "职场";
                cats[104] = "文化";
                cats[105] = "IT";
                cats[106] = "体育";
                cats[107] = "情感";
                cats[108] = "女人";
                cats[109] = "八卦";
                cats[110] = "财经";
                cats[111] = "股票";
                cats[112] = "校园";
                cats[113] = "教育";
                cats[114] = "旅游";
                cats[115] = "时尚";
                cats[116] = "美食";
                cats[117] = "汽车";
                cats[118] = "房产";
                cats[119] = "家居";
                cats[120] = "育儿";
                cats[121] = "健康";
                cats[122] = "军事";
                cats[123] = "游戏";
                cats[124] = "随笔";
                return cats;
            }
        }
    }
}
