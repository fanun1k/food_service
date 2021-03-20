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
            dpInicio.SelectedDate = DateTime.Now;
            dpFinal.SelectedDate = DateTime.Now;
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
            totDes.Text = dataListDesayuno[0].ToString() + " Bs.";
            cantDes.Text = dataListDesayuno[1].ToString();

            var dataListLunch = snackImpl.SelectTotalGeneral("LONCHE", obtenerFechaInicio(), obtenerFechaFinal());
            totLunch.Text = dataListLunch[0].ToString() + " Bs.";
            cantLunch.Text = dataListLunch[1].ToString();

            var CantidadAlmuerzo = registroImpl.obtenerCantAlmuerzoOCenaGeneral("ALMUERZO", obtenerFechaInicio(), obtenerFechaFinal());
            var dataAlmuerzo = itemImpl.SelectPrecioPorNombre("3-ALMUERZO") * CantidadAlmuerzo;
            cantAlmu.Text = CantidadAlmuerzo.ToString();
            totAlmu.Text = dataAlmuerzo.ToString() + " Bs.";

            var CantidadCena = registroImpl.obtenerCantAlmuerzoOCenaGeneral("CENA", obtenerFechaInicio(), obtenerFechaFinal());
            var dataCena = itemImpl.SelectPrecioPorNombre("4-CENA") * CantidadCena;
            cantCena.Text = CantidadCena.ToString();
            totCena.Text = dataCena.ToString() + " Bs.";

            var totalComedor = dataListDesayuno[0] + dataListLunch[0] + dataAlmuerzo + dataCena;
            totComedor.Text = totalComedor.ToString() + " Bs.";
            //snack

            var totalSnack = snackImpl.SelectTotalSnackSinLoncheGeneral(obtenerFechaInicio(), obtenerFechaFinal());
            totSnackU.Text = totalSnack.ToString() + " Bs.";

            //otros
            totComedorU.Text = totalComedor.ToString() + " Bs.";
            totalTodo.Text = (totalComedor + totalSnack).ToString() + " Bs.";
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            generarReportes();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
