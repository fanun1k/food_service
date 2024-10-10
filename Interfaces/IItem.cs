using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Interfaces
{
    public interface IItem:IDao<Item>
    {
        DataTable SelectLunch();
        Item SelectItem(int id);
        DataTable SelectDataTableItem(int id);
        DataTable SelectItemsVentaSnack(int idOrden);
    }
}
