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
    public class ClienteImpl : ICliente
    {
        public int Delete(Cliente t)
        {
            throw new NotImplementedException();
        }

        public int Insert(Cliente t)
        {
            throw new NotImplementedException();
        }

        public DataTable Select()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// metodo que devuelve un objeto del tipo Cliente de la base de datos, apartir de un codigo de cliente
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public Cliente Select(int codigo)
        {
            DataTable dt = new DataTable();
            Cliente cliente=null;
            string query = @"SELECT nombre,paterno,materno,fotografia,estado,id
                             FROM cliente
                             WHERE codigo=@codigo";
            SqlCommand cmd;

            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@codigo", codigo);
                dt= DBImplementation.ExecuteDataTableCommand(cmd);

                if (dt.Rows.Count > 0)
                {
                    byte[] fotografia = new byte[0];

                    cliente = new Cliente();
                    cliente.Nombre = dt.Rows[0][0].ToString();
                    cliente.Paterno = dt.Rows[0][1].ToString();
                    cliente.Materno = dt.Rows[0][2].ToString();
                    if (dt.Rows[0][3] != null && dt.Rows[0][3].ToString().Length > 0)
                    {
                        fotografia = (byte[])dt.Rows[0][3];
                        cliente.Fotografia = fotografia;
                    }
                    cliente.Estado = dt.Rows[0][4].ToString().Trim();
                    cliente.Id = int.Parse(dt.Rows[0][5].ToString());
                }
                return cliente;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int Update(Cliente t)
        {
            throw new NotImplementedException();
        }

        public DataTable GetTableCliente(int codigo)
        {
            string query = @"SELECT *
                              FROM cliente
                              WHERE codigo=@codigo";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@codigo", codigo);

                return DBImplementation.ExecuteDataTableCommand(cmd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
