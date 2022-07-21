using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Lib.Domains
{
    public class Parametros
    {
        public int IdParametro { get; set; }
        public string NombreParametro { get; set; }
        public string TipoParametro { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public string EstadoParametro { get; set; }
        public string Usuario { get; set; }

        //Para Ingreso
        public int IdTipoParametro { get; set; }
        public int IdEstado { get; set; }

        public static Parametros ConParametrosDR(IDataRecord dataRecord)
        {
            Parametros parametros = new Parametros
            {
                IdParametro = int.Parse(dataRecord[0].ToString()),
                TipoParametro = dataRecord[1].ToString(),
                NombreParametro = dataRecord[2].ToString(),
                Valor = dataRecord[3].ToString(),
                Descripcion = dataRecord[4].ToString(),
                EstadoParametro = dataRecord[5].ToString(),
                Usuario = dataRecord[6].ToString(),
            };

            return parametros;
        }
    }
}
