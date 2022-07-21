using BM.Lib.Domains;
using BM.Lib.Domains.AS.Inversion;
using BM.Lib.Repositories.Accesos;
using BM.Lib.Repositories.Accesos.AS;
using BM.Lib.Repositories.Interfaces;
using BM.Lib.Repositories.Interfaces.AS;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace BM_DepositoPlazo.Models
{
    public class CrearInversionDto
    {
        private static readonly bool isOnline = Convert.ToInt32(ConfigurationManager.AppSettings["Online"]) == 1;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //dao
        private IAuditoriaDao auditoriaDao;
        private ICrearInversionDao crearInversionDao;

        //
        private DatosAuditoria datosAuditoria;

        public CrearInversionResp CrearInversion(CrearInversionReq crearI)
        {
            CrearInversionResp crearInversion;
            datosAuditoria = new DatosAuditoria();
            crearInversionDao = new CrearInversionDao();
            auditoriaDao = new AuditoriaDao();

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(crearI);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = crearI.Auditoria.NutOrigen;
                    datosAuditoria.Ip = crearI.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsInversion";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;
                    Log.Info("Guardar Auditoria");

                    int id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    //Ejecutar Crear Inversion aS400
                    Log.Info("Ejecutar Crear Inversion SP.");
                    crearInversion = crearInversionDao.CrearInversion(crearI);
                    Log.Info("Crear Inversion con exito");

                    /*crearInversion = new CrearInversionResp();
                    crearInversion.FechaDeposito = crearI.FecEmision;
                    crearInversion.FechaVencimiento = crearI.FecVencimiento;
                    crearInversion.NumeroDeposito = 11;
                    crearInversion.Nombreoficial = "Johanna";*/

                    //crear inversión en sql aplicativa Graba Contrato Info
                    Log.Info("Ejecutar Crear Inversion SP BD Aplicativa");
                    int crea = crearInversionDao.GrabaContratoDeposito(crearI, crearInversion);

                }
                else
                {
                    crearInversion = new CrearInversionResp
                    {
                        NumeroDeposito = 12323,
                        FechaDeposito = DateTime.Now,
                        FechaVencimiento = DateTime.Now,
                        Nombreoficial = "johanna"
                    };
                    Log.Info("Servicio Offline Crear Inversion ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return crearInversion;
        }

        public List<ConsultarInversionesResp> ConsultarInversiones(ConsultaInversionesReq consultarInversionesReq)
        {
            List<ConsultarInversionesResp> listaInversionesResp = new List<ConsultarInversionesResp>();
            datosAuditoria = new DatosAuditoria();
            crearInversionDao = new CrearInversionDao();
            auditoriaDao = new AuditoriaDao();

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(consultarInversionesReq);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = consultarInversionesReq.Auditoria.NutOrigen;
                    datosAuditoria.Ip = consultarInversionesReq.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsInversion";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;
                    Log.Info("Guardar Auditoria: " + jsonString);

                    int id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    Log.Info("Ejecutar ConsultarInversiones SP.");
                    listaInversionesResp = crearInversionDao.ConsultarInversiones(consultarInversionesReq);
                    Log.Info("fin ConsultarInversiones SP.");
                }
                else
                {

                    Log.Info("Servicio Offline ConsultarInversiones.");
                }
            }
            catch
            {
                throw;
            }

            return listaInversionesResp;
        }


        public string ActualizarInversion(ActualizarInversionReq actualizarInversionReq)
        {
            string repuesta = "";
            datosAuditoria = new DatosAuditoria();
            crearInversionDao = new CrearInversionDao();
            auditoriaDao = new AuditoriaDao();

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(actualizarInversionReq);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = actualizarInversionReq.Auditoria.NutOrigen;
                    datosAuditoria.Ip = actualizarInversionReq.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsInversion";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;
                    Log.Info("Guardar Auditoria.");
                    
                    int id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    Log.Info("Ejecutar ActualizarInversion SP.");
                    repuesta = crearInversionDao.ActualizarInversion(actualizarInversionReq);
                    Log.Info("fin ActualizarInversion SP.");
                }
                else
                {
                    Log.Info("Servicio Offline ActualizarInversion.");
                }
            }
            catch
            {
                throw;
            }
            return repuesta;
        }

        public string ValidarParametros(ActualizarInversionReq req)
        {
            string Salida = "";
            if (string.IsNullOrEmpty(req.Identificacion.ToString()))
                Salida = "Debe ingresar Identificacion";
            if (string.IsNullOrEmpty(req.Titular))
                Salida = "Debe ingresar Titular";
            if (string.IsNullOrEmpty(req.Monto.ToString()))
                Salida = "Debe ingresar Monto";
            if (string.IsNullOrEmpty(req.FechaEmision.ToString()))
                Salida = "Debe ingresar FechaEmision";
            if (string.IsNullOrEmpty(req.FechaVencimiento.ToString()))
                Salida = "Debe ingresar FechaVencimiento";
            if (string.IsNullOrEmpty(req.PagoIntereses))
                Salida = "Debe ingresar PagoIntereses";
            if (string.IsNullOrEmpty(req.TipoRenovacion))
                Salida = "Debe ingresar TipoRenovacion";
            if (string.IsNullOrEmpty(req.CuentaCredito.ToString()))
                Salida = "Debe ingresar CuentaCredito";

            return Salida;
        }
    }
}
