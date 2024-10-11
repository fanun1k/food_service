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
        public int InsertRegistroTickes(Registro t)
        {
            string query = @"INSERT INTO registro(cliente,fecha,hora,turno,tipo)
				                           VALUES(@cliente,@fecha,@hora,@turno,@tipo)";
            try
            {
                SqlCommand cmd = DBImplementation.CreateBasicCommand(query);
                idAux = DBImplementation.GetIdentityFromTable("registro");

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

        public int obtenerCantAlmuerzoOCena(string tipo, string fecha)
        {
            string query = @"SELECT SUM(cantidad)
                             FROM registro
                             WHERE turno = @tipo AND fecha = @fecha AND estado = 'ACTIVO';";
            SqlCommand cmd;
            DataTable dt;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                var res = dt.Rows[0][0].ToString();
                if (res == "")
                {
                    return 0;
                }
                return int.Parse(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int obtenerCantAlmuerzoOCenaPOrId(string tipo, string fechaInicio, string fechaFinal, int idCliente)
        {
            bool hayInicio = false;
            bool hayFinal = false;
            string query;
            SqlCommand cmd;
            DataTable dt;

            string queryTodo = @"SELECT SUM(cantidad)
                                FROM registro
                                WHERE turno = @tipo AND cliente = @idCliente AND estado = 'ACTIVO';";

            string queryFechaInicio = @"SELECT SUM(cantidad)
                                        FROM registro
                                        WHERE turno = @tipo AND cliente = @idCliente AND estado = 'ACTIVO' AND fecha >= @fechaInicio;";

            string queryFechaFinal = @"SELECT SUM(cantidad)
                                        FROM registro
                                        WHERE turno = @tipo AND cliente = @idCliente AND estado = 'ACTIVO' AND fecha <= @fechaFinal;";

            string queryAmbasFechas = @"SELECT SUM(cantidad)
                                    FROM registro
                                    WHERE turno = @tipo AND cliente = @idCliente AND estado = 'ACTIVO' AND fecha >= @fechaInicio AND fecha <= @fechaFinal;";
            

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
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                if (hayInicio)
                {
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                }
                if (hayFinal)
                {
                    cmd.Parameters.AddWithValue("@fechaFinal", fechaFinal);
                }
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                var res = dt.Rows[0][0].ToString();
                if (res == "")
                {
                    return 0;
                }
                return int.Parse(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int obtenerCantAlmuerzoOCenaGeneral(string tipo, string fechaInicio, string fechaFinal)
        {
            bool hayInicio = false;
            bool hayFinal = false;
            string query;
            SqlCommand cmd;
            DataTable dt;

            string queryTodo = @"SELECT SUM(cantidad)
                                FROM registro
                                WHERE turno = @tipo AND estado = 'ACTIVO';";

            string queryFechaInicio = @"SELECT SUM(cantidad)
                                        FROM registro
                                        WHERE turno = @tipo AND estado = 'ACTIVO' AND fecha >= @fechaInicio;";

            string queryFechaFinal = @"SELECT SUM(cantidad)
                                        FROM registro
                                        WHERE turno = @tipo AND estado = 'ACTIVO' AND fecha <= @fechaFinal;";

            string queryAmbasFechas = @"SELECT SUM(cantidad)
                                    FROM registro
                                    WHERE turno = @tipo AND estado = 'ACTIVO' AND fecha >= @fechaInicio AND fecha <= @fechaFinal;";


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
                cmd.Parameters.AddWithValue("@tipo", tipo);
                if (hayInicio)
                {
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                }
                if (hayFinal)
                {
                    cmd.Parameters.AddWithValue("@fechaFinal", fechaFinal);
                }
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                var res = dt.Rows[0][0].ToString();
                if (res == "")
                {
                    return 0;
                }
                return int.Parse(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int obtenerCantAlmuerzoOCenaPOrIdTodo(string tipo, int idCliente)
        {
            string query = @"SELECT SUM(cantidad)
                             FROM registro
                             WHERE turno = @tipo AND cliente = @idCliente AND estado = 'ACTIVO';";
            SqlCommand cmd;
            DataTable dt;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                var res = dt.Rows[0][0].ToString();
                if (res == "")
                {
                    return 0;
                }
                return int.Parse(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int obtenerCantSnack(string tipo, string fecha)
        {
            //aqui es lunch?
            string query = @"SELECT SUM(cantidad)
                            FROM registro
                            WHERE cuenta = @tipo AND turno = 'LUNCH' AND estado = 'ACTIVO' AND fecha = @fecha;";
            SqlCommand cmd;
            DataTable dt;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                var res = dt.Rows[0][0].ToString();
                if (res == "")
                {
                    return 0;
                }
                return int.Parse(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int obtenerCantSnackPorid(string tipo, string fecha, int idCliente)
        {
            //aqui es lunch?
            string query = @"SELECT SUM(cantidad)
                            FROM registro
                            WHERE cuenta = @tipo AND turno = 'LUNCH' AND cliente = @idCliente AND estado = 'ACTIVO' AND fecha = @fecha;";
            SqlCommand cmd;
            DataTable dt;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                var res = dt.Rows[0][0].ToString();
                if (res == "")
                {
                    return 0;
                }
                return int.Parse(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int obtenerCantSnackPorIdTodo(string tipo, int idCliente)
        {
            //aqui es lunch?
            string query = @"SELECT SUM(cantidad)
                            FROM registro
                            WHERE cuenta = @tipo AND turno = 'LUNCH' AND estado = 'ACTIVO' AND cliente = @idCliente;";
            SqlCommand cmd;
            DataTable dt;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@idCliente", idCliente);
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                var res = dt.Rows[0][0].ToString();
                if (res == "")
                {
                    return 0;
                }
                return int.Parse(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable obtenerRegistroPOrTurno(string fecha, string turno, double precio)
        {
            string query = @"SELECT (ISNULL(cli.paterno,'')+' '+ ISNULL(cli.materno, '') + ' '+ISNULL(cli.nombre,'')) AS 'Nombre Cliente', reg.turno AS 'Nombre Producto' , @precio AS 'precio' ,SUM(reg.cantidad) AS 'Cantidad', @precio * SUM(reg.cantidad) AS 'Total'
                            FROM registro AS reg
                            INNER JOIN cliente AS cli ON cli.id = reg.cliente
                            WHERE reg.turno = @turno AND reg.fecha = @fecha AND reg.estado = 'ACTIVO'
                            GROUP BY cli.paterno, cli.materno, cli.nombre, reg.turno;";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                cmd.Parameters.AddWithValue("@turno", turno);
                cmd.Parameters.AddWithValue("@precio", precio);
                return DBImplementation.ExecuteDataTableCommand(cmd);               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
