using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProOnline.DataConnect;
using WEB_DLL;

namespace ProOnline.Class
{
    public static class ntsSecurityServices
    {
        public static bool HasPermission(decimal UserID, string FunctionName, TypeAudit audit)
        {
            if (!ntsSqlFunctions._IsRegis)
                return false;

            UsersDataContext _vdata = new UsersDataContext();
            tblUserPermiss user = _vdata.tblUserPermisses.Where(userP => userP.maNguoidungpr_sd == UserID && userP.tblFunction.functionName == FunctionName).FirstOrDefault();
            if (user != null)
                return HasPermission(audit, Convert.ToInt32(ntsSecurity._mDecrypt(user.permission, "rateAnd2012", true).Split(';')[2]));

            return false;
        }

        public static void SetPermission(decimal UserID, string FunctionName, TypeAudit audit)
        {
            if (!ntsSqlFunctions._IsRegis)
                return;
            UsersDataContext _vdata = new UsersDataContext();
            tblFunction function = _vdata.tblFunctions.Where(p => p.functionName == FunctionName).FirstOrDefault();
            if (function != null)
                return;
            tblUserPermiss permis = new tblUserPermiss();
            permis.maNguoidungpr_sd = UserID;
            permis.tblFunction = function;
            permis.permission = ntsSecurity._mEncrypt(UserID.ToString() + ";" + function.functionIDpr.ToString() + ";" + ((int)audit).ToString(), "rateAnd2012", true);
            _vdata.tblUserPermisses.InsertOnSubmit(permis);
            _vdata.SubmitChanges();
        }

        public static bool HasPermission(TypeAudit audit, int permission)
        {
            if (((int)audit & permission) == (int)audit)
                return true;
            return false;
        }
    }
}
