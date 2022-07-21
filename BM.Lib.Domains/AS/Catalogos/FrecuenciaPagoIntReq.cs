using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS.Catalogos
{
    public class FrecuenciaPagoIntReq
    {
        public Auditoria Auditoria { get; set; }
        public decimal Plazo { get; set; }
    }
}
