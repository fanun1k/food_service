using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace Interfaces
{
    public interface IAsistencia:IDao<Asistencia>
    {
        string RegistrarAsistencia(int id);
        void RegistrarEntrada(int id);
        void RegistrarSalida(int id);
    }
}
