using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Transactions;
//comunicacion con las capas
using INDAABIN.DI.CONTRATOS.Datos;
using INDAABIN.DI.CONTRATOS.ModeloNegocios;


namespace INDAABIN.DI.CONTRATOS.AccesoDatos
{
    public class Catalogos
    {
           

        //Catalogo de TipoContrato
        public List<TipoArrendamiento> ObtenerCptosTipoArrendamiento()
        {
            List<TipoArrendamiento> lista = new List<TipoArrendamiento>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    lista = Conn.Cat_TipoArrendamiento.Select(a => new TipoArrendamiento
                    {
                        IdTipoArrendamiento = a.IdTipoArrendamiento,
                        DescTipoArrendamiento = a.DescripcionTipoArrendamiento
                    }
                        ).ToList();
                }
                
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lista;

        }



        //Catalogo de  Tema
        public List<TemaConcepto> ObtenerTemasCptos()
        {
            List<TemaConcepto> listaTemasCpto = new List<TemaConcepto>();
            try
            {
                using (ArrendamientoInmuebleEntities Conn = new ArrendamientoInmuebleEntities())
                {
                    listaTemasCpto = Conn.Cat_Tema.Select(a => new TemaConcepto
                    {
                        IdTema = a.IdTema,
                        DescripcionTema = a.DescripcionTema
                    }
                        ).ToList();
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listaTemasCpto;

        }

    }
}
