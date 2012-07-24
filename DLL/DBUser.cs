using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DLL
{
    /// <summary>
    /// db.users
    /// </summary>
    public class DBUser
    {
        string _name;
        string _password;
        string _fullname;
        string _sex;
        DateTime _birthday;
        string _email;
        string _url;
        string _hangye;
        string _state;
        string _city;
        string _qq;
        string _msn;
        string _jianjie;
        string _xingqu;
        string _qianming;
        string _blogtitle;
        string _blogsubtitle;
        DateTime _uptime;
        bool _showbirthday;
        bool _showemail;
        bool _showqq;
        bool _showmsn;
        int _view_count;

        public DBUser(string name)
        {
            _name = name;

            Load();
        }

        private void Load()
        {
            string sql = "select * from [users] where [_name]='{0}';";
            sql = String.Format(sql, _name.Replace("'", ""));
            DataTable dt = DB.GetTable(sql);
            if (dt == null || dt.Rows.Count == 0)
            {
                _name = "";
                return;
            }
            DataRow row = dt.Rows[0];
            _name = row["_name"].ToString();
            _password = row["_password"].ToString();
            _fullname = row["fullname"].ToString();
            _sex = row["sex"].ToString();
            try { _birthday = DateTime.Parse(row["birthday"].ToString()); }
            catch { _birthday = DateTime.Parse("1980-1-1"); }
            _email = row["email"].ToString();
            _url = row["url"].ToString();
            _hangye = row["hangye"].ToString();
            _state = row["state"].ToString();
            _city = row["city"].ToString();
            _qq = row["qq"].ToString();
            _msn = row["msn"].ToString();
            _jianjie = row["jianjie"].ToString();
            _xingqu = row["xingqu"].ToString();
            _qianming = row["qianming"].ToString();
            _blogtitle = row["blog_title"].ToString();
            _blogsubtitle = row["blog_subtitle"].ToString();
            try { _uptime = DateTime.Parse(row["uptime"].ToString()); }
            catch { _uptime = DateTime.Now; }
            _showbirthday = row["birthday_ok"].ToString() == "1";
            _showemail = row["email_ok"].ToString() == "1";
            _showqq = row["qq_ok"].ToString() == "1";
            _showmsn = row["msn_ok"].ToString() == "1";
            _view_count = (int)row["view_cnt"];
        }

        public bool Exist
        {
            get { return _name != ""; }
        }

        public string Username
        {
            get { return _name; }
        }
        public string Password
        {
            get { return _password; }
        }
        public string Fullname
        {
            get { return _fullname; }
        }
        public string Sex
        {
            get { return _sex; }
        }
        public DateTime Birthday
        {
            get { return _birthday; }
        }
        public string Email
        {
            get { return _email; }
        }
        public string Url
        {
            get { return _url; }
        }
        public string Hangye
        {
            get { return _hangye; }
        }
        public string State
        {
            get { return _state; }
        }
        public string City
        {
            get { return _city; }
        }
        public string QQ
        {
            get { return _qq; }
        }
        public string MSN
        {
            get { return _msn; }
        }
        public string Jianjie
        {
            get { return _jianjie; }
        }
        public string Xingqu
        {
            get { return _xingqu; }
        }
        public string Qianming
        {
            get { return _qianming; }
        }
        public string BlogTitle
        {
            get { return _blogtitle; }
        }
        public string BlogSubtitle
        {
            get { return _blogsubtitle; }
        }
        public DateTime Regtime
        {
            get { return _uptime; }
        }
        public bool ShowBirthday
        {
            get { return _showbirthday; }
        }
        public bool ShowEmail
        {
            get { return _showemail; }
        }
        public bool ShowQQ
        {
            get { return _showqq; }
        }
        public bool ShowMSN
        {
            get { return _showmsn; }
        }
        public int ViewCount
        {
            get { return _view_count; }
        }
    }
}
