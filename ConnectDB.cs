using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Configuration;

namespace DTWTest
{
    class ConnectDB
    {
        public OleDbConnection connect()
        {
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["MuayThaiDBConnectionString"].ToString();
            return con;
        }

    }
}
