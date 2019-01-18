using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.CSharp;

namespace ProOnline.Class
{
    public class JSonHelper
    {
        public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static object FromJson(string obj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(obj);
        }

        public static DataTable ToTable(string obj)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(obj);
        }
    }
}