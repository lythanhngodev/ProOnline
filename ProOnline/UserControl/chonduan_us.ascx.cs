using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using WEB_DLL;
using ProOnline.Class;
using Obout.Grid;

namespace ProOnline.UserControl
{
    public partial class chonduan_us : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridChonDuAn_us.DataSource = null;
                GridChonDuAn_us.DataBind();
            }
        }
        protected void GridChonDuAn_us_OnRebind(object sender, EventArgs e)
        {
            try
            {
                SqlFunction sqlFun = new SqlFunction(HttpContext.Current.Session.GetConnectionString2());
                GridChonDuAn_us.DataSource = sqlFun.GetData(@"SELECT sttDuAnpr,maDuAn,tenDuAn,tenChuDauTu=(SELECT tenDonVi FROM tblDMDonVi WHERE maDonVipr=maDonVipr_chudautu),diaDiemXD FROM dbo.tblDuAn WHERE tblDuAn.maDonVipr_sd=N'" + HttpContext.Current.Session.GetDonVi().maDonVi + "'");
                GridChonDuAn_us.DataBind();
            }
            catch (Exception)
            {
                GridChonDuAn_us.DataSource = null;
                GridChonDuAn_us.DataBind();
            }
        }
    }
}