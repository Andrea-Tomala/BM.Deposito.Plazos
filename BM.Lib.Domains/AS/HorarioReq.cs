using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS
{
    public class HorarioReq
    {
        public Auditoria Auditoria { get; set; }
        public int Agencia { get; set; }
        public decimal Servicio { get; set; }
    }
}
