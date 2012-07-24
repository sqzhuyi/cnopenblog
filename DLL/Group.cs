using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DLL
{
    /// <summary>
    /// db.group
    /// </summary>
    public class Group
    {
        private int _id;
        private string _name;
        private string _creator;
        private Category _category;
        private string _description;
        private string _gonggao;
        private string _tags;
        private DateTime _uptime;
        private int _redu;

        public Group(int ID)
        {
            _id = ID;

            LoadData();
        }

        private void LoadData()
        {
            SqlParameter par = new SqlParameter("@groupID", _id);

            DataTable dt = DB.TableFromProcedure("getGroup", new SqlParameter[1] { par });

            if (dt == null || dt.Rows.Count == 0)
            {
                _id = 0;
                return;
            }
            DataRow row = dt.Rows[0];

            _name = row["g_name"].ToString();
            _creator = row["g_user_name"].ToString();
            _category = new Category();
            _category.ID = (int)row["_id"];
            _category.Name = row["_name"].ToString();
            _description = row["g_description"].ToString();
            _gonggao = row["g_gonggao"].ToString();
            _tags = row["g_tags"].ToString();
            _uptime = (DateTime)row["g_uptime"];

            if (!int.TryParse(row["g_redu"].ToString(), out _redu))
                _redu = 0;
        }

        public bool Exist
        {
            get { return _id > 0; }
        }

        public int ID
        {
            get { return _id; }
        }
        public string Name
        {
            get { return _name; }
        }
        public string Creator
        {
            get { return _creator; }
        }
        public Category gCategory
        {
            get { return _category; }
        }
        public string Description
        {
            get { return _description; }
        }
        public string Gonggao
        {
            get { return _gonggao; }
        }
        public string Tags
        {
            get { return _tags; }
        }
        public DateTime Uptime
        {
            get { return _uptime; }
        }
        public int Redu
        {
            get { return _redu; }
        }
    }

}
