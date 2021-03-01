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

namespace food_service
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ItemImpl itemImpl;
        List<Item> items;
        ObservableCollection<Item> itemsVenta;
        Item item = null;
        bool itemExiste = false;

        public MainWindow()
        {
            InitializeComponent();
            items = new List<Item>();
            cargarItemsBDDALista();
            itemsVenta = new ObservableCollection<Item>();
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
                    items.Add(new Item()
                    {
                        Id = int.Parse(dataRow["id"].ToString()),
                        Nombre = dataRow["nombre"].ToString(),
                        Precio = decimal.Parse(dataRow["precio"].ToString()),
                        Imagen = bmi,
                        Visibilidad = "Hidden",
                        VisibilidadBotones="Hidden"
                       
                    });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }

        }

        private void btnAumentarCantidad_Click(object sender, RoutedEventArgs e)
        {
            aumentarCantidad((lbItems.SelectedItem as Item).Id);          
            (lbItems.SelectedItem as Item).Cantidad = (lbItems.SelectedItem as Item).Cantidad + 1;          
            actualizarListaPedido();
        }

        private void btnQuitarCantidad_Click(object sender, RoutedEventArgs e)
        {
            
            if ((lbItems.SelectedItem as Item).Cantidad > 1)
            {
                (lbItems.SelectedItem as Item).Cantidad = (lbItems.SelectedItem as Item).Cantidad - 1;
                quitarCantidad((lbItems.SelectedItem as Item).Id);
            }
            actualizarListaPedido();
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
                    (lbItems.SelectedItem as Item).VisibilidadBotones = "Hidden";
                    itemExiste = false;
                    eliminarPedido((lbItems.SelectedItem as Item).Id);
                    actualizarListaPedido();

                }
                else
                {
                    
                    (lbItems.SelectedItem as Item).Cantidad = 1;
                    foreach (Item item1 in items)
                    {
                        if (item1.VisibilidadBotones == "Visible")
                        {
                            item1.VisibilidadBotones = "Hidden";
                            break;
                        }
                    }
                    (lbItems.SelectedItem as Item).Visibilidad = "Visible";
                    (lbItems.SelectedItem as Item).VisibilidadBotones = "Visible";

                    item = new Item()
                    {
                        Id = (lbItems.SelectedItem as Item).Id,
                        Cantidad = 1,
                        Nombre = (lbItems.SelectedItem as Item).Nombre,
                        Precio = (lbItems.SelectedItem as Item).Precio,
                    };
                    if (!verificarSiSePidio(item))
                    {
                        itemsVenta.Add(item);
                    }                   
                    actualizarListaPedido();

                }
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
            else
            {
                MessageBox.Show("Debe escoger por lo menos un producto");
            }
        }
    }
}
