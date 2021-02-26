using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Asistencia
    {
        private int id;
        private int usuario;
        private DateTime fecha_ingreso;
        private DateTime fecha_salida;
        private DateTime hora_ingreso;
        private DateTime hora_salida;
        private string tipo;
        private string estado;
        public Asistencia()
        {

        }

        public Asistencia(int id, int usuario, DateTime fecha_ingreso, DateTime fecha_salida, DateTime hora_ingreso, DateTime hora_salida, string tipo, string estado)
        {
            this.id = id;
            this.usuario = usuario;
            this.fecha_ingreso = fecha_ingreso;
            this.fecha_salida = fecha_salida;
            this.hora_ingreso = hora_ingreso;
            this.hora_salida = hora_salida;
            this.tipo = tipo;
            this.estado = estado;
        }

        #region getSet
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


        public DateTime Hora_salida
        {
            get { return hora_salida; }
            set { hora_salida = value; }
        }

        public DateTime Hora_ingreso
        {
            get { return hora_ingreso; }
            set { hora_ingreso = value; }
        }

        public DateTime Fecha_salida
        {
            get { return fecha_salida; }
            set { fecha_salida = value; }
        }


        public DateTime Fecha_ingreso
        {
            get { return fecha_ingreso; }
            set { fecha_ingreso = value; }
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

    }
}
