using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;


namespace Interfaces
{
    public interface ISnack:IDao<Snack>
    {
        void Insert(Orden orden, List<Snack> listSnack,string tipo);
        int GetIdSnack();
        DataTable GetTableSnackWhitOrderID(int id);
    }
}
