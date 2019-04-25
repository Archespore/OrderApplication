using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;

namespace OrderForm
{
    public class DatabaseConnection
    {
        private SqlConnection connection { set; get; }
        public DatabaseConnection()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString);
        }

        /// <summary>
        /// Opens the sql connection if it is currently not opened
        /// </summary>
        public bool openConnection()
        {
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (Exception error) { MessageBox.Show(error.ToString(), "SQL open connection error!"); }
            }
            return false;
        }

        /// <summary>
        /// Closes the sql connection if it is currently not closed
        /// </summary>
        public bool closeConnection()
        {
            if (connection.State != ConnectionState.Closed)
            {
                try
                {
                    connection.Close();
                    return true;
                }
                catch (SqlException error) { MessageBox.Show(error.ToString(), "SQL close connection error!"); }
            }
            return false;
        }

        /// <summary>
        /// Issues a reader query with the specified string
        /// </summary>
        /// <param name="query">The SQL command to query</param>
        /// <returns>The data reader from the query, or null if connection is not open</returns>
        public SqlDataReader SQLReaderQuery(string query)
        {
            SqlDataReader reader = null;
            if (openConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                try { reader = command.ExecuteReader(); }
                catch (Exception error) { MessageBox.Show(error.ToString(), "SQL query error!"); }
            }
            return reader;
        }

        /// <summary>
        /// Issues a non query with the specified string
        /// </summary>
        /// <param name="query">The SQL command to query</param>
        /// <returns>the number of rows affected, or 0 if connection is not open</returns>
        public int SQLNonQuery(string query)
        {
            int rows = 0;
            if (openConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                try { rows = command.ExecuteNonQuery(); }
                catch (Exception error) { MessageBox.Show(error.ToString(), "SQL query error!"); }
                closeConnection();
            }
            return rows;
        }

        /// <summary>
        /// Issues a scalar with the specified string
        /// </summary>
        /// <param name="query">The SQL command to query</param>
        /// <returns>the first column of the first row affected, or null if connection is not open</returns>
        public object SQLScalarQuery(string query)
        {
            object data = null;
            if (openConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                try { data = command.ExecuteScalar(); }
                catch (Exception error) { MessageBox.Show(error.ToString(), "SQL query error!"); }
                closeConnection();
            }
            return data;
        }
    }
}
