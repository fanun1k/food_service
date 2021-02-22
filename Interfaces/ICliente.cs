using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data;

namespace Interfaces
{
    public interface ICliente:IDao<Cliente>
    {
        Cliente Select(int codigo);
        DataTable GetTableCliente(int codigo);
    }
}
