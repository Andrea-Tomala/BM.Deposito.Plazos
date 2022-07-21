using BM.Lib.Domains;
using BM.Lib.Domains.AS;
using BM.Lib.Repositories.Conexion;
using BM.Lib.Repositories.Interfaces.AS;
using IBM.Data.DB2.iSeries;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BM.Lib.Repositories.Accesos.AS
{
    public class RegistraEventosDao : IRegistraEventosDao
    {
        ConsultasAS sql;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string RegistrarEventos(Evento evento)
        {
            sql = new ConsultasAS();
            int respuesta;


            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY023";
                sql.Command.Parameters.Add("i_Opcionn", iDB2DbType.iDB2Decimal).Value = evento.Opcion;
                sql.Command.Parameters.Add("i_TipoDocume", iDB2DbType.iDB2Char).Value = evento.TipoIdentificacion;
                sql.Command.Parameters.Add("i_NumDocume", iDB2DbType.iDB2Decimal).Value = Convert.ToDecimal(evento.Identificacion);
                sql.Command.Parameters.Add("i_CodCer", iDB2DbType.iDB2Char).Value = evento.CodCertificado;
                sql.Command.Parameters.Add("i_NumCer", iDB2DbType.iDB2Decimal).Value = evento.NumeroDeposito;
                sql.Command.Parameters.Add("i_PAmsg", iDB2DbType.iDB2Decimal).Value = evento.CodError;
                
                /*
                sql.Command.Parameters.Add("i_FechaEvento", iDB2DbType.iDB2Date).Value = evento.FechaEvento;
                sql.Command.Parameters.Add("i_ProcesoEvento", iDB2DbType.iDB2Char).Value = evento.ProcesoEvento;
                sql.Command.Parameters.Add("i_canal", iDB2DbType.iDB2Char).Value = evento.Canal;
                sql.Command.Parameters.Add("i_TipoIdentificacion", iDB2DbType.iDB2Char).Value = evento.TipoIdentificacion;
                sql.Command.Parameters.Add("i_NumeroId", iDB2DbType.iDB2Char).Value = evento.NumeroId;
                sql.Command.Parameters.Add("i_NombresCliente", iDB2DbType.iDB2Char).Value = evento.NombresCliente;
                sql.Command.Parameters.Add("i_Oficial", iDB2DbType.iDB2Char).Value = evento.Oficial;
                sql.Command.Parameters.Add("i_Oficina", iDB2DbType.iDB2Char).Value = evento.Oficina;
                sql.Command.Parameters.Add("i_FechaEmision", iDB2DbType.iDB2Date).Value = evento.FechaEmision;
                sql.Command.Parameters.Add("i_FechaVencimiento", iDB2DbType.iDB2Date).Value = evento.FechaVencimiento;
                sql.Command.Parameters.Add("i_FechaCancelacion", iDB2DbType.iDB2Date).Value = evento.FechaCancelacion;
                sql.Command.Parameters.Add("i_Plazo", iDB2DbType.iDB2Decimal).Value = evento.Plazo;
                sql.Command.Parameters.Add("i_Monto", iDB2DbType.iDB2Decimal).Value = evento.Monto;
                sql.Command.Parameters.Add("i_InteresGanar", iDB2DbType.iDB2Decimal).Value = evento.InteresGanar;
                sql.Command.Parameters.Add("i_Impuesto", iDB2DbType.iDB2Decimal).Value = evento.Impuesto;
                sql.Command.Parameters.Add("i_ValorNetoRecibir", iDB2DbType.iDB2Decimal).Value = evento.ValorNetoRecibir;
                sql.Command.Parameters.Add("i_InteresGanado", iDB2DbType.iDB2Decimal).Value = evento.InteresGanado;
                sql.Command.Parameters.Add("i_InteresRecibir", iDB2DbType.iDB2Decimal).Value = evento.InteresRecibir;
                sql.Command.Parameters.Add("i_CuentaCredito", iDB2DbType.iDB2Char).Value = evento.CuentaCredito;
                sql.Command.Parameters.Add("i_MontoPagadoCta", iDB2DbType.iDB2Decimal).Value = evento.MontoPagadoCta;
                sql.Command.Parameters.Add("i_MontoRenovado", iDB2DbType.iDB2Decimal).Value = evento.MontoRenovado;
                sql.Command.Parameters.Add("i_TipoRenovacion", iDB2DbType.iDB2Char).Value = evento.TipoRenovacion;*/

                //error
                sql.Command.Parameters.Add("p_Titulo", iDB2DbType.iDB2Char).Value = evento.MensajeValidacion;
                sql.Command.Parameters.Add("p_CodRet", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("p_MsgRet", iDB2DbType.iDB2Char).Value = string.Empty;

                //sql.Command.Parameters["p_Titulo"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_CodRet"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_MsgRet"].Direction = ParameterDirection.InputOutput;
                //

                respuesta = sql.EjecutaQuery();

                int codError = Convert.ToInt32(sql.Command.Parameters["p_CodRet"].Value);
                string msgError = Convert.ToString(sql.Command.Parameters["p_MsgRet"].Value).Trim();

                if (codError == 0)
                {
                  return msgError;
                }
                Log.Error("error al ejecutar SP SCDY023 : " + msgError);
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