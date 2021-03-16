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
    public class OrdenImpl : IOrden
    {
        public int Delete(Orden t)
        {
            throw new NotImplementedException();
        }

        public DataTable GetOrdenForId(int idOrden)
        {
            string query = @"SELECT * FROM orden
                             WHERE id=@idOrden";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@idOrden",idOrden);
                return DBImplementation.ExecuteDataTableCommand(cmd);
            }
            catch (Exception ex)
            {

                throw ex ;
            }
        }

        public int Insert(Orden t)
        {
            throw new NotImplementedException();
        }

        public DataTable Select()
        {
            throw new NotImplementedException();
        }

        public int Update(Orden t)
        {
            throw new NotImplementedException();
        }
    }
}
