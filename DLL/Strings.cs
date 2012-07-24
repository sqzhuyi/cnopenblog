using System;
using System.Collections.Generic;
using System.Text;

namespace DLL
{
    public class Strings
    {
        public static string JSVoid
        {
            get { return "javascript:void(0);"; }
        }

        public static string UserBigImageError
        {
            get { return "onerror='this.src=\"/upload/photo/nophoto.jpg\";'"; }
        }
        public static string UserSmallImageError
        {
            get { return "onerror='this.src=\"/upload/photo/nophoto-s.jpg\";'"; }
        }

        public static string GroupBigImageError
        {
            get { return "onerror='this.src=\"/upload/group/nophoto.jpg\";'"; }
        }
        public static string GroupSmallImageError
        {
            get { return "onerror='this.src=\"/upload/group/nophoto-s.jpg\";'"; }
        }

        public static string ImageOnload
        {
            get { return "onload=\"if(this.offsetWidth>400){this.style.width='400px';this.style.height='auto';}\""; }
        }
    }
}
