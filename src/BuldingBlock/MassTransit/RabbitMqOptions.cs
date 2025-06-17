using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuldingBlock.MassTransit
{
    public class RabbitMqOptions
    {
        public string HostName { get; set; }
        public string ExchangeName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
