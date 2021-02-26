using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Implementation
{
    public class UsuarioImpl : IUsuario
    {
        public int Delete(Usuario t)
        {
            throw new NotImplementedException();
        }

        public int Insert(Usuario t)
        {
            throw new NotImplementedException();
        }

        public DataTable Select()
        {
            throw new NotImplementedException();
        }

        public DataTable SelectUsuario(int id)
        {
            string query = @"SELECT *
                             FROM usuario
                             WHERE id=@id";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@id",id);
                return DBImplementation.ExecuteDataTableCommand(cmd);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int Update(Usuario t)
        {
            throw new NotImplementedException();
        }

    }
}
