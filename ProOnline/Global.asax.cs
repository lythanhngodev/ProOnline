using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using WEB_DLL;
using ProOnline.DataConnect;
using ProOnline.Class;

namespace ProOnline
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //ntsDataConnect._mCreateFileConnectString("USERSQLBH_NTS_2017", "QLBH_NTS_2017");
            ntsDataConnect._mCreateFileConnectString("UserProOnline_laptrinh", "ProOnline_laptrinh2016");
            ntsSqlFunctions ntsSQLServerFunctions = new ntsSqlFunctions(ntsSqlRunType.Web);

            Session.SetConnectionString1(ntsDataConnect._mGetConnectStringFromFile1());
            Session.SetConnectionString2(ntsDataConnect._mGetConnectStringFromFile2());
         
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session.Remove("lanDangNhap");
            UsersDataContext db = new UsersDataContext();
            db.sp_SetOffline(Session.GetCurrentUserID());
            Session.RemoveAll();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}