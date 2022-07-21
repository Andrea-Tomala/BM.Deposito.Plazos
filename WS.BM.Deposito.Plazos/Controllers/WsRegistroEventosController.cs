using BM.Lib.Domains.AS;
using BM_DepositoPlazo.Models;
using log4net;
using System;
using System.Reflection;
using System.Web.Http;
using WS.BM.Deposito.Plazos.Models;

namespace WS.BM.Deposito.Plazos.Controllers
{
    public class WsRegistroEventosController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RegistraEventosDto registraEventosDto = new RegistraEventosDto();
        private string ValidaParametros = "";


        [HttpPost]
        [Route("api/WsRegistroEventos/RegistrarEventos")]
        public IHttpActionResult RegistrarEventos(Evento evento)
        {
            string respuesta;
            try {
                Log.Info("Inicio Controller CreaciónInversión Validar parametros.");
                ValidaParametros = registraEventosDto.ValidarParametros(evento);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);

                respuesta = registraEventosDto.RegistrarEventos(evento);
                Log.Info("Fin RegistrarEventos con exito.");
                return Ok(respuesta);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }

            
        }


        

    }
}