using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS.Catalogos
{
    public class FrecuenciaPagoInt
    {
        public string CodFrecuencia { get; set; }
        public string DescFrecuencia { get; set; }

        public static FrecuenciaPagoInt ConFrecuenciaPagoIntDR(IDataRecord dataRecord)
        {
            FrecuenciaPagoInt frecuenciaPagoInt = new FrecuenciaPagoInt
            {
                CodFrecuencia = dataRecord[0].ToString().Trim(),
                DescFrecuencia = dataRecord[1].ToString().Trim(),
            };

            return frecuenciaPagoInt;
        }
    }
}
