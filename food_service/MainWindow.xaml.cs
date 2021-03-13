﻿using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Model;
using Implementation;
using System.Data;
using food_service.ventanas;
using ControlesFoodService;
using System.ComponentModel;
using System.Collections.Generic;

namespace food_service
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window,INotifyPropertyChanged

    {
        ItemImpl itemImpl;
        Cliente cliente=null;
        ClienteImpl clienteImpl;
        Orden orden;
        SnackImpl snackImpl;
        Snack snack;
        
        ItemSnack itemSnack;        
        public MainWindow()
        {
            InitializeComponent();
            cargarItemsBDDALista();
            lbItemsVenta.ItemsSource = ItemsVenta.items;
            DataContext = this;
            btnEnter.IsEnabled = false;
        }
        private string codigo="";

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value;
                OnPropertyChanged("Codigo");
            }
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
        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsVenta.GetTotal()>0 )
            {
                GridIngresarCodigoVenta.Visibility = Visibility.Visible;
            }
        }
        #region botones_de_las_pestañas
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
        #endregion
        #region botones_numericos
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            Apilar("0");
        }
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Apilar("1");
        }
        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            Apilar("2");
        }
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            Apilar("3");
        }
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            Apilar("4");
        }
        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            Apilar("5");
        }
        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            Apilar("6");
        }
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            Apilar("7");
        }
        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            Apilar("8");
        }
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            Apilar("9");
        }
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (Codigo.Length>1)
            {
                Codigo = Codigo.Substring(0,Codigo.Length-1);
                btnEnter.IsEnabled = false;
            }
            else
            {
                LimpiarPantalla();
            }
        }
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarPantalla();
        }
        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RealizarVentaSnack();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void btnCerrarComedor_Click(object sender, RoutedEventArgs e)
        {
            GridIngresarCodigoVenta.Visibility = Visibility.Collapsed;
        }
        #endregion
        private void Apilar(string v)
        {
            switch (Codigo.Length)
            {
                case 0:
                    Codigo += v;
                    break;
                case 1:
                    Codigo += v;
                    break;
                case 2:
                    Codigo += v;
                    break;
                case 3:
                    Codigo += v;
                    MostrarDatos(Codigo);
                    break;
            }
        }
        private void MostrarDatos(string codigo)
        {
            try
            {
                clienteImpl = new ClienteImpl();
                cliente = clienteImpl.Select(int.Parse(codigo));
                if (cliente!=null)
                {
                    if (cliente.Fotografia != null)
                    {
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(cliente.Fotografia);
                        ms.Seek(0, System.IO.SeekOrigin.Begin);
                        BitmapImage bmi = new BitmapImage();
                        bmi.BeginInit();
                        bmi.StreamSource = ms;
                        bmi.EndInit();
                        imgFotografia.Source = bmi;
                    }
                    if (cliente.Estado.Trim() == "INACTIVO")
                    {
                        btnEnter.IsEnabled = false;
                        tbNombre.Text = cliente.Paterno + " " + cliente.Materno + ", " + cliente.Nombre + " (" + cliente.Estado + ")";
                    }
                    else
                    {
                        btnEnter.IsEnabled = true;
                        tbNombre.Text = cliente.Paterno + " " + cliente.Materno + ", " + cliente.Nombre;
                    }
                }
                else
                {
                    tbNombre.Text = "EL CLIENTE NO EXISTE";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        private void LimpiarPantalla()
        {
            Uri uri = new Uri("/food_service;component/appData/imgDefecto.png", UriKind.Relative);
            imgFotografia.Source = new BitmapImage(uri);
            Codigo = "";
            tbNombre.Text = "INGRESE SU CODIGO O TARJETA";
            btnEnter.IsEnabled = false;
        }
        void RealizarVentaSnack()
        {

            try
            {
                snackImpl = new SnackImpl();
                itemImpl = new ItemImpl();
                orden = new Orden()
                {
                    Monto = ItemsVenta.GetTotal(),
                    Cliente = cliente.Id
                };
                List<Snack> lista = new List<Snack>();
                foreach (Item item in ItemsVenta.GetItemsVenta())
                {
                    snack = new Snack
                    {
                        Cliente = cliente.Id,
                        Item = item.Id,
                        Precio = item.Precio,
                        Cantidad =item.Cantidad,
                        Total = item.Precio
                    };
                    lista.Add(snack);
                }

                snackImpl.Insert(orden, lista);
                // ImprimirTicketLunch(snackImpl.GetIdSnack(), int.Parse(cliente.Codigo));
                LimpiarPantalla();
                GridIngresarCodigoVenta.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
