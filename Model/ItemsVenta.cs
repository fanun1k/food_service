using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public static class ItemsVenta
    {
        public static ObservableCollection<Item> items = new ObservableCollection<Item>();
        public static decimal total;

        public static ObservableCollection<Item> GetItemsVenta()
        {
            return items;
        }
        public static void AddItemVenta(Item item)
        {
            items.Add(item);
        }
        public static void DeleteItem(int id)
        {
            items.Remove(items.Where(item => item.Id == id).Single());
        }
        public static decimal GetTotal()
        {
            total = 0;
            foreach (var item in items)
            {
                total = total += item.Total;
            }
            return total;
        }
    }
}
