<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using DLL;

public class Handler : IHttpHandler, IRequiresSessionState
{
    HttpContext http = null;
    
    public void ProcessRequest(HttpContext context)
    {
        http = context;
        http.Response.ContentType = "text/plain";

        AjaxRequest();
    }

    public bool IsReusable
    {
        get { return false; }
    }
    private void AjaxRequest()
    {
        if (Par("checkuser") != null)
        {
            CheckUser();
            return;
        }
        if (Par("reg") != null)
        {
            Register();
            return;
        }
        if (Par("baseinfo") != null)
        {
            EditBaseInfo();
            return;
        }
        if (Par("chgpwd") != null)
        {
            ChangePassword();
            return;
        }
        if (Par("writeblog") != null)
        {
            CreateBlog();
            return;
        }
        if (Par("editblog") != null)
        {
            EditBlog();
            return;
        }
        if (Par("addcomment") != null)
        {
            AddComment();
            return;
        }
        if (Par("login") != null)
        {
            Login();
            return;
        }
        if (Par("addfavorite") != null)
        {
            AddFavorite();
            return;
        }
        if (Par("addmsg") != null)
        {
            AddMessage();
            return;
        }
        if (Par("replymsg") != null)
        {
            ReplyMessage();
            return;
        }
        if (Par("deletemsg") != null)
        {
            DeleteMessage();
            return;
        }
        if (Par("deletecomment") != null)
        {
            DeleteComment();
            return;
        }
        if (Par("addcat") != null)
        {
            AddFavoriteCategory();
            return;
        }
        if (Par("delcat") != null)
        {
            DeleteFavoriteCategory();
            return;
        }
        if (Par("delfavorite") != null)
        {
            DeleteFavorite();
            return;
        }
        if (Par("creategroup") != null)
        {
            CreateGroup();
            return;
        }
        if (Par("newpost") != null)
        {
            NewPost();
            return;
        }
        if (Par("editpost") != null)
        {
            EditPost();
            return;
        }
        if (Par("deletetopic") != null)
        {
            DeleteTopic();
            return;
        }
        if (Par("replytopic") != null)
        {
            ReplyTopic(); 
            return;
        }
        if (Par("editgroup") != null)
        {
            EditGroup();
            return;
        }
        if (Par("hidetopic") != null)
        {
            HideTopic();
            return;
        }
        if (Par("toptopic") != null)
        {
            TopTopic();
            return;
        }
        if (Par("hidereply") != null)
        {
            HideReply();
            return;
        }
        if (Par("joingroup") != null)
        {
            JoinGroup();
            return;
        }
        if (Par("outgroup") != null)
        {
            OutGroup();
            return;
        }
        if (Par("setoutgroup") != null)
        {//踢出群组
            SetOutGroup();
            return;
        }
        if (Par("deletegroup") != null)
        {
            DeleteGroup();
            return;
        }
        if (Par("applygroup") != null)
        {//申请管理员
            ApplyGroup();
            return;
        }
        if (Par("applyok") != null)
        {
            ApplyOk();
            return;
        }
        if (Par("applyno") != null)
        {
            ApplyNo();
            return;
        }
        if (Par("setgroupadmin") != null)
        {
            SetGroupAdmin();
            return;
        }
        if (Par("addfriend") != null)
        {
            AddFriend();
            return;
        }
        if (Par("deleteblog") != null)
        {
            DeleteBlog();
            return;
        }
        if (Par("deletefriend") != null)
        {
            DeleteFriend();
            return;
        }
        if(Par("replyshortblog")!=null)
        {
            ReplyShortBlog();
            return;
        }
        if (Par("sendshortblog") != null)
        {
            SendShortBlog();
            return;
        }
        if (Par("saveblogtitle") != null)
        {
            SaveBlogTitle();
            return;
        }
        if (Par("savebg") != null)
        {
            SaveBG();
            return;
        }
        if (Par("savecolumn") != null)
        {
            SaveColumn();
            return;
        }
    }
    
    /// <summary>
    /// 获取传输的参数
    /// </summary>
    private string Par(string key)
    {
        return http.Request[key];
    }
    /// <summary>
    /// 输出内容
    /// </summary>
    private void Print(string s)
    {
        http.Response.Write(s);
    }

