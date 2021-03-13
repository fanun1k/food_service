﻿using System;
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
using System.ComponentModel;

namespace food_service
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        ItemImpl itemImpl;
        SnackImpl snackImpl;
        List<Item> items;
        ObservableCollection<Item> itemsVenta;
        Item item = null;
        bool itemExiste = false;
        ItemSnack itemSnack;

        private decimal total;

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public decimal Total
        {
            get { return ItemsVenta.GetTotal(); }
            set { total = value;
                OnPropertyChanged("Total");
            }
        }



        public MainWindow()
        {
            InitializeComponent();
            items = new List<Item>();
            cargarItemsBDDALista();
            lbItemsVenta.ItemsSource = ItemsVenta.items;
            DataContext = this;
            mostrarUltimoReporte();
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
                        Imagen = bmi
                    });
                    lbItems.Items.Add(itemSnack);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }

        }
        //private decimal obtenerTotal()
        //{
        //    decimal total = 0;
        //    foreach (var item in itemsVenta)
        //    {
        //        total = total + (item.Precio * item.Cantidad);
        //    }
        //    return total;
        //}

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {

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
        private void btnComedor_Click(object sender, RoutedEventArgs e)
        {
            ventanas.vntComedor vc = new ventanas.vntComedor();
            vc.Show();
        }
        private void btnAsistencia_Click(object sender, RoutedEventArgs e)
        {
            ventanas.VntAsistencia vntAsistencia = new ventanas.VntAsistencia();
            vntAsistencia.Show();
        }

        private void eliminarPedido(int Id)
        {
            itemsVenta.Remove(itemsVenta.Where(item => item.Id == Id).Single());
        }

        private void btnReportePorPersona_Click(object sender, RoutedEventArgs e)
        {
            VntIngresarCliente vic = new VntIngresarCliente();
            vic.Show();
        }

        private void btnReportePorDia_Click(object sender, RoutedEventArgs e)
        {
            VntReportePorDia vrpd = new VntReportePorDia();
            vrpd.Show();
        }

        private void mostrarUltimoReporte()
        {
            int x = 0;
            snackImpl = new SnackImpl();
            var ultimaVenta = snackImpl.SelectUltimaVentaSnack();
            lblFecha.Text = DateTime.Parse(ultimaVenta.Rows[0][1].ToString()).ToString("dd-MM-yyyy");
            lblNombre.Content = ultimaVenta.Rows[0][0];
            foreach (DataRow venta in ultimaVenta.Rows)
            {
                lblItems.Content = venta[2].ToString() + " | "+ venta[3].ToString() + " | " + venta[4].ToString() + " | " + venta[5].ToString() + "\n";
            }
        }
    }
}
