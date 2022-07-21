using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS.Catalogos
{
    public class TipoRenovacionInv
    {
        public string CodTipoRenovacion { get; set; }
        public string DescTipoRenovacion { get; set; }

        public static TipoRenovacionInv ConTipoRenovacionInvDR(IDataRecord dataRecord)
        {
            TipoRenovacionInv tipoRenovacionInv = new TipoRenovacionInv
            {
                CodTipoRenovacion = dataRecord["ARGUME"].ToString().Trim(),
                DescTipoRenovacion = dataRecord["DESCOR"].ToString().Trim(),
            };

            return tipoRenovacionInv;
        }
    }
}
