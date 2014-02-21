using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ZargateTicketPrint.Classes;

namespace ZargateTicketPrint.Database
{
    internal class MySqlConnector
    {
        private readonly string _database;
        private readonly string _password;
        private readonly string _server;
        private readonly string _username;
        private MySqlConnection _connection;


        public MySqlConnector()
        {
            _server = Mysql.Default.server;
            _database = Mysql.Default.database;
            _username = Mysql.Default.username;
            _password = Mysql.Default.password;
            Initialize();
        }

        private void Initialize()
        {
            string connectionString = "SERVER=" + _server + ";" + "DATABASE=" +
                                      _database + ";" + "UID=" + _username + ";" + "PASSWORD=" + _password + ";";
            _connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                _connection.Open();
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Logger.Add("Cannot connect to server.", Logger.Severity.ERROR);
                        break;

                    case 1045:
                        Logger.Add("Invalid username/password, please try again", Logger.Severity.ERROR);
                        break;

                    default:
                        Logger.Add("Cannot connect to server.", Logger.Severity.ERROR);
                        break;
                }
                return false;
            }
            return true;
        }

        private void CloseConnection()
        {
            try
            {
                _connection.Close();
            }
            catch (MySqlException ex)
            {
                Logger.Add(ex.Message, Logger.Severity.WARNING);
            }
        }

        public DataTable CommandWithResult(MySqlCommand query)
        {
            if (OpenConnection())
            {
                //Update Command
                query.Connection = _connection;
                query.CommandType = CommandType.Text;

                //Create a data reader and Execute the command
                MySqlDataReader dataReader = query.ExecuteReader();

                //Read the data and store them in a DataTable
                var table = new DataTable();
                table.Load(dataReader);

                CloseConnection();

                return table;
            }
            return null;
        }

        public int? CommandWithoutResult(MySqlCommand query)
        {
            if (OpenConnection())
            {
                //Update Command
                query.Connection = _connection;
                query.CommandType = CommandType.Text;

                //Execute the command
                int result = query.ExecuteNonQuery();

                CloseConnection();
                return result;
            }
            return null;
        }

        public int? BatchCommandWithoutResult(List<MySqlCommand> queries)
        {
            if (OpenConnection())
            {
                int result = 0;
                foreach (MySqlCommand mySqlCommand in queries)
                {
                    //Update Command
                    mySqlCommand.Connection = _connection;
                    mySqlCommand.CommandType = CommandType.Text;

                    //Execute the command
                    result = + mySqlCommand.ExecuteNonQuery();
                }

                CloseConnection();
                return result;
            }
            return null;
        }
    }
}