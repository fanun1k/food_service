using System;
using System.Collections.Generic;
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
using Implementation;
using Model;
namespace food_service.ventanas
{
    /// <summary>
    /// Lógica de interacción para VntAsistencia.xaml
    /// </summary>
    public partial class VntAsistencia : Window
    {
        public string numeroFicha = "";
        UsuarioImpl usuarioImpl;
        DataTable dt;
        Usuario usuario = null;
        AsistenciaImpl asistenciaImpl;



        public VntAsistencia()
        {
            InitializeComponent();
            btnEnter.IsEnabled = false;
           
        }

        #region botones_numericos
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("0");
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("1");
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("2");
        }
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("3");
        }
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("4");
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("5");
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("6");
        }
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("7");
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("8");
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            Acoplar("9");
        }
        #endregion

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (numeroFicha.Length>0)
            {
                btnEnter.IsEnabled = false;
                numeroFicha = numeroFicha.Substring(0,numeroFicha.Length-1);
                tbCodigo.Text = numeroFicha;
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
                if (usuario!=null)
                {
                    asistenciaImpl = new AsistenciaImpl();
                    btnEnter.IsEnabled = false;
                    tbNombreComensal.Text=asistenciaImpl.RegistrarAsistencia(usuario.Id);
                    numeroFicha = "";                  
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void btnCerrarComedor_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void Acoplar(string numero)
        {
            switch (numeroFicha.Length)
            {
                case 0:
                    LimpiarPantalla();
                    numeroFicha+=(numero);
                    tbCodigo.Text = numeroFicha;
                    break;
                case 1:
                    numeroFicha+=(numero);
                    tbCodigo.Text = numeroFicha;
                    break;
                case 2:
                    numeroFicha += (numero);
                    tbCodigo.Text = numeroFicha;
                    break;
                case 3:
                    numeroFicha += (numero);
                    tbCodigo.Text = numeroFicha;
                    btnEnter.IsEnabled = true;      
                    ImprimirDatosCliente(int.Parse(numeroFicha));
                    break;
            }

        }

        private void ImprimirDatosCliente(int v)
        {
            try
            {
                usuarioImpl = new UsuarioImpl();
                dt = new DataTable();
                dt = usuarioImpl.SelectUsuario(v);
                if (dt.Rows.Count>0)
                {
                    usuario = new Usuario()
                    {
                        Id = int.Parse(dt.Rows[0][0].ToString()),
                        Nombre =dt.Rows[0][3].ToString(),
                        Paterno = dt.Rows[0][1].ToString(),
                        Materno = dt.Rows[0][2].ToString()
                    };

                    tbNombreComensal.Text = usuario.Paterno+" "+usuario.Materno+" "+usuario.Nombre;
                }
                else
                {
                    tbNombreComensal.Text = "EL USUARIO NO EXISTE";
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void LimpiarPantalla()
        {
            numeroFicha = "";
            tbCodigo.Text = "";
            tbNombreComensal.Text = "INGRESESU NUMERO DE FICHA";
            btnEnter.IsEnabled = false;

        }
    }
}