    private void CheckUser()
    {
        string un = Par("checkuser");
        if (!Tools.CheckUsername(un))
        {
            Print("false");
            return;
        }
        int c = (int)DB.GetValue("select count(*) from [users] where [_name]='" + un + "'");

        if (c == 0) Print("true");
        else Print("false");
    }

    private void Register()
    {
        if (Cookie.GetCookie("reged") != "")
        {
            Print("false");
            return;
        }
        Cookie.SetCookie("reged", "1", DateTime.Now.AddMinutes(10));

        string name = Par("name").ToLower();
        if (!Tools.CheckUsername(name))
        {
            Print("false");
            return;
        }
        DateTime birthday = DateTime.Now;
        DateTime.TryParse(Par("birthday") + "-1", out birthday);
        string sex = Par("sex") == "1" ? "男" : "女";
        string fullname = Tools.FilterName(Par("fullname"));

        Insert.User(name, Par("pwd"), fullname, sex, birthday, Par("email"));

        CKUser.Login(name, fullname, Par("email"), false);

        Print("true");
    }

    private void EditBaseInfo()
    {
        DateTime birthday = DateTime.Now;
        DateTime.TryParse(Par("birthday") + "-1", out birthday);
        string sex = Par("sex") == "1" ? "男" : "女";
        string url = Par("url").Replace("'", "").Trim();
        if (!url.ToLower().StartsWith("http://")) url = "http://" + url;
        if (url == "http://") url = "";

        string fullname = Tools.FilterName(Par("fullname"));
        
        string jianjie = Par("jianjie").Replace("\r", "").Replace("\n", " ").Replace("'", "''");
        jianjie = Regex.Replace(jianjie, @"<.+?>", "").Trim();
        string xingqu = Par("xingqu").Replace("\r", "").Replace("\n", " ").Replace("'", "''");
        xingqu = Regex.Replace(xingqu, @"<.+?>", "").Trim();
        string qianming = Par("qianming").Replace("\r", "").Replace("\n", " ").Replace("'", "''");
        qianming = Regex.Replace(qianming, @"<.+?>", "").Trim();

        string sql = "update [users] set [fullname]='{0}',[sex]='{1}',[birthday]='{2}',[email]='{3}',[url]='{4}',[hangye]='{5}',[state]='{6}',[city]='{7}',[qq]='{8}',[msn]='{9}',[jianjie]='{10}',[xingqu]='{11}',[qianming]='{12}',[blog_title]='{13}',[blog_subtitle]='{14}',[birthday_ok]={15},[email_ok]={16},[qq_ok]={17},[msn_ok]={18} where [_name]='{19}'";
        sql = String.Format(sql, fullname, sex, birthday.ToString("yyyy-MM-dd"), Par("email").Replace("'", "").Trim(), url, Par("hangye").Replace("'", "").Trim(), Par("state").Replace("'", "").Trim(), Par("city").Replace("'", "").Trim(), Par("qq").Replace("'", "").Trim(), Par("msn").Replace("'", "").Trim(), jianjie, xingqu, qianming,
            Par("blogtitle").Replace("'", "''").Trim(), Par("blogsubtitle").Replace("'", "''").Trim(), (Par("showbirthday") == "1" ? "1" : "0"), (Par("showemail") == "1" ? "1" : "0"), (Par("showqq") == "1" ? "1" : "0"), (Par("showmsn") == "1" ? "1" : "0"), CKUser.Username);

        DB.Execute(sql);

        Print(sql);
    }

    private void ChangePassword()
    {
        string sql = "if (select count(*) from [users] where [_name]='{0}' and [_password]='{1}')>0 begin ";
        sql += "update [users] set [_password]='{2}' where [_name]='{0}';select 1 end ";
        sql += "else begin select 0 end";

        sql = String.Format(sql, CKUser.Username, Par("oldpwd").Replace("'", "''"), Par("newpwd").Replace("'", "''"));

        int c = (int)DB.GetValue(sql);

        Print(c.ToString());
    }

