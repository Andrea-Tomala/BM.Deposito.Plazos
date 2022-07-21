using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS.Inversion
{
    public class CuentasDebito
    {
        public string Nombre { get; set; }
        public string Prefijo { get; set; }
        public string Cuenta { get; set; }
        public string CodigoProducto { get; set; }
        public string TipoProducto { get; set; }
        public decimal Monto { get; set; }
    }
}
