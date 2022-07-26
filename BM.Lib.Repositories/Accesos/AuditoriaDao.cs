using BM.Lib.Domains;
using BM.Lib.Repositories.Conexion;
using BM.Lib.Repositories.Interfaces;
using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace BM.Lib.Repositories.Accesos
{
    public class AuditoriaDao : IAuditoriaDao
    {
        ConsultaSQL sql;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int IngresaLogRequest(DatosAuditoria datosAuditoria)
        {
            
            int codError = 0;
            string msgError;
            int idEjecuta;

            try
            {
                sql = new ConsultaSQL();
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SP_INV_ING_LogRequest";
                sql.Command.Parameters.AddWithValue("@psi_token", datosAuditoria.Token);
                sql.Command.Parameters.AddWithValue("@psi_ip", datosAuditoria.Ip);
                sql.Command.Parameters.AddWithValue("@psi_clase", datosAuditoria.Clase);
                sql.Command.Parameters.AddWithValue("@psi_metodo", datosAuditoria.Metodo);
                sql.Command.Parameters.AddWithValue("@psi_data", datosAuditoria.InputData);

                var codigoError = new SqlParameter("@pio_codigoError", SqlDbType.Int) { Direction = ParameterDirection.Output };
                sql.Command.Parameters.Add(codigoError);

                var mensajeError = new SqlParameter("@pso_mensajeError", SqlDbType.NVarChar, 150) { Direction = ParameterDirection.Output };
                sql.Command.Parameters.Add(mensajeError);

                idEjecuta = sql.EjecutaQuery();

                //Parametros de Salida
                codError = Convert.ToInt32(sql.Command.Parameters["@pio_codigoError"].Value);
                msgError = Convert.ToString(sql.Command.Parameters["@pso_mensajeError"].Value);

                if (codError != 0)
                {
                    Log.Error("error al ejecutar sp SP_INV_ING_LogRequest: " + msgError);
                    throw new ExcepcionSistema(msgError, codError);
                }
            }
            catch (SqlException ex)
            {
                Log.Error("SQL: " + ex.Message, ex);
                sql.CerrarConexion();
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

            return codError;
        }
    }
}