    private void CreateBlog()
    {
        string title = Par("_title").Trim();
        int cat = int.Parse(Par("_cat"));
        int subcat = int.Parse(Par("_subcat"));
        string zhaiyao = Par("_zhaiyao").Replace("\r", "").Replace("\n", " ").Trim();
        string body = Tools.ToAnquanHtml(Par("_body"));
        string tag = TagTools.Filter(Par("_tag"));
        int nocomment = int.Parse(Par("_nocomment"));

        string filepath = Insert.CreateBlog(title, cat, subcat, zhaiyao, tag, body, nocomment);

        Print(filepath);
    }

    private void EditBlog()
    {
        int blogID = int.Parse(Par("editblog"));

        string title = Par("_title").Trim();
        int cat = int.Parse(Par("_cat"));
        int subcat = int.Parse(Par("_subcat"));
        string zhaiyao = Par("_zhaiyao").Replace("\r", "").Replace("\n", " ").Trim();
        string body = Tools.ToAnquanHtml(Par("_body"));
        string tag = TagTools.Filter(Par("_tag"));
        int nocomment = int.Parse(Par("_nocomment"));
        
        string filepath = Update.EditBlog(blogID, title, cat, subcat, zhaiyao, tag, body, nocomment);

        Print(filepath);
    }

    private void AddComment()
    {
        if (Cookie.GetCookie("addedcomment") != "") return;
        Cookie.SetCookie("addedcomment", "1", DateTime.Now.AddMinutes(3));
        
        int blogID = int.Parse(Par("blogid"));

        string c_name = Regex.Replace(Par("c_name"), @"[^\w ]", "");
        string c_url = Par("c_url").Split(' ')[0].Replace("'", "");
        int rating = int.Parse(Par("rating"));
        string c_content = Par("c_content").Replace("\r", "").Replace("\n", "{br}");
        string c_ip = http.Request.UserHostAddress;
        string u_name = CKUser.Username;

        Insert.AddComment(blogID, u_name, c_name, c_url, rating, c_content, c_ip);

        Print("null");
    }

    private void Login()
    {
        string uname = Par("uname").Replace("'", "").Trim();
        string pwd = Par("pwd").Trim();

        DBUser user = new DBUser(uname);
        bool ok = false;
        if (user.Username != "")
        {
            ok = pwd.ToLower() == user.Password.ToLower();
        }
        if (ok)
        {
            CKUser.Login(user.Username, user.Fullname, user.Email, Par("remmeber") != null);
            Print("true");
        }
        else
        {
            Print("false");
        }
    }

    private void AddFavorite()
    {
        int blogID = int.Parse(Par("blogid"));
        int cat = int.Parse(Par("cat"));
        string newcat = Par("newcat") != null ? Par("newcat") : "";

        string username = CKUser.Username;
        if (username == "") return;

        Insert.AddFavorite(cat, newcat, blogID, username);
        
        Print("true");
    }

    private void AddMessage()
    {
        if (Cookie.GetCookie("addedmsg") != "") return;
        Cookie.SetCookie("addedmsg", "1", DateTime.Now.AddMinutes(10));
        
        string to = Par("_to").Replace("'", "");
        string from = CKUser.Username != "" ? CKUser.Username : http.Request.UserHostAddress;
        string body = Par("msg").Replace("\r", "").Replace("\n", " ").Replace("'", "''");

        string sql = "insert into [inbox]([_from],[_to],[_body]) values('{0}','{1}','{2}')";
        sql = String.Format(sql, from, to, body);

        DB.Execute(sql);

        Print("null");
    }

    private void ReplyMessage()
    {
        if (!CKUser.IsLogin) return;
        
        int id = int.Parse(Par("replymsg"));
        string msg = Par("msg").Replace("\r", "").Replace("\n", " ").Replace("'", "''");

        string sql = "insert into [inbox]([_fid],[_from],[_to],[_body]) select (case [_fid] when 0 then [_id] else [_fid] end),[_to],[_from],'{1}' from [inbox] where [_id]={0}";
        sql = String.Format(sql, id, msg);
        DB.Execute(sql);
        Par("null");
    }

    private void DeleteMessage()
    {
        if (!CKUser.IsLogin) return;

        int id = int.Parse(Par("deletemsg"));

        string sql = "delete from [inbox] where [_id]={0} or ([_fid]={0} and [_to]='{1}');";
        sql += "update [inbox] set [_fid]=0 where [_fid]={0};";
        sql = String.Format(sql, id, CKUser.Username);
        DB.Execute(sql);
        Print("null");
    }

