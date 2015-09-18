using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ZargateTicketPrint.Api
{
    class TicketFetchDto
    {
        [JsonProperty(PropertyName = "ticketid")]
        public int TicketId { get; set; }

        [JsonProperty(PropertyName = "eventid")]
        public int EventId { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public long Ref { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "tid_ankommet")]
        public long TimeArrived { get; set; }

        [JsonProperty(PropertyName = "navn")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "nick")]
        public string Nick { get; set; }

        [JsonProperty(PropertyName = "seat")]
        public int? Seat { get; set; }

        [JsonProperty(PropertyName = "row")]
        public int? Row { get; set; }

    }
}
