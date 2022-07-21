using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS
{
    public class Rendimiento
    {
        public decimal Tasa { get; set; }
        public decimal InteresGanar { get; set; }
        public decimal Impuesto { get; set; }
        public decimal InteresRecibir { get; set; }
        public decimal TotalPag { get; set; }
    }
}
