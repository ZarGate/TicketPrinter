using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ZargateTicketPrint.Classes;

namespace ZargateTicketPrint.Api
{
    internal class TicketHelper
    {
        public static ICollection<Ticket> FetchTickets()
        {
            var tickets = new List<Ticket>();
            using (var client = CustomWebClient())
            {
                var requestedResult = client.DownloadString(Api.Default.FetchArrivedEndpoint);
                var requestedTickets = JsonConvert.DeserializeObject<List<TicketFetchDto>>(requestedResult);
                foreach (var ticketDto in requestedTickets)
                {
                    var arrived = UnixTimeStampToDateTime(ticketDto.TimeArrived);
                    if (string.IsNullOrWhiteSpace(ticketDto.Name))
                    {
                        tickets.Add(new Ticket(ticketDto.TicketId, ticketDto.Ref,
                            Ticket.ParseTypeToVariant(ticketDto.Type), arrived));
                    }
                    else
                    {
                        tickets.Add(new Ticket(ticketDto.TicketId, ticketDto.Row ?? 0, ticketDto.Seat ?? 0, ticketDto.Name,
                            ticketDto.Ref, Ticket.ParseTypeToVariant(ticketDto.Type), arrived));
                    }
                }
            }
            return tickets;
        }

        public static void SetAsPrinted(int ticketId)
        {
            var ticket = new TicketPrintedDto
            {
                Id = ticketId
            };
            var postData = JsonConvert.SerializeObject(ticket);
            var encoding = new UTF8Encoding();
            using (var client = CustomWebClient())
            {
                client.UploadData(Api.Default.SetPrintedEndpoint, encoding.GetBytes(postData));
            }
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private static WebClient CustomWebClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                   | SecurityProtocolType.Tls11
                                                   | SecurityProtocolType.Tls12
                                                   | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);
            var client = new WebClient();
            client.Headers.Add("x-custom-authorization", Api.Default.Secret);
            return client;
        }
    }
}