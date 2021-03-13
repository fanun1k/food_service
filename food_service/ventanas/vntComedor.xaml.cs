using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Implementation;
using Model;

namespace food_service.ventanas
{
    /// <summary>
    /// Lógica de interacción para vntComedor.xaml
    /// </summary>
    public partial class vntComedor : Window,IDisposable
    {
        ClienteImpl clienteImpl = null;
        RegistroImpl registroImpl = null;
        ItemImpl itemImpl = null;
        ServerImpl serverImpl;
        SnackImpl snackImpl;

        Cliente cliente=null;
        Registro registro = null;
        Orden orden;
        Item item;
        Snack snack;
        DateTime fechaHoraServidor;
        string textoQR = "";
        Tickets.TicketsComedor tiketImprimir;
        Tickets.TicketLunch ticketLunch;

        //estos datatable nos serviran para descaragar los datos de la tabla
        //registro y cliente para poder hacer la impresion del ticket
        DataTable dtRegistro;
        DataTable dtCliente;
        DataTable dtsnack;
        DataTable dtItem;


        public vntComedor()
        {
            InitializeComponent();
            btnEnter.IsEnabled = false;
            cargarComboLonches();
        }

        private void btnCerrarComedor_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// el metodo recibe la cadena que se obtuvo del QR, la valida y retorna una respuesta en entero
        /// </summary>
        /// <param name="lecturaQr">cadena de la lectura QR</param>
        /// <returns>retorna el codigo el cliente si es que lo tiene, si no lo tiene retorna 0</returns>
        private int ValidarCadenaQr(string lecturaQr)
        {
            int codigo = 0;
            try
            {
                string[] cadenaValidar = lecturaQr.Split('Ñ');
                if (cadenaValidar[0] == "CODIGO")
                {
                    if (cadenaValidar[1].Length>4)
                    {
                        int distanciaParaCortar = cadenaValidar[1].Trim().Length - 6;
                        codigo = int.Parse(cadenaValidar[1].Trim().Substring(0, distanciaParaCortar));
                    }
                    else
                    {
                        codigo = int.Parse(cadenaValidar[1].Trim());
                    }
                    
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return codigo;

        }

        /// <summary>
        /// metodo que leera uno por uno los caracteres impresos en el codigo QR
        /// y los ira juntando en la variable textoQR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vntComedor1_TextInput(object sender, TextCompositionEventArgs e)
        {
            textoQR = textoQR + e.Text;

        }

        /// <summary>
        /// evento que detectara cuando una tecla es pulsada
        /// en el Qr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void vntComedor_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    numeroFicha.Clear();
                    registroImpl = new RegistroImpl();
                    int codigoLeido =ValidarCadenaQr(textoQR.Trim());
                    if (codigoLeido > 0)
                    {
                        for (int i = 0; i < codigoLeido.ToString().Length; i++)
                        {
                            numeroFicha.Enqueue(int.Parse(codigoLeido.ToString().Substring(i, 1)));
                        }                      
                        ImprimirDatosCliente(codigoLeido);
                        //ralizar la accion cuando se utiliza QR                       
                        serverImpl = new ServerImpl();
                        fechaHoraServidor = serverImpl.FechaHoraServer();

                        switch (fechaHoraServidor.Hour)
                        {
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                                // en caso sea la hora en el servidor 8:00 9:00 o 10:00 se vendera Lunch
                                btnEnter.IsEnabled = false;
                                RealizarVentaLunch();
                                numeroFicha.Clear();
                                break;

                            case 11:
                            case 12:
                            case 13:
                                // en caso sea la hora en el servidor 11:00 12:00 o 13:00 se vendera Almuerzo
                                btnEnter.IsEnabled = false;
                                RealizarVentaEnRegistro("CODIGO QR", "ALMUERZO");
                                numeroFicha.Clear();
                                break;

                            case 17:
                            case 18:
                            case 19:
                                // en caso sea la hora en el servidor 17:00 18:00 o 19:00 se vendera Cena
                                btnEnter.IsEnabled = false;
                                RealizarVentaEnRegistro("CODIGO QR", "CENA");
                                numeroFicha.Clear();
                                break;
                            default:
                                numeroFicha.Clear();
                                LimpiarPantalla();
                                tbNombreComensal.Text = "FUERA DE TURNO";
                                break;
                        }
                        numeroFicha.Clear();

                    }
                    else
                    {
                        
                    }
                    textoQR = "";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("ups! ocurrio un error, contactese con su encargado de sistemas.\n error: " + ex.Message);
            }

        }


