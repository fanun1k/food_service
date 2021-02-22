using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Snack
    {
        private int id;
        private int usuario;
        private int cliente;
        private int item;
        private DateTime fecha;
        private DateTime hora;
        private double precio;
        private int cantidad;
        private double total;
        private string tipo;
        private string estado;
        private int order;
        private int contado;

        #region get set
        public int Contado
        {
            get { return contado; }
            set { contado = value; }
        }


        public int Orden
        {
            get { return order; }
            set { order = value; }
        }


        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }


        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }


        public double Total
        {
            get { return total; }
            set { total = value; }
        }


        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }


        public double Precio
        {
            get { return precio; }
            set { precio = value; }
        }


        public DateTime Hora
        {
            get { return hora; }
            set { hora = value; }
        }



        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }


        public int Item
        {
            get { return item; }
            set { item = value; }
        }



        public int Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }


        public int Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }


        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        #endregion
        public Snack()
        {

        }

        public Snack(int id, int usuario, int cliente, int item, DateTime fecha, DateTime hora, double precio, int cantidad, double total, string tipo, string estado, int order, int contado)
        {
            this.id = id;
            this.usuario = usuario;
            this.cliente = cliente;
            this.item = item;
            this.fecha = fecha;
            this.hora = hora;
            this.precio = precio;
            this.cantidad = cantidad;
            this.total = total;
            this.tipo = tipo;
            this.estado = estado;
            this.order = order;
            this.contado = contado;
        }
    }
}
