using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;
using Implementation;
using System.Data;

namespace food_service
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ItemImpl itemImpl;
        List<Item> items;
        List<Item> itemsVenta;
        Item item = null;
        bool itemExiste = false;
        public MainWindow()
        {
            InitializeComponent();
            items = new List<Item>();
            cargarItemsBDDALista();
            itemsVenta = new List<Item>();
            lbItems.ItemsSource = items;
            lbItemsVenta.ItemsSource = itemsVenta;
            DataContext = itemsVenta;
            DataContext = items;
        }

        private void btnComedor_Click(object sender, RoutedEventArgs e)
        {
            ventanas.vntComedor vc = new ventanas.vntComedor();
            vc.Show();
        }
        private void cargarItemsBDDALista()
        {
            byte[] fotografia = new byte[0];
            itemImpl = new ItemImpl();
            DataTable dt = itemImpl.Select();
            BitmapImage bmi;
            foreach (DataRow dataRow in dt.Rows)
            {
                if (dataRow["imagen"] != null && dataRow["imagen"].ToString().Length > 0)
                {
                    bmi = new BitmapImage();
                    fotografia = (byte[])dataRow["imagen"];
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(fotografia);
                    ms.Seek(0, System.IO.SeekOrigin.Begin);
                    bmi.BeginInit();
                    bmi.StreamSource = ms;
                    bmi.EndInit();
                }
                else
                {
                    Uri uri = new Uri("/food_service;component/appData/itemPorDefecto.png", UriKind.Relative);
                    bmi = new BitmapImage(uri);
                }
                items.Add(new Item()
                {
                    Id = int.Parse(dataRow["id"].ToString()),
                    Nombre = dataRow["nombre"].ToString(),
                    Precio = double.Parse(dataRow["precio"].ToString()),
                    Imagen = bmi,
                    Visibilidad = "Hidden"
                });
            }

        }

        private void btnAumentarCantidad_Click(object sender, RoutedEventArgs e)
        {
            (lbItems.SelectedItem as Item).Cantidad = (lbItems.SelectedItem as Item).Cantidad + 1;

        }

        private void btnQuitarCantidad_Click(object sender, RoutedEventArgs e)
        {
            if ((lbItems.SelectedItem as Item).Cantidad > 1)
            {
                (lbItems.SelectedItem as Item).Cantidad = (lbItems.SelectedItem as Item).Cantidad - 1;
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                if (itemExiste == false)
                {
                    itemExiste = true;
                }
            }
            else
            {
                itemExiste = false;
            }
        }
        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (itemExiste)
            {
                if ((lbItems.SelectedItem as Item).Cantidad > 0)
                {
                    (lbItems.SelectedItem as Item).Cantidad = 0;
                    (lbItems.SelectedItem as Item).Visibilidad = "Hidden";
                    itemExiste = false;

                }
                else
                {

                    (lbItems.SelectedItem as Item).Cantidad = 1;
                    (lbItems.SelectedItem as Item).Visibilidad = "Visible";


                    item = new Item()
                    {
                        Id = (lbItems.SelectedItem as Item).Id,
                        Cantidad = 1,
                        Nombre = (lbItems.SelectedItem as Item).Nombre,
                        Precio = (lbItems.SelectedItem as Item).Precio,
                    };
                    itemsVenta.Add(item);

                }
            }
        }

        private void btnAsistencia_Click(object sender, RoutedEventArgs e)
        {
            ventanas.VntAsistencia vntAsistencia = new ventanas.VntAsistencia();
            vntAsistencia.Show();
        }
    }
}
