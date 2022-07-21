using IBM.Data.DB2.iSeries;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace BM.Lib.Repositories.Conexion
{
    public class Bases
    {
        string sql = "";
        
        public SqlConnection ConexionSQL()
        {
            try
            {
                sql = ConfigurationManager.ConnectionStrings["conexionSQL"].ConnectionString;
                return new SqlConnection(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public iDB2Connection ConexionAS()
        {
            try
            {
                sql = ConfigurationManager.ConnectionStrings["conexionAS"].ConnectionString;
                return new iDB2Connection(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
