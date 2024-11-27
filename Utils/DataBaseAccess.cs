using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Utils
{
    public class DataBaseAccess
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataReader reader;

        public SqlDataReader excecuteQueryWithResult()
        {
            command.Connection = connection;
            try
            {
                connection.Open();
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SqlDataReader Reader
        {
            get { return reader; }
        }
        public DataBaseAccess()
        {
        
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            connection = new SqlConnection(connectionString);
            command = new SqlCommand();
        }

        public void setQuery(string Query)
        {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = Query;
        }

        public void excecuteQuery()
        {
            command.Connection = connection;
            try
            {
                connection.Open();
                reader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void excecuteAction()
        {
            command.Connection = connection;
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setParameter(string name, object value)
        {
            command.Parameters.AddWithValue(name, value);
        }

        public void CloseConnection()
        {
            if (reader != null)
            {
                reader.Close();
            }
            connection.Close();
        }

        public void clearParameters()
        {
            command.Parameters.Clear();
        }
    }
}
