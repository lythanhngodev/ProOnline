using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace ProOnline.Class
{
    public class childTreeGrid
    {
        private decimal _PrimaryKey;
        private string _Column01;
        private string _Column02;
        private string _Column03;
        private string _Column04;
        private string _Column05;

        public decimal PrimaryKey
        {
            get { return this._PrimaryKey; }
            set { this._PrimaryKey = value; }
        }

        public string Column01
        {
            get { return this._Column01; }
            set { this._Column01 = value; }
        }

        public string Column02
        {
            get { return this._Column02; }
            set { this._Column02 = value; }
        }

        public string Column03
        {
            get { return this._Column03; }
            set { this._Column03 = value; }
        }

        public string Column04
        {
            get { return this._Column04; }
            set { this._Column04 = value; }
        }

        public string Column05
        {
            get { return this._Column05; }
            set { this._Column05 = value; }
        }

        public childTreeGrid(decimal primaryKey, string column01)
        {
            this._PrimaryKey = primaryKey;
            this._Column01 = column01;
        }

        public childTreeGrid(decimal primaryKey, string column01, string column02)
        {
            this._PrimaryKey = primaryKey;
            this._Column01 = column01;
            this._Column02 = column02;
        }

        public childTreeGrid()
        {
        }
    }
}
