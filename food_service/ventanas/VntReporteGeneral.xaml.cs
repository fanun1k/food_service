using Implementation;
using Model;
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
using System.Windows.Shapes;

namespace food_service.ventanas
{
    /// <summary>
    /// Lógica de interacción para VntReporteGeneral.xaml
    /// </summary>
    /// 

    enum ItemId
    {
        Desayuno = 121,
        Lunch = 123,
        Almuerzo = 118,
        Cena = 122
    }


    public partial class VntReporteGeneral : Window
    {
        SnackImpl snackImpl;
        ItemImpl itemImpl;
        RegistroImpl registroImpl;
        bool usuarioSeleccionado;
        public static Cliente clienteActual;
        public VntReporteGeneral()
        {
            InitializeComponent();
            configurarFechaActual();
            usuarioSeleccionado = false;
            configurarBotones();
        }

        private DateTime obtenerFechaComedor()
        {
            return dpComedor.SelectedDate.Value.Date;
        }

        private void configurarFechaActual()
        {
            dpComedor.SelectedDate = DateTime.Now;
            generarReportesComedor(obtenerFechaComedor());         
        }
        private void generarReportesComedor(DateTime fecha)
        {
            snackImpl = new SnackImpl();
            itemImpl = new ItemImpl();
            registroImpl = new RegistroImpl();

            if (usuarioSeleccionado)
            {
                //comedor
                var dataListDesayuno = snackImpl.SelectTotalPorId((int)ItemId.Desayuno, fecha.ToString("yyyy-MM-dd"), clienteActual.Id);
                totDes.Content = dataListDesayuno[0].ToString() + " Bs.";
                cantDes.Content = dataListDesayuno[1];

                var dataListLunch = snackImpl.SelectTotalPorId((int)ItemId.Lunch, fecha.ToString("yyyy-MM-dd"), clienteActual.Id);
                totLunch.Content = dataListLunch[0].ToString() + " Bs.";
                cantLunch.Content = dataListLunch[1];

                var CantidadAlmuerzo = registroImpl.obtenerCantAlmuerzoOCenaPOrId("ALMUERZO", fecha.ToString("yyyy-MM-dd"), clienteActual.Id);
                var dataAlmuerzo = itemImpl.SelectPrecio((int)ItemId.Almuerzo) * CantidadAlmuerzo;
                cantAlmu.Content = CantidadAlmuerzo;
                totAlmu.Content = dataAlmuerzo.ToString() + " Bs.";

                var CantidadCena = registroImpl.obtenerCantAlmuerzoOCenaPOrId("CENA", fecha.ToString("yyyy-MM-dd"), clienteActual.Id);
                var dataCena = itemImpl.SelectPrecio((int)ItemId.Cena) * CantidadCena;
                cantCena.Content = CantidadCena;
                totCena.Content = dataCena.ToString() + " Bs.";

                var totalComedor = dataListDesayuno[0] + dataListLunch[0] + dataAlmuerzo + dataCena;
                totComedor.Content = totalComedor.ToString() + " Bs.";
                //snack

                var cantPersonal = registroImpl.obtenerCantSnackPorid("PERSONAL", fecha.ToString("yyyy-MM-dd"), clienteActual.Id);
                var dataPersonal = itemImpl.SelectPrecio((int)ItemId.Lunch) * cantPersonal;
                totPersonal.Content = dataPersonal.ToString() + " Bs.";


                var totalSnack = dataPersonal;
                totSnack.Content = totalSnack.ToString() + " Bs.";

                //otros
                totComedorU.Content = totalComedor.ToString() + " Bs.";
                totSnackU.Content = totalSnack.ToString() + " Bs.";
                totalTodo.Content = (totalComedor + totalSnack).ToString() + " Bs.";
            }
            else
            {
                //comedor
                var dataListDesayuno = snackImpl.SelectTotal((int)ItemId.Desayuno, fecha.ToString("yyyy-MM-dd"));
                totDes.Content = dataListDesayuno[0].ToString() + " Bs.";
                cantDes.Content = dataListDesayuno[1];

                var dataListLunch = snackImpl.SelectTotal((int)ItemId.Lunch, fecha.ToString("yyyy-MM-dd"));
                totLunch.Content = dataListLunch[0].ToString() + " Bs.";
                cantLunch.Content = dataListLunch[1];

                var CantidadAlmuerzo = registroImpl.obtenerCantAlmuerzoOCena("ALMUERZO", fecha.ToString("yyyy-MM-dd"));
                var dataAlmuerzo = itemImpl.SelectPrecio((int)ItemId.Almuerzo) * CantidadAlmuerzo;
                cantAlmu.Content = CantidadAlmuerzo;
                totAlmu.Content = dataAlmuerzo.ToString() + " Bs.";

                var CantidadCena = registroImpl.obtenerCantAlmuerzoOCena("CENA", fecha.ToString("yyyy-MM-dd"));
                var dataCena = itemImpl.SelectPrecio((int)ItemId.Cena) * CantidadCena;
                cantCena.Content = CantidadCena;
                totCena.Content = dataCena.ToString() + " Bs.";

                var totalComedor = dataListDesayuno[0] + dataListLunch[0] + dataAlmuerzo + dataCena;
                totComedor.Content = totalComedor.ToString() + " Bs.";
                //snack

                var cantPersonal = registroImpl.obtenerCantSnack("PERSONAL", fecha.ToString("yyyy-MM-dd"));
                var dataPersonal = itemImpl.SelectPrecio((int)ItemId.Lunch) * cantPersonal;
                totPersonal.Content = dataPersonal.ToString() + " Bs.";


                var totalSnack = dataPersonal;
                totSnack.Content = totalSnack.ToString() + " Bs.";

                //otros
                totComedorU.Content = totalComedor.ToString() + " Bs.";
                totSnackU.Content = totalSnack.ToString() + " Bs.";
                totalTodo.Content = (totalComedor + totalSnack).ToString() + " Bs.";
            }
        }

        private void dpFecha_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            generarReportesComedor(obtenerFechaComedor());
        }

        private void configurarBotones()
        {
            if (!usuarioSeleccionado)
            {
                btnRegistroGeneralUsuario.IsEnabled = false;
                btnLimpiarUsuario.IsEnabled = false;
            }
        }

        private void btnSeleccionarUsuario_Click(object sender, RoutedEventArgs e)
        {
            VntIngresarCliente vic = new VntIngresarCliente();
            vic.Show();
        }

        private void btnRegistroGeneralUsuario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //comedor
                var dataListDesayuno = snackImpl.SelectTotalPorIdTotal((int)ItemId.Desayuno, clienteActual.Id);
                totDes.Content = dataListDesayuno[0].ToString() + " Bs.";
                cantDes.Content = dataListDesayuno[1];

                var dataListLunch = snackImpl.SelectTotalPorIdTotal((int)ItemId.Lunch, clienteActual.Id);
                totLunch.Content = dataListLunch[0].ToString() + " Bs.";
                cantLunch.Content = dataListLunch[1];

                var CantidadAlmuerzo = registroImpl.obtenerCantAlmuerzoOCenaPOrIdTodo("ALMUERZO", clienteActual.Id);
                var dataAlmuerzo = itemImpl.SelectPrecio((int)ItemId.Almuerzo) * CantidadAlmuerzo;
                cantAlmu.Content = CantidadAlmuerzo;
                totAlmu.Content = dataAlmuerzo.ToString() + " Bs.";

                var CantidadCena = registroImpl.obtenerCantAlmuerzoOCenaPOrIdTodo("CENA", clienteActual.Id);
                var dataCena = itemImpl.SelectPrecio((int)ItemId.Cena) * CantidadCena;
                cantCena.Content = CantidadCena;
                totCena.Content = dataCena.ToString() + " Bs.";

                var totalComedor = dataListDesayuno[0] + dataListLunch[0] + dataAlmuerzo + dataCena;
                totComedor.Content = totalComedor.ToString() + " Bs.";
                //snack

                var cantPersonal = registroImpl.obtenerCantSnackPorIdTodo("PERSONAL", clienteActual.Id);
                var dataPersonal = itemImpl.SelectPrecio((int)ItemId.Lunch) * cantPersonal;
                totPersonal.Content = dataPersonal.ToString() + " Bs.";


                var totalSnack = dataPersonal;
                totSnack.Content = totalSnack.ToString() + " Bs.";

                //otros
                totComedorU.Content = totalComedor.ToString() + " Bs.";
                totSnackU.Content = totalSnack.ToString() + " Bs.";
                totalTodo.Content = (totalComedor + totalSnack).ToString() + " Bs.";
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void btnLimpiarUsuario_Click(object sender, RoutedEventArgs e)
        {
            clienteActual = new Cliente();
            lblUsuario.Content = "";
            btnRegistroGeneralUsuario.IsEnabled = false;
            btnLimpiarUsuario.IsEnabled = false;
            usuarioSeleccionado = false;
        }

        private void btnVerificar_Click(object sender, RoutedEventArgs e)
        {    
           //solo prueba
                lblUsuario.Content = clienteActual.Nombre;
                btnRegistroGeneralUsuario.IsEnabled = true;
                btnLimpiarUsuario.IsEnabled = true;
                usuarioSeleccionado = true;
                     
        }
    }
}
