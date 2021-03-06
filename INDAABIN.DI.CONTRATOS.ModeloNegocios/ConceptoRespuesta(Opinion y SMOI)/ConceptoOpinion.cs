﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INDAABIN.DI.CONTRATOS.ModeloNegocios
{

    //En este objeto se mapea un objeto : ConceptoRespValor compuesto de N respuestas, a uno de este tipo que soporta 2 respuetas, y
    //que sirve para exponerse a la vista
    public class ConceptoOpinion
    {
        public int IdConceptoRespValor { get; set; }
               
        public decimal NumOrden  { get; set; }
       
        //para la personalizacion del # de orden decimal
        public String NumOrdenVisual
        {
            //retornar valor decimal en string con o sin parte decimal, para exponer en la vista
            get {

                //si su parte decimal es 0.0, exponer como entero
                //TODO: REPH tratar de quitar la limitación que solo se puedan mostrar 99 subpreguntas, tal vez sería mas simple agregar una columna para indicar el orden y otra columna para el numero de la pregunta

                float numDecimal = float.Parse("0," + NumOrden.ToString().Split('.')[1]);
               
                if (numDecimal == 0.0)//no tiene parte decimal, entonces exponer commo un entero
                    return int.Parse(NumOrden.ToString().Split('.')[0]).ToString();//obtener parte entera
                else //si tiene parte decimal >0, exponerla
                {
                    //para casos de 3.01 exponer como 3.1, se hace en la implementacion, no en la BD, porque si no lo expone como 3.10, pareciendo que hay 10 items
                    if (numDecimal < 10)
                    {
                        //si de la 1er parte es cero, mostrar el siguiente
                        byte Num1Decimal = byte.Parse(numDecimal.ToString().Split()[0]);
                        return int.Parse(NumOrden.ToString().Split('.')[0]).ToString() + "." + Num1Decimal.ToString();
                    }
                        
                    else
                        return NumOrden.ToString(); //expoener tal como esta en la BD
                }  
            }
        }

        public int IdTema { get; set; }

        public String DescripcionTema { get; set; }

        public int IdConcepto { get; set; } //FK

        public string DescripcionConcepto { get; set; }

        public string FundamentoLegal { get; set; }
        public bool EsDeterminante { get; set; }

        public int IdRespuesta { get; set; } //FK se usa para identificar los cptos que son tema 

        //particularidad de la implementacion de este objeto de negocio, que tiene 2 posibles respuestas
        public string DescValorMinimo { get; set; }
        public decimal? ValorMinimo { get; set; }
        public string DescValorMaximo { get; set; }
        public decimal? ValorMaximo { get; set; }

        public decimal? ValorPonderacionRespuesta { get; set; }
    
        public AplicacionConcepto AplicacionConcepto
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        

    }
}
