using BM.Lib.Domains;
using BM.Lib.Repositories.Conexion;
using BM.Lib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Lib.Repositories.Accesos
{
    public class ParametrosDao : IParametrosDao
    {
        IDataReader dataReader;

        public List<Parametros> ConsultaParametros()
        {
            List<Parametros> listaParametros = new List<Parametros>();
            ConsultaSQL sql = new ConsultaSQL();

            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SP_INV_CON_Parametros";
                dataReader = sql.EjecutaReader();
                while (dataReader.Read())
                {
                    listaParametros.Add(Parametros.ConParametrosDR(dataReader));
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                sql.CerrarConexion();
            }
            return listaParametros;
        }
    }
}
