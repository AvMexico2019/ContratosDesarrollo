using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Hubs
{
    public class BroadcasterHub : Hub
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

    }
}