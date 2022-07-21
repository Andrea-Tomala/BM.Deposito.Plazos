using BM.Lib.Domains;
using BM.Lib.Domains.AS.Catalogos;
using BM.Lib.Repositories.Accesos;
using BM.Lib.Repositories.Accesos.AS;
using BM.Lib.Repositories.Interfaces;
using BM.Lib.Repositories.Interfaces.AS;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;

namespace BM_DepositoPlazo.Models
{
    public class CatalogosDto
    {
        private static readonly bool isOnline = Convert.ToInt32(ConfigurationManager.AppSettings["Online"]) == 1;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //Dao
        private IAuditoriaDao auditoriaDao;
        private ICatalogosDao catalogosDao;
        private IParametrosDao parametrosDao;

        //
        private DatosAuditoria datosAuditoria;

        public string ValidarParametros(Auditoria auditoria)
        {
            string Salida = "";
            if (string.IsNullOrEmpty(auditoria.Cliente))
                Salida = "Debe ingresar Cliente";
            if (string.IsNullOrEmpty(auditoria.NutOrigen))
                Salida = "Debe ingresar Token";
            if (string.IsNullOrEmpty(auditoria.PuntoAcceso))
                Salida = "Debe ingresar Identificador de Acceso";
            if (string.IsNullOrEmpty(Convert.ToString(auditoria.TipoPuntoAcceso)))
                Salida = "Debe ingresar Tipo de Acceso 0 = IP, 1 = Dspositivo, 2 = Otro";
            if (string.IsNullOrEmpty(auditoria.UsuarioFinal))
                Salida = "Debe ingresar UsuarioFinal";

            return Salida;
        }

        public List<FrecuenciaPagoInt> GetFrecuenciaPagoInt(FrecuenciaPagoIntReq frecuenciaPago)
        {
            List<FrecuenciaPagoInt> listaFrecuenciaPagos = new List<FrecuenciaPagoInt>();
            datosAuditoria = new DatosAuditoria();
            auditoriaDao = new AuditoriaDao();
            catalogosDao = new CatalogosDao();

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(frecuenciaPago);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = frecuenciaPago.Auditoria.NutOrigen;
                    datosAuditoria.Ip = frecuenciaPago.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsCatalogos";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;

                    Log.Info("Guardar Auditoria: " + jsonString);
                    int id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    //Ejecutar Get Frecuencia Pago Int
                    Log.Info("Ejecutar Get Frecuencia Pago Int SP.");
                    listaFrecuenciaPagos = catalogosDao.GetFrecuenciaPagoInt(frecuenciaPago.Plazo);
                    Log.Info("Get Frecuencia Pago Interes con exito");
                }
                else
                {
                    listaFrecuenciaPagos.Add(new FrecuenciaPagoInt
                    {
                        CodFrecuencia = "V",
                        DescFrecuencia = "Vencimiento"
                    });
                    Log.Info("Servicio Offline Get Frecuencia Pago Int ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return listaFrecuenciaPagos;
        }

        public List<TipoRenovacionInv> GetTipoRenovacionInv(TipoRenovInvReq tipoRenov)
        {
            List<TipoRenovacionInv> listaTipoRenovacion = new List<TipoRenovacionInv>();
            List<TipoRenovacionInv> listaRenovacion = new List<TipoRenovacionInv>();
            datosAuditoria = new DatosAuditoria();
            auditoriaDao = new AuditoriaDao();
            catalogosDao = new CatalogosDao();
            parametrosDao = new ParametrosDao();
            
            List<Parametros> ListaParametros = new List<Parametros>();

            string descripcion = "";

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(tipoRenov);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = tipoRenov.Auditoria.NutOrigen;
                    datosAuditoria.Ip = tipoRenov.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsCatalogos";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;

                    Log.Info("Guardar Auditoria: " + jsonString);
                    int id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    //Ejecutar Get Tipo Renovacion Inv
                    Log.Info("Ejecutar Get Tipo Renovacion Inv SP.");
                    listaTipoRenovacion = catalogosDao.GetTipoRenovacionInv(tipoRenov.TipoPlazo);

                    Log.Info("Get Tipo Renovacion Inversion con exito");

                    ListaParametros = parametrosDao.ConsultaParametros();

                    //cambiar descripcion
                    listaTipoRenovacion.ForEach(x =>
                    {
                        descripcion = "";
                        descripcion = ListaParametros.Find(y => y.NombreParametro == x.CodTipoRenovacion).Valor;
                        
                        if (string.IsNullOrEmpty(descripcion))
                        {
                            descripcion = x.DescTipoRenovacion;
                        }
                        listaRenovacion.Add(new TipoRenovacionInv { CodTipoRenovacion = x.CodTipoRenovacion, DescTipoRenovacion = descripcion });

                        Log.Info("Nueva Lista de Renovaciones");
                    });
                }
                else
                {
                    listaRenovacion.Add(new TipoRenovacionInv
                    {
                        CodTipoRenovacion = "NR",
                        DescTipoRenovacion = "no renovacion"
                    });
                    Log.Info("Servicio Offline GetTipoRenovacionInv ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return listaRenovacion;
        }
    }
}
