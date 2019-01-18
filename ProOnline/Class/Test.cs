using AjaxPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[AjaxNamespace("NameSpace")]
public partial class SortTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(SortTest));
    }

    [AjaxPro.AjaxMethod]
    public string test()
    {
        return "Test";
    }
}
//ar retStr = SortNameSpace.SortTest.Save(order).value