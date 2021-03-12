using Implementation;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace food_service.ventanas
{
    /// <summary>
    /// Lógica de interacción para VntSnack.xaml
    /// </summary>
    public partial class VntSnack : Window
    {
        ClienteImpl clienteImpl = null;
        SnackImpl snackImpl = null;
        Cliente cliente = null;
        decimal total;
        ObservableCollection<Item> lista;
        string codigoClienteBase;


        public VntSnack(decimal totalTodo, ObservableCollection<Item> listaPedidos)
        {
            InitializeComponent();
            btnEnter.IsEnabled = false;
            total = totalTodo;
            lista = listaPedidos;
        }

        private void btnCerrarComedor_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

      
        /// <summary>
        /// metodo que muestra los datos del usuario si el codigo leido existe
        /// el metodo crea un objeto del tipo Cliente que se utiliza para hacer 
        /// el INSERT en la base de datos
        /// </summary>
        /// <param name="codigoLeido"></param>


        Queue<int> numeroFicha = new Queue<int>();
        #region metodos que ayudan a agregar numeros a la variable numeroFicha segun los botones que pulsamos en la vista
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            Apilar(0);
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Apilar(1);
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            Apilar(2);
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            Apilar(3);
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            Apilar(4);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            Apilar(5);
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            Apilar(6);
        }
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            Apilar(7);
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            Apilar(8);
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            Apilar(9);
        }
        #endregion

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            //RealizarVentaEnRegistro("TOUCH");
            //numeroFicha.Clear();
            try
            {
                decimal monto = 0;
                foreach (var item in lista)
                {
                    monto = monto + (item.Precio * item.Cantidad);
                }
                if (monto == total)
                {

                    snackImpl = new SnackImpl();
                    clienteImpl = new ClienteImpl();
                    var codigoCliente = clienteImpl.SelectIdPorCodigo(int.Parse(codigoClienteBase)).Id;
                    snackImpl.Insert(new Orden(total, codigoCliente), convertirAListaSnacks(lista)); ;
                    MessageBox.Show("Pedido insertado con exito");
                    MainWindow.itemsVenta = new ObservableCollection<Item>();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Los valores totales no coinciden");
                    this.Close();
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show("ups! ocurrio un error, contactese con su encargado de sistemas.\n error: " + ex.Message);
            }
        }

        private List<Snack> convertirAListaSnacks(ObservableCollection<Item> listaObservable)
        {
            List<Snack> nuevaLista = new List<Snack>();
            clienteImpl = new ClienteImpl();
            var codigoCliente = clienteImpl.SelectIdPorCodigo(int.Parse(codigoClienteBase)).Id;
            foreach (var item in listaObservable)
            {
                nuevaLista.Add(new Snack(codigoCliente, item.Id, item.Precio, item.Cantidad, item.Precio * item.Cantidad));
            }
            return nuevaLista;
        }

        /// <summary>
        /// borra el ultimo numero ingresado y limpia la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            btnEnter.IsEnabled = false;
            int aux;
            if (numeroFicha.Count > 0)
            {

                for (int i = 1; i < numeroFicha.Count; i++)
                {
                    aux = numeroFicha.Peek();
                    numeroFicha.Dequeue();
                    numeroFicha.Enqueue(aux);
                }
                numeroFicha.Dequeue();
                LimpiarPantalla();
                MostrarPila();
            }
            else
            {
                LimpiarPantalla();
            }


        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            numeroFicha.Clear();
            LimpiarPantalla();
            btnEnter.IsEnabled = false;
        }

        /// <summary>
        /// metodo que utiliza un  Queue(cola) para ir apilado los numeros marcados desde la vista
        /// solo apila 4 digitos y luego bloquea el teclado de la pantalla
        /// al estar 4 digitos apilados el metodo busca en la bdd al cliente con el codigo
        /// que esta guardado en la cola
        /// </summary>
        /// <param name="numero"></param>
        private void Apilar(int numero)
        {

            switch (numeroFicha.Count)
            {
                case 0:
                    LimpiarPantalla();
                    numeroFicha.Enqueue(numero);
                    MostrarPila();
                    break;
                case 1:
                    numeroFicha.Enqueue(numero);
                    MostrarPila();
                    break;
                case 2:
                    numeroFicha.Enqueue(numero);
                    MostrarPila();
                    break;
                case 3:
                    numeroFicha.Enqueue(numero);
                    MostrarPila();
                    string codigoLeido = "";
                    foreach (int n in numeroFicha)
                    {
                        codigoLeido = codigoLeido + n;
                    }
                    ImprimirDatosCliente(int.Parse(codigoLeido));
                    codigoClienteBase = codigoLeido;
                    break;
            }
        }

        /// <summary>
        /// metodo que nos sirbe para mostrar los numeros de la pila 
        /// en el TextBlock de la vista 
        /// </summary>
        private void MostrarPila()
        {
            string fichaAMostrar = "";
            foreach (int numero in numeroFicha)
            {
                fichaAMostrar = fichaAMostrar + numero;
            }
            tbCodigo.Text = fichaAMostrar;
        }

        /// <summary>
        /// metodo que nos limpiara los datos de la pantalla y pondra unos por defecto
        /// </summary>
        private void LimpiarPantalla()
        {
            Uri uri = new Uri("/food_service;component/appData/imgDefecto.png", UriKind.Relative);
            imgFotografia.Source = new BitmapImage(uri);
            tbCodigo.Text = "";
            tbNombreComensal.Text = "INGRESE SU CODIGO O TARJETA";
        }

        private void ImprimirDatosCliente(int codigoLeido)
        {
            try
            {
                Uri uri = new Uri("/food_service;component/appData/imgDefecto.png", UriKind.Relative);
                imgFotografia.Source = new BitmapImage(uri);
                tbCodigo.Text = codigoLeido.ToString();
                clienteImpl = new ClienteImpl();

                cliente = clienteImpl.Select(codigoLeido);
                if (cliente != null)
                {
                    cliente.Codigo = codigoLeido.ToString();
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
                        tbNombreComensal.Text = cliente.Paterno + " " + cliente.Materno + ", " + cliente.Nombre + " (" + cliente.Estado + ")";
                    }
                    else
                    {
                        btnEnter.IsEnabled = true;
                        tbNombreComensal.Text = cliente.Paterno + " " + cliente.Materno + ", " + cliente.Nombre;

                    }
                }
                else
                {
                    tbNombreComensal.Text = "CODIGO NO REGISTRADO";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("ups! ocurrio un error, contactese con su encargado de sistemas.\n error: " + ex.Message);
            }

        }
        
    }
}
