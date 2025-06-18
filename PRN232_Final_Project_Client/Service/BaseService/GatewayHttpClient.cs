using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BaseService
{
    public class GatewayHttpClient
    {
        public HttpClient Client { get; }

        public GatewayHttpClient(HttpClient client)
        {
            Client = client;
        }
    }
}
