using INDAABIN.DI.CONTRATOS.Aplicacion.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.Admin
{
    public partial class Logger : System.Web.UI.Page
    {
        readonly BackgroundWorker worker = new BackgroundWorker();
        readonly long millisecondsToWait = 50;

        static Lazy<IHubContext> hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<BroadcasterHub>()
        );

        protected static IHubContext Hub
        {
            get { return hub.Value; }
        }

        private static Object _sync = new Object();

        public void refreshLogger()
        {
            lock (_sync)
            {
                var logger = INDAABIN.DI.CONTRATOS.Negocio.NG.ConsultarBitacora();

                if (logger != null)
                {
                    Hub.Clients.All.broadCastEvent(logger);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                worker.DoWork += delegate(object s, DoWorkEventArgs args)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    while (true)
                    {
                        if (stopwatch.ElapsedMilliseconds >= millisecondsToWait)
                        {
                            refreshLogger();
                        }

                        Thread.Sleep(1);
                    }
                };

                //worker.RunWorker(null);
            }

        }
    }
}