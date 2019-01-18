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
using System.Xml.Linq;
using ProOnline.Class;
using System.Windows.Forms;
using System.Web.SessionState;

namespace ProOnline
{
    public partial class ntsFormModel : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {
            //AjaxPro.Utility.RegisterTypeForAjax(typeof(THBC2014.ntsFormModel));
            if (Session.GetCurrentUser() == null)
            {
                Response.Redirect("~/HeThong/Login.aspx?returnUrl=" + Request.Url.PathAndQuery); //chuyển sang trang Login
            }
            base.OnInit(e);
        }

        public decimal _GetCurrentUserID()
        {
            return Session.GetCurrentUserID();
        }

        public string _GetCurrentUserName()
        {
            return Session.GetCurrentUser().tenDangNhap;
        }

        public string _GetCurrentDatetimeMMddyyyy()
        {
            return Session.GetCurrentDatetimeMMddyyyy("MM/dd/yyyy");
        }

        public string _GetCurrentDatetimeddMMyyyy()
        {
            return Session.GetCurrentDatetimeMMddyyyy("dd/MM/yyyy");
        }

        public static DateTime _mConvertStringddMMyyyyToDateTime(string value)
        {
            return DateTime.Parse(value, System.Globalization.CultureInfo.GetCultureInfo("en-gb"));
        }

        public static DateTime _mConvertStringMMddyyyyToDateTime(string value)
        {
            return DateTime.Parse(value, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }
        [AjaxPro.AjaxMethod]
        public DateTime _mConvertddMMyyyyToDateTime(string value)
        {
            return DateTime.Parse(value, System.Globalization.CultureInfo.GetCultureInfo("en-gb"));
        }
        [AjaxPro.AjaxMethod]
        public DateTime _mConvertMMddyyyyToDateTime(string value)
        {
            return DateTime.Parse(value, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }
        [AjaxPro.AjaxMethod]
        public string _mConvertMMddyyyyToddMMyyyy(string value)
        {
            return DateTime.Parse(value.Substring(0,10), System.Globalization.CultureInfo.GetCultureInfo("en-US")).ToString("dd/MM/yyyy").Substring(0, 10);
        }

        [AjaxPro.AjaxMethod] //lấy ID người dùng trên Session
        public decimal _mGetCurrentUserID()
        {
            return Session.GetCurrentUserID();
        }
        [AjaxPro.AjaxMethod] //Lấy tên người dùng trên Session
        public string _mGetCurrentUserName()
        {
            return Session.GetCurrentUser().tenDangNhap;
        }
        [AjaxPro.AjaxMethod] //Lấy ngày sử dụng hệ thống của người dùng trên Session
        public string _mGetCurrentDatetimeMMddyyyy()
        {
            return Session.GetCurrentDatetimeMMddyyyy("MM/dd/yyyy");
        }
        [AjaxPro.AjaxMethod] //Lấy ngày sử dụng hệ thống của người dùng trên Session
        public string _mGetCurrentDatetimeddMMyyyy()
        {
            return Session.GetCurrentDatetimeMMddyyyy("dd/MM/yyyy");
        }

        [AjaxPro.AjaxMethod]
        public void _mSetFullScreen(bool value)
        {
            Session.SetFullScreen(value);
        }

        [AjaxPro.AjaxMethod]
        public bool _mGetFullScreen()
        {
            return Session.GetFullScreen();
        }
    }
}
