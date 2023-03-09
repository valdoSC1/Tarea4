using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace API.DataAccess
{
    public class DatosSQL
    {
        SqlConnection conexion = new SqlConnection();
        private IConfiguration? configuration;

        public DatosSQL()
        {
            try
            {
                StringBuilder strConnectionString = new StringBuilder();

                strConnectionString.Append("Data Source=");
                strConnectionString.Append("tiusr11pl.cuc-carrera-ti.ac.cr.\\MSSQLSERVER2019");

                strConnectionString.Append(";Initial Catalog=");
                strConnectionString.Append("tiusr4pl_MOHISATarea4");

                strConnectionString.Append(";User=");
                strConnectionString.Append("Tarea4Mohisa");

                strConnectionString.Append(";Password=");
                strConnectionString.Append("lg49J*99k");

                this.conexion = new SqlConnection(strConnectionString.ToString());

                this.conexion.Open();

                this.conexion.Close();
            }
            catch (SqlException sql)
            {
                throw sql;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IConfiguration? Configuration { get => configuration; set => configuration = value; }

        public void ExecuteSP(string SPName, List<SqlParameter> ListaParametros)
        {
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = SPName,
                    Connection = this.conexion
                };

                foreach (SqlParameter param in ListaParametros)
                {
                    cmd.Parameters.Add(param);
                }

                this.conexion.Open();

                cmd.ExecuteNonQuery();

                this.conexion.Close();
            }
            catch (SqlException sql)
            {
                if (conexion.State == ConnectionState.Open)
                {
                    this.conexion.Close();
                }
                throw sql;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet ExecuteSPWithDS(string SPName, List<SqlParameter> ListaParametros)
        {
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = SPName,
                    Connection = this.conexion
                };

                foreach (SqlParameter param in ListaParametros)
                {
                    cmd.Parameters.Add(param);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataSet datos = new DataSet();

                adapter.Fill(datos);

                return datos;
            }
            catch (SqlException sql)
            {
                if (conexion.State == ConnectionState.Open)
                {
                    this.conexion.Close();
                }
                throw sql;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ExecuteSPWithDT(string SPName, List<SqlParameter> ListaParametros)
        {
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = SPName,
                    Connection = this.conexion
                };

                foreach (SqlParameter param in ListaParametros)
                {
                    cmd.Parameters.Add(param);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable datos = new DataTable();

                adapter.Fill(datos);

                return datos;
            }
            catch (SqlException sql)
            {
                if (conexion.State == ConnectionState.Open)
                {
                    this.conexion.Close();
                }
                throw sql;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}


