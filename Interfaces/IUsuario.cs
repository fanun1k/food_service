using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Interfaces
{
    public interface IUsuario:IDao<Usuario>
    {
        DataTable SelectUsuario(int id);
    }
}
