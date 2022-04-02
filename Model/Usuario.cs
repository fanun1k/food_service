using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Usuario
    {
        private int id;
        private string nombre;
        private string paterno;
        private string materno;
        private string login;
        private string password;
        private string documento;
        private byte[] imagen;
        private string estado;
        private DateTime fechaIngreso;
        private DateTime fechaNacimiento;
        private string cargo;

        #region getSet
        public string Cargo
        {
            get { return cargo; }
            set { cargo = value; }
        }

        public DateTime FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }

        public DateTime FechaIngreso
        {
            get { return fechaIngreso; }
            set { fechaIngreso = value; }
        }

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public byte[] Imagen
        {
            get { return imagen; }
            set { imagen = value; }
        }

        public string Documento
        {
            get { return documento; }
            set { documento = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
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

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        #endregion

        public Usuario()
        {

        }

        public Usuario(int id, string nombre, string paterno, string materno, string login, string password, string documento, byte[] imagen, string estado, DateTime fechaIngreso, DateTime fechaNacimiento, string cargo)
        {
            this.id = id;
            this.nombre = nombre;
            this.paterno = paterno;
            this.materno = materno;
            this.login = login;
            this.password = password;
            this.documento = documento;
            this.imagen = imagen;
            this.estado = estado;
            this.fechaIngreso = fechaIngreso;
            this.fechaNacimiento = fechaNacimiento;
            this.cargo = cargo;
        }
    }
}
