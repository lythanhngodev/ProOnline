using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WEB_DLL;
using ProOnline.Class;
using System.Web.SessionState;
namespace ProOnline.UserControl
{
    public partial class LeftBar : System.Web.UI.UserControl
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder sb_ = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            sb.Append("");
          
        }
        protected void rpMainMenu_OnInit(object sender, EventArgs e)
        {
            rpMainMenu.DataSource = LoadMainMenu_lv1();
            rpMainMenu.DataBind();

        }
        protected string CheckOpen(string motherID)
        {
            DataTable dtData = LoadMainMenu_child(motherID);
            sb_.Clear() ;
            if (dtData != null && dtData.Rows.Count > 0)
            {
              
                foreach (DataRow row in dtData.Rows)
                {
                    if (row["pathFile"].ToString() == HttpContext.Current.Request.Url.AbsolutePath.ToString())
                    {
                        sb_.Append("active open");
                    }
                    else
                    {
                        DataTable dtData1 = LoadMainMenu_child(row["functionName"].ToString());
                        if (dtData1 != null && dtData1.Rows.Count > 0)
                        {

                            foreach (DataRow row1 in dtData1.Rows)
                            {
                                if (row1["pathFile"].ToString() == HttpContext.Current.Request.Url.AbsolutePath.ToString())
                                {
                                    sb_.Append("active open");
                                }
                                else
                                {
                                    DataTable dtData2 = LoadMainMenu_child(row1["functionName"].ToString());
                                    if (dtData2 != null && dtData2.Rows.Count > 0)
                                    {

                                        foreach (DataRow row2 in dtData2.Rows)
                                        {
                                            if (row2["pathFile"].ToString() == HttpContext.Current.Request.Url.AbsolutePath.ToString())
                                            {
                                                sb_.Append("active open");
                                            }
                                            else
                                            {
                                                DataTable dtData3 = LoadMainMenu_child(row2["functionName"].ToString());
                                                if (dtData3 != null && dtData3.Rows.Count > 0)
                                                {

                                                    foreach (DataRow row3 in dtData3.Rows)
                                                    {
                                                        if (row3["pathFile"].ToString() == HttpContext.Current.Request.Url.AbsolutePath.ToString())
                                                        {
                                                            sb_.Append("active open");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return sb_.ToString();
        }
        protected string ChildMenu(string motherID)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtData = LoadMainMenu_child(motherID);
            sb.Append("");
            if (dtData != null && dtData.Rows.Count > 0)
            {
                sb.Append("<ul class='submenu nav-hide'>");
                foreach (DataRow row in dtData.Rows)
                {
                    if (row["pathFile"].ToString() == HttpContext.Current.Request.Url.AbsolutePath.ToString())
                    {
                        sb.AppendFormat("<li class=\"active\"><a href=\"{0}\">", row["pathFile"]);
                    }
                    else
                    {
                        if (row["pathFile"].ToString() == "" || row["pathFile"].ToString() == null)
                        {
                            sb.AppendFormat("<li class=\"" + CheckOpen(row["functionName"].ToString()) + "\"><a class=\"dropdown-toggle\" href=\"{0}\">", row["pathFile"]);
                        }
                        else
                        {
                            sb.AppendFormat("<li class=\"" + CheckOpen(row["functionName"].ToString()) + "\"><a href=\"{0}\">", row["pathFile"]);
                        }
                    }
                    sb.AppendFormat("<i class='menu-icon {0}' ></i>", row["IconName"]);
                    sb.AppendFormat("{0}", row["dienGiai"]);
                    if (row["pathFile"].ToString() == "" || row["pathFile"].ToString() == null)
                    {
                        sb.Append("<b class=\"arrow fa fa-angle-down\"></b></a>");
                    }
                    else
                    {
                        sb.Append("</a>");
                    }
                    sb.Append(ChildMenu(row["functionName"].ToString()));
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }

            return sb.ToString();
        }
        public DataTable LoadMainMenu_child(string motherID)
        {

            try
            {
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
                return sqlFun.GetData(@"SELECT functionName,dienGiai,nhomChucnang,pathFile,IconName,IDMainMenu,functionIDpr FROM dbo.tblFunctions   WHERE IDMainMenu = '" + motherID + "' AND functionIDpr IN (SELECT functionIDpr_sd FROM dbo.tblUserPermiss  where maNguoidungpr_sd = N'" + HttpContext.Current.Session.GetCurrentUserID() + "')    ORDER BY tenFile asc");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable LoadMainMenu_lv1()
        {
            try
            {
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString1());
                return sqlFun.GetData(@"SELECT functionName,dienGiai,nhomChucnang,pathFile,IconName,IDMainMenu,functionIDpr FROM dbo.tblFunctions   WHERE (IDMainMenu = '0') AND functionIDpr IN (SELECT functionIDpr_sd FROM dbo.tblUserPermiss where maNguoidungpr_sd = N'" + HttpContext.Current.Session.GetCurrentUserID() + "')   ORDER BY tenFile asc");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}