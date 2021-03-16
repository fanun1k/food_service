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

namespace food_service.UserControls
{
    /// <summary>
    /// Lógica de interacción para ItemSnack.xaml
    /// </summary>
    public partial class ItemSnack : UserControl
    {
        private Item itemMostrar;
        public Item ItemMostrar
        {
            get { return itemMostrar; }
            set
            {
                itemMostrar = value;
                cargarItem();
            }
        }
        public ItemSnack()
        {
            InitializeComponent();

        }
        private void btnAumentarCantidad_Click(object sender, RoutedEventArgs e)
        {
            ItemsVenta.AddCant(ItemMostrar.Id);
            tbCantidad.Text = ItemMostrar.Cantidad.ToString();
        }
        private void btnQuitarCantidad_Click(object sender, RoutedEventArgs e)
        {
            if (ItemMostrar.Cantidad > 1)
            {
                ItemsVenta.PutOffCant(ItemMostrar.Id);
                tbCantidad.Text = ItemMostrar.Cantidad.ToString();
            }
        }
        public void OnItemSelected()
        {
            if (stackOcultar.Visibility == Visibility.Visible)
            {
                ItemMostrar.Cantidad = 0;
                stackOcultar.Visibility = Visibility.Hidden;
                borderCantidad.Visibility = Visibility.Hidden;
                tbCantidad.Text = itemMostrar.Cantidad.ToString();
                ItemsVenta.DeleteItem(ItemMostrar.Id);
            }
            else
            {
                ItemMostrar.Cantidad = 1;
                stackOcultar.Visibility = Visibility.Visible;
                borderCantidad.Visibility = Visibility.Visible;
                tbCantidad.Text = itemMostrar.Cantidad.ToString();
                ItemsVenta.AddItemVenta(ItemMostrar);
            }
        }
        private void cargarItem()
        {
            try
            {
                tbNombre.Text = ItemMostrar.Nombre;
                tbPrecio.Text = ItemMostrar.Precio.ToString();
                tbCantidad.Text = ItemMostrar.Cantidad.ToString();
                imgImage.Source = ItemMostrar.Imagen;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnItemSelected_Click(object sender, RoutedEventArgs e)
        {
            OnItemSelected();
        }
    }
}
