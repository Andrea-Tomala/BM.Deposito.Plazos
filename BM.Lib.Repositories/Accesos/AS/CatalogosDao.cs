using BM.Lib.Domains;
using BM.Lib.Domains.AS.Catalogos;
using BM.Lib.Repositories.Conexion;
using BM.Lib.Repositories.Interfaces.AS;
using IBM.Data.DB2.iSeries;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Data;

namespace BM.Lib.Repositories.Accesos.AS
{
    public class CatalogosDao : ICatalogosDao
    {
        ConsultasAS sql;
        private readonly ILog Log = LogManager.GetLogger(typeof(LoggerManager));

        public List<FrecuenciaPagoInt> GetFrecuenciaPagoInt (decimal plazo)
        {
            sql = new ConsultasAS();
            List<FrecuenciaPagoInt> listaFrecuenciaPagos = new List<FrecuenciaPagoInt>();
            IDataReader dataReader;

            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY018";
                sql.Command.Parameters.Add("i_Plazo", iDB2DbType.iDB2Decimal).Value = plazo;

                //InOut
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
                        listaFrecuenciaPagos.Add(FrecuenciaPagoInt.ConFrecuenciaPagoIntDR(dataReader));
                    }
                    return listaFrecuenciaPagos;
                }
                Log.Error("error al ejecutar sp SCDY018 : " + msgError);
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
                //GC.SuppressFinalize(this);
            }
        }

        public List<TipoRenovacionInv> GetTipoRenovacionInv(string tipoPlazo)
        {
            sql = new ConsultasAS();
            List<TipoRenovacionInv> listaTipoRenovacion = new List<TipoRenovacionInv>();
            IDataReader dataReader;

            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY019";
                sql.Command.Parameters.Add("i_tipoPlazo", iDB2DbType.iDB2Char).Value = tipoPlazo;

                //InOut
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
                        listaTipoRenovacion.Add(TipoRenovacionInv.ConTipoRenovacionInvDR(dataReader));
                    }
                    return listaTipoRenovacion;
                }
                Log.Error("error al ejecutar sp SCDY019: " + msgError);
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
                //GC.SuppressFinalize(this);
            }
        }
    }
}