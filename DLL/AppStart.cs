using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DLL
{
    public class AppStart
    {
        public static void Setup()
        {
            DataTable dt = DB.GetTable("select [t_name] from [blog_tags] order by [t_name]");
            foreach (DataRow row in dt.Rows)
            {
                Tag.Add(row[0].ToString().ToLower());
            }
        }
    }
}
