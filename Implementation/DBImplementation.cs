using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation
{
    public class DBImplementation
    {
        //public static string connectionString = "Data Source = 192.168.1.5,1433; Initial Catalog = food_service;User Id=user1;Password=user1";
        public static string connectionString = "data source = localhost; initial catalog = food_service; Integrated Security = True";

        public static SqlCommand CreateBasicCommand()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            return cmd;
        }
        public static SqlCommand CreateBasicCommand(string query)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = connection;
            return cmd;
        }
        public static List<SqlCommand> CreateNBasicCommand(int n)
        {
            List<SqlCommand> res = new List<SqlCommand>();
            SqlConnection connection = new SqlConnection(connectionString);

            for (int i = 0; i < n; i++)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                res.Add(cmd);
            }
            return res;
        }
        public static string ExecuteScalarCommand(SqlCommand cmd)
        {
            try
            {
                cmd.Connection.Open();
                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        public static void ExecuteNBasicCommmand(List<SqlCommand> cmds)
        {
            //transaccion
            SqlTransaction tran = null;
            SqlCommand cmd1 = cmds[0];
            try
            {
                cmd1.Connection.Open();
                tran = cmd1.Connection.BeginTransaction();
                foreach (SqlCommand item in cmds)
                {
                    item.Transaction = tran;
                    item.ExecuteNonQuery();
                }
                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                cmd1.Connection.Close();
            }
        }
        public static int GetIdentityFromTable(string tabla)
        {
            int res = -1;
            string query = "SELECT IDENT_CURRENT('" + tabla + "')+IDENT_INCR('" + tabla + "')";
            try
            {
                SqlCommand cmd = CreateBasicCommand(query);
                res = int.Parse(ExecuteScalarCommand(cmd));
            }
            catch (Exception)
            {
                throw;
            }
            return res;

        }


        #region Execute Command
        public static int ExecuteBasicCommand(SqlCommand cmd)
        {
            int res = -1;
            try
            {
                cmd.Connection.Open();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return res;
        }

        public static DataTable ExecuteDataTableCommand(SqlCommand cmd)
        {
            DataTable res = new DataTable();
            try
            {
                cmd.Connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return res;
        }
        #endregion

        #region DataReader 
        public static SqlDataReader ExecuteDataReaderCommand(SqlCommand cmd)
        {
            SqlDataReader res = null;
            try
            {
                cmd.Connection.Open();
                res = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //acá no se cierra la conexión. se cierra una vez llamado al metodo
            return res;
        }
        public static DateTime fechaHoraServidor()
        {
            DateTime res;
            string query = "SELECT GETDATE()";
            try
            {
                SqlCommand cmd = CreateBasicCommand(query);
                res = DateTime.Parse(ExecuteScalarCommand(cmd));
            }
            catch (Exception)
            {
                throw;
            }
            return res;
        }
        #endregion

    }
}
