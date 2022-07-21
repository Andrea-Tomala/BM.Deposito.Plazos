using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS
{
    public class CalcularInversionFinal
    {
        public decimal Tasa { get; set; }
        public decimal InteresGanar { get; set; }
        public decimal Impuesto { get; set; }
        public decimal InteresRecibir { get; set; }
        public decimal TotalPag { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal DiasPlazoFinal { get; set; }
    }
}
