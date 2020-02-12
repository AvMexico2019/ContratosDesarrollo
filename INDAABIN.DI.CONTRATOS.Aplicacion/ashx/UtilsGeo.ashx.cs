using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
//using ComponentUtils.dao;
//using ComponentUtils.dto;
using Component.dto;
using Component.dao;

namespace INDAABIN.DI.CONTRATOS.Aplicacion.ashx
{
    /// <summary>
    /// Descripción breve de UtilsGeo
    /// </summary>
    public class UtilsGeo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            JsonRet jsonRet = new JsonRet();
            JavaScriptSerializer gjson = new JavaScriptSerializer();

            string method = string.Empty;

            try
            {
                method = context.Request.Params["method"];
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["statisticsCore"].ToString();

                if (method == "IS_INSIDE")
                {
                    string wktToValidate = context.Request.Params["wkt"];
                    string idScope = context.Request.Params["idScope"];

                    UtilsGeoDAO dao = new UtilsGeoDAO(connectionString);
                    string result = dao.IsInsideGeometry(wktToValidate, idScope);

                    jsonRet.Code = 100;
                    jsonRet.Msg = "OK";
                    jsonRet.Obj = result;
                }
                else if (method == "GET_SCOPE_GEOMETRY")
                {
                    string scope = context.Request.Params["scope"];
                    string idScope = context.Request.Params["idScope"];

                    UtilsGeoDAO dao = new UtilsGeoDAO(connectionString);
                    GeometryWKT geometry = dao.GetGeometryWKT(scope, idScope);

                    jsonRet.Code = 100;
                    jsonRet.Msg = "OK";
                    jsonRet.Obj = geometry;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                var json = gjson.Serialize(jsonRet);
                context.Response.ContentType = "text/json";
                context.Response.Write(json);
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}