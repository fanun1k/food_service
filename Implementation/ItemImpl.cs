﻿using System;
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
                result.Precio = decimal.Parse(dt.Rows[0][0].ToString());

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

        public double SelectPrecio(int id)
        {
            
            DataTable dt;
            SqlCommand cmd;
            string query = @"SELECT precio
                                FROM item
                                WHERE id = @id;";
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@id", id);
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                return double.Parse(dt.Rows[0][0].ToString());                 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public double SelectPrecioPorNombre(string name)
        {

            DataTable dt;
            SqlCommand cmd;
            string query = @"SELECT precio
                            FROM item
                            WHERE nombre LIKE @name;";
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@name", name);
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                return double.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable SelectItemsVentaSnack(int idOrden)
        {
            string query = @"SELECT I.id,I.nombre,I.descripcion,I.Precio
                            FROM item I
                            INNER JOIN snack S ON I.id=S.item
                            WHERE S.orden=@idOrden";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@idOrden",idOrden);
                return DBImplementation.ExecuteDataTableCommand(cmd);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
