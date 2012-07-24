using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DLL
{
    public class Data
    {
        /// <summary>
        /// 查询一定条件的博客
        /// </summary>
        /// <param name="orderfield">排序字段</param>
        /// <param name="pageIndex">页码，从0开始</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="where">条件（1=1）</param>
        /// <param name="desc">排序规则（true==desc）</param>
        /// <returns></returns>
        public static DataTable Blogs(string orderfield, int pageIndex, int pageSize, string where, bool desc)
        {
            return GetPagingData("[blog]", "*", orderfield, "[_id]", pageIndex, pageSize, where, desc);
        }

        public static DataTable FeedBack(int pageIndex, int pageSize, string where)
        {
            string fieldlist = "[blog_comment].[_id] as [_cid],[blog_id],[title],[filepath],[blog_comment].[user_name],[fullname],[url],[content],[ip],[blog_comment].[uptime]";

            return GetPagingData("[blog_comment],[blog]", fieldlist, "[_cid]", "[blog_comment].[_id]", pageIndex, pageSize, where, true);
        }

        public static DataTable GetPagingData(string tablename, string fieldlist, string orderfield, string keyfield, int pageIndex, int pageSize, string where, bool desc)
        {
            SqlParameter[] pars = new SqlParameter[8];
            pars[0] = new SqlParameter("@tablename", tablename);
            pars[1] = new SqlParameter("@fieldlist", fieldlist);
            pars[2] = new SqlParameter("@orderfield", orderfield);
            pars[3] = new SqlParameter("@keyfield", keyfield);
            pars[4] = new SqlParameter("@pageindex", pageIndex);
            pars[5] = new SqlParameter("@pagesize", pageSize);
            pars[6] = new SqlParameter("@strwhere", where);
            pars[7] = new SqlParameter("@ordertype", desc ? "1" : "0");

            return DB.TableFromProcedure("GetPagingData", pars);
        }

        public static DataTable GetHotGroup(int size)
        {
            return GetHotGroup(size, 0);
        }

        /// <summary>
        /// 获取一些随机人气群组
        /// </summary>
        /// <param name="size">要获取的群组数</param>
        /// <param name="cat">所属类别</param>
        /// <returns></returns>
        public static DataTable GetHotGroup(int size, int cat)
        {
            if (cat < 11) cat = 0;

            int max = size * 2;
            max = new Random().Next(size, max);

            SqlParameter[] pars = new SqlParameter[3];
            pars[0] = new SqlParameter("@size", size);
            pars[1] = new SqlParameter("@max", max);
            pars[2] = new SqlParameter("@cat", cat);

            return DB.TableFromProcedure("getHotGroup", pars);
        }
        /// <summary>
        /// 获取热门标签
        /// </summary>
        /// <returns></returns>
        public static string GetHotTags()
        {
            DataTable dt = null;

            System.Web.HttpContext http = System.Web.HttpContext.Current;

            if (http.Cache["dt_sta"] != null)
            {
                dt = (DataTable)http.Cache["dt_sta"];
            }
            try { object o = dt.Rows[0]["t_name"]; }
            catch
            {
                string sql = "select top 50 [t_name],[t_cnt] from [blog_tags] order by [t_cnt] desc";
                dt = DB.GetTable(sql);
                http.Cache.Insert("dt_hottag", dt, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Select("", "t_name asc"))
            {
                sb.AppendFormat("<a href='/tag/{1}{3}' style='font-size:{2}px'>{0}</a> ", row["t_name"], row["t_name"].ToString().Replace(" ", "+"), Math.Min(((int)row["t_cnt"] + 9), 28), Settings.Ext);
            }
            return sb.ToString();
        }
    }
}