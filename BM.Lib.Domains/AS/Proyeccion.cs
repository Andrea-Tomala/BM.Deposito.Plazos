using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BM.Lib.Domains.AS
{
    public class Proyeccion
    {
        public int Plazo { get; set; }
        public decimal Tasa { get; set; }
        public decimal InteresGanar { get; set; }
        public decimal InteresRecibir { get; set; }
        public decimal Impuesto { get; set; }
        public decimal TotalRecibir { get; set; }

        public static Proyeccion ConProyeccionDR(IDataRecord dataRecord)
        {
            Proyeccion proyeccion = new Proyeccion
            {
                Plazo = int.Parse(dataRecord[0].ToString()),
                Tasa = decimal.Parse(dataRecord[1].ToString()),
                InteresGanar = decimal.Parse(dataRecord[2].ToString()),
                Impuesto = decimal.Parse(dataRecord[3].ToString()),
                InteresRecibir = decimal.Parse(dataRecord[4].ToString()),
                TotalRecibir = decimal.Parse(dataRecord[5].ToString())
            };

            return proyeccion;
        }
    }
}
