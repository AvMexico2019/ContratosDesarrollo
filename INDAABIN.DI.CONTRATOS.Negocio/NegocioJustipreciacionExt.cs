using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//comunicacion con las capas 
using INDAABIN.DI.CONTRATOS.ModeloNegocios; //entities
using INDAABIN.DI.CONTRATOS.AccesoDatos; //DAL
using INDAABIN.DI.ModeloNegocio;
//using INDAABIN.DI.CONTRATOS.ModeloNegocios.ContratoArrto; //interconexion al BUS

namespace INDAABIN.DI.CONTRATOS.Negocio
{
    public class NegocioJustipreciacionExt
    {

        public Boolean InsertarJustipreciacionAvaluos(ModeloNegocios.SolicitudAvaluosExt objJustipreciacion)
        {
            bool ok = false;
            //llamado  a la capa de datos, para insertar la informacion 
            AccesoDatos.JustipreciacionDAL conn = new AccesoDatos.JustipreciacionDAL();
            ok = conn.Inserta(objJustipreciacion);
            return ok;
        }

        public List<ModeloNegocios.SolicitudAvaluosExt> ObtenerJustipreciacionesRegistradas(Filtro filtro)
        {
            List<ModeloNegocios.SolicitudAvaluosExt> ListJustipreciacionesRegistrados;
            
            ListJustipreciacionesRegistrados = JustipreciacionDAL.ObtenerJustipreciacionesRegistrados(filtro);
           
            return ListJustipreciacionesRegistrados;
        }
    
    }
}
