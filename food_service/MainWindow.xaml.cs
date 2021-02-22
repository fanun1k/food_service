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
        public MainWindow()
        {
            InitializeComponent();
            items = new List<Item>();
            cargarItemsBDDALista();
            itemsVenta = new List<Item>();
            lbItems.ItemsSource = items;
            lbItemsVenta.ItemsSource = itemsVenta;
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
                }); ; ;
            }

        }
        private void lbItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((lbItems.SelectedItem as Item).Cantidad > 0)
            {
                (lbItems.SelectedItem as Item).Cantidad = 0;
                (lbItems.SelectedItem as Item).Visibilidad = "Hidden";
                
            }
            else
            {
                (lbItems.SelectedItem as Item).Cantidad = 1;
                (lbItems.SelectedItem as Item).Visibilidad = "Visible";
            }
        }

        private void btnAumentarCantidad_Click(object sender, RoutedEventArgs e)
        {
            (lbItems.SelectedItem as Item).Cantidad = (lbItems.SelectedItem as Item).Cantidad + 1;
        }

        private void btnQuitarCantidad_Click(object sender, RoutedEventArgs e)
        {
            (lbItems.SelectedItem as Item).Cantidad = (lbItems.SelectedItem as Item).Cantidad - 1;
        }
    }
}
