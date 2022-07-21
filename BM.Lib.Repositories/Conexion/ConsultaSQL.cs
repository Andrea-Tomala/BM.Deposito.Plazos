using System.Data;
using System.Data.SqlClient;

namespace BM.Lib.Repositories.Conexion
{
    public class ConsultaSQL
    {
        private SqlConnection sqlConnection;
        public SqlCommand Command { get; set; }
        
        private readonly Bases conexion = new Bases();

        public ConsultaSQL()
        {
            sqlConnection = conexion.ConexionSQL();
            Command = new SqlCommand();
        }

        public void AbrirConexion()
        {
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();
        }

        public void CerrarConexion()
        {
            try
            {
                if (sqlConnection.State != ConnectionState.Closed)
                    sqlConnection.Close();

            }catch
            {
                if (sqlConnection.State != ConnectionState.Closed)
                    throw;
            }
            
        }

        public IDataReader EjecutaReader()
        {
            IDataReader retValue;
            try
            {
                AbrirConexion();
                Command.Connection = sqlConnection;
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
                AbrirConexion();
                Command.Connection = sqlConnection;
                retValue = Command.ExecuteNonQuery();
                CerrarConexion();
            }
            catch
            {
                throw;
            }
            return retValue;
        }
    }
}