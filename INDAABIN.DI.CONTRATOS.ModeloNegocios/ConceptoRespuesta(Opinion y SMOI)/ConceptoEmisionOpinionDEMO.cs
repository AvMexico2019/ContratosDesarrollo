using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{
    public class ConceptoEmisionOpinionDEMO
    {

        public byte Tema { get; set; }

        public int ?NumOrdenFisico { get; set; }

        //Cpto-Pregunta
        public string NumOrdenLogico { get; set; }
        public string Concepto { get; set; }
        public bool EsTemaCpto { get; set; }
        public string DescValorMinimo { get; set; }
        public byte ?ValorMinimo { get; set; }
        public string DescValorMaximo { get; set; }
        public byte ?ValorMaximo { get; set; }
        public bool ?EsDeterminante { get; set; }
        public int ?FundamentoLegal { get; set; }

        //Respuesta
        public byte ?ValorRespuesta { get; set; }

      //  List<Respuesta> ValorRespuesta { get; set; }

      

      

    }
}
