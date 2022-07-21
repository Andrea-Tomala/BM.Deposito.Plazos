using BM.Lib.Domains;

namespace BM.Lib.Repositories.Interfaces
{
    public interface IAuditoriaDao
    {
        int IngresaLogRequest(DatosAuditoria datosAuditoria);
    }
}