    private void DeleteComment()
    {
        if (!CKUser.IsLogin) return;
        
        int id = int.Parse(Par("deletecomment"));

        string sql = "delete from [blog_comment] where [_id]=" + id.ToString();
        DB.Execute(sql);
        Print("null");
    }

    private void AddFavoriteCategory()
    {
        if (!CKUser.IsLogin) return;
        string cat = Par("newcat");
        if (cat == null || cat == "") return;

        string sql = "insert into [favorite_category]([user_name],[cat_name]) values('{0}','{1}');select @@identity;";
        sql = String.Format(sql, CKUser.Username, cat.Replace("'", "''"));
        string s = DB.GetValue(sql).ToString();
        Print(s);
    }

    private void DeleteFavoriteCategory()
    {
        if (!CKUser.IsLogin) return;
        int id = int.Parse(Par("delcat"));
        int firstid = int.Parse(Par("firstid"));
        
        string sql = "delete from [favorite_category] where [_id]={0}; update [favorite] set [cat_id]={1} where [cat_id]={0};";
        sql = String.Format(sql, id, firstid);
        DB.Execute(sql);

        Print("null");
    }

    private void DeleteFavorite()
    {
        if (!CKUser.IsLogin) return;
        int id = int.Parse(Par("delfavorite"));

        string sql = "delete from [favorite] where [_id]={0} and [user_name]='{1}'";
        sql = String.Format(sql, id, CKUser.Username);

        DB.Execute(sql);
        Print("null");
    }

    private void CreateGroup()
    {
        if (!CKUser.IsLogin) return;
        string gname = Par("g_name").Trim();
        int catID = int.Parse(Par("catid"));
        string jianjie = Par("jianjie");
        if (jianjie.Length > 500) jianjie = jianjie.Substring(0, 500);
        jianjie = Regex.Replace(jianjie,@"[\s]{2,}", " ").Trim();
        string tags = TagTools.Filter(Par("tags"));

        int groupID = Insert.CreateGroup(gname, CKUser.Username, catID, jianjie, tags);
        Print(groupID.ToString());
    }

    private void NewPost()
    {
        if (!CKUser.IsLogin) return;
        int groupID = int.Parse(Par("group"));
        string title = Par("title");
        if (title.Length > 50) title = title.Substring(0, 50);
        string body = Tools.ToAnquanHtml(Par("body"));
        if (body.Length > 4000) body = body.Substring(0, 4000);

        int topicID = Insert.AddTopic(CKUser.Username, groupID, title, body);

        Print(topicID.ToString());
    }
    private void EditPost()
    {
        if (!CKUser.IsLogin) return;
        int topicID = int.Parse(Par("topic"));
        string title = Par("title");
        if (title.Length > 50) title = title.Substring(0, 50);
        string body = Tools.ToAnquanHtml(Par("body"));
        if (body.Length > 4000) body = body.Substring(0, 4000);

        string sql = "update [topic] set [t_title]='{0}',[t_content]='{1}',[t_last_edit_time]=getdate() where [t_id]={2}";
        sql = String.Format(sql, title.Replace("'", "''"), body.Replace("'", "''"), topicID);
        
        DB.Execute(sql);

        Print(topicID.ToString());
    }

    private void ReplyTopic()
    {
        if (!CKUser.IsLogin) return;
        int topicID = int.Parse(Par("topic"));
        string body = Tools.ToAnquanHtml(Par("body"));
        if (body.Length > 2000) body = body.Substring(0, 2000);

        string location = Insert.AddReply(CKUser.Username, topicID, body);

        Print(location);
    }

