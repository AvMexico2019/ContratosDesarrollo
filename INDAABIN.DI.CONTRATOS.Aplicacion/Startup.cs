using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(INDAABIN.DI.CONTRATOS.Aplicacion.Startup))]

namespace INDAABIN.DI.CONTRATOS.Aplicacion
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
