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
    public class TableroDao : ITableroDao
    {
        ConsultasAS sql;
        iDB2DataReader dataReader;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<Tablero> ConsultaMallaT(TableroReq mallaReq)
        {
            List<Tablero> listaMalla = new List<Tablero>();
            sql = new ConsultasAS();

            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY014";

                //InOut
                //error
                sql.Command.Parameters.Add("p_titulo", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("p_codret", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("p_msgret", iDB2DbType.iDB2Char).Value = string.Empty;

                sql.Command.Parameters["p_titulo"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_codret"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_msgret"].Direction = ParameterDirection.InputOutput;

                dataReader = sql.EjecutaReader();

                int codError = Convert.ToInt32(sql.Command.Parameters["p_codret"].Value);
                string msgError = Convert.ToString(sql.Command.Parameters["p_msgret"].Value).Trim();

                if (codError == 0)
                {
                    while (dataReader.Read())
                    {
                        listaMalla.Add(Tablero.ConMallaTableroDR(dataReader));
                    }
                    return listaMalla;
                }
                Log.Error("error al ejecutar sp SCDY014 : " + msgError);
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