    private void EditGroup()
    {
        if (!CKUser.IsLogin) return;
        
        int groupID = int.Parse(Par("editgroup"));
        string name = Par("g_name").Replace("'", "''").Trim();
        int catID = int.Parse(Par("catid"));
        string tags = TagTools.Filter(Par("tags"));
        string jianjie = Par("jianjie").Replace("\r", "").Replace("\n", " ");
        if (jianjie.Length > 200) jianjie = jianjie.Substring(0, 200);
        jianjie = Regex.Replace(jianjie, @"[\s]{2,}", " ").Replace("'", "''").Trim();
        string gonggao = Par("gonggao").Replace("\r", "").Replace("\n", " ");
        if (gonggao.Length > 200) gonggao = gonggao.Substring(0, 200);
        gonggao = Regex.Replace(gonggao, @"[\s]{2,}", " ").Replace("'", "''").Trim();

        string sql = "update [group] set [g_name]='{0}',[g_cat_id]={1},[g_description]='{2}',[g_gonggao]='{3}',[g_tags]='{4}'";
        sql += " where [g_id]=(select [gm_g_id] from [group_member] where [gm_g_id]={5} and [gm_user_name]='{6}' and [gm_class]=1)";
        sql = String.Format(sql, name, catID, jianjie, gonggao, tags, groupID, CKUser.Username);

        DB.Execute(sql);

        Print("null");
    }

    private void DeleteTopic()
    {
        if (!CKUser.IsLogin) return;
        
        int topicID = int.Parse(Par("deletetopic"));

        string sql = "declare @cnt int ";
        sql += "set @cnt=(select count(*) from [topic] where [t_user_name]='{0}' and [t_id]={1}) ";
        sql += "if @cnt=0 begin ";
        sql += "set @cnt=(select count(*) from [group_member] where [gm_class]=1 and [gm_user_name]='{0}' and [gm_g_id]=(select [t_g_id] from [topic] where [t_id]={1})) end ";
        sql += "if @cnt=1 begin ";
        sql += "delete from [topic] where [t_id]={1} end ";

        sql = String.Format(sql, CKUser.Username, topicID);
        
        DB.Execute(sql);

        Print("null");
    }

    private void HideTopic()
    {
        if (!CKUser.IsLogin) return;
        
        int topicID = int.Parse(Par("hidetopic"));

        string sql = "declare @cnt int ";
        sql += "set @cnt=(select count(*) from [group_member] where [gm_class]=1 and [gm_user_name]='{0}' and [gm_g_id]=(select [t_g_id] from [topic] where [t_id]={1})) end ";
        sql += "if @cnt=1 begin ";
        sql += "update [topic] set [t_hide]=([t_hide]+1)%2 where [t_id]={1} end ";

        sql = String.Format(sql, CKUser.Username, topicID);

        DB.Execute(sql);

        Print("null");
    }

    private void TopTopic()
    {
        if (!CKUser.IsLogin) return;

        int topicID = int.Parse(Par("toptopic"));

        string sql = "declare @cnt int;";
        sql += "set @cnt=(select count(*) from [group_member] where [gm_class]=1 and [gm_user_name]='{0}' and [gm_g_id]=(select [t_g_id] from [topic] where [t_id]={1}));";
        sql += "if @cnt=1 begin ";
        sql += "update [topic] set [t_istop]=([t_istop]+1)%2 where [t_id]={1} end ";

        sql = String.Format(sql, CKUser.Username, topicID);

        DB.Execute(sql);

        Print("null");
    }

    private void HideReply()
    {
        if (!CKUser.IsLogin) return;

        int replyID = int.Parse(Par("hidereply"));
        int topicID = int.Parse(Par("topic"));

        string sql = "declare @cnt int ";
        sql += "set @cnt=(select count(*) from [group_member] where [gm_class]=1 and [gm_user_name]='{0}' and [gm_g_id]=(select [t_g_id] from [topic] where [t_id]={1})) end ";
        sql += "if @cnt=1 begin ";
        sql += "update [reply] set [r_hide]=([r_hide]+1)%2 where [r_id]={2} end ";

        sql = String.Format(sql, CKUser.Username, topicID, replyID);

        DB.Execute(sql);

        Print("null");
    }

    private void JoinGroup()
    {
        if (!CKUser.IsLogin) return;
        
        int groupID = int.Parse(Par("joingroup"));

        string sql = "exec joinGroup {0},'{1}'";
        sql = String.Format(sql, groupID, CKUser.Username);

        DB.Execute(sql);

        Print("null");
    }

    private void OutGroup()
    {
        if (!CKUser.IsLogin) return;

        int groupID = int.Parse(Par("outgroup"));

        string sql = "exec outGroup {0},'{1}'";
        sql = String.Format(sql, groupID, CKUser.Username);

        DB.Execute(sql);

        Print("null");
    }
    private void SetOutGroup()
    {
        if (!CKUser.IsLogin) return;

        int groupID = int.Parse(Par("setoutgroup"));
        string uname = Par("un").Replace("'", "");

        string sql = "exec setOutGroup {0},'{1}','{2}'";
        sql = String.Format(sql, groupID, uname, CKUser.Username);

        DB.Execute(sql);

        Print("null");
    }

    private void DeleteGroup()
    {
        if (!CKUser.IsLogin) return;

        int groupID = int.Parse(Par("deletegroup"));

        string sql = "exec deleteGroup {0},'{1}'";
        sql = String.Format(sql, groupID, CKUser.Username);

        DB.Execute(sql);

        string path = Settings.BasePath + @"upload\group\" + groupID.ToString();
        
        IO.DeleteFile(path + ".jpg");
        IO.DeleteFile(path + "-s.jpg");

        Print("null");
    }

    private void ApplyGroup()
    {
        if (!CKUser.IsLogin) return;

        int groupID = int.Parse(Par("applygroup"));

        string sql = "exec applyGroup {0},'{1}'";
        sql = String.Format(sql, groupID, CKUser.Username);

        DB.Execute(sql);
        
        Print("null");
    }

    private void ApplyOk()
    {
        if (!CKUser.IsLogin) return;

        int msgID = int.Parse(Par("applyok"));
        int groupID = int.Parse(Par("gid"));
        string uname = Par("uname").Replace("'", "");

        string sql = "exec applyGroupOK {0},{1},'{2}'";
        sql = String.Format(sql, msgID, groupID, uname);
        DB.Execute(sql);

        Print("null");
    }

    private void ApplyNo()
    {
        if (!CKUser.IsLogin) return;
        
        int msgID = int.Parse(Par("applyno"));
        int groupID = int.Parse(Par("gid"));
        string uname = Par("uname").Replace("'", "");
        
        string sql = "exec applyGroupNO {0},{1},'{2}'";
        sql = String.Format(sql, msgID, groupID, uname);
        DB.Execute(sql);

        Print("null");
    }

    private void SetGroupAdmin()
    {
        if (!CKUser.IsLogin) return;

        int isadmin = int.Parse(Par("setgroupadmin"));
        int groupID = int.Parse(Par("gid"));
        string uname = Par("un").Replace("'", "");

        string sql = "exec setGroupAdmin {0},'{1}','{2}',{3}";
        sql = String.Format(sql, groupID, uname, CKUser.Username, isadmin);
        DB.Execute(sql);

        Print("null");
    }

    private void AddFriend()
    {
        Print("null");
        
        if (!CKUser.IsLogin) return;

        string name = Par("name").Replace("'", "");
        if (CKUser.Username == name.ToLower()) return;
        string sql = "if (select count(*) from [friends] where [f_user_name]='{0}' and [f_friend_name]='{1}')=0 begin ";
        sql += "insert into [friends]([f_user_name],[f_friend_name]) values('{0}','{1}') ";
        sql += "insert into [inbox]([_to],[_body]) values('{1}','{2}') ";
        sql += "end";
        sql = String.Format(sql, CKUser.Username, name, Resources.Note.AddFriend.Replace("@user", "@" + CKUser.Username));

        DB.Execute(sql);
    }

    private void DeleteBlog()
    {
        Print("null");
        
        int blogID = int.Parse(Par("deleteblog"));

        if (!CKUser.IsLogin) return;

        string sql = "select [filepath] from [blog] where [_id]={0} and [user_name]='{1}'";
        sql = String.Format(sql, blogID, CKUser.Username);
        object obj = DB.GetValue(sql);
        if (obj == null || obj.ToString() == "") return;
        
        sql = "delete from [blog] where [_id]={0};";
        sql += "delete from [blog_comment] where [blog_id]={0};";
        sql = String.Format(sql, blogID);
        DB.Execute(sql);

        IO.DeleteFile(Settings.BasePath.TrimEnd('\\') + obj.ToString());
    }

