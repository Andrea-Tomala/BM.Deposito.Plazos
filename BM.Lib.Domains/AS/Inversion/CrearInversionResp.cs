using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS.Inversion
{
    public class CrearInversionResp
    {
        public int NumeroDeposito { get; set; }
        public DateTime FechaDeposito { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Nombreoficial { get; set; }

        public decimal TasaEfectiva { get; set; }
        public decimal InteresMensual { get; set; }
        public decimal InteresMennsualRec { get; set; }
    }
}
