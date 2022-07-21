using BM.Lib.Domains.AS;

namespace BM.Lib.Repositories.Interfaces.AS
{
    public interface IRendimientoDao
    {
        Rendimiento ConsultaRendimiento(RendimientoReq req);
    }
}
