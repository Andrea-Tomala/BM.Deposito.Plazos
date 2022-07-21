using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BM.Lib.Domains.AS;
namespace BM.Lib.Repositories.Interfaces.AS
{
 
    public interface IRegistraEventosDao
    {
        string RegistrarEventos(Evento evento);

    }
}
