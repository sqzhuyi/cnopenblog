<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        DLL.AppStart.Setup();
    }
    
    void Application_End(object sender, EventArgs e) 
    {

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        if (Request.Url.ToString().ToLower().StartsWith("http://cnopenblog.com"))
        {
            Response.Redirect(Request.Url.ToString().Replace("http://", "http://www."));
        }
    }
        
    void Application_Error(object sender, EventArgs e) 
    {

    }

    void Session_Start(object sender, EventArgs e) 
    {
        DLL.UserSetting.Online++;
    }

    void Session_End(object sender, EventArgs e) 
    {
        // sessionstate=InProc 时，才会引发 Session_End 事件。
        DLL.UserSetting.Online--;
    }
       
</script>
