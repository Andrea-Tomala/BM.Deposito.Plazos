using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS.Inversion
{
   public class ConsultarInversionesResp
    {
        public int NumDeposito { get; set; }
        public string Titular { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string EstadoDeposito { get; set; }
        public string PagoIntereses { get; set; }
        public string TipoRenovacion { get; set; }
        public int CuentaCredito { get; set; }

        public static ConsultarInversionesResp ConsultarInversionesDR(IDataRecord dataRecord)
        {
            ConsultarInversionesResp consultarInversionesResp = new ConsultarInversionesResp
            {
                NumDeposito = Convert.ToInt32(dataRecord[0].ToString()),
                Titular = dataRecord[1].ToString().Trim(),
                //consultarInversionesResp.canal = Convert.ToDecimal(dataRecord[2]); I
                Monto = Convert.ToDecimal(dataRecord[3]),
                FechaEmision = DateTime.ParseExact(dataRecord[4].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                FechaVencimiento = DateTime.ParseExact(dataRecord[5].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                PagoIntereses = dataRecord[6].ToString().Trim(),
                TipoRenovacion = dataRecord[7].ToString().Trim(),
                EstadoDeposito = dataRecord[8].ToString().Trim(),
                //consultarInversionesResp.InteresCacula = Convert.ToDecimal(dataRecord[9]);
                //consultarInversionesResp.RenovAutomatico = Convert.ToDecimal(dataRecord[10]);
                CuentaCredito = Convert.ToInt32(dataRecord[11].ToString().Trim())
            };


            return consultarInversionesResp;
        }

    }
}
