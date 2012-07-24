using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web;
using System.Web.UI;

namespace DLL
{
    public class Tools
    {
        #region 加密/解密
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString)
        {
            if (encryptString == "") return "";
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes("openblog");
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString)
        {
            if (decryptString == "") return "";
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes("openblog");
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 缩略图
        /// <summary>
        /// 生成缩略图（指定宽度）
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="originalImagePath">文件流（与原路径二者选一）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        public static void MakeThumbnail(string originalImagePath, Stream fileStream, string thumbnailPath, int width)
        {
            MakeThumbnail(originalImagePath, fileStream, thumbnailPath, width, width, "W");
        }

        ///  <summary> 
        /// 生成缩略图 
        ///  </summary> 
        ///  <param name="originalImagePath">源图路径（物理路径）</param> 
        ///  <param name="fileStream">文件流（与原路径二者选一）</param> 
        ///  <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        ///  <param name="width">缩略图宽度</param> 
        ///  <param name="height">缩略图高度</param> 
        ///  <param name="mode">生成缩略图的方式(HW,W,H,Cut)</param>
        public static void MakeThumbnail(string originalImagePath, Stream fileStream, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = null;
            if (originalImagePath != "") originalImage = Image.FromFile(originalImagePath);
            else originalImage = Image.FromStream(fileStream);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
            }

            
            Image bitmap = new Bitmap(towidth, toheight);//新建一个bmp图片 

            Graphics g = Graphics.FromImage(bitmap);//新建一个画板 

            g.InterpolationMode = InterpolationMode.High;//设置高质量插值法 

            g.SmoothingMode = SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度 

            g.Clear(Color.Transparent);//清空画布并以透明背景色填充 

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="thumbnailPath"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int x, int y, int width, int height)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            Image bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);

            g.DrawImage(originalImage, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);

            try
            {
                originalImage.Dispose();
                bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// 随机码（length 6）
        /// </summary>
        /// <returns></returns>
        public static string RandomCode()
        {
            Random r = new Random();
            string c = "";
            c += (char)r.Next(49, 58); // 1 - 9 (not 0)
            c += (char)r.Next(65, 79); // A - N (not O)
            c += (char)r.Next(97, 111); // a - n (not o)
            c += (char)r.Next(49, 58); // 1 - 9 (not 0)
            c += (char)r.Next(80, 91); // P - Z
            c += (char)r.Next(112, 123); // p - z
            return c;
        }

        public static bool CheckUsername(string name)
        {
            name = name.ToLower();
            
            bool ok = new Regex(@"^[a-z]{1}[a-z0-9_]{2,19}$", RegexOptions.IgnoreCase).IsMatch(name);

            if (ok)
            {
                ok = Regex.Replace(name, @"(openblog|system|admin|cao|fuck|gan|ri)", "").Length == name.Length;
            }
            return ok;
        }

        public static string FilterName(string name)
        {
            return Regex.Replace(name, @"[^\w\s]+", "").Trim();
        }

        /// <summary>
        /// 过滤iframe script
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static string ToAnquanHtml(string raw)
        {
            return Regex.Replace(raw, @"<(/?)(script|iframe)([^>]*)>", "&lt;$1$2$3&gt;", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public static string RemoveHtml(string raw)
        {
            return Regex.Replace(raw, @"<.+?>", "");
        }

        public static string HtmlDecode(string raw)
        {
            return HttpUtility.HtmlDecode(raw);
        }

        public static string HtmlEncode(string raw)
        {
            return HttpUtility.HtmlEncode(raw);
        }

        public static string UrlDecode(string raw)
        {
            return HttpUtility.UrlDecode(raw);
        }

        public static string UrlEncode(string raw)
        {
            return HttpUtility.UrlEncode(raw);
        }

        public static string CutString(string raw, int length)
        {
            if (raw.Length > length) return raw.Substring(0, length).TrimEnd('.') + "...";
            else return raw;
        }

        public static void PrintScript(Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "sc", script, true); 
        }

        public static string DateString(string datetime)
        {
            return DateString(datetime, false);
        }
        public static string DateString(string datetime, bool noyear)
        {
            DateTime d = DateTime.Now;
            if (DateTime.TryParse(datetime, out d))
            {
                if(noyear) return d.ToString(Settings.DateFormat.Substring(5));
                else return d.ToString(Settings.DateFormat);
            }
            else return datetime;
        }

        public static string IPString(string ip)
        {
            if (ip.Split('.').Length != 4) return ip;
            else return ip.Substring(0, ip.LastIndexOf(".")) + ".*";
        }

        /// <summary>
        /// 获取页码HTML
        /// </summary>
        /// <param name="pageIndex">当前页索引（从0开始）</param>
        /// <param name="pageCount">总共页数</param>
        /// <param name="pageNumber">显示页码数</param>
        /// <param name="url">页码连接（/list.aspx?page={0}）</param>
        /// <returns></returns>
        public static string GetPager(int pageIndex, int pageCount, int pageNumber, string url)
        {
            int start = pageIndex - pageNumber / 2;
            int end = pageIndex + pageNumber / 2;
            if (start < 0)
            {
                end -= start;
                start = 0;
            }
            if (end > pageCount - 1)
            {
                start -= end - (pageCount - 1);
                end = pageCount - 1;
            }
            start = Math.Max(1, start + 1);
            end = Math.Min(end + 1, pageCount);

            string link = "<a href='" + url + "'>{0}</a>";
            if (url.StartsWith("javascript"))
            {
                link = "<a href='" + Strings.JSVoid + "' onclick='" + url + "return false;'>{0}</a>";
            }

            StringBuilder sb = new StringBuilder();

            if (pageIndex > 0) sb.AppendFormat(link.Replace("{0}</a>", "{1}</a>"), pageIndex, "上一页");

            for (int p = start; p <= end; p++)
            {
                if (p == pageIndex + 1) sb.AppendFormat("<b>{0}</b>", p);
                else sb.AppendFormat(link, p);
            }
            if (pageIndex + 1 < pageCount) sb.AppendFormat(link.Replace("{0}</a>", "{1}</a>"), pageIndex + 2, "下一页");

            return sb.ToString();
        }

        /// <summary>
        /// 转换接受到得参数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UtoGB(string str)
        {
            string[] ss = str.Split('%');
            byte[] bs = new Byte[ss.Length - 1];
            for (int i = 1; i < ss.Length; i++)
            {
                bs[i - 1] = Convert.ToByte(Convert2Hex(ss[i]));   //ss[0]为空串  
            }
            char[] chrs = System.Text.Encoding.GetEncoding("GB2312").GetChars(bs);
            string s = "";
            for (int i = 0; i < chrs.Length; i++)
            {
                s += chrs[i].ToString();
            }
            return s;
        }

        protected static string Convert2Hex(string pstr)
        {
            if (pstr.Length == 2)
            {
                pstr = pstr.ToUpper();
                string hexstr = "0123456789ABCDEF";
                int cint = hexstr.IndexOf(pstr.Substring(0, 1)) * 16 + hexstr.IndexOf(pstr.Substring(1, 1));
                return cint.ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 为short blog格式化时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string FormatDate(string datetime)
        {
            DateTime now = DateTime.Now;
            DateTime d = now;
            DateTime.TryParse(datetime, out d);
            TimeSpan ts = now - d;
            //return String.Format("day:{0},hour:{1}", ts.Days, ts.Hours);
            if (d.Year < now.Year) return d.ToString("yy-M-d hh:mm");

            if (ts.Days > 0) return d.ToString("M-d hh:mm");

            if (ts.Hours > 0) return "大约" + ts.Hours.ToString() + "小时以前";

            if (ts.Minutes > 0) return "大约" + ts.Minutes.ToString() + "分钟以前";

            return "刚刚";
        }

        /// <summary>
        /// 格式化文章内容
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string FormatBody(string body)
        {
            Regex reg = new Regex(@"(?:^|(?<!<(?:a|pre)\b(?>[^<>]*))>)(?>[^<>]*)(?:<|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Tools tools = new Tools();
            string result = reg.Replace(body, tools.RegReplace);
            return result;
        }
        List<string> tags = Tag.AllTags;
        private string RegReplace(Match m)
        {
            string temp = m.Value;
            List<string> list = new List<string>();
            foreach (string tag in tags)
            {
                int index = temp.IndexOf(tag);
                if (index > -1)
                {
                    list.Add(tag);
                    temp = temp.Substring(0, index) + "<a href='/tag/" + tag + Settings.Ext + "'>" + tag + "</a>" + temp.Substring(index + tag.Length);
                }
            }
            foreach (string s in list)
            {
                tags.Remove(s);
            }
            list.Clear();
            return temp;
        }


    }
}
