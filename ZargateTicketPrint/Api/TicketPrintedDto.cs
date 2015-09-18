using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ZargateTicketPrint.Api
{
    class TicketPrintedDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
