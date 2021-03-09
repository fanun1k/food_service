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
using System.Windows.Shapes;
using Model;
using Implementation;
using System.ComponentModel;

namespace food_service.ventanas
{
    /// <summary>
    /// Lógica de interacción para VtnRegistrar.xaml
    /// </summary>
    public partial class VtnRegistrar : Window, INotifyPropertyChanged
    {
        private string codigo="";
        string tipo = "";


        ClienteImpl clienteImpl;
        Cliente cliente;
        Registro registro;
        RegistroImpl registroImpl;
        SnackImpl snackImpl;
        ItemImpl itemImpl;

        Orden orden;
        Item item;
        Snack snack;
        Ticket ticket;

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
                OnPropertyChanged("Codigo");} 
      
        }    
        public event PropertyChangedEventHandler PropertyChanged;
        

        public VtnRegistrar()
        {
            InitializeComponent();
            DataContext = this;
            calendario.SelectedDate = DateTime.Now;
            btnAceptar.IsEnabled = false;
            cargarComboLonches();
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            Apilar("0");
        }

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
                    tbNombre.Text = cliente.Paterno + " " + cliente.Materno + " " + cliente.Nombre;
                    btnAceptar.IsEnabled = true;
                }
                else
                {
                    tbNombre.Text = "NO EXISTE EL CLIENTE";
                    btnAceptar.IsEnabled = false;
                    cliente=null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
              
            }
        }
        #region botones de la inaterfaz
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
        #endregion
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            btnAceptar.IsEnabled = false;
            try
            {
                registroImpl = new RegistroImpl();
                    if (cliente!=null && tipo!="")
                    {
                        switch (tipo)
                        {
                            case "LUNCH":
                                
                                break;
                            case "ALMUERZO":
                            registro = new Registro() { Cliente = cliente.Id, Fecha = DateTime.Parse(calendario.SelectedDate.ToString()), Hora = DateTime.Parse("12:00:00"),Turno="ALMUERZO",Tipo="TOUCH" };
                            if (registroImpl.Insert(registro)>0)
                            {
                                tbNombre.Text = "";
                                codigo = "";
                                //Imprimir el ticket
                                MessageBox.Show("Registro exitoso a");

                            }
                            break;
                            case "CENA":
                            registro = new Registro() { Cliente = cliente.Id, Fecha = DateTime.Parse(calendario.SelectedDate.ToString()), Hora = DateTime.Parse("18:00:00"), Turno = "CENA", Tipo = "TOUCH" };
                            if (registroImpl.Insert(registro) > 0)
                            {
                                tbNombre.Text = "";
                                codigo = "";
                                //Imprimir el ticket
                                MessageBox.Show("Registro exitoso c");
                            }

                            break;
                        }
                    }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Codigo = "";
            tbNombre.Text = "";
        }

        private void bntLunch_Click(object sender, RoutedEventArgs e)
        {
            tipo = "LUNCH";
            bntLunch.Background = Brushes.SlateGray;
            btnCena.Background = Brushes.Transparent;
            btnAlmuerzo.Background = Brushes.Transparent;
        }

        private void btnAlmuerzo_Click(object sender, RoutedEventArgs e)
        {
            tipo = "ALMUERZO";
            bntLunch.Background = Brushes.Transparent;
            btnCena.Background = Brushes.Transparent;
            btnAlmuerzo.Background = Brushes.SlateGray;
        }

        private void btnCena_Click(object sender, RoutedEventArgs e)
        {
            tipo = "CENA";
            bntLunch.Background = Brushes.Transparent;
            btnCena.Background = Brushes.SlateGray;
            btnAlmuerzo.Background = Brushes.Transparent;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void RealizarVentaEnRegistro(string tipo, string turno)
        {
            ticket = new Ticket();
            try
            {
                registroImpl = new RegistroImpl();
                if (cliente != null)
                {
                    registro = new Registro
                    {
                        Cliente = cliente.Id,
                        Tipo = tipo,
                        Turno = turno
                    };
                    registroImpl = new RegistroImpl();
                    int resultado = registroImpl.Insert(registro);
                    if (resultado > 0)
                    {
                        ticket.ImprimirTicketRegistro(registroImpl.GetIdRegistro(), int.Parse(cliente.Codigo));
                        //MessageBox.Show("se hizo el insert con exito");
                    }
                    else
                    {
                        MessageBox.Show("no se pudo insertar un registro");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("error: " + ex.Message);
            }
        }
        void RealizarVentaLunch()
        {
            try
            {
                ticket = new Ticket();
                snackImpl = new SnackImpl();
                List<Snack> listSnack = new List<Snack>();
                itemImpl = new ItemImpl();
                item = itemImpl.SelectItem(int.Parse(cbLonches.SelectedValue.ToString()));

                orden = new Orden
                {
                    Monto = item.Precio,
                    Cliente = cliente.Id
                };
                snack = new Snack
                {
                    Cliente = cliente.Id,
                    Item = item.Id,
                    Precio = item.Precio,
                    Cantidad = 1,
                    Total = item.Precio
                };
                listSnack.Add(snack);
                snackImpl.Insert(orden, listSnack);
                ticket.ImprimirTicketLunch(snackImpl.GetIdSnack(), int.Parse(cliente.Codigo));
            }
            catch (Exception ex)
            {

                MessageBox.Show("error: " + ex.Message);
            }
        }
        public void cargarComboLonches()
        {
            try
            {
                itemImpl = new ItemImpl();

                cbLonches.DisplayMemberPath = "nombre";
                cbLonches.SelectedValuePath = "id";
                cbLonches.ItemsSource = itemImpl.SelectLunch().DefaultView;
                cbLonches.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ups! ocurrio un error, contactese con su encargado de sistemas.  \n error: " + ex.Message);
            }
        }
    }
}
