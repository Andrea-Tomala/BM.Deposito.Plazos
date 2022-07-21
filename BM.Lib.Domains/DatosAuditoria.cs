using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains
{
    public class DatosAuditoria
    {
        public string Token { get; set; }
        public string Ip { get; set; }
        public string Clase { get; set; }
        public string Metodo { get; set; }
        public string InputData { get; set; }

    }
}
