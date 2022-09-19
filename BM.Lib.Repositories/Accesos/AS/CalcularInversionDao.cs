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
    public class CalcularInversionDao : ICalculaInversionDao
    {
        ConsultasAS sql;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Rendimiento CalcularInversion(RendimientoReqCalcularInv req)
        {
            sql = new ConsultasAS();
            Rendimiento result = new Rendimiento();
            int respuesta;
            decimal numDocum;

            try
            {
                numDocum = Convert.ToDecimal(req.Identificacion);

                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY017";
                //sql.Command.Parameters.Add("i_Perpa", iDB2DbType.iDB2Char).Value = req.PagoInt;
                sql.Command.Parameters.Add("i_Inver", req.IMonto);
                sql.Command.Parameters.Add("i_Plazo", req.IPlazo);
                sql.Command.Parameters.Add("i_Tipdo", iDB2DbType.iDB2Char).Value = req.ITipoIdentificacion;
                sql.Command.Parameters.Add("i_Docum", iDB2DbType.iDB2Decimal).Value = numDocum;

                //InOut
                sql.Command.Parameters.Add("r_TasaInte", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_InteGana", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_ImpuPaga", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_InteReci", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_TotaInve", iDB2DbType.iDB2Decimal).Value = 0;

                sql.Command.Parameters["r_TasaInte"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_InteGana"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_ImpuPaga"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_InteReci"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_TotaInve"].Direction = ParameterDirection.InputOutput;

                //error
                sql.Command.Parameters.Add("p_titulo", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("p_codret", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("p_msgret", iDB2DbType.iDB2Char).Value = string.Empty;

                sql.Command.Parameters["p_titulo"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_codret"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_msgret"].Direction = ParameterDirection.InputOutput;
                //

                respuesta = sql.EjecutaQuery();

                int codError = Convert.ToInt32(sql.Command.Parameters["p_codret"].Value);
                string msgError = Convert.ToString(sql.Command.Parameters["p_msgret"].Value).Trim();
                
                if (codError == 0)
                {
                    result.Tasa = Convert.ToDecimal(sql.Command.Parameters["r_TasaInte"].Value);
                    result.InteresGanar = Convert.ToDecimal(sql.Command.Parameters["r_InteGana"].Value);
                    result.Impuesto = Convert.ToDecimal(sql.Command.Parameters["r_ImpuPaga"].Value);
                    result.InteresRecibir = Convert.ToDecimal(sql.Command.Parameters["r_InteReci"].Value);
                    result.TotalPag = Convert.ToDecimal(sql.Command.Parameters["r_TotaInve"].Value);
                    return result;
                }
                Log.Info("error al ejecutar sp SCDY017: " + msgError);
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

        public FechaLaborable ObtenerFechalab(DateTime fecha)
        {
            sql = new ConsultasAS();
            FechaLaborable fechaResp = new FechaLaborable();
            int respuesta;

            try
            {
                //debe ser laborable
                // ScdY020DL
                //DEsaRMAr LA FECHA EN AñO MeS DIA DE eMIsION y DevUELVE FecHA LaBOrABlES
                //lo REStO dEl 1 ESOS SERIAN LOS DIAs PLAZo JuLIANO
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY020DL";
                sql.Command.Parameters.Add("i_Ani", iDB2DbType.iDB2Decimal).Value = Convert.ToDecimal(fecha.Year);
                sql.Command.Parameters.Add("i_Mes", iDB2DbType.iDB2Decimal).Value = Convert.ToDecimal(fecha.Month);
                sql.Command.Parameters.Add("i_Dia", iDB2DbType.iDB2Decimal).Value = Convert.ToDecimal(fecha.Day);

                //InOut
                sql.Command.Parameters.Add("r_fecLab", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("r_nomDia", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("r_numLab", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_desLab", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("r_numHoy", iDB2DbType.iDB2Decimal).Value = 0;

                sql.Command.Parameters["r_fecLab"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_nomDia"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_numLab"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_desLab"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_numHoy"].Direction = ParameterDirection.InputOutput;

                //error
                sql.Command.Parameters.Add("p_titulo", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("p_codret", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("p_msgret", iDB2DbType.iDB2Char).Value = string.Empty;

                sql.Command.Parameters["p_titulo"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_codret"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_msgret"].Direction = ParameterDirection.InputOutput;
                //

                respuesta = sql.EjecutaQuery();

                int codError = Convert.ToInt32(sql.Command.Parameters["p_codret"].Value);
                string msgError = Convert.ToString(sql.Command.Parameters["p_msgret"].Value).Trim();

                if (codError == 0)
                {
                    fechaResp.FechaLab = DateTime.ParseExact(sql.Command.Parameters["r_fecLab"].Value.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    fechaResp.DiaLaborable = Convert.ToString(sql.Command.Parameters["r_nomDia"].Value).Trim();
                    //decimal numLab = Convert.ToDecimal(sql.Command.Parameters["r_numLab"].Value);
                    //string desLab = Convert.ToString(sql.Command.Parameters["r_desLab"].Value);
                    fechaResp.NumeroLaborable = Convert.ToDecimal(sql.Command.Parameters["r_numHoy"].Value);//735950M
                    return fechaResp;
                }
                Log.Info("error al ejecutar sp SCDY020DL: " + msgError);
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
                //GC.SuppressFinalize(this);
            }

        }

        public string ObtenerHorarioNormalDif(DateTime fecha, int agencia, decimal servicio)
        {
            sql = new ConsultasAS();
            string horario;
            int respuesta;

            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCTY076";
                sql.Command.Parameters.Add("P_AGENCI", iDB2DbType.iDB2Decimal).Value = Convert.ToDecimal(agencia);
                sql.Command.Parameters.Add("P_HORA", iDB2DbType.iDB2Decimal).Value = Convert.ToDecimal(fecha.Hour + fecha.Minute + fecha.Second);
                sql.Command.Parameters.Add("P_SERVICIO", iDB2DbType.iDB2Decimal).Value = servicio;

                sql.Command.Parameters.Add("P_FECPRO", iDB2DbType.iDB2Decimal);
                sql.Command.Parameters.Add("P_FECBLE", iDB2DbType.iDB2Decimal);
                sql.Command.Parameters.Add("P_HORARI", iDB2DbType.iDB2Char);
                sql.Command.Parameters.Add("P_CODRET", iDB2DbType.iDB2Decimal);
                sql.Command.Parameters.Add("P_MSGRET", iDB2DbType.iDB2VarChar);

                sql.Command.Parameters["P_FECPRO"].Direction = ParameterDirection.Output;
                sql.Command.Parameters["P_FECBLE"].Direction = ParameterDirection.Output;
                sql.Command.Parameters["P_HORARI"].Direction = ParameterDirection.Output;
                sql.Command.Parameters["P_CODRET"].Direction = ParameterDirection.Output;
                sql.Command.Parameters["P_MSGRET"].Direction = ParameterDirection.Output;
                //
                respuesta = sql.EjecutaQuery();

                int codError = Convert.ToInt32(sql.Command.Parameters["P_CODRET"].Value);
                string msgError = Convert.ToString(sql.Command.Parameters["P_MSGRET"].Value).Trim();

                if (codError == 0)
                {
                    horario = Convert.ToString(sql.Command.Parameters["P_HORARI"].Value).Trim();
                    return horario;
                }
                Log.Info("error al ejecutar sp SCTY076: " + msgError);
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
