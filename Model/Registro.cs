using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Registro
    {
        private int id;
        private int cliente;
        private DateTime fecha;
        private DateTime hora;
        private string turno;
        private string cuenta;
        private int cantidad;
        private string tipo;
        private string estado;

        #region get set

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
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


        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }


        public string Cuenta
        {
            get { return cuenta; }
            set { cuenta = value; }
        }


        public string Turno
        {
            get { return turno; }
            set { turno = value; }
        }


        public DateTime Hora
        {
            get { return hora; }
            set { hora = value; }
        }


        public int Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }


        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        #endregion

        public Registro()
        {

        }

        public Registro(int id, int cliente, DateTime fecha, DateTime hora, string turno, string cuenta, int cantidad, string tipo, string estado)
        {
            this.id = id;
            this.cliente = cliente;
            this.fecha = fecha;
            this.hora = hora;
            this.turno = turno;
            this.cuenta = cuenta;
            this.cantidad = cantidad;
            this.tipo = tipo;
            this.estado = estado;
        }



    }
}
