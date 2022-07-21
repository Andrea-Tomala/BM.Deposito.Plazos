using BM.Lib.Domains.AS.Inversion;
using System.Collections.Generic;

namespace BM.Lib.Repositories.Interfaces.AS
{
    public interface ICrearInversionDao
    {
        CrearInversionResp CrearInversion(CrearInversionReq req);
        int GrabaContratoDeposito(CrearInversionReq crearInversion, CrearInversionResp inversionResp);
        List<ConsultarInversionesResp> ConsultarInversiones(ConsultaInversionesReq consulta);
        string ActualizarInversion(ActualizarInversionReq actualizarInversionReq);
    }
}