    private void DeleteFriend()
    {
        Print("null");
        
        if (!CKUser.IsLogin) return;

        string fname = Par("deletefriend").Replace("'", "");
        string sql = "delete from [friends] where [f_user_name]='{0}' and [f_friend_name]='{2}'";
        sql = String.Format(sql, CKUser.Username, fname);
        DB.Execute(sql);
    }

    private void ReplyShortBlog()
    {
        Print("null");
        if (!CKUser.IsLogin) return;
        
        int sb_id = int.Parse(Par("replyshortblog"));
        string msg = Par("_msg").Replace("\r", "").Replace("\n", " ");
        if (msg.Length > 240) msg = msg.Substring(0, 240);

        string sql = "insert into [short_blog_reply]([sbr_sb_id],[sbr_user_name],[sbr_content]) values({0},'{1}','{2}');";
        sql += "update [short_blog] set [sb_reply_cnt]=[sb_reply_cnt]+1 where [sb_id]={0};";
        sql = String.Format(sql, sb_id, CKUser.Username, msg);
        DB.Execute(sql);
    }

    private void SendShortBlog()
    {
        Print("null");
        if (!CKUser.IsLogin) return;

        Cookie.SetCookie("sendshortblog", "1", DateTime.Now.AddMinutes(2));

        string msg = Par("_msg").Replace("\r", "").Replace("\n", "");
        if (msg.Length > 240) msg = msg.Substring(0, 240);
        msg = msg.Replace("'", "''");

        string sql = "insert into [short_blog]([sb_user_name],[sb_content]) values('{0}','{1}')";
        sql = String.Format(sql, CKUser.Username, msg);

        DB.Execute(sql);
    }

    private void SaveBlogTitle()
    {
        Print("null");
        if (!CKUser.IsLogin) return;

        string title = Par("blogtitle").Trim();
        if (title.Length > 30) title = title.Substring(0, 30).Trim();
        title = title.Replace("'","''");

        string subtitle = Par("blogsubtitle").Trim();
        if (subtitle.Length > 50) subtitle = subtitle.Substring(0, 50).Trim();
        subtitle = subtitle.Replace("'", "''");

        string sql = "update [users] set [blog_title]='{0}',[blog_subtitle]='{1}' where [_name]='{2}'";
        sql = String.Format(sql, title, subtitle, CKUser.Username);

        DB.Execute(sql);
    }

    private void SaveBG()
    {
        Print("null");
        if (!CKUser.IsLogin) return;

        int bg = int.Parse(Par("savebg"));

        string sql = "update [user_settings] set [us_bg]={0} where [us_user_name]='{1}'";
        sql = String.Format(sql, bg, CKUser.Username);

        DB.Execute(sql);
    }

    private void SaveColumn()
    {
        Print("null");
        if (!CKUser.IsLogin) return;

        string username = CKUser.Username;

        int cat_ok = int.Parse(Par("c_cat"));
        int group_ok = int.Parse(Par("c_group"));
        int friend_ok = int.Parse(Par("c_friend"));
        int link_ok = int.Parse(Par("c_link"));
        int msg_ok = int.Parse(Par("c_msg"));

        string sql = "update [user_settings] set [us_cat_ok]={0},[us_group_ok]={1},[us_friend_ok]={2},[us_link_ok]={3},[us_msg_ok]={4}";
        sql += " where [us_user_name]='{5}';";
        sql += "delete from [user_link] where [ul_user_name]='{5}';";
        sql = String.Format(sql, cat_ok, group_ok, friend_ok, link_ok, msg_ok, username);

        string sqlm = "insert into [user_link]([ul_user_name],[ul_title],[ul_url]) values('"+username+"','{0}','{1}');";
        
        for (int i = 1; i < 6; i++)
        {
            if (Par("linktitle" + i.ToString()) != null)
            {
                string ltit = Par("linktitle" + i.ToString()).Trim();
                string lurl = Par("linkurl" + i.ToString()).Trim();

                if (ltit.Length > 50) ltit = ltit.Substring(0, 50).Trim();
                if (!lurl.ToLower().StartsWith("http://")) lurl = "http://" + lurl;

                sql += String.Format(sqlm, ltit.Replace("'", "''"), lurl.Replace("'", ""));
            }
        }
        DB.Execute(sql);
    }
}