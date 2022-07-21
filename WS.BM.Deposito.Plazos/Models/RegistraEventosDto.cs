using BM.Lib.Domains;
using BM.Lib.Domains.AS;
using BM.Lib.Repositories.Accesos;
using BM.Lib.Repositories.Accesos.AS;
using BM.Lib.Repositories.Interfaces;
using BM.Lib.Repositories.Interfaces.AS;
using log4net;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Reflection;

namespace WS.BM.Deposito.Plazos.Models
{
    public class RegistraEventosDto
    {
        private static readonly bool isOnline = Convert.ToInt32(ConfigurationManager.AppSettings["Online"]) == 1;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IRegistraEventosDao registraEventos;

        public string ValidarParametros(Evento evento)
        {
            string salida ="";
            if (string.IsNullOrEmpty(evento.Opcion.ToString()))
                salida = "Debe ingresar Opcion";
            if (string.IsNullOrEmpty(evento.TipoIdentificacion))
                salida = "Debe ingresar TipoIdentificacion";
            if (string.IsNullOrEmpty(evento.Identificacion))
                salida = "Debe ingresar Identificacion";
            if (string.IsNullOrEmpty(evento.NumeroDeposito.ToString()))
                salida = "Debe ingresar NumeroDeposito";
            if (string.IsNullOrEmpty(evento.CodCertificado))
                salida = "Debe Enviar Codigo Certificado parametrizado ";

            return salida;
        }


        public string RegistrarEventos(Evento evento)
        {
            string respuesta;
            registraEventos = new RegistraEventosDao();

            try
            {
                if (isOnline)
                {
                    Log.Info("Ejecutar Crear RegistraEventos SP.");
                    respuesta = registraEventos.RegistrarEventos(evento);
                    Log.Info("Crear RegistraEventos con exito");
                }
                else
                {
                    respuesta = "Servicio Offline Crear RegistraEventos";
                    Log.Info("Servicio Offline Crear RegistraEventos ejecutado con exito.");
                }
            }
            catch
            {
                throw;
            }
            return respuesta;
        }
    }
}