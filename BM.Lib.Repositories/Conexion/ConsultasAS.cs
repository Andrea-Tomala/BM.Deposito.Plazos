using IBM.Data.DB2.iSeries;
using System.Data;

namespace BM.Lib.Repositories.Conexion
{
    public class ConsultasAS
    {
        private iDB2Connection asConnection;
        public iDB2Command Command { get; set; }

        private readonly Bases conexion = new Bases();

        public ConsultasAS()
        {
            asConnection = conexion.ConexionAS();
            Command = new iDB2Command();
        }

        public void AbrirConexionAS()
        {
            if (asConnection.State != ConnectionState.Open)
                asConnection.Open();
        }

        public void CerrarConexionAS()
        {
            try
            {
                if (asConnection.State != ConnectionState.Closed)
                    asConnection.Close();
            }
            catch
            {
                if (asConnection.State != ConnectionState.Closed)
                    throw;
            }
        }

        public iDB2DataReader EjecutaReader()
        {
            iDB2DataReader retValue;
            try
            {
                AbrirConexionAS();
                Command.Connection = asConnection;
                retValue = Command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                throw;
            }
            return retValue;
        }

        public int EjecutaQuery()
        {
            int retValue;
            try
            {
                AbrirConexionAS();
                Command.Connection = asConnection;
                retValue = Command.ExecuteNonQuery();
                CerrarConexionAS();
            }
            catch
            {
                throw;
            }
            return retValue;
        }
    }
}