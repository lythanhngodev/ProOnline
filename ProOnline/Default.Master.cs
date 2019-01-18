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
using ProOnline.DataConnect;
using WEB_DLL;
using ProOnline.Class;
namespace ProOnline
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProOnline.Default));
            string permiss = "";
            string url = ((System.Web.UI.UserControl)(sender)).Request.AppRelativeCurrentExecutionFilePath.ToString();
            Session.Add("CurrentFilePath", url.Replace("~", ""));
            permiss = PhanQuyenChucnang();
            Session.Add("CurrentPermiss", permiss);
        }
        public string PhanQuyenChucnang()
        {
            string permiss = "";
            string _vPermissValue = "";
            string CurrentFilePath = "";
            try
            {
                CurrentFilePath = Session["CurrentFilePath"].ToString();
                if (CurrentFilePath.ToString() != "")
                {
                    SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
                    permiss = sqlFun.GetOneStringField("SELECT permission FROM tblUserPermiss WHERE functionIDpr_sd=" + Session["IDfunction"].ToString() + " AND maNguoidungpr_sd=" + HttpContext.Current.Session.GetCurrentUserID() + "");

                    string _vPermiss = permiss.ToString();
                    _vPermiss = WEB_DLL.ntsSecurity._mDecrypt(_vPermiss, "rateAnd2012", true).Split(';')[2];
                    _vPermissValue += ntsSecurityServices.HasPermission(TypeAudit.View, Convert.ToInt32(_vPermiss)).ToString().ToLower();
                    _vPermissValue += ";" + ntsSecurityServices.HasPermission(TypeAudit.AddNew, Convert.ToInt32(_vPermiss)).ToString().ToLower();
                    _vPermissValue += ";" + ntsSecurityServices.HasPermission(TypeAudit.Delete, Convert.ToInt32(_vPermiss)).ToString().ToLower();
                    _vPermissValue += ";" + ntsSecurityServices.HasPermission(TypeAudit.Edit, Convert.ToInt32(_vPermiss)).ToString().ToLower();
                    _vPermissValue += ";" + ntsSecurityServices.HasPermission(TypeAudit.LoadData, Convert.ToInt32(_vPermiss)).ToString().ToLower();
                    _vPermissValue += ";" + ntsSecurityServices.HasPermission(TypeAudit.Print, Convert.ToInt32(_vPermiss)).ToString().ToLower();
                    _vPermissValue += ";" + ntsSecurityServices.HasPermission(TypeAudit.PlusP1, Convert.ToInt32(_vPermiss)).ToString().ToLower();
                    _vPermissValue += ";" + ntsSecurityServices.HasPermission(TypeAudit.PlusP2, Convert.ToInt32(_vPermiss)).ToString().ToLower();
                    _vPermissValue += ";" + ntsSecurityServices.HasPermission(TypeAudit.PlusP3, Convert.ToInt32(_vPermiss)).ToString().ToLower();
                }
            }
            catch
            { }
            return _vPermissValue;
        }
        [AjaxPro.AjaxMethod]
        public string GetCurrentPermiss()
        {
            return HttpContext.Current.Session["CurrentPermiss"].ToString();
        }

        [AjaxPro.AjaxMethod]
        public string getIDRecord(string tabbleName, string fielName, string khoaChinh)
        {
            try
            {
                decimal nguoiDung = 0;
                SqlFunction sqlFunc = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                nguoiDung = sqlFunc.GetOneDecimalField(@"SELECT nguoiThaoTac FROM " + tabbleName + " WHERE " + fielName + "=N'" + khoaChinh + "'");
                if (nguoiDung == HttpContext.Current.Session.GetCurrentUserID())
                    return "true";
                else
                    return "false";
            }
            catch
            {
                return "false";
            }
        }
    }

}
