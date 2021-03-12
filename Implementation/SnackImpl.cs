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
    public class SnackImpl : ISnack
    {
        int idAux=0;
        public int Delete(Snack t)
        {
            throw new NotImplementedException();
        }

        public int GetIdSnack()
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

        public DataTable GetTableSnack(int id)
        {
            string query = @"SELECT *
                              FROM snack
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

        public int Insert(Snack t)
        {
            throw new NotImplementedException();
        }

        public void Insert(Orden orden, List<Snack> listSnack)
        {
            string query1 = @"  INSERT INTO orden(fecha,hora,monto,descuento,cliente,estado)
                                            VALUES(CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,@monto,0,@cliente,'ENTREGADO')";

           
            string query2 = @"INSERT INTO snack(cliente,item,precio,cantidad,total,tipo,orden)
                                         VALUES(@cliente,@item,@precio,@cantidad,@total,0,@orden)";
            try
            {
                
                List<SqlCommand> cmds = DBImplementation.CreateNBasicCommand(listSnack.Count+1);
                idAux = DBImplementation.GetIdentityFromTable("snack");
                int idOrden = DBImplementation.GetIdentityFromTable("orden");
                for (int i = 0; i <cmds.Count; i++)
                {
                    if (i==0)
                    {
                        cmds[0].CommandText = query1;
                        cmds[0].Parameters.AddWithValue("@monto",orden.Monto);
                        cmds[0].Parameters.AddWithValue("@cliente", orden.Cliente);
                    }
                    else
                    {
                        cmds[i].CommandText = query2;
                        cmds[i].Parameters.AddWithValue("@cliente",listSnack[i-1].Cliente);
                        cmds[i].Parameters.AddWithValue("@item", listSnack[i - 1].Item);
                        cmds[i].Parameters.AddWithValue("@precio", listSnack[i - 1].Precio);
                        cmds[i].Parameters.AddWithValue("@cantidad", listSnack[i - 1].Cantidad);
                        cmds[i].Parameters.AddWithValue("@total", listSnack[i - 1].Total);
                        cmds[i].Parameters.AddWithValue("@orden",idOrden);
                    }
                }
                DBImplementation.ExecuteNBasicCommmand(cmds);
            }
            catch (Exception ex )
            {

                throw ex;
            }
        }

        public DataTable Select()
        {
            throw new NotImplementedException();
        }

        public int Update(Snack t)
        {
            throw new NotImplementedException();
        }

        public List<double> SelectTotal(int idItem, string fecha)
        {
            List<double> list = new List<double>();
            string query = @"SELECT SUM(total), SUM(cantidad)
                            FROM snack 
                            WHERE item = @id AND fecha = @fechaT AND estado = 'ACTIVO';";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@id", idItem);
                cmd.Parameters.AddWithValue("@fechaT", fecha);
                var res = DBImplementation.ExecuteDataTableCommand(cmd);
                var tot = res.Rows[0][0].ToString();
                var cant = res.Rows[0][1].ToString();
                if (tot == "")
                {
                    list.Add(0);
                }
                else
                {
                    list.Add(double.Parse(tot));
                }

                if (cant == "")
                {
                    list.Add(0);
                }
                else
                {
                    list.Add(double.Parse(cant));
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<double> SelectTotalPorId(string nombreItem, string fechaInicio, string fechaFinal, int idCliente)
        {
            List<double> list = new List<double>();
            SqlCommand cmd;
            bool hayInicio = false;
            bool hayFinal = false;
            string query;
            string queryTodo = @"SELECT SUM(sna.total), SUM(sna.cantidad)
                                FROM snack AS sna
                                INNER JOIN item AS it ON it.id = sna.item
                                WHERE it.nombre LIKE @nombreItem AND cliente = @idCliente  AND sna.estado = 'ACTIVO';";

            string queryFechaInicio = @"SELECT SUM(sna.total), SUM(sna.cantidad)
                                        FROM snack AS sna
                                        INNER JOIN item AS it ON it.id = sna.item
                                        WHERE it.nombre LIKE @nombreItem AND cliente = @idCliente  AND sna.estado = 'ACTIVO' AND fecha >= @fechaInicio;";
            string queryFechaFinal = @"SELECT SUM(sna.total), SUM(sna.cantidad)
                                        FROM snack AS sna
                                        INNER JOIN item AS it ON it.id = sna.item
                                        WHERE it.nombre LIKE @nombreItem AND cliente = @idCliente  AND sna.estado = 'ACTIVO' AND fecha <= @fechaFinal;";
            string queryAmbasFechas = @"SELECT SUM(sna.total), SUM(sna.cantidad)
                                        FROM snack AS sna
                                        INNER JOIN item AS it ON it.id = sna.item
                                        WHERE it.nombre LIKE @nombreItem AND cliente = @idCliente  AND sna.estado = 'ACTIVO' AND fecha >= @fechaInicio AND fecha <= @fechaFinal;";
            if (fechaInicio == "" && fechaFinal == "")
            {
                query = queryTodo;
            }
            else if (fechaInicio != "" && fechaFinal == "")
            {
                query = queryFechaInicio;
                hayInicio = true;
            }
            else if (fechaInicio == "" && fechaFinal != "")
            {
                query = queryFechaFinal;
                hayFinal = true;
            }
            else
            {
                query = queryAmbasFechas;
                hayInicio = true;
                hayFinal = true;
            }

            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@nombreItem", "%"+nombreItem+"%");
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                if (hayInicio)
                {
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                }
                if (hayFinal)
                {
                    cmd.Parameters.AddWithValue("@fechaFinal", fechaFinal);
                }
                var res = DBImplementation.ExecuteDataTableCommand(cmd);
                var tot = res.Rows[0][0].ToString();
                var cant = res.Rows[0][1].ToString();
                if (tot == "")
                {
                    list.Add(0);
                }
                else
                {
                    list.Add(double.Parse(tot));
                }

                if (cant == "")
                {
                    list.Add(0);
                }
                else
                {
                    list.Add(double.Parse(cant));
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public double SelectTotalSnackSinLonche(string fechaInicio, string fechaFinal, int idCliente)
        {
            SqlCommand cmd;
            bool hayInicio = false;
            bool hayFinal = false;
            string query;
            string queryTodo = @"SELECT SUM(sna.total)
                                FROM snack AS sna
                                INNER JOIN item AS it ON it.id = sna.item
                                WHERE it.nombre NOT LIKE '%LONCHE%' AND cliente = @idCliente  AND sna.estado = 'ACTIVO';";

            string queryFechaInicio = @"SELECT SUM(sna.total)
                                        FROM snack AS sna
                                        INNER JOIN item AS it ON it.id = sna.item
                                        WHERE it.nombre NOT LIKE '%LONCHE%' AND cliente = @idCliente  AND sna.estado = 'ACTIVO' AND fecha >= @fechaInicio;";
            string queryFechaFinal = @"SELECT SUM(sna.total)
                                        FROM snack AS sna
                                        INNER JOIN item AS it ON it.id = sna.item
                                        WHERE it.nombre NOT LIKE '%LONCHE%' AND cliente = @idCliente  AND sna.estado = 'ACTIVO' AND fecha <= @fechaFinal;";
            string queryAmbasFechas = @"SELECT SUM(sna.total)
                                        FROM snack AS sna
                                        INNER JOIN item AS it ON it.id = sna.item
                                        WHERE it.nombre NOT LIKE '%LONCHE%' AND cliente = @idCliente  AND sna.estado = 'ACTIVO' AND fecha >= @fechaInicio AND fecha <= @fechaFinal;";
            if (fechaInicio == "" && fechaFinal == "")
            {
                query = queryTodo;
            }
            else if (fechaInicio != "" && fechaFinal == "")
            {
                query = queryFechaInicio;
                hayInicio = true;
            }
            else if (fechaInicio == "" && fechaFinal != "")
            {
                query = queryFechaFinal;
                hayFinal = true;
            }
            else
            {
                query = queryAmbasFechas;
                hayInicio = true;
                hayFinal = true;
            }

            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                if (hayInicio)
                {
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                }
                if (hayFinal)
                {
                    cmd.Parameters.AddWithValue("@fechaFinal", fechaFinal);
                }
                var res = DBImplementation.ExecuteDataTableCommand(cmd);
                var tot = res.Rows[0][0].ToString();
                if (tot == "")
                {
                    return 0;
                }
                else
                {
                    return double.Parse(tot);
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<double> SelectTotalPorIdTotal(int idItem, int idCliente)
        {
            List<double> list = new List<double>();
            string query = @"SELECT SUM(total), SUM(cantidad)
                            FROM snack 
                            WHERE item = @id AND cliente = @idCliente AND estado = 'ACTIVO';";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@id", idItem);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                var res = DBImplementation.ExecuteDataTableCommand(cmd);
                var tot = res.Rows[0][0].ToString();
                var cant = res.Rows[0][1].ToString();
                if (tot == "")
                {
                    list.Add(0);
                }
                else
                {
                    list.Add(double.Parse(tot));
                }

                if (cant == "")
                {
                    list.Add(0);
                }
                else
                {
                    list.Add(double.Parse(cant));
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
