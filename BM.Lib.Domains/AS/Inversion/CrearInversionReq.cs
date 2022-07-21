using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS.Inversion
{
    public class CrearInversionReq
    {
        public Auditoria Auditoria { get; set; }
        public string CanalCaptacion { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Email { get; set; }
        public List<CuentasDebito> CuentasADebitar { get; set; }
        public decimal MontoInvertir { get; set; }
        public int DiasPlazo { get; set; }
        public decimal Tasa { get; set; }
        public decimal Impuesto { get; set; }
        public decimal InteresRecibir { get; set; }
        public decimal InteresGanar { get; set; }
        public bool AgregarBeneficiario { get; set; }
        public List<Beneficiario> Beneficiarios { get; set; }
        public CuentaCredito CuentaAcreditar { get; set; }
        public string PagoInteres { get; set; }
        public string VencimientoInversion { get; set; }
        public DateTime FecEmision { get; set; }
        public DateTime FecVencimiento { get; set; }
        public bool AceptacionContrato { get; set; }
        //public string ReferenciaOTP { get; set; }
        public string CodigoOTP { get; set; }
        public string CodigoOficinaOficialCredito { get; set; }
    }
}
