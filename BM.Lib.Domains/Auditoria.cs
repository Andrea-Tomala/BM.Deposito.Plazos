using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains
{
    public class Auditoria
    {
        public string Cliente { get; set; }
        //GUID o Token
        public string NutOrigen { get; set; }
        //Numero de IP - MAC Nombre del Dispositivo
        public string PuntoAcceso { get; set; }
        //IP Dispositivo Otro
        public int TipoPuntoAcceso { get; set; }
        public string UsuarioFinal { get; set; }
    }
}
