using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS.Inversion
{
    public class ConsultaInversionesReq
    {
        public Auditoria Auditoria { get; set; }

        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
    }
}
