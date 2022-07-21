using BM.Lib.Domains;
using BM.Lib.Domains.AS;
using BM.Lib.Repositories.Conexion;
using BM.Lib.Repositories.Interfaces.AS;
using IBM.Data.DB2.iSeries;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace BM.Lib.Repositories.Accesos.AS
{
    public class ProyeccionDao : IProyeccion
    {
        ConsultasAS sql;
        IDataReader dataReader;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<Proyeccion> ConsultaProyeccion(ProyeccionReq req)
        {
            List<Proyeccion> listaProyeccion = new List<Proyeccion>();
            sql = new ConsultasAS();

            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY016";
                sql.Command.Parameters.Add("i_Monto", iDB2DbType.iDB2Decimal).Value = req.Monto;

                //parametros inout error
                //error
                sql.Command.Parameters.Add("p_titulo", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("p_codret", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("p_msgret", iDB2DbType.iDB2Char).Value = string.Empty;

                sql.Command.Parameters["p_titulo"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_codret"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_msgret"].Direction = ParameterDirection.InputOutput;
                //

                dataReader = sql.EjecutaReader();

                int codError = Convert.ToInt32(sql.Command.Parameters["p_codret"].Value);
                string msgError = Convert.ToString(sql.Command.Parameters["p_msgret"].Value).Trim();

                if (codError == 0)
                {
                    while (dataReader.Read())
                    {
                        listaProyeccion.Add(Proyeccion.ConProyeccionDR(dataReader));
                    }
                    return listaProyeccion;
                }
                Log.Error("error al ejecutar sp SCDY016 : " + msgError);
                throw new ExcepcionSistema(msgError, codError);

            }
            catch (iDB2Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw new ExcepcionSistema(ex.Message, 501, ex);
            }
            catch (ExcepcionSistema ex)
            {
                Log.Error(ex.Message, ex);
                throw new ExcepcionSistema("Inconveniente en la consulta de los datos.", 503, ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw new ExcepcionSistema(ex.Message, 501, ex);
            }
            finally
            {
                Log.Debug("Fin");
                sql.CerrarConexionAS();
            }
        }
    }
}
