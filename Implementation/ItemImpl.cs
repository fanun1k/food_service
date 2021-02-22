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
    public class ItemImpl : IItem
    {
        public int Delete(Item t)
        {
            throw new NotImplementedException();
        }

        public int Insert(Item t)
        {
            throw new NotImplementedException();
        }

        public DataTable Select()
        {
            string query = @"SELECT * 
                            FROM item 
                            WHERE estado='ACTIVO'
                            ORDER BY nombre";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                return DBImplementation.ExecuteDataTableCommand(cmd);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable SelectDataTableItem(int id)
        {
            string query = @"SELECT * 
                             FROM item 
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

        public Item SelectItem(int id)
        {
            Item result = new Item();
            DataTable dt;
            SqlCommand cmd;
            string query = @"SELECT precio 
                                FROM item
                                WHERE id=@id";         
            try
            {
                cmd= DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@id",id);
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                result.Id = id;
                result.Precio = double.Parse(dt.Rows[0][0].ToString());

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable SelectLunch()
        {
            string query = @"SELECT I.id,(I.nombre+' '+CONVERT(varchar,I.precio)+'Bs.') AS nombre
                                    FROM item I
                                    WHERE nombre LIKE '%LONCHE%' AND estado='ACTIVO'";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);

                return DBImplementation.ExecuteDataTableCommand(cmd);              
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int Update(Item t)
        {
            throw new NotImplementedException();
        }
    }
}
