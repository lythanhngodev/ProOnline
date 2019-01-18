using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.SessionState;
using System.Xml.Linq;
using ProOnline.Class;
using ProOnline.DataConnect;
using WEB_DLL;
using System.Text;
using System.Data.SqlClient;
using System.Web.Services;
namespace ProOnline.UserControl
{
    [System.Web.Script.Services.ScriptService]
    public partial class Header : System.Web.UI.UserControl 
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (HttpContext.Current.Session.GetCurrentUser().tenDangNhap != "")
                {
                    lblUser_us.InnerHtml = HttpContext.Current.Session.GetCurrentUser().tenDangNhap;
                }
            }
            catch
            {
            }


        }

    }
}