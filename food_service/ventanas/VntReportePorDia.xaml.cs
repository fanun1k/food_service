using Implementation;
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
    /// Lógica de interacción para VntReportePorDia.xaml
    /// </summary>
    public partial class VntReportePorDia : Window
    {
        SnackImpl snackImpl;
        ItemImpl ItemImpl;
        RegistroImpl registroImpl;

        public VntReportePorDia()
        {
            InitializeComponent();
            configInicio();
        }

        private void dpDia_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            sacarReportes();
        }

        private void sacarReportes()
        {
            snackImpl = new SnackImpl();
            ItemImpl = new ItemImpl();
            registroImpl = new RegistroImpl();

            dgLonche.ItemsSource = null;
            dgLonche.ItemsSource = snackImpl.SelectAllLunchesPorUsuario(obtenerfechaActual()).DefaultView;

            dgAlmuerzo.ItemsSource = null;
            dgAlmuerzo.ItemsSource = registroImpl.obtenerRegistroPOrTurno(obtenerfechaActual(), "ALMUERZO", ItemImpl.SelectPrecioPorNombre("3-ALMUERZO")).DefaultView;

            dgCena.ItemsSource = null;
            dgCena.ItemsSource = registroImpl.obtenerRegistroPOrTurno(obtenerfechaActual(), "CENA", ItemImpl.SelectPrecioPorNombre("4-CENA")).DefaultView;

            dgSnack.ItemsSource = null;
            dgSnack.ItemsSource = snackImpl.SelectAllNotLunchesPorUsuario(obtenerfechaActual()).DefaultView;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string obtenerfechaActual()
        {
            return dpDia.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
        }
        private void configInicio()
        {
            dpDia.SelectedDate = DateTime.Now;
            dpDia.DisplayDateEnd = DateTime.Today;
            sacarReportes();
        }
    }
}
