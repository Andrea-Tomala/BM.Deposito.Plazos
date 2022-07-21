using BM.Lib.Domains;
using BM.Lib.Domains.AS;
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
    public class InversionDto
    {
        private static readonly bool isOnline = Convert.ToInt32(ConfigurationManager.AppSettings["Online"]) == 1;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //Dao
        private  ITableroDao tableroDao;
        private IProyeccion proyeccionDao;
        private IRendimientoDao rendimientoDao;
        private IAuditoriaDao auditoriaDao;
        private ICalculaInversionDao calculaInversionDao;

        //
        private DatosAuditoria datosAuditoria;

        public List<Tablero> ConsultarTablero(TableroReq tableroReq)
        {
            List<Tablero> listaMalla = new List<Tablero>();
            datosAuditoria = new DatosAuditoria();
            auditoriaDao = new AuditoriaDao();
            tableroDao = new TableroDao();
            int id;

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(tableroReq);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = tableroReq.Auditoria.NutOrigen;
                    datosAuditoria.Ip = tableroReq.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsInversion";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;
                    Log.Info("Guardar Auditoria.");
 
                    id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    //Ejecutar Consulta Tablero
                    Log.Info("Ejecutar Consulta Tablero SP.");
                    listaMalla = tableroDao.ConsultaMallaT(tableroReq);
                    Log.Info("Consultar Tablero con exito");
                }
                else
                {
                    listaMalla.Add(new Tablero
                    {
                        MontoDesde = "500",
                        MontoHasta = "900",
                        Rango1 = "1.00",
                        Rango2 = "1.25",
                        Rango3 = "1.50",
                        Rango4 = "2.50",
                        Rango5 = "2.60",
                        Rango6 = "2.70",
                        Rango7 = "3.50",
                        Rango8 = "3.75"
                    });
                    Log.Info("Servicio Offline Consultar Tablero ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return listaMalla;
        }


        public Rendimiento CalcularRendimiento(RendimientoReq rendimientoReq)
        {
            Rendimiento rendimiento;
            datosAuditoria = new DatosAuditoria();
            auditoriaDao = new AuditoriaDao();
            rendimientoDao = new RendimientoDao();
            int id;

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(rendimientoReq);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = rendimientoReq.Auditoria.NutOrigen;
                    datosAuditoria.Ip = rendimientoReq.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsInversion";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;
                    Log.Info("Guardar Auditoria: " + jsonString);

                    id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    //Ejecutar Calcular Rendimiento
                    Log.Info("Ejecutar Calcular Rendimiento SP.");
                    rendimiento = rendimientoDao.ConsultaRendimiento(rendimientoReq);
                    Log.Info("Calcular Rendimiento con exito");
                }
                else
                {
                    rendimiento = new Rendimiento
                    {
                        Tasa = 3.50M,
                        Impuesto = 10.50M,
                        InteresGanar = 525M,
                        InteresRecibir = 514.50M,
                        TotalPag = 15514.50M
                    };
                    Log.Info("Servicio Offline Calcular Rendimiento ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return rendimiento;
        }


        public List<Proyeccion> CalcularProyeccion(ProyeccionReq proyeccion)
        {
            List<Proyeccion> listaProyeccion = new List<Proyeccion>();
            datosAuditoria = new DatosAuditoria();
            auditoriaDao = new AuditoriaDao();
            proyeccionDao = new ProyeccionDao();
            int id;

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(proyeccion);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = proyeccion.Auditoria.NutOrigen;
                    datosAuditoria.Ip = proyeccion.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsInversion";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;

                    Log.Info("Guardar Auditoria: " + jsonString);
                    id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    //Ejecutar Calcular Rendimiento
                    Log.Info("Ejecutar Calcular Proyeccion SP.");
                    listaProyeccion = proyeccionDao.ConsultaProyeccion(proyeccion);
                    Log.Info("Calcular Proyeccion con exito");
                }
                else
                {
                    listaProyeccion.Add(new Proyeccion
                    {
                        Plazo = 30,
                        Tasa = 1.25M,
                        InteresGanar = 12.67M,
                        InteresRecibir = 12.42M,
                        Impuesto = 0.25M,
                        TotalRecibir = 512.42M
                    }); 
                    Log.Info("Servicio Offline Calcular Proyeccion ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return listaProyeccion;
        }

        public CalcularInversionFinal CalcularInversion(RendimientoReqCalcularInv rendimientoReq)
        {
            Rendimiento inversion;
            datosAuditoria = new DatosAuditoria();
            auditoriaDao = new AuditoriaDao();
            calculaInversionDao = new CalcularInversionDao();
            FechaLaborable fechaEmision;
            FechaLaborable fechaVencimiento;
            CalcularInversionFinal calcularInv;
            decimal dias;
            int id;

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(rendimientoReq);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = rendimientoReq.Auditoria.NutOrigen;
                    datosAuditoria.Ip = rendimientoReq.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsInversion";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;
                    Log.Info("Guardar Auditoria.");

                    id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    //Ejecutar Calcular Inversion
                    //Obtener fecha emision laborable
                    Log.Info("Ejecutar Obtener Fecha lab Emision SP.");

                    fechaEmision = calculaInversionDao.ObtenerFechalab(DateTime.Now);

                    Log.Info("Ejecutar Obtener Fecha lab Vencimiento SP.");
                    //fechaVencimiento = calculaInversionDao.ObtenerFechalab(DateTime.Now.AddDays(Convert.ToDouble(rendimientoReq.IPlazo)));
                    fechaVencimiento = calculaInversionDao.ObtenerFechalab(fechaEmision.FechaLab.AddDays(Convert.ToDouble(rendimientoReq.IPlazo)));

                    dias = fechaVencimiento.NumeroLaborable - fechaEmision.NumeroLaborable;

                    Log.Info("Ejecutar Calcular Inversion SP.");
                    rendimientoReq.IPlazo = dias;
                    
                    inversion = calculaInversionDao.CalcularInversion(rendimientoReq);
                    Log.Info("Calcular Inversion con exito");

                    calcularInv = new CalcularInversionFinal
                    {
                        Tasa = inversion.Tasa,
                        Impuesto = inversion.Impuesto,
                        InteresGanar = inversion.InteresGanar,
                        InteresRecibir = inversion.InteresRecibir,
                        TotalPag = inversion.TotalPag,
                        DiasPlazoFinal = dias,
                        FechaEmision = fechaEmision.FechaLab,
                        FechaVencimiento = fechaVencimiento.FechaLab
                    };
                }
                else
                {
                    calcularInv = new CalcularInversionFinal
                    {
                        Tasa = 3.85M,
                        Impuesto = 0,
                        InteresGanar = 58.39M,
                        InteresRecibir = 58.39M,
                        TotalPag = 1558.39M,
                        DiasPlazoFinal = 365,
                        FechaEmision = DateTime.Now,
                        FechaVencimiento = DateTime.Now.AddDays(365)
                    };
                    Log.Info("Servicio Offline Calcular Inversion ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return calcularInv;
        }
        
        public string ObtenerDiaLab(Auditoria auditoria)
        {
            datosAuditoria = new DatosAuditoria();
            auditoriaDao = new AuditoriaDao();
            FechaLaborable fechaEmision;
            calculaInversionDao = new CalcularInversionDao();
            int id;

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(auditoria);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = auditoria.NutOrigen;
                    datosAuditoria.Ip = auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsInversion";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;
                    Log.Info("Guardar Auditoria.");

                    id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    //Obtener fecha emision laborable
                    Log.Info("Ejecutar ObtenerFechalab SP.");

                    fechaEmision = calculaInversionDao.ObtenerFechalab(DateTime.Now);
                }
                else
                {
                    fechaEmision = new FechaLaborable
                    {
                        DiaLaborable = "LUNES"
                    };
                    Log.Info("Servicio Offline ObtenerFechalab ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return fechaEmision.DiaLaborable;
        }

        public string ObtenerHorario(HorarioReq horarioReq)
        {
            datosAuditoria = new DatosAuditoria();
            auditoriaDao = new AuditoriaDao();
            calculaInversionDao = new CalcularInversionDao();
            string horario;
            int id;

            try
            {
                if (isOnline)
                {
                    //Guardar auditoria
                    string jsonString = JsonConvert.SerializeObject(horarioReq.Auditoria);
                    datosAuditoria.InputData = jsonString;
                    datosAuditoria.Token = horarioReq.Auditoria.NutOrigen;
                    datosAuditoria.Ip = horarioReq.Auditoria.PuntoAcceso;
                    datosAuditoria.Clase = "WsInversion";
                    datosAuditoria.Metodo = MethodBase.GetCurrentMethod().Name;
                    Log.Info("Guardar Auditoria.");

                    id = auditoriaDao.IngresaLogRequest(datosAuditoria);

                    //Obtener Obtener Horario Normal Diferido 
                    Log.Info("Ejecutar ObtenerHorarioNormalDif SP.");
                    horario = calculaInversionDao.ObtenerHorarioNormalDif(DateTime.Now, horarioReq.Agencia, horarioReq.Servicio);
                }
                else
                {
                    horario = "N";
                    Log.Info("Servicio Offline Obtener Horario Normal Dif ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return horario;
        }

    }
}