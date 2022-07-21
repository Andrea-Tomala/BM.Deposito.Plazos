using BM.Lib.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Lib.Repositories.Interfaces
{
    public interface IParametrosDao
    {
        List<Parametros> ConsultaParametros();
    }
}
