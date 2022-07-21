using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BM.Lib.Domains.AS
{
    public class Tablero
    {
        public string MontoDesde { get; set; }
        public string MontoHasta { get; set; }
        public string Rango1 { get; set; }
        public string Rango2 { get; set; }
        public string Rango3 { get; set; }
        public string Rango4 { get; set; }
        public string Rango5 { get; set; }
        public string Rango6 { get; set; }
        public string Rango7 { get; set; }
        public string Rango8 { get; set; }

        public static Tablero ConMallaTableroDR(IDataRecord dataRecord)
        {
            Tablero mallaTablero = new Tablero
            {
                MontoDesde = dataRecord[0].ToString(),
                MontoHasta = dataRecord[1].ToString(),
                Rango1 = dataRecord[2].ToString(),
                Rango2 = dataRecord[3].ToString(),
                Rango3 = dataRecord[4].ToString(),
                Rango4 = dataRecord[5].ToString(),
                Rango5 = dataRecord[6].ToString(),
                Rango6 = dataRecord[7].ToString(),
                Rango7 = dataRecord[8].ToString(),
                Rango8 = dataRecord[9].ToString()
            };

            return mallaTablero;
        }
    }
}