        /// <summary>
        /// metodo que muestra los datos del usuario si el codigo leido existe
        /// el metodo crea un objeto del tipo Cliente que se utiliza para hacer 
        /// el INSERT en la base de datos
        /// </summary>
        /// <param name="codigoLeido"></param>
        private void ImprimirDatosCliente(int codigoLeido)
        {
            try
            {
                Uri uri = new Uri("/food_service;component/appData/imgDefecto.png", UriKind.Relative);
                imgFotografia.Source = new BitmapImage(uri);
                tbCodigo.Text = codigoLeido.ToString();
                clienteImpl = new ClienteImpl();

                cliente = clienteImpl.Select(codigoLeido);
                if (cliente!=null)
                {
                    cliente.Codigo = codigoLeido.ToString();
                    if (cliente.Fotografia!=null)
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
                        tbNombreComensal.Text = cliente.Paterno + " " + cliente.Materno + ", " + cliente.Nombre + " (" + cliente.Estado+")";
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
                serverImpl = new ServerImpl();
                fechaHoraServidor = serverImpl.FechaHoraServer();

                switch (fechaHoraServidor.Hour)
                {
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        // en caso sea la hora en el servidor 8:00 9:00 o 10:00 se vendera Lunch
                        btnEnter.IsEnabled = false;
                        RealizarVentaLunch();                        
                        numeroFicha.Clear();
                        break;
                        
                    case 11:
                    case 12:
                    case 13:
                        // en caso sea la hora en el servidor 11:00 12:00 o 13:00 se vendera Almuerzo
                        btnEnter.IsEnabled = false;
                        RealizarVentaEnRegistro("TOUCH","ALMUERZO");                        
                        numeroFicha.Clear();
                        break;

                    case 17:
                    case 18:
                    case 19:
                        // en caso sea la hora en el servidor 17:00 18:00 o 19:00 se vendera Cena
                        btnEnter.IsEnabled = false;
                        RealizarVentaEnRegistro("TOUCH","CENA");                       
                        numeroFicha.Clear();
                        break;
                    default:
                        numeroFicha.Clear();
                        LimpiarPantalla();
                        tbNombreComensal.Text = "FUERA DE TURNO";
                        break;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("ups! ocurrio un error, contactese con su encargado de sistemas.\n error: " + ex.Message);
            }
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
            if (numeroFicha.Count>0)
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

                    btnEnter.IsEnabled = true;
                    string codigoLeido = "";
                    foreach (int n in numeroFicha)
                    {
                        codigoLeido = codigoLeido + n;
                    }
                    ImprimirDatosCliente(int.Parse(codigoLeido));
                    break;
                 
                }          
        }

        /// <summary>
        /// metodo que nos sirbe para mostrar los numeros de la pila 
        /// en el TextBlock de la vista 
        /// </summary>
        private void MostrarPila()
        {
            string fichaAMostrar="";
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

        /// <summary>
        /// metodo que hace el insert en la Base de Datos y hace la llamada al metodo imprimirTicket
        /// </summary>
        /// <param name="tipo">parametro para establecer si es tipo QR o TOUCH</param>
        private void RealizarVentaEnRegistro(string tipo,string turno)
        {
            try
            {
                registroImpl = new RegistroImpl();
                if (cliente != null)
                {
                    registro = new Registro
                    {
                        Cliente = cliente.Id,
                        Tipo = tipo,
                        Turno=turno
                    };
                    registroImpl = new RegistroImpl();
                    int resultado = registroImpl.Insert(registro);
                    if (resultado > 0)
                    {
                        ImprimirTicketRegistro(registroImpl.GetIdRegistro(),int.Parse(cliente.Codigo));
                        
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
                snackImpl = new SnackImpl();
                List<Snack> listSnack = new List<Snack>();
                itemImpl = new ItemImpl();
                item = itemImpl.SelectItem(int.Parse(cbLonches.SelectedValue.ToString()));
                
                orden = new Orden {
                    Monto = item.Precio,
                    Cliente = cliente.Id
                };

                snack = new Snack {
                    Cliente = cliente.Id,
                    Item =item.Id,
                    Precio=item.Precio,
                    Cantidad=1,
                    Total=item.Precio                   
                };
                listSnack.Add(snack);
                snackImpl.Insert(orden, listSnack);
                ImprimirTicketLunch(snackImpl.GetIdSnack(),int.Parse(cliente.Codigo));
                
                                
            }
            catch (Exception ex)
            {

                MessageBox.Show("ups! ocurrio un error, contactese con su encargado de sistemas.\n error: " + ex.Message);
            }
        }


        /// <summary>
        /// llena un DataSet con los balores de la tabla determinados por el id del registro 
        /// y el codigo del cliente, el DataSet es enviado al CrystalReport para poder generar el vale
        /// de consumo y ser impreso
        /// </summary>
        /// <param name="id">el id del tiket registrado</param>
        /// <param name="codigo"> numero de ficha del cliente</param>
        private void ImprimirTicketRegistro(int id,int codigo)
        {
            try
            {

                registroImpl = new RegistroImpl();
                clienteImpl = new ClienteImpl();
                dtRegistro = new DataTable();
                dtCliente = new DataTable();


                dtRegistro = registroImpl.GetTableRegistro(id);
              
                dtCliente = clienteImpl.GetTableCliente(codigo);
                //MessageBox.Show(dtCliente.Rows[0][1].ToString());
                //MessageBox.Show(dtCliente.Rows[0][2].ToString());
                //MessageBox.Show(dtCliente.Rows[0][3].ToString());



                tiketImprimir = new Tickets.TicketsComedor();

                tiketImprimir.Database.Tables["registro"].SetDataSource(dtRegistro);
               
                tiketImprimir.Database.Tables["cliente"].SetDataSource(dtCliente);


                tiketImprimir.PrintToPrinter(1, true, 0, 0);



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                tiketImprimir.Dispose();
            }
        }
        void ImprimirTicketLunch(int idSnack,int codigo)
        {
            try
            {
                dtCliente = new DataTable();
                dtItem = new DataTable();
                dtsnack = new DataTable();

                snackImpl = new SnackImpl();
                clienteImpl = new ClienteImpl();
                itemImpl = new ItemImpl();
               
                dtsnack = snackImpl.GetTableSnack(idSnack);
                dtCliente = clienteImpl.GetTableCliente(codigo);
                dtItem = itemImpl.SelectDataTableItem(int.Parse(dtsnack.Rows[0][3].ToString()));


                ticketLunch = new Tickets.TicketLunch();
                ticketLunch.Database.Tables["snack"].SetDataSource(dtsnack);
                ticketLunch.Database.Tables["cliente"].SetDataSource(dtCliente);
                ticketLunch.Database.Tables["item"].SetDataSource(dtItem);
                ticketLunch.PrintToPrinter(1,true,0,0);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                ticketLunch.Dispose();
            }

        }

        private void btnConfigurar_Click(object sender, RoutedEventArgs e)
        {
            gridConfigurar.Visibility = Visibility.Visible;
        }

        private void btnCerrarConfig_Click(object sender, RoutedEventArgs e)
        {
            gridConfigurar.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// carga el comboBox con los items que tengan en el nomnbre 
        /// la palabra 'LONCHE'
        /// </summary>
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
