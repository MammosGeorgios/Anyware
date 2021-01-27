using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anyware.Models
{
    public enum InvoiceStatus
    {
        PendingShipment, PendingPayment, CompletedPayment, Cancelled
    }
}