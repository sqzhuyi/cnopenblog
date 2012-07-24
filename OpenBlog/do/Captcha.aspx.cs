using System;
using System.Drawing;

public partial class do_Captcha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "image/jpeg";
        Response.Clear();

        string s = DLL.SessionFacade.ConfirmCode;

        Font f = new Font("Times New Roman", 26, FontStyle.Italic | FontStyle.Bold, GraphicsUnit.Pixel);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;

        Bitmap bmp = new Bitmap(110, 40, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.White);

        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

        g.DrawString(s, f, Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), format);
        /*
        for (int i = -2; i < bmp.Width / 10; i++)
        {
            g.DrawLine(Pens.OrangeRed, i * 10, 0, i * 10 + 20, bmp.Height);
        }
        for (int i = -2; i < bmp.Width / 10 + 10; i++)
        {
            g.DrawLine(Pens.Blue, i * 10, 0, i * 10 - 60, bmp.Height);
        }
        */
        g.Dispose();

        bmp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

    }
}
