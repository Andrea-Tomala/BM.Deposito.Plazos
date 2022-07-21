using BM.Lib.Domains.AS.Catalogos;
using BM_DepositoPlazo.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;

namespace BM_DepositoPlazo.Controllers
{
    public class WsCatalogosController : ApiController
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CatalogosDto catalogosDto = new CatalogosDto();
        private string ValidaParametros = "";

        [HttpPost]
        [Route("api/WsCatalogos/FrecuenciaPagoInt")]
        public IHttpActionResult GetFrecuenciaPagoInt(FrecuenciaPagoIntReq frecuenciaPagoIntReq)
        {
            List<FrecuenciaPagoInt> listaFrecuenciaPagos;

            try
            {
                Log.Info("Inicio Controller GetFrecuenciaPagoInt. Validar parametros.");

                ValidaParametros = catalogosDto.ValidarParametros(frecuenciaPagoIntReq.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);
                if (string.IsNullOrEmpty(frecuenciaPagoIntReq.Plazo.ToString()))
                    return BadRequest("Debe ingresar Dias Plazo");

                listaFrecuenciaPagos = catalogosDto.GetFrecuenciaPagoInt(frecuenciaPagoIntReq);
                Log.Info("Fin GetFrecuenciaPagoInt con exito.");
                return Ok(listaFrecuenciaPagos);
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/WsCatalogos/TipoRenovacionInv")]
        public IHttpActionResult GetTipoRenovacionInv(TipoRenovInvReq req)
        {
            List<TipoRenovacionInv> listaTipoRenovacionInv;

            try
            {
                Log.Info("Inicio Controller ConsultarTablero. Validar parametros.");

                ValidaParametros = catalogosDto.ValidarParametros(req.Auditoria);
                if (!string.IsNullOrEmpty(ValidaParametros))
                    return BadRequest(ValidaParametros);
                if (string.IsNullOrEmpty(req.TipoPlazo))
                    return BadRequest("Debe ingresar Tipo Plazo");

                listaTipoRenovacionInv = catalogosDto.GetTipoRenovacionInv(req);
                Log.Info("Fin GetTipoRenovacionInv con exito.");
                
                return Ok(listaTipoRenovacionInv);
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }

    }
}