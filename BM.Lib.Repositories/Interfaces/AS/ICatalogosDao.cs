using BM.Lib.Domains.AS.Catalogos;
using System.Collections.Generic;

namespace BM.Lib.Repositories.Interfaces.AS
{
    public interface ICatalogosDao
    {
        List<FrecuenciaPagoInt> GetFrecuenciaPagoInt(decimal plazo);
        List<TipoRenovacionInv> GetTipoRenovacionInv(string tipoPlazo);
    }
}
