using BM.Lib.Domains;
using BM.Lib.Domains.AS;
using BM.Lib.Repositories.Conexion;
using BM.Lib.Repositories.Interfaces.AS;
using IBM.Data.DB2.iSeries;
using log4net;
using System;
using System.Data;
using System.Reflection;

namespace BM.Lib.Repositories.Accesos.AS
{
    public class RendimientoDao : IRendimientoDao
    {

        ConsultasAS sql;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Rendimiento ConsultaRendimiento(RendimientoReq req)
        {
            sql = new ConsultasAS();
            Rendimiento result = new Rendimiento();

            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY015";
                sql.Command.Parameters.Add("i_Monto", iDB2DbType.iDB2Decimal).Value = req.IMonto;
                sql.Command.Parameters.Add("i_Plazo", iDB2DbType.iDB2Decimal).Value = req.IPlazo;

                //InOut
                sql.Command.Parameters.Add("r_TasaIn", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_InGana", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_Impues", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_InReci", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_TotPag", iDB2DbType.iDB2Decimal).Value = 0;

                sql.Command.Parameters["r_TasaIn"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_InGana"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_Impues"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_InReci"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_TotPag"].Direction = ParameterDirection.InputOutput;

                //error
                sql.Command.Parameters.Add("p_titulo", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("p_codret", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("p_msgret", iDB2DbType.iDB2Char).Value = string.Empty;

                sql.Command.Parameters["p_titulo"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_codret"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_msgret"].Direction = ParameterDirection.InputOutput;
                //

                int respuesta = sql.EjecutaQuery();

                int codError = Convert.ToInt32(sql.Command.Parameters["p_codret"].Value);
                string msgError = Convert.ToString(sql.Command.Parameters["p_msgret"].Value).Trim();

                if (codError == 0)
                {
                    result.Tasa = Convert.ToDecimal(sql.Command.Parameters["r_TasaIn"].Value);
                    result.InteresGanar = Convert.ToDecimal(sql.Command.Parameters["r_InGana"].Value);
                    result.Impuesto = Convert.ToDecimal(sql.Command.Parameters["r_Impues"].Value);
                    result.InteresRecibir = Convert.ToDecimal(sql.Command.Parameters["r_InReci"].Value);
                    result.TotalPag = Convert.ToDecimal(sql.Command.Parameters["r_TotPag"].Value);

                    return result;
                }
                Log.Error("error al ejecutar SP SCDY015 : " + msgError);
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
            }

        }
    }
}
