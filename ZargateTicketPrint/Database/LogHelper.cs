using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ZargateTicketPrint.Classes;

namespace ZargateTicketPrint.Database
{
    static class LogHelper
    {
        public static void UploadLog()
        {
            var messages = new List<LogMessage>(Logger.GetLogToUploadToSQL());
            var queries = new List<MySqlCommand>();
            foreach (LogMessage logMessage in messages)
            {
                const string query = "INSERT INTO zg_logger (timestamp, message, severity, area)" +
                                     "VALUES (unix_timestamp(@timestamp), @message, @severity, @area)";
                var cmd = new MySqlCommand{CommandText = query};
                cmd.Parameters.Add("@timestamp", MySqlDbType.Int64).Value = 
                    (logMessage.Timestamp.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
                cmd.Parameters.Add("@message", MySqlDbType.Text).Value = logMessage.Message;
                cmd.Parameters.Add("@severity", MySqlDbType.Int16).Value = logMessage.Severity;
                cmd.Parameters.Add("@area", MySqlDbType.VarChar).Value = "ticket";
                queries.Add(cmd);
            }

            //create mysql command
            var mysqlConnector = new MySqlConnector();
            try
            {
                mysqlConnector.BatchCommandWithoutResult(queries);

                // Log success of updating ticket
                Logger.Add("Uploaded all log events to database");
            }
            catch (Exception ex)
            {
                Logger.Add("Failed to uploaded all log events to database" + Environment.NewLine + ex.Message, Logger.Severity.ERROR);
            }
        }
    }
}
