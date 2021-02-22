using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;
using Model;

namespace Implementation
{
    public class RegistroImpl : IRegistro
    {
        int idAux = 0;


        public int Delete(Registro t)
        {
            throw new NotImplementedException();
        }

        public int GetIdRegistro()
        {
            try
            {
                return idAux;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetTableRegistro(int id)
        {
            string query = @"SELECT * 
                              FROM registro
                              WHERE id=@id";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@id", id);
                               
                return DBImplementation.ExecuteDataTableCommand(cmd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(Registro t)
        {
            string query = @"INSERT INTO registro(cliente,fecha,hora,turno,tipo)
				                           VALUES(@cliente,@fecha,@hora,@turno,@tipo)";
            try
            {
                SqlCommand cmd = DBImplementation.CreateBasicCommand(query);
                idAux = DBImplementation.GetIdentityFromTable("registro");
                t.Fecha =DateTime.Parse(DBImplementation.fechaHoraServidor().ToString("yyy-MM-dd"));
                t.Hora= DateTime.Parse(DBImplementation.fechaHoraServidor().ToString("H:m:ss"));
                
                
                cmd.Parameters.AddWithValue("@cliente", t.Cliente);
                cmd.Parameters.AddWithValue("@fecha", t.Fecha);
                cmd.Parameters.AddWithValue("@hora", t.Hora);
                cmd.Parameters.AddWithValue("@turno", t.Turno);
                cmd.Parameters.AddWithValue("@tipo", t.Tipo);
                return DBImplementation.ExecuteBasicCommand(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            };
        }

        public DataTable Select()
        {
            throw new NotImplementedException();
        }

        public int Update(Registro t)
        {
            throw new NotImplementedException();
        }
        
    }
}
