using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Model
{
    

    public class Item:INotifyPropertyChanged
    {
        private int id;
        private string nombre;
        private string descripcion;
        private double precio;
        private int categoria;
        private int stock;
        private BitmapImage imagen;
        private string estado;
        private int cantidad;
        private string visibilidad;

       


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged!=null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(property));
            }
        }




        #region get set
        public string Visibilidad
        {
            get { return visibilidad; }
            set { visibilidad = value;
                OnPropertyChanged("Visibilidad");
            }
        }
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value;
                OnPropertyChanged("Cantidad");
            }
        }

        public BitmapImage Imagen
        {
            get { return imagen; }
            set { imagen = value; }
        }

        public int Stock
        {
            get { return stock; }
            set { stock = value; }
        }


        public int Categoria
        {
            get { return categoria; }
            set { categoria = value; }
        }


        public double Precio
        {
            get { return precio; }
            set { precio = value; }
        }


        public string Descipcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }


        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }


        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        #endregion
        public Item()
        {

        }
        public Item(int id, string nombre, string descripcion, double precio, int categoria, int stock, BitmapImage imagen, string estado)
        {
            this.id = id;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.precio = precio;
            this.categoria = categoria;
            this.stock = stock;
            this.imagen = imagen;
            this.estado = estado;
        }
    }
}
