using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Orden
    {
        private int id;
        private DateTime fecha;
        private DateTime hora;
        private decimal monto;
        private double descuente;
        private int documento;
        private int cliente;
        private int usuario;
        private string estado;

        #region get set
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }


        public int Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }


        public int Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }


        public int Documento
        {
            get { return documento; }
            set { documento = value; }
        }


        public double Descuento
        {
            get { return descuente; }
            set { descuente = value; }
        }


        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
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


        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        #endregion
        public Orden()
        {

        }

        public Orden(int id, DateTime fecha, DateTime hora, decimal monto, double descuente, int documento, int cliente, int usuario, string estado)
        {
            this.id = id;
            this.fecha = fecha;
            this.hora = hora;
            this.monto = monto;
            this.descuente = descuente;
            this.documento = documento;
            this.cliente = cliente;
            this.usuario = usuario;
            this.estado = estado;
        }

        public Orden(decimal monto, int cliente)
        {       
            this.monto = monto;         
            this.cliente = cliente;
        }
        public Orden(decimal monto, int cliente, DateTime fecha, DateTime hora)
        {
            this.fecha = fecha;
            this.hora = hora;
            this.monto = monto;
            this.cliente = cliente;
        }
    }
}
