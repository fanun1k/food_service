using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Cliente
    {
        private int id;
        private string nombre;
        private string paterno;
        private string materno;
        private string documento;
        private DateTime fechaIngreso;
        private string codigo;
        private byte[] fotografia;
        private string estado;
        private int tipo;

        public Cliente()
        {

        }

        public Cliente(string nombre, string paterno, string materno, string documento, DateTime fechaIngreso, string codigo, byte[] fotografia, string estado, int tipo)
        {
            this.nombre = nombre;
            this.paterno = paterno;
            this.materno = materno;
            this.documento = documento;
            this.fechaIngreso = fechaIngreso;
            this.codigo = codigo;
            this.fotografia = fotografia;
            this.estado = estado;
            this.tipo = tipo;
        }

        #region get set
        public int Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }


        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }


        public byte[] Fotografia
        {
            get { return fotografia; }
            set { fotografia = value; }
        }


        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }


        public DateTime FechaIngreso
        {
            get { return fechaIngreso; }
            set { fechaIngreso = value; }
        }


        public string Documento
        {
            get { return documento; }
            set { documento = value; }
        }


        public string Materno
        {
            get { return materno; }
            set { materno = value; }
        }


        public string Paterno
        {
            get { return paterno; }
            set { paterno = value; }
        }


        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        #endregion

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

    }
}
