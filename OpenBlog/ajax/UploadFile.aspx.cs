using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using DLL;

public partial class ajax_UploadFile : System.Web.UI.Page
{
    protected string script;
    private int bigWidth = 120;
    private int smallWidth = 42;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["up"] != null)
        {
            HttpFileCollection files = Request.Files;
            if (files.Count == 0)
            {
                Response.Write("请选择要上传的文件");
            }
            else if (Request["kind"] == "img")
            {
                string dir = Settings.BasePath + "upload\\images\\" + DateTime.Now.ToString("yyyyMM");
                string fname = dir + "\\" + DateTime.Now.ToString("ddhhmmss") + DateTime.Now.Millisecond.ToString();
                string fn = files[0].FileName;
                fname += fn.Substring(fn.LastIndexOf("."));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                files[0].SaveAs(fname);

                script = "top.loat('" + fname.Replace(Settings.BasePath, Settings.BaseURL).Replace("\\", "/") + "');";
            }
            else if (Request["kind"] == "group")
            {
                string gid = Request["data"];

                string newpath = Server.MapPath("/upload/group/" + gid + ".jpg");
                string s = newpath.Replace(".jpg", "-s.jpg");

                Tools.MakeThumbnail("", files[0].InputStream, newpath, bigWidth);
                Tools.MakeThumbnail("", files[0].InputStream, s, smallWidth);

                script = "top.loat();";
            }
            else
            {
                string newpath = Server.MapPath("/upload/photo/" + CKUser.Username + ".jpg");
                string b = newpath.Replace(".jpg", "-b.jpg");
                string s = newpath.Replace(".jpg", "-s.jpg");

                files[0].SaveAs(b);

                Stream imgStream = files[0].InputStream;

                Tools.MakeThumbnail("", imgStream, newpath, bigWidth);
                Tools.MakeThumbnail("", imgStream, s, smallWidth);

                System.Drawing.Image img = System.Drawing.Image.FromStream(imgStream);
                if (img.Width < bigWidth + 30) script = "top.loat(false);";
                else script = "top.loat(true);";
            }
        }
        if (Request["cutimg"] != null)
        {
            int x = int.Parse(Request["x1"]);
            int y = int.Parse(Request["y1"]);
            int width = int.Parse(Request["x2"]) - x;
            int height = int.Parse(Request["y2"]) - y;
            string b = Server.MapPath("/upload/photo/" + CKUser.Username + "-b.jpg");
            string p = b.Replace("-b.jpg", ".jpg");
            Tools.MakeThumbnail(b, p, x, y, width, height);
            IO.DeleteFile(b);
            Tools.MakeThumbnail(p, null, b.Replace("-b.jpg", "-s.jpg"), smallWidth);

            Response.Write("<img src='/upload/photo/" + CKUser.Username + ".jpg' />");
            Response.End();
        }
    }

}
