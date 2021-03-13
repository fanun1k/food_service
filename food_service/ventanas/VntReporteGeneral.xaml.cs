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
    //TODO: refactor
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

        public VntReporteGeneral()
        {
            InitializeComponent();
            configurarYBuscar();  
            
        }

        private string obtenerFechaInicio()
        {           
            try
            {
                return dpInicio.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            }
            catch (Exception)
            {

                return "";
            }
        }
        private string obtenerFechaFinal()
        {
            try
            {
                return dpFinal.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            }
            catch (Exception)
            {

                return "";
            }          
        }

        private void configurarYBuscar()
        {
            dpInicio.DisplayDateEnd = DateTime.Today;
            dpFinal.DisplayDateEnd = DateTime.Today;
            generarReportes();         
        }
        private void generarReportes()
        {

            snackImpl = new SnackImpl();
            itemImpl = new ItemImpl();
            registroImpl = new RegistroImpl();

            //comedor
            var dataListDesayuno = snackImpl.SelectTotalGeneral("DESAYUNO", obtenerFechaInicio(), obtenerFechaFinal());
            totDes.Content = dataListDesayuno[0].ToString() + " Bs.";
            cantDes.Content = dataListDesayuno[1];

            var dataListLunch = snackImpl.SelectTotalGeneral("LONCHE", obtenerFechaInicio(), obtenerFechaFinal());
            totLunch.Content = dataListLunch[0].ToString() + " Bs.";
            cantLunch.Content = dataListLunch[1];

            var CantidadAlmuerzo = registroImpl.obtenerCantAlmuerzoOCenaGeneral("ALMUERZO", obtenerFechaInicio(), obtenerFechaFinal());
            var dataAlmuerzo = itemImpl.SelectPrecioPorNombre("3-ALMUERZO") * CantidadAlmuerzo;
            cantAlmu.Content = CantidadAlmuerzo;
            totAlmu.Content = dataAlmuerzo.ToString() + " Bs.";

            var CantidadCena = registroImpl.obtenerCantAlmuerzoOCenaGeneral("CENA", obtenerFechaInicio(), obtenerFechaFinal());
            var dataCena = itemImpl.SelectPrecioPorNombre("4-CENA") * CantidadCena;
            cantCena.Content = CantidadCena;
            totCena.Content = dataCena.ToString() + " Bs.";

            var totalComedor = dataListDesayuno[0] + dataListLunch[0] + dataAlmuerzo + dataCena;
            totComedor.Content = totalComedor.ToString() + " Bs.";
            //snack

            var totalSnack = snackImpl.SelectTotalSnackSinLoncheGeneral(obtenerFechaInicio(), obtenerFechaFinal());
            totSnackU.Content = totalSnack.ToString() + " Bs.";

            //otros
            totComedorU.Content = totalComedor.ToString() + " Bs.";
            totalTodo.Content = (totalComedor + totalSnack).ToString() + " Bs.";
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            generarReportes();
        }

        private void btnReiniciarInicio_Click(object sender, RoutedEventArgs e)
        {
            dpInicio.SelectedDate = null;
        }

        private void btnReiniciarFinal_Click(object sender, RoutedEventArgs e)
        {
            dpFinal.SelectedDate = null;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
