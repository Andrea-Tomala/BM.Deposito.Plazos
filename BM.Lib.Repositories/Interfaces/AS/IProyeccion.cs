using BM.Lib.Domains.AS;
using System.Collections.Generic;

namespace BM.Lib.Repositories.Interfaces.AS
{
    public interface IProyeccion
    {
        List<Proyeccion> ConsultaProyeccion(ProyeccionReq req);
    }
}
