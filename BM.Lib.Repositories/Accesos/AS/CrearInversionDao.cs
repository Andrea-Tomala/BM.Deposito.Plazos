using BM.Lib.Domains;
using BM.Lib.Domains.AS.Inversion;
using BM.Lib.Repositories.Conexion;
using BM.Lib.Repositories.Interfaces.AS;
using IBM.Data.DB2.iSeries;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using BM.Lib.Domains.AS.Catalogos;
using System.Text;
using Microsoft.SqlServer.Server;
using BM.Lib.Repositories.Interfaces;
using System.Configuration;

namespace BM.Lib.Repositories.Accesos.AS
{
    public class CrearInversionDao : ICrearInversionDao
    {
        ConsultasAS sql;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ICatalogosDao catalogosDao;
        private IParametrosDao parametrosDao;

        public CrearInversionResp CrearInversion(CrearInversionReq req)
        {
            sql = new ConsultasAS();
            CrearInversionResp result = new CrearInversionResp();
            decimal numDoc, fechaEmi, fechaVenc, montoDebito;
            string cuentaDebito, beneficiario, cuentaCredito, nombresR;
            int respuesta;

            try
            {
                numDoc = Convert.ToDecimal(req.Identificacion);
                fechaEmi = Convert.ToDecimal(req.FecEmision.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
                fechaVenc = Convert.ToDecimal(req.FecVencimiento.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
                cuentaDebito = req.CuentasADebitar.FirstOrDefault().Prefijo + req.CuentasADebitar.FirstOrDefault().Cuenta;
                montoDebito = Convert.ToDecimal(req.CuentasADebitar.FirstOrDefault().Monto);
                cuentaCredito = req.CuentaAcreditar.Prefijo + req.CuentaAcreditar.Cuenta;

                if (req.AgregarBeneficiario)
                    beneficiario = req.Beneficiarios.FirstOrDefault().Nombres;
                else
                    beneficiario = req.Nombres;

                if (req.Nombres.Length < 20)
                    nombresR = req.Nombres.PadRight(20, ' ');
                else
                    nombresR = req.Nombres;

                nombresR = nombresR.Substring(0, 20);

                XDocument xmlBeneficiarios = ListaBeneficiariosXML(req.Beneficiarios);

                XDocument xmlCuentasDeb = ListaCuentasDebitosXML(req.CuentasADebitar);


                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY020";
                sql.Command.Parameters.Add("xmlBeneficiariosIn", iDB2DbType.iDB2Xml).Value = xmlBeneficiarios.ToString();

                sql.Command.Parameters.Add("xmlCuentasDebitoIn", iDB2DbType.iDB2Xml).Value = xmlCuentasDeb.ToString();

                sql.Command.Parameters.Add("i_Titular", iDB2DbType.iDB2Char).Value = nombresR;
                sql.Command.Parameters.Add("i_TipDocume", iDB2DbType.iDB2Char).Value = req.TipoIdentificacion;
                sql.Command.Parameters.Add("i_NumDocume", iDB2DbType.iDB2Decimal).Value = numDoc;
                
                //el nombre primer beneficiario 
                sql.Command.Parameters.Add("i_Beneficiar", iDB2DbType.iDB2Char).Value = beneficiario;
                
                //la primera cuenta de debitos
                sql.Command.Parameters.Add("i_CueDebito", iDB2DbType.iDB2Decimal).Value = cuentaDebito;
                sql.Command.Parameters.Add("i_MonDebito", iDB2DbType.iDB2Decimal).Value = montoDebito;
                sql.Command.Parameters.Add("i_MonInversi", iDB2DbType.iDB2Decimal).Value = req.MontoInvertir;

                sql.Command.Parameters.Add("i_diaPlazo", iDB2DbType.iDB2Decimal).Value = req.DiasPlazo;
                sql.Command.Parameters.Add("i_tasNominal", iDB2DbType.iDB2Decimal).Value = req.Tasa;
                
                sql.Command.Parameters.Add("i_intCalcula", iDB2DbType.iDB2Decimal).Value = req.InteresGanar;
                //impuesto
                sql.Command.Parameters.Add("i_intRetenid", iDB2DbType.iDB2Decimal).Value = req.Impuesto;
                //
                sql.Command.Parameters.Add("i_NetRecibir", iDB2DbType.iDB2Decimal).Value = req.InteresRecibir;
                
                sql.Command.Parameters.Add("i_fecEmision", iDB2DbType.iDB2Decimal).Value = fechaEmi;
                sql.Command.Parameters.Add("i_fecVencimi", iDB2DbType.iDB2Decimal).Value = fechaVenc;
                sql.Command.Parameters.Add("i_forPago", iDB2DbType.iDB2Char).Value = req.PagoInteres;
                sql.Command.Parameters.Add("i_tipRenova", iDB2DbType.iDB2Char).Value = req.VencimientoInversion;

                sql.Command.Parameters.Add("i_valIntMens", iDB2DbType.iDB2Decimal).Value = req.InteresGanar;
                sql.Command.Parameters.Add("i_valTotMens", iDB2DbType.iDB2Decimal).Value = req.InteresRecibir;

                sql.Command.Parameters.Add("i_CueCredito", iDB2DbType.iDB2Decimal).Value = cuentaCredito;

                //sql.Command.Parameters.Add("i_revAutomat", iDB2DbType.iDB2Char).Value = string.Empty;
                //sql.Command.Parameters.Add("i_otpReferen", iDB2DbType.iDB2Char).Value = req.ReferenciaOTP;
                //sql.Command.Parameters.Add("i_otpCodigo", iDB2DbType.iDB2Char).Value = req.CodigoOTP;
                //sql.Command.Parameters.Add("i_codOfiOcre", iDB2DbType.iDB2Decimal).Value = req.CodigoOficinaOficialCredito;

                //InOut
                sql.Command.Parameters.Add("r_numDeposito", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_fecEmision", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_fecVencimi", iDB2DbType.iDB2Decimal).Value = 0;
                //nuevo
                sql.Command.Parameters.Add("r_tasefectiv", iDB2DbType.iDB2Decimal).Value = 0;
                //
                sql.Command.Parameters.Add("r_nomOfiCre", iDB2DbType.iDB2Char).Value = string.Empty;

                sql.Command.Parameters.Add("r_intMensua", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("r_intRecmes", iDB2DbType.iDB2Decimal).Value = 0;

                sql.Command.Parameters["r_numDeposito"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_fecEmision"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_fecVencimi"].Direction = ParameterDirection.InputOutput;
                //decimal inout
                sql.Command.Parameters["r_tasefectiv"].Direction = ParameterDirection.InputOutput;

                sql.Command.Parameters["r_nomOfiCre"].Direction = ParameterDirection.InputOutput;

                sql.Command.Parameters["r_intMensua"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["r_intRecmes"].Direction = ParameterDirection.InputOutput;

                //error
                sql.Command.Parameters.Add("p_Titulo", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("p_CodRet", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("p_MsgRet", iDB2DbType.iDB2Char).Value = string.Empty;

                sql.Command.Parameters["p_Titulo"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_CodRet"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_MsgRet"].Direction = ParameterDirection.InputOutput;
                //
                Log.Info("Antes de ejeuctar sp");
                respuesta = sql.EjecutaQuery();

                int codError = Convert.ToInt32(sql.Command.Parameters["p_CodRet"].Value);
                string msgError = Convert.ToString(sql.Command.Parameters["p_MsgRet"].Value).Trim();

                if (codError == 0)
                {
                    result.NumeroDeposito = Convert.ToInt32(sql.Command.Parameters["r_numDeposito"].Value);
                    result.FechaDeposito = DateTime.ParseExact(sql.Command.Parameters["r_fecEmision"].Value.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                    result.FechaVencimiento = DateTime.ParseExact(sql.Command.Parameters["r_fecVencimi"].Value.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                    result.Nombreoficial = Convert.ToString(sql.Command.Parameters["r_nomOfiCre"].Value).Trim();
                    result.TasaEfectiva = Convert.ToDecimal(sql.Command.Parameters["r_tasefectiv"].Value);
                    result.InteresMensual = Convert.ToDecimal(sql.Command.Parameters["r_intmensua"].Value);
                    result.InteresMennsualRec = Convert.ToDecimal(sql.Command.Parameters["r_intrecmes"].Value);

                    return result;
                }
                Log.Error("error al ejecutar SP SCDY020 : " + msgError);
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

        public int GrabaContratoDeposito(CrearInversionReq crearInversion, CrearInversionResp inversionResponse)
        {
            ConsultaSQL sql = new ConsultaSQL();
            int codError = 0;
            string msgError;
            int idEjecuta;

            XDocument xmlDatosDeposito = DatosDepositoXML(crearInversion);
            XDocument xmlBeneficiarios = ListaBeneficiariosXML(crearInversion.Beneficiarios);
            XDocument xmlDatosContrato = DatosContratoXML(crearInversion, inversionResponse);
            XDocument xmlDatosNotifica = DatosNotificacionXML(crearInversion.Nombres, inversionResponse);

            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SP_IVD_ING_RegistroDeposito";
                sql.Command.Parameters.AddWithValue("@Psi_TokenRequest", crearInversion.Auditoria.NutOrigen);
                sql.Command.Parameters.AddWithValue("@Psi_Identificacion", crearInversion.Identificacion);
                sql.Command.Parameters.AddWithValue("@Psi_NumDeposito", inversionResponse.NumeroDeposito);
                sql.Command.Parameters.AddWithValue("@Psi_NombresCliente", crearInversion.Nombres);
                sql.Command.Parameters.AddWithValue("@Pxi_DatosDeposito", xmlDatosDeposito.ToString());
                sql.Command.Parameters.AddWithValue("@Pxi_DatosBeneficiarios", xmlBeneficiarios.ToString());
                sql.Command.Parameters.AddWithValue("@Psi_OficinaCaptacion", crearInversion.CodigoOficinaOficialCredito);
                sql.Command.Parameters.AddWithValue("@Psi_CodigoActivacion", crearInversion.CodigoOTP);
                sql.Command.Parameters.AddWithValue("@Pbi_AceptacionContrato", crearInversion.AceptacionContrato);
                sql.Command.Parameters.AddWithValue("@Psi_CanalCaptacion", crearInversion.CanalCaptacion);
                sql.Command.Parameters.AddWithValue("@Pdi_FechaCaptacion", inversionResponse.FechaDeposito);
                sql.Command.Parameters.AddWithValue("@Pdi_FechaVencimiento", inversionResponse.FechaVencimiento);
                sql.Command.Parameters.AddWithValue("@Psi_EmailNotificacion", crearInversion.Email);
                sql.Command.Parameters.AddWithValue("@Pxi_DatosNotificacion", xmlDatosNotifica.ToString());
                sql.Command.Parameters.AddWithValue("@Pxi_DatosContrato", xmlDatosContrato.ToString());
                sql.Command.Parameters.AddWithValue("@Psi_UsuarioProceso", crearInversion.Auditoria.UsuarioFinal);

                var codigoError = new SqlParameter("@Pio_CodigoError", SqlDbType.Int) { Direction = ParameterDirection.Output };
                sql.Command.Parameters.Add(codigoError);

                var mensajeError = new SqlParameter("@Pso_MensajeError", SqlDbType.NVarChar, 150) { Direction = ParameterDirection.Output };
                sql.Command.Parameters.Add(mensajeError);

                idEjecuta = sql.EjecutaQuery();

                //Parametros de Salida
                codError = Convert.ToInt32(sql.Command.Parameters["@Pio_CodigoError"].Value);
                msgError = Convert.ToString(sql.Command.Parameters["@Pso_MensajeError"].Value);

                if (codError != 0)
                {
                    Log.Error("error al ejecutar sp SP_IVD_ING_CapRegistroDeposito : " + msgError);
                    throw new ExcepcionSistema(msgError, codError);
                }
            }
            catch (SqlException ex)
            {
                sql.CerrarConexion();
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

            return codError;
        }

        private XDocument ListaBeneficiariosXML(List<Beneficiario> listBeneficiarios)
        {
            XNamespace xns = "";
            XDeclaration xDeclaration = new XDeclaration("1.0", "utf-8", "yes");
            XDocument xDoc = new XDocument(xDeclaration);
            XElement xRoot = new XElement(xns + "xmlBeneficiariosIn");
            xDoc.Add(xRoot);

            listBeneficiarios.ForEach(x =>
            {
                XElement XE_Beneficiarios = new XElement(xns + "BeneficiarioIn");
                XElement tipoCed = new XElement(xns + "tipoCedula", x.TipoIdent);
                XElement cedula = new XElement(xns + "numeroCedula", x.Cedula);
                XElement nombres = new XElement(xns + "nombreCedula", x.Nombres);
                XE_Beneficiarios.Add(tipoCed);
                XE_Beneficiarios.Add(cedula);
                XE_Beneficiarios.Add(nombres);
                xRoot.Add(XE_Beneficiarios);
            });
            return xDoc;
        }

        private XDocument ListaCuentasDebitosXML(List<CuentasDebito> listaCuentas)
        {
            string formaPago = ConfigurationManager.AppSettings["formaPagoDeb"];
            string smonto = "";
            XNamespace xns = "";
            XDeclaration xDeclaration = new XDeclaration("1.0", "utf-8", "yes");
            XDocument xDoc = new XDocument(xDeclaration);
            XElement xRoot = new XElement(xns + "xmlCuentasDebitoIn");
            xDoc.Add(xRoot);

            listaCuentas.ForEach(x =>
            {
                smonto = DosDecimales(x.Monto.ToString());

                XElement XE_CuentasDebito = new XElement(xns + "CuentasDebitoIn");
                XElement formaDeb = new XElement(xns + "formaDebito", formaPago);
                XElement prefijo = new XElement(xns + "prefijoDebito", x.Prefijo);
                XElement cuenta = new XElement(xns + "cuentaDebito", x.Cuenta);
                XElement montoDebito = new XElement(xns + "valorDebito", smonto);
                XE_CuentasDebito.Add(formaDeb);
                XE_CuentasDebito.Add(prefijo);
                XE_CuentasDebito.Add(cuenta);
                XE_CuentasDebito.Add(montoDebito);
                xRoot.Add(XE_CuentasDebito);
            });
            return xDoc;
        }

        private XDocument DatosDepositoXML(CrearInversionReq datos)
        {
            List<CuentasDebito> listCuentasDebito;
            XNamespace xns = "";
            XDeclaration xDeclaration = new XDeclaration("1.0", "utf-8", "yes");
            XDocument xDoc = new XDocument(xDeclaration);
            XElement xRoot = new XElement(xns + "DatosDeposito");
            xDoc.Add(xRoot);

            XElement XE_DatosDeposito = new XElement(xns + "Datos");
            XElement nombres = new XElement(xns + "NOMBRES", datos.Nombres);
            XElement monto = new XElement(xns + "MONTO", datos.MontoInvertir);
            XElement plazo = new XElement(xns + "PLAZO", datos.DiasPlazo);
            XElement tasa = new XElement(xns + "TASA", datos.Tasa);
            XElement intRecibir = new XElement(xns + "INTERESRECIBE", datos.InteresRecibir);

            XE_DatosDeposito.Add(nombres);
            XE_DatosDeposito.Add(monto);
            XE_DatosDeposito.Add(plazo);
            XE_DatosDeposito.Add(tasa);
            XE_DatosDeposito.Add(intRecibir);
            xRoot.Add(XE_DatosDeposito);



            listCuentasDebito = datos.CuentasADebitar;
            listCuentasDebito.ForEach(x =>
            {
                XElement XE_CuentasDebito = new XElement(xns + "CuentasDebito");
                XElement nombre = new XElement(xns + "Nombre", x.Nombre);
                XElement prefijo = new XElement(xns + "Prefijo", x.Prefijo);
                XElement cuenta = new XElement(xns + "Cuenta", x.Cuenta);
                XElement codigoProducto = new XElement(xns + "CodigoProducto", x.CodigoProducto);
                XElement tipoProducto = new XElement(xns + "TipoProducto", x.TipoProducto);
                XElement montoDebito = new XElement(xns + "MontoDebito", x.Monto);
                XE_CuentasDebito.Add(nombre);
                XE_CuentasDebito.Add(prefijo);
                XE_CuentasDebito.Add(cuenta);
                XE_CuentasDebito.Add(codigoProducto);
                XE_CuentasDebito.Add(tipoProducto);
                XE_CuentasDebito.Add(montoDebito);
                xRoot.Add(XE_CuentasDebito);
            });

            return xDoc;
        }

        private XDocument DatosContratoXML(CrearInversionReq datos, CrearInversionResp inversionResp)
        {
            List<CuentasDebito> listCuentasDebito;
            List<Beneficiario> listBeneficiarios;
            List<FrecuenciaPagoInt> listaFrecuenciaPagos = new List<FrecuenciaPagoInt>();
            //List<TipoRenovacionInv> listaTipoRenovacion = new List<TipoRenovacionInv>();
            catalogosDao = new CatalogosDao();
            FrecuenciaPagoInt frecuenciaPago = new FrecuenciaPagoInt();
            //TipoRenovacionInv tipoRenovacionInv = new TipoRenovacionInv();
            Parametros parametrosRenova = new Parametros();
            string pagoInteres = "", renova ="", smonto, sintRecibe, sintPagar, stasaEfec, sintMen, sintMensR, simp, smontoDeb, stasa;
            parametrosDao = new ParametrosDao();
            List<Parametros> listaParametros = new List<Parametros>();

            //Obtener Catalogos
            //Ejecutar Get Frecuencia Pago Int
            try
            {
                Log.Info("DatosContratoXML> Ejecutar Get Frecuencia Pago Int SP.");
                listaFrecuenciaPagos = catalogosDao.GetFrecuenciaPagoInt(Convert.ToDecimal(datos.DiasPlazo));
                Log.Info("Get Frecuencia Pago Interes con exito");

                frecuenciaPago = listaFrecuenciaPagos.Find(x => x.CodFrecuencia == datos.PagoInteres);

                if (string.IsNullOrEmpty(frecuenciaPago.DescFrecuencia))
                    pagoInteres = datos.PagoInteres;
                else
                    pagoInteres = frecuenciaPago.DescFrecuencia;

                Log.Info("DatosContratoXML> Ejecutar GetTipoRenovacionInv SP.");
                //listaTipoRenovacion = catalogosDao.GetTipoRenovacionInv(frecuenciaPago.CodFrecuencia);

                listaParametros = parametrosDao.ConsultaParametros();
                
                //Log.Info("GetTipoRenovacionInv con exito");
                
                //Cambiar descripcion
                parametrosRenova = listaParametros.Find(x => x.NombreParametro == datos.VencimientoInversion);

                if (string.IsNullOrEmpty(parametrosRenova.Valor))
                    renova = datos.VencimientoInversion;
                else
                    renova = parametrosRenova.Valor;
            }
            catch
            {
                Log.Info("Error en la consulta de catalogos");
                pagoInteres = datos.PagoInteres;
                renova = datos.VencimientoInversion;
                
            }

            smonto = DosDecimales(datos.MontoInvertir.ToString());
            sintRecibe = DosDecimales(datos.InteresRecibir.ToString());
            sintPagar = DosDecimales(datos.InteresGanar.ToString());
            simp = DosDecimales(datos.Impuesto.ToString());
            sintMen = DosDecimales(inversionResp.InteresMensual.ToString());
            sintMensR = DosDecimales(inversionResp.InteresMennsualRec.ToString());
            stasaEfec = DosDecimales(inversionResp.TasaEfectiva.ToString());
            stasa = DosDecimales(datos.Tasa.ToString());

            XNamespace xns = "";
            XDeclaration xDeclaration = new XDeclaration("1.0", "utf-8", "yes");
            XDocument xDoc = new XDocument(xDeclaration);
            XElement xRoot = new XElement(xns + "datos");
            xDoc.Add(xRoot);

            XElement XE_DatosDeposito = new XElement(xns + "DatosDeposito");
            XElement nombres = new XElement("dato", new XAttribute("campo", "NOMBRES"), new XAttribute("valor", datos.Nombres));
            XElement monto = new XElement("dato", new XAttribute("campo", "MONTO"), new XAttribute("valor", smonto));
            XElement plazo = new XElement("dato", new XAttribute("campo", "PLAZO"), new XAttribute("valor", datos.DiasPlazo));
            XElement tasa = new XElement("dato", new XAttribute("campo", "TASA"), new XAttribute("valor", stasa));
            //XElement intRecibir = new XElement("dato", new XAttribute("campo", "INTERESRECIBE"), new XAttribute("valor", datos.InteresRecibir));

            XElement intRecibir = new XElement("dato", new XAttribute("campo", "INTERESRECIBE"), new XAttribute("valor", sintRecibe));
            XElement fechaEmi = new XElement("dato", new XAttribute("campo", "FECHAEMISION"), new XAttribute("valor", inversionResp.FechaDeposito));
            XElement fechaVenc = new XElement("dato", new XAttribute("campo", "FECHAVENCIMIENTO"), new XAttribute("valor", inversionResp.FechaVencimiento));
            XElement codigoOTP = new XElement("dato", new XAttribute("campo", "CODIGOOTP"), new XAttribute("valor", datos.CodigoOTP));
            //NUEVOS
            //XElement impuesto = new XElement("dato", new XAttribute("campo", "IMPUESTO"), new XAttribute("valor", datos.Impuesto));
            XElement impuesto = new XElement("dato", new XAttribute("campo", "IMPUESTO"), new XAttribute("valor", simp));

            XElement oficial = new XElement("dato", new XAttribute("campo", "OFICIAL"), new XAttribute("valor", string.Concat(datos.CodigoOficinaOficialCredito, "-", inversionResp.Nombreoficial)));
            //XElement intGana = new XElement("dato", new XAttribute("campo", "INTERESGANAR"), new XAttribute("valor", datos.InteresGanar));
            XElement intGana = new XElement("dato", new XAttribute("campo", "INTERESGANAR"), new XAttribute("valor", sintPagar));

            XElement pagoInt = new XElement("dato", new XAttribute("campo", "PAGOINT"), new XAttribute("valor", pagoInteres));
            XElement renovacion = new XElement("dato", new XAttribute("campo", "RENOVA"), new XAttribute("valor", renova));
            XElement prefijoC = new XElement("dato", new XAttribute("campo", "PREFIJOC"), new XAttribute("valor", datos.CuentaAcreditar.Prefijo));
            XElement cuentaC = new XElement("dato", new XAttribute("campo", "CUENTAC"), new XAttribute("valor", datos.CuentaAcreditar.Cuenta)); 
            XElement tasaEfectiva = new XElement("dato", new XAttribute("campo", "TASAEFECTIVA"), new XAttribute("valor", stasaEfec));

            XElement intMensual = new XElement("dato", new XAttribute("campo", "INTMENSUAL"), new XAttribute("valor", sintMen));
            XElement intMensualRec = new XElement("dato", new XAttribute("campo", "INTMENSUALREC"), new XAttribute("valor", sintMensR));

            XE_DatosDeposito.Add(nombres);
            
            XE_DatosDeposito.Add(monto);
            XE_DatosDeposito.Add(plazo);
            XE_DatosDeposito.Add(tasa);
            XE_DatosDeposito.Add(intRecibir);
            XE_DatosDeposito.Add(fechaEmi);
            XE_DatosDeposito.Add(fechaVenc);
            XE_DatosDeposito.Add(codigoOTP);
            //
            XE_DatosDeposito.Add(impuesto);
            XE_DatosDeposito.Add(oficial);
            XE_DatosDeposito.Add(intGana);
            XE_DatosDeposito.Add(pagoInt);
            XE_DatosDeposito.Add(renovacion);
            XE_DatosDeposito.Add(prefijoC);
            XE_DatosDeposito.Add(cuentaC);
            XE_DatosDeposito.Add(tasaEfectiva);
            XE_DatosDeposito.Add(intMensual);
            XE_DatosDeposito.Add(intMensualRec);
            xRoot.Add(XE_DatosDeposito);

            listBeneficiarios = datos.Beneficiarios;
            listBeneficiarios.ForEach(x =>
            {
                XElement XE_Beneficiarios = new XElement(xns + "BeneficiarioIn");
                XElement nombresB = new XElement("NOMBRESB", x.Nombres);
                XElement cedulaB = new XElement("CEDULAB", x.Cedula);

                XE_Beneficiarios.Add(cedulaB);
                XE_Beneficiarios.Add(nombresB);
                xRoot.Add(XE_Beneficiarios);
            });
            

            listCuentasDebito = datos.CuentasADebitar;
            listCuentasDebito.ForEach(y =>
            {
                smontoDeb = DosDecimales(y.Monto.ToString());

                XElement XE_CuentasDebito = new XElement(xns + "CuentasDebito");
                XElement nombresD = new XElement("NOMBRESD", y.Nombre);
                XElement prefijoD = new XElement("PREFIJOD", y.Prefijo);
                XElement cuentaD = new XElement("CUENTAD", y.Cuenta);
                XElement montoD = new XElement("MONTODEB", smontoDeb);

                XE_CuentasDebito.Add(nombresD);
                XE_CuentasDebito.Add(prefijoD);
                XE_CuentasDebito.Add(cuentaD);
                XE_CuentasDebito.Add(montoD);
                xRoot.Add(XE_CuentasDebito);
            });

            return xDoc;
        }

        private string DosDecimales(string cadena)
        {
            string entero = "", decimales="", resultado="";
            try
            {
                //identificar si tiene punto o coma
                if (cadena.IndexOf(',', 0) > 0)
                {
                    entero = cadena.Substring(0, cadena.IndexOf(',', 0) + 1);
                    decimales = cadena.Substring(cadena.IndexOf(',', 0) + 1);

                    if (!string.IsNullOrEmpty(decimales) && decimales.Length < 2)
                    {
                        decimales = decimales.PadRight(2, '0');
                    }
                    else
                    {
                        decimales = decimales.Substring(0, 2);
                    }

                    resultado = entero.Replace(',', '.') + decimales;
                }
                else if (cadena.IndexOf('.', 0) > 0)
                {
                    entero = cadena.Substring(0, cadena.IndexOf('.', 0) + 1);
                    decimales = cadena.Substring(cadena.IndexOf('.', 0) + 1);

                    if (!string.IsNullOrEmpty(decimales) && decimales.Length < 2)
                    {
                        decimales = decimales.PadRight(2, '0');
                    }
                    else
                    {
                        decimales = decimales.Substring(0, 2);
                    }

                    resultado = entero + decimales;
                }
                else
                    resultado = string.Concat(cadena, ".00");

            }
            catch
            {
                Log.Info("Error al convertir a string: " + cadena);
                resultado = cadena;
            }

            return resultado;
        }

        private XDocument DatosNotificacionXML(string nombres, CrearInversionResp inversionResp)
        {
            XNamespace xns = "";
            XDeclaration xDeclaration = new XDeclaration("1.0", "utf-8", "yes");
            XDocument xDoc = new XDocument(xDeclaration);
            XElement xRoot = new XElement(xns + "correo");
            xDoc.Add(xRoot);

            XElement XE_DatosNotifica = new XElement(xns + "datos");
            XElement nombresC = new XElement("dato", new XAttribute("campo", "NOMBRES"), new XAttribute("valor", nombres));
            XElement deposito = new XElement("dato", new XAttribute("campo", "DEPOSITO"), new XAttribute("valor", inversionResp.NumeroDeposito));

            XE_DatosNotifica.Add(nombresC);
            XE_DatosNotifica.Add(deposito);
            xRoot.Add(XE_DatosNotifica);

            return xDoc;
        }

        public List<ConsultarInversionesResp> ConsultarInversiones(ConsultaInversionesReq conReq)
        {
            sql = new ConsultasAS();
            List<ConsultarInversionesResp> listaInversionesResp = new List<ConsultarInversionesResp>();
            IDataReader dataReader;

            try
            {
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY021";
                sql.Command.Parameters.Add("i_TipDocume", iDB2DbType.iDB2Char).Value = conReq.TipoIdentificacion;
                sql.Command.Parameters.Add("i_NumDocume", iDB2DbType.iDB2Decimal).Value = Convert.ToDecimal(conReq.Identificacion);

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
                        listaInversionesResp.Add(ConsultarInversionesResp.ConsultarInversionesDR(dataReader));
                    }
                    
                    return listaInversionesResp;
                }
                Log.Error("error al ejecutar SP SCDY021 : " + msgError);
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

        public string ActualizarInversion(ActualizarInversionReq req)
        {
            string msgError;
            sql = new ConsultasAS();
            decimal numDoc, fechaEmi, fechaVenc;
            int respuesta;

            try
            {
                numDoc = Convert.ToDecimal(req.Identificacion);
                fechaEmi = Convert.ToDecimal(req.FechaEmision.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                fechaVenc = Convert.ToDecimal(req.FechaVencimiento.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture));
                sql.Command.CommandType = CommandType.StoredProcedure;
                sql.Command.CommandText = "SIAFO06.SCDY022";
                
                sql.Command.Parameters.Add("i_TipDocume", iDB2DbType.iDB2Char).Value = req.TipoIdent;
                sql.Command.Parameters.Add("i_NumDocume", iDB2DbType.iDB2Decimal).Value = numDoc;
                sql.Command.Parameters.Add("i_NumCer", iDB2DbType.iDB2Decimal).Value = req.NumDeposito;
                sql.Command.Parameters.Add("i_Titular", iDB2DbType.iDB2Char).Value = req.Titular;
                sql.Command.Parameters.Add("i_MonInversi", iDB2DbType.iDB2Decimal).Value = req.Monto;
                sql.Command.Parameters.Add("i_fecEmision", iDB2DbType.iDB2Decimal).Value = fechaEmi;
                sql.Command.Parameters.Add("i_fecVencimi", iDB2DbType.iDB2Decimal).Value = fechaVenc;
                sql.Command.Parameters.Add("i_estInversi", iDB2DbType.iDB2Char).Value = req.EstadoDeposito;
                sql.Command.Parameters.Add("i_frePagoInt", iDB2DbType.iDB2Char).Value = req.PagoIntereses;
                sql.Command.Parameters.Add("i_tipRenovac", iDB2DbType.iDB2Char).Value = req.TipoRenovacion;

                sql.Command.Parameters.Add("i_CueCredito", iDB2DbType.iDB2Decimal).Value = req.CuentaCredito;
                //error
                sql.Command.Parameters.Add("p_Titulo", iDB2DbType.iDB2Char).Value = string.Empty;
                sql.Command.Parameters.Add("p_CodRet", iDB2DbType.iDB2Decimal).Value = 0;
                sql.Command.Parameters.Add("p_MsgRet", iDB2DbType.iDB2Char).Value = string.Empty;

                sql.Command.Parameters["p_Titulo"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_CodRet"].Direction = ParameterDirection.InputOutput;
                sql.Command.Parameters["p_MsgRet"].Direction = ParameterDirection.InputOutput;
                //

                respuesta = sql.EjecutaQuery();

                int codError = Convert.ToInt32(sql.Command.Parameters["p_CodRet"].Value);
                msgError = Convert.ToString(sql.Command.Parameters["p_MsgRet"].Value).Trim();

                if (codError == 0)
                {
                    return msgError;
                }

                Log.Error("error al ejecutar SP SCDY022 : " + msgError);
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


    }
}
