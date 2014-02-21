using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ZargateTicketPrint.Classes;

namespace ZargateTicketPrint.Database
{
    internal class TicketHelper
    {
        public List<Ticket> FetchArrivedPeople()
        {
            var tickets = new List<Ticket>();

            const string query =
                "SELECT u.userid as userid, t.ticketid as ticketid, t.ref as ref, t.rad as rad, t.sete as sete, u.navn as navn, t.type as type " +
                "FROM zg_users u, zg_tickets t " +
                "WHERE t.ankommet = '1'  " +
                "AND t.printed = '0'  " +
                "AND u.userid = t.userid";

            var mysqlConnector = new MySqlConnector();
            var cmd = new MySqlCommand {CommandText = query};
            DataTable ticketTable = mysqlConnector.CommandWithResult(cmd);

            if (ticketTable == null)
            {
                return null;
            }

            foreach (DataRow row in ticketTable.Rows)
            {
                try
                {
                    int userId = int.Parse(row["userid"].ToString());
                    int ticketId = int.Parse(row["ticketid"].ToString());
                    int rad = int.Parse(row["rad"].ToString());
                    int sete = int.Parse(row["sete"].ToString());
                    string navn = row["navn"].ToString();
                    int refNr = int.Parse(row["ref"].ToString());

                    // Try to parse the output of field type
                    Ticket.Variants type = Ticket.ParseTypeToVariant(row["type"].ToString());

                    // Add ticket to tickets
                    tickets.Add(new Ticket(ticketId, userId, rad, sete, navn, refNr, type));

                    // Log success of fetching ticket
                    Logger.Add("Fetched a ticket from database with TicketID: " + ticketId);
                }
                catch (Exception ex)
                {
                    Logger.Add(ex.Message, Logger.Severity.ERROR);
                }
            }

            return tickets;
        }

        public List<Ticket> FetchArrivedPeopleWithNoName()
        {
            var tickets = new List<Ticket>();

            const string query =
                "SELECT t.ticketid as ticketid, t.ref as ref, t.type as type " +
                "FROM zg_tickets t " +
                "WHERE t.ankommet = '1'  " +
                "AND t.printed = '0' " +
                "AND t.userid = '0' ";

            var mysqlConnector = new MySqlConnector();
            var cmd = new MySqlCommand { CommandText = query };
            DataTable ticketTable = mysqlConnector.CommandWithResult(cmd);

            if (ticketTable == null)
            {
                return null;
            }

            foreach (DataRow row in ticketTable.Rows)
            {
                try
                {
                    int ticketId = int.Parse(row["ticketid"].ToString());
                    int refNr = int.Parse(row["ref"].ToString());

                    // Try to parse the output of field type
                    Ticket.Variants type = Ticket.ParseTypeToVariant(row["type"].ToString());

                    // Add ticket to tickets
                    tickets.Add(new Ticket(ticketId, refNr, type));

                    // Log success of fetching ticket
                    Logger.Add("Fetched a ticket from database with TicketID: " + ticketId);
                }
                catch (Exception ex)
                {
                    Logger.Add(ex.Message, Logger.Severity.ERROR);
                }
            }

            return tickets;
        }

        public static void SetAsPrinted(int ticketId)
        {
            const string query = "Update zg_tickets SET printed='1' WHERE ticketid=@ticketid";

            //create mysql command
            var mysqlConnector = new MySqlConnector();
            var cmd = new MySqlCommand {CommandText = query};
            cmd.Parameters.Add("@ticketid", MySqlDbType.Int32).Value = ticketId;

            try
            {
                mysqlConnector.CommandWithoutResult(cmd);
            }
            catch (Exception ex)
            {
                Logger.Add(
                    "Failed to set ticket as printed in database. TicketID: " + ticketId + Environment.NewLine +
                    ex.Message, Logger.Severity.ERROR);
                return;
            }
            // Log success of updating ticket
            Logger.Add("Set a ticket as printed in database with TicketID: " + ticketId);
        }
    }
}