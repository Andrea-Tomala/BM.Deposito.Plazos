using BM.Lib.Domains.AS;
using System;

namespace BM.Lib.Repositories.Interfaces.AS
{
    public interface ICalculaInversionDao
    {
        //Rendimiento CalcularInversion(RendimientoReq req);
        Rendimiento CalcularInversion(RendimientoReqCalcularInv req);
        FechaLaborable ObtenerFechalab(DateTime fecha);
        string ObtenerHorarioNormalDif(DateTime fecha, int agencia, decimal servicio);
    }
}
