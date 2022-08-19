using BM.Lib.Domains;
using BM.Lib.Domains.AS;
using BM.Lib.Domains.AS.Inversion;
using BM_DepositoPlazo.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;

namespace BM_DepositoPlazo.Controllers
{

    public class WsInversionController : ApiController
    {
        private readonly InversionDto inversionDto = new InversionDto();
        private readonly CatalogosDto catalogosDto = new CatalogosDto();
        private readonly CrearInversionDto crearInversionDto = new CrearInversionDto();

        //
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string ValidaParametros = "";

        [HttpPost]
        [Route("api/WsInversion/ConsultarTablero")]
        public IHttpActionResult ConsultarTablero(TableroReq mallaReq)
        {
            List<Tablero> listaMalla;

            try
            {
                Log.Info("Inicio Controller ConsultarTablero. Validar parametros.");
                ValidaParametros = catalogosDto.ValidarParametros(mallaReq.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);

                listaMalla = inversionDto.ConsultarTablero(mallaReq);
                Log.Info("Fin Controller ConsultarTablero  con exito.");
                
                return Ok(listaMalla);
            
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/WsInversion/CalcularRendimiento")]
        public IHttpActionResult CalcularRendimiento(RendimientoReq rendimientoReq)
        {
            Rendimiento rendimiento;

            try
            {
                Log.Info("Inicio Controller CalcularRendimiento. Validar parametros.");

                ValidaParametros = catalogosDto.ValidarParametros(rendimientoReq.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);
                if (string.IsNullOrEmpty(rendimientoReq.IMonto.ToString()))
                    return BadRequest("Debe ingresar Monto");
                if (string.IsNullOrEmpty(rendimientoReq.IPlazo.ToString()))
                    return BadRequest("Debe ingresar Dias Plazo");

                rendimiento = inversionDto.CalcularRendimiento(rendimientoReq);
                Log.Info("Fin Controller CalcularRendimiento  con exito.");

                return Ok(rendimiento);
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("api/WsInversion/CalcularProyeccion")]
        public IHttpActionResult CalcularProyeccion(ProyeccionReq proyeccionReq)
        {
            List<Proyeccion> listaProyeccion;

            try
            {
                Log.Info("Inicio Controller CalcularProyeccion. Validar parametros.");

                ValidaParametros = catalogosDto.ValidarParametros(proyeccionReq.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);
                if (string.IsNullOrEmpty(Convert.ToString(proyeccionReq.Monto)))
                    return BadRequest("Debe ingresar Monto");

                listaProyeccion = inversionDto.CalcularProyeccion(proyeccionReq);
                Log.Info("Fin Controller CalcularProyeccion  con exito.");
                
                return Ok(listaProyeccion);
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/WsInversion/CalcularInversion")]
        public IHttpActionResult CalcularInversion(RendimientoReqCalcularInv rendimientoReq)
        {
            CalcularInversionFinal calcularInvFinal;

            try
            {
                Log.Info("Inicio Controller CalcularInversion. Validar parametros.");

                ValidaParametros = catalogosDto.ValidarParametros(rendimientoReq.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);
                if (string.IsNullOrEmpty(rendimientoReq.IMonto.ToString()))
                    return BadRequest("Debe ingresar Monto");
                if (string.IsNullOrEmpty(rendimientoReq.IPlazo.ToString()))
                    return BadRequest("Debe ingresar Dias Plazo");

                if (string.IsNullOrEmpty(rendimientoReq.Identificacion.ToString()))
                    return BadRequest("Falta la identificación");
                if (string.IsNullOrEmpty(rendimientoReq.ITipoIdentificacion.ToString()))
                    return BadRequest("Falta la identificación");

                calcularInvFinal = inversionDto.CalcularInversion(rendimientoReq);
                Log.Info("Fin Controller CalcularInversion  con exito.");

                return Ok(calcularInvFinal);
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/WsInversion/ObtenerDiaLab")]
        public IHttpActionResult ObtenerDiaLab(Auditoria auditoria)
        {
            string diasLab;

            try
            {
                Log.Info("Inicio Controller CalcularInversion. Validar parametros.");

                ValidaParametros = catalogosDto.ValidarParametros(auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);

                diasLab = inversionDto.ObtenerDiaLab(auditoria);
                Log.Info("Fin Controller CalcularInversion  con exito.");

                return Ok(diasLab);
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/WsInversion/CrearInversion")]
        public IHttpActionResult CrearInversion(CrearInversionReq crearInversionReq)
        {
            CrearInversionResp crearInversion;

            try
            {
                Log.Info("Inicio Controller CrearInversion. Validar parametros.");

                ValidaParametros = catalogosDto.ValidarParametros(crearInversionReq.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);

                if (!crearInversionReq.AceptaTerminos)
                    return BadRequest("Debe Aceptar Terminos y Condiciones");
                if (string.IsNullOrEmpty(crearInversionReq.Identificacion))
                    return BadRequest("Debe Ingresar Identificacion");
                if (string.IsNullOrEmpty(crearInversionReq.TipoIdentificacion))
                    return BadRequest("Debe Ingresar Tipo Identificacion");
                if (string.IsNullOrEmpty(crearInversionReq.Tasa.ToString()))
                    return BadRequest("Debe Ingresar Tasa");
                if (string.IsNullOrEmpty(crearInversionReq.MontoInvertir.ToString()))
                    return BadRequest("Debe Ingresar Monto");
                if (string.IsNullOrEmpty(crearInversionReq.InteresGanar.ToString()))
                    return BadRequest("Debe Ingresar Interes a Ganar");
                if (string.IsNullOrEmpty(crearInversionReq.InteresRecibir.ToString()))
                    return BadRequest("Debe Ingresar Interes a Recibir");
                if (string.IsNullOrEmpty(crearInversionReq.Impuesto.ToString()))
                    return BadRequest("Debe Ingresar Impuesto");
                if (string.IsNullOrEmpty(crearInversionReq.PagoInteres))
                    return BadRequest("Debe Ingresar Pago de Intereses");
                if (string.IsNullOrEmpty(crearInversionReq.VencimientoInversion))
                    return BadRequest("Debe Ingresar Vencimiento de Inversion");
                if (string.IsNullOrEmpty(crearInversionReq.CodigoOTP))
                    return BadRequest("Debe Ingresar Codigo OTP");
                if (string.IsNullOrEmpty(crearInversionReq.CodigoOficinaOficialCredito))
                    return BadRequest("Debe Ingresar Codigo de Oficina");

                //crear inversión en AS400
                crearInversion = crearInversionDto.CrearInversion(crearInversionReq);
                Log.Info("Fin Controller CrearInversion  con exito.");

                return Ok(crearInversion);
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        //CONSULTARINVERSIONES
        [HttpPost]
        [Route("api/WsInversion/ConsultarInversiones")]
        public IHttpActionResult ConsultarInversiones(ConsultaInversionesReq consultarInversionesReq)
        {
            List<ConsultarInversionesResp> listaInversionesResp;
            try
            {
                Log.Info("Inicio Controller ConsultarInversiones. Validar parametros.");
                ValidaParametros = catalogosDto.ValidarParametros(consultarInversionesReq.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);

                if (string.IsNullOrEmpty(consultarInversionesReq.Identificacion))
                    return BadRequest("Debe Ingresar Identificacion");
                if (string.IsNullOrEmpty(consultarInversionesReq.TipoIdentificacion))
                    return BadRequest("Debe Ingresar Tipo Identificacion");

                // proceso el metodo
                listaInversionesResp = crearInversionDto.ConsultarInversiones(consultarInversionesReq);
                Log.Info("Fin Controller ConsultarInversiones  con exito.");

                return Ok(listaInversionesResp);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        //ACTUALIZARINVERSION
        [HttpPost]
        [Route("api/WsInversion/ActualizarInversion")]
        public IHttpActionResult ActualizarInversion(ActualizarInversionReq actualizarInversionReq)
        {
            string respuesta;

            try
            {
                Log.Info("Inicio Controller ActualizarInversion. Validar parametros.");
                ValidaParametros = catalogosDto.ValidarParametros(actualizarInversionReq.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);

                ValidaParametros = crearInversionDto.ValidarParametros(actualizarInversionReq);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);

                // proceso el metodo
                respuesta = crearInversionDto.ActualizarInversion(actualizarInversionReq);
                Log.Info("Fin Controller ActualizarInversion  con exito.");
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("api/WsInversion/HorarioDifNormal")]
        public IHttpActionResult ObtenerHorario(HorarioReq horarioReq)
        {
            string horario;

            try
            {
                Log.Info("Inicio Controller CalcularInversion. Validar parametros.");

                ValidaParametros = catalogosDto.ValidarParametros(horarioReq.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);
                if (string.IsNullOrEmpty(horarioReq.Agencia.ToString()) || horarioReq.Agencia == 0)
                    return BadRequest("Debe ingresar Agencia");
                if (string.IsNullOrEmpty(horarioReq.Servicio.ToString()))
                    return BadRequest("Debe ingresar Codigo de Servicio");

                horario = inversionDto.ObtenerHorario(horarioReq);
                Log.Info("Fin Controller CalcularInversion  con exito.");

                return Ok(horario);
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
    }
}