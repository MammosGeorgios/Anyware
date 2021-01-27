
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
namespace Anyware.Hubs
{
    [HubName("invoicesHub")]
    public class InvoicesHub:Hub
    {
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<InvoicesHub>();
            context.Clients.All.refreshInvoiceData();
        }
    }
}