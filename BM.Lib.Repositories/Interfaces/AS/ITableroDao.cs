using BM.Lib.Domains.AS;
using System.Collections.Generic;

namespace BM.Lib.Repositories.Interfaces.AS
{
    public interface ITableroDao
    {
        List<Tablero> ConsultaMallaT(TableroReq mallaTableroReq);
    }
}
