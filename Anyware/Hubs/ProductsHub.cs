using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
namespace Anyware.Hubs
{
    [HubName("productsHub")]
    public class ProductsHub:Hub
    {
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ProductsHub>();
            context.Clients.All.refreshProductData();
        }
    }
}