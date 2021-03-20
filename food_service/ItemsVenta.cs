using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace food_service
{
    public static class ItemsVenta
    {
        
        public static ObservableCollection<Item> items = new ObservableCollection<Item>();
        public static decimal total;
        public delegate void delegado(decimal t);

        public static event delegado pasarTotal;
        public static ObservableCollection<Item> GetItemsVenta()
        {
            return items;
        }
        public static void AddItemVenta(Item item)
        {
            items.Add(item);
            pasarTotal(GetTotal());
        }
        public static void DeleteItem(int id)
        {
            items.Remove(items.Where(item => item.Id == id).Single());
            pasarTotal(GetTotal());
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

        internal static void AddCant(int id)
        {
            foreach (var item in items)
            {
                if (item.Id==id)
                {
                    item.Cantidad++;
                }
            }
            pasarTotal(GetTotal());
        }
        internal static void PutOffCant(int id)
        {
            foreach (var item in items)
            {
                if (item.Id == id)
                {
                    item.Cantidad--;
                }
            }
            pasarTotal(GetTotal());
        }

        public static void ClearItems()
        {
            items.Clear();
        }
    }
}
