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
    public class AsistenciaImpl : IAsistencia
    {
        public int Delete(Asistencia t)
        {
            throw new NotImplementedException();
        }

        public int Insert(Asistencia t)
        {
            throw new NotImplementedException();
        }

        public string RegistrarAsistencia(int id)
        {
            string res = "";
            string query = @"SELECT * 
                            FROM asistencia
                            WHERE fecha_ingreso=CONVERT(varchar,GETDATE()) AND usuario=@id";
            string query2 = @"SELECT * 
                            FROM asistencia
                            WHERE fecha_ingreso=CONVERT(varchar,GETDATE()) AND usuario=@id AND fecha_salida IS NULL";
            SqlCommand cmd;
            DataTable dt;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@id",id);
                dt = DBImplementation.ExecuteDataTableCommand(cmd);
                if (dt.Rows.Count>0)
                {
                    cmd = DBImplementation.CreateBasicCommand(query2);
                    cmd.Parameters.AddWithValue("@id",id);
                    dt = DBImplementation.ExecuteDataTableCommand(cmd);
                    if (dt.Rows.Count>0)
                    {
                        RegistrarSalida(int.Parse(dt.Rows[0][0].ToString()));
                        res="SALIDA "+ DBImplementation.fechaHoraServidor().ToString("HH:mm");
                    }
                    else
                    {
                        RegistrarEntrada(id);
                        res = "INGRESO " + DBImplementation.fechaHoraServidor().ToString("HH:mm");
                    }

                }
                else
                {
                    RegistrarEntrada(id);
                    res = "INGRESO "+DBImplementation.fechaHoraServidor().ToString("HH:mm");
                }
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegistrarEntrada(int id)
        {
            string query = @"INSERT INTO asistencia(usuario,fecha_ingreso,hora_ingreso,tipo)
                                            VALUES(@id,GETDATE(),GETDATE(),'TOUCH')";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@id",id);
                DBImplementation.ExecuteBasicCommand(cmd);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void RegistrarSalida(int id)
        {
            string query = @"UPDATE asistencia SET fecha_salida=GETDATE(),hora_salida=GETDATE()
                            WHERE id=@id";
            SqlCommand cmd;
            try
            {
                cmd = DBImplementation.CreateBasicCommand(query);
                cmd.Parameters.AddWithValue("@id",id);
                DBImplementation.ExecuteBasicCommand(cmd);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable Select()
        {
            throw new NotImplementedException();
        }

        public int Update(Asistencia t)
        {
            throw new NotImplementedException();
        }
    }
}
