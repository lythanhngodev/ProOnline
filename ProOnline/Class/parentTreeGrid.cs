using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using ProOnline;

namespace ProOnline.Class
{
    public class parentTreeGrid
    {
        private decimal _paPrimaryKey;
        private string _paColumn01;
        private string _paColumn02;
        private List<childTreeGrid> _childList = new List<childTreeGrid>();

        public decimal paPrimaryKey
        {
            get { return this._paPrimaryKey; }
            set { this._paPrimaryKey = value; }
        }

        public string paColumn01
        {
            get { return this._paColumn01; }
            set { this._paColumn01 = value; }
        }

        public string paColumn02
        {
            get { return this._paColumn02; }
            set { this._paColumn02 = value; }
        }

        public List<childTreeGrid> childList
        {
            get { return this._childList; }
            set { this._childList = value; }
        }

        public List<parentTreeGrid> GetparentTreeGrid(string _vSqlQuery)
        {
            List<parentTreeGrid> parentList = new List<parentTreeGrid>();
            int i = -1;

            string connectionString = HttpContext.Current.Session.GetConnectionString2();
            SqlConnection myConnection = new SqlConnection(connectionString);
            SqlCommand myCommand = new SqlCommand(_vSqlQuery, myConnection);

            try
            {
                myConnection.Open();
                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    parentTreeGrid parentTreeGrid = new parentTreeGrid();
                    parentTreeGrid.paPrimaryKey = Convert.ToInt32(reader["parentpr"]);
                    parentTreeGrid.paColumn01 = reader["Ten"] as String;

                    if (!IDAlreadyExists(parentTreeGrid.paPrimaryKey, parentList))
                    {
                        parentList.Add(parentTreeGrid);
                        i++;
                        parentList[i].childList.Add(new childTreeGrid(Convert.ToInt32(reader["Chilpr"]), reader["ChilName"] as String, reader["ChilName"] as String));
                    }
                    else
                    {
                        parentList[i].childList.Add(new childTreeGrid(Convert.ToInt32(reader["Chilpr"]), reader["ChilName"] as String, reader["ChilName"] as String));
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                // Logg the exception here
            }
            finally
            {
                myConnection.Close();
                myCommand.Dispose();
            }

            return parentList;
        }

        private bool IDAlreadyExists(decimal categoryID, List<parentTreeGrid> _parentIDTreeGrid)
        {
            bool result = false;

            foreach (parentTreeGrid _parentTreeGrid in _parentIDTreeGrid)
            {
                if (_parentTreeGrid.paPrimaryKey == categoryID)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
