using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS.Inversion
{
    public class ActualizarInversionReq
    {
        public Auditoria Auditoria { get; set; }
        public int NumDeposito { get; set; }
        public string TipoIdent { get; set; }
        public string Identificacion { get; set; }
        public string Titular { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string EstadoDeposito { get; set; }
        public string PagoIntereses { get; set; }
        public string TipoRenovacion { get; set; }
        public int CuentaCredito { get; set; }

        
    }
}
