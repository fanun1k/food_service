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
using System.Collections.ObjectModel;
using food_service.ventanas;
using ControlesFoodService;

namespace food_service
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ItemImpl itemImpl;
        List<Item> items;

        ItemSnack itemSnack;

        ObservableCollection<Item> itemsVenta;

        public MainWindow()
        {
            InitializeComponent();
            items = new List<Item>();
            cargarItemsBDDALista();
            itemsVenta = new ObservableCollection<Item>();
            lbItemsVenta.ItemsSource = itemsVenta;
            DataContext = itemsVenta;
        }

        private void btnComedor_Click(object sender, RoutedEventArgs e)
        {
            ventanas.vntComedor vc = new ventanas.vntComedor();
            vc.Show();
        }
        private void cargarItemsBDDALista()
        {
            try
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
                    itemSnack= new ItemSnack();
                    itemSnack.ItemMostrar=(new Item()
                    {
                        Id = int.Parse(dataRow["id"].ToString()),
                        Nombre = dataRow["nombre"].ToString(),
                        Precio = decimal.Parse(dataRow["precio"].ToString()),
                        Imagen = bmi,
                        Visibilidad = "Hidden",
                        VisibilidadBotones = "Hidden"
                    });
                    lbItems.Items.Add(itemSnack);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }

        }




        private void btnAsistencia_Click(object sender, RoutedEventArgs e)
        {
            ventanas.VntAsistencia vntAsistencia = new ventanas.VntAsistencia();
            vntAsistencia.Show();
        }

        private void agregar(object sender, RoutedEventArgs e)
        {
            ventanas.VntAsistencia vntAsistencia = new ventanas.VntAsistencia();
            vntAsistencia.Show();
        }

        private void aumentarCantidad(int Id)
        {
            foreach (var item in itemsVenta)
            {
                if (item.Id == Id)
                {
                    item.Cantidad++;
                    break;
                }
            }
        }

        private void quitarCantidad(int Id)
        {
            foreach (var item in itemsVenta)
            {
                if (item.Id == Id)
                {
                    item.Cantidad--;
                    break;
                }
            }
        }
        private void actualizarListaPedido()
        {
            var listaItemsPedido = from c in itemsVenta
            select c.Cantidad +" "+ c.Nombre + " " + c.Precio + " "+ c.Cantidad * c.Precio;
            //lbItemsVenta.ItemsSource = listaItemsPedido;
            lblTotal.Text = obtenerTotal() + " Bs.";
        }
        private bool verificarSiSePidio(Item item)
        {
            foreach (var itemEnVenta in itemsVenta)
            {
                if (itemEnVenta.Id == item.Id)
                {
                    return true;
                }            
            }
            return false;
        }

        private void eliminarPedido(int Id)
        {
            itemsVenta.Remove(itemsVenta.Where(item => item.Id == Id).Single());
        }

        private decimal obtenerTotal()
        {
            decimal total = 0;
            foreach (var item in itemsVenta)
            {
                total = total + (item.Precio * item.Cantidad);
            }
            return total;
        }

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            decimal total = obtenerTotal(); 
            if (total > 0)
            {
                ObservableCollection<Item> nuevaLista = itemsVenta;
                ventanas.VntSnack vs = new ventanas.VntSnack(total, nuevaLista);
                vs.ShowInTaskbar = false;
                vs.Show();
                           
                itemsVenta = new ObservableCollection<Item>();
                actualizarListaPedido();                
            }
        }
        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            ventanas.VtnRegistrar vtn = new ventanas.VtnRegistrar();
            vtn.Show();
        }

        private void btnMostrarReporteGeneral_Click(object sender, RoutedEventArgs e)
        {
            VntReporteGeneral crg = new VntReporteGeneral();
            crg.Show();
        }

    }
}
