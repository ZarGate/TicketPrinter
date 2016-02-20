using System.Collections.Generic;
using Com.SharpZebra.Printing;
using ZargateTicketPrint.Api;
using ZargateTicketPrint.ZebraHelpers;

namespace ZargateTicketPrint.Classes
{
    internal class Tickets
    {
        public void PrintTicketsFromDatabase()
        {
            if (Printer.Default.PrinterName1 == "")
            {
                Logger.Add("No printer selected!", Logger.Severity.ERROR);
                return;
            }

            ICollection<Ticket> arrived = TicketHelper.FetchTickets();
            if (arrived == null)
            {
                return;
            }
            doPrint(arrived);
        }

        private void doPrint(ICollection<Ticket> tickets)
        {
            foreach (Ticket ticket in tickets)
            {
                string printer;
                var command = new ZargateLabel(ticket.Navn, ticket.Rad, ticket.Sete, ticket.TypeString,
                    ticket.RefNr.ToString());
                if (ticket.Type == Ticket.Variants.VIP || ticket.Type == Ticket.Variants.HELG || ticket.Type == Ticket.Variants.GRATIS || ticket.Type == Ticket.Variants.HELGI)
                {
                    printer = Printer.Default.PrinterName1;
                }
                else if (ticket.Type == Ticket.Variants.HELGU)
                {
                    printer = Printer.Default.PrinterName1;
                }
                else
                {
                    printer = Printer.Default.PrinterName2;
                }
                string instruction = command.ToZebraInstruction();
                new ZebraPrinter(printer).Print(instruction);
                Logger.Add("Printed ticket  on " + printer + " with TicketID: " + ticket.TicketId);
                TicketHelper.SetAsPrinted(ticket.TicketId);
            }
        }
    }
}