<%@ Page Title="Programación cuestionarios" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsApplication1.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Ejemplo Cuestionario.</h3>
    <script type="text/javascript" src="D:/Desarrollos/FirstJacaScript/WebApplication1/Scripts/program.js">
    <script type="text/javascript">
        var ListModHabilitar = [];
        var qqsavedRegistro = "RegEdoCuesg7g76g76tio";//"RegEdoCuestionario"

        function Habilita(pregunta) {
            if (document[qqsavedRegistro][pregunta]) {
                var reactivo = document[qqsavedRegistro][pregunta];
                reactivo.habilitado = true;
                reactivo.valor = "";
                //ListModHabilitar.push(pregunta);
                return 1;
            }
            else
                return 0;
        }
        
        function DesHabilita(pregunta) {
            if (document[qqsavedRegistro][pregunta]) {
                var reactivo = document[qqsavedRegistro][pregunta];
                reactivo.habilitado = false;
                reactivo.valor = "";
                //ListModHabilitar.push(pregunta);
                return 1;
            }
            else
                return 0;
        }

        function Respondida(pregunta) {
            if (document[qqsavedRegistro][pregunta]) {
                var reactivo = document[qqsavedRegistro][pregunta];
                if (reactivo.valor == "")
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        function Habilitada(pregunta) {
            if (document[qqsavedRegistro][pregunta])
                return document[qqsavedRegistro][pregunta].habilitado;
            else
                return false;
        }

        function Avanza(pregunta) {
            if (document[qqsavedRegistro][pregunta]) {
                document[qqsavedRegistro][pregunta].avanza = true;
                return 1;
            }   
            return 0;    
        }

        function NoAvanza(pregunta) {
            if (document[qqsavedRegistro][pregunta]) {
                document[qqsavedRegistro][pregunta].avanza = false;
                return 1;
            }
            return 0;    
        }

        function Favorable(pregunta) {
            if (document[qqsavedRegistro][pregunta]) {
                document[qqsavedRegistro][pregunta].favorable = true;
                return 1;
            }
            return 0;    
        }

        function NoFavorable(pregunta) {
            if (document[qqsavedRegistro][pregunta]) {
                document[qqsavedRegistro][pregunta].favorable = false;
                return 1;
            }
            return 0;   
        }

        function Respuesta(pregunta) {
            if (document[qqsavedRegistro][pregunta]) {
                return document[qqsavedRegistro][pregunta].valor;
            }
            return "";
        }

        function Valor(pregunta) {
            if (pregunta in document["DicPreguntas"]) {
                var HTMLTag = document["DicPreguntas"][pregunta];
                
                switch (HTMLTag.getAttribute('tipodato')) {
                    case 'binario':
                        if (Respuesta(pregunta) == "Si")
                            return true;
                        else
                            return false;
                        break;
                    case 'texto':
                        return Respuesta(pregunta);
                        break;
                    case 'enum':
                        return Respuesta(pregunta);
                        break;
                    case 'numerico':
                        return parseInt(Respuesta(pregunta));
                        break;
                    case 'fecha':
                        return Respuesta(pregunta);
                        break;
                    case 'decimal':
                        return parseFloat(Respuesta(pregunta));
                        break;
                    case 'alfanumerico':
                        return Respuesta(pregunta);
                        break;
                    default:
                        alert('No debe ocurrir');
                        break;
                }
            }
            return "";
        }

        var IniciaCuestionario = function () {
            var aux = document[qqsavedRegistro];
            var MaxPreguntas = 3;
            //var Avanza = document["Avanza"] = false;
            //var Favorable = document["Favorable"] = false;
            

            if (aux == undefined || aux == null || ObjectLength_Legacy(aux) < MaxPreguntas) {
                aux = document.querySelectorAll('[pregunta]');
                document[qqsavedRegistro] = {}; //Diccionario de cambios
                document["DicPreguntas"] = {}; //Diccionario de elementos

                //Se invierte el flujo de asignacion para que las primeras reglas tengan mayor prioridad
                for (var i = 0; i < aux.length; i++) {
                    //Jerarqui de validacion en HTML. 1. Desabled, 2. Requiered, 3. Pattern.
                    var key = aux[i].getAttribute('pregunta');
                    document[qqsavedRegistro][key] = {
                        valor: aux[i].value.toString(),
                        valido: aux[i].validity.valid,
                        habilitado: aux[i].getAttribute('disabled') ? false : true,
                        avanza: false,
                        favorable: false
                    };
                    document["DicPreguntas"][key] = aux[i];
                    aux[i].setAttribute('onchange', "EventoPorReglaPorDesenfoque('" + key + "')");
                }
                return aux.length;
            }
            else
                return 0;
        };

        //Por desenfoque del campo o TODO: al precionar tecla Enter o Tabular.


        var EventoPorReglaPorDesenfoque = function (key) {
            var EdoPregunta = document[qqsavedRegistro][key]; // este es el estado de la pregunta
            var pregunta = document["DicPreguntas"][key]; // este es el tag de HTML en la pagina

            //La valides se toma de la Jerarquia: 1. Requiered, 2. Pattern.
            if (pregunta.hasAttribute('disabled')) {
                //TODO: Validar que tipo de elemento es
                EdoPregunta.valor = pregunta.value = ""; // Esto nunca se va a ejecutar
                alert('Esto no se debio ejecutar');
                //SE SUPONE QUE SE VALIDA CON RegExp y Requiered para hacerlo forzoso
            } else if (pregunta.validity.valid) {
                EdoPregunta.valido = true;
                EdoPregunta.valor = pregunta.value;
            } else
                EdoPregunta.valido = false;

            ClientServerNegocio(false, key);

            for (var keyA in document[qqsavedRegistro]) {
            //while ((keyA = ListModHabilitar.pop())) {
                var EdoPregunta = document[qqsavedRegistro][keyA];
                var pregunta = document["DicPreguntas"][keyA];

                if (EdoPregunta.habilitado) {
                    pregunta.removeAttribute('disabled');
                } else {
                    pregunta.setAttribute("disabled", "disabled");
                }
            }
        }

        //C# Ejemplos
        //Ejemplo de interpretacion nativa de Microsoft: https://www.codeproject.com/articles/30999/scriptengine-user-defined-calculations-in-c-vb-jsc
        //Configuracuion avanzada con "chakra": https://stackoverflow.com/questions/4744105/parse-and-execute-js-by-c-sharp
        //No desserializar jsonPreguntasDict, solo recibir como cadena y alimentar la funcion ClientServerNegocio;
        //Descerializacion para la respuesta en C#: var values = JsonConvert.DeserializeObject<Dictionary<string, MiObjeto>>(ClientServerNegocio(json));
        var ClientServerNegocio = function (jsonPreguntasDict, key) {
            //Estructura: keyNumPregunta: value(valor: string, valido: bool, habilitado: bool)

            //Del lado del cliente no des-serializa. Comparten variable
            //if (jsonPreguntasDict)
            //    var RegEdoCuestionario = JSON.parse(jsonPreguntasDict);
            //else
            //    var RegEdoCuestionario = document[qqsavedRegistro];

            // el contenido del switch se genera a partir de la base de datos
            switch (key) {
                case '0':
                    break;

                case '1':
                    if (Habilitada('1')) {
                        if (Respondida('1')) {
                            Avanza('1');
                            if (Respuesta('1') == "Si") {
                                Habilita('1.1');
                                DesHabilita('2');
                                DesHabilita('3');
                                Favorable('1');
                            }
                            else {
                                DesHabilita('1.1');
                                Habilita('2');
                                Habilita('3');
                                NoFavorable('1');
                            } 
                        }
                        else
                            NoAvanza('1');  
                    }
                    break;

                case '1.1': // hay que validar con una expresion regular
                    if (Habilitada('1.1')) {
                        if (Respondida('1.1')) {
                            Favorable('1.1');
                            Avanza('1.1');
                        }
                        else {
                            NoFavorable('1.1');
                            NoAvanza('1.1');
                        }    
                    }
                    break;

                case '2':
                    if (Habilitada('2')) {
                        if (Respuesta('2') == "") {
                            NoFavorable('2');
                            NoAvanza('2');
                        }
                        else {
                            Favorable('2');
                            Avanza('2');
                        }
                    }
                    break;
                case '3':
                    break;
                case '3.1':
                    break;
                case '3.2':
                    break;
                case '3.3':
                    break;
                case '4':
                    break;
                case '4.1':
                    break;
                case '4.2':
                    break;
                case '4.3':
                    break;
                case '4.4':
                    break;
                case '4.5':
                    break;
                case '4.6':
                    break;
                case '4.7':
                    break;
                case '5':
                    break;
                case '6':
                    break;
                case '7':
                    break;
                case '7.1':
                    break;
                case '7.2':
                    break;
                case '7.3':
                    break;
                case '7.4':
                    break;
                case '8':
                    break;
                case '9':
                    break;
                case '10':
                    break;
                case '10.1':
                    break;
                case '10.2':
                    break;
                case '10.3':
                    break;
                default:
                    break;
            }
            //1.1
            //var aux = pregunta['1.1'];
            //if (aux.habilitado)
            //{
            //} else {
            //    if (aux.valido && aux == 'flof') {
            //        pregunta['1.2'].habilitado = true;
            //        pregunta['1.2'].valor = "";
            //    } else {
            //        pregunta['1.2'].habilitado = false;
            //    }
            //}
            //---

            //Del lado del cliente no serializa. Comparten variable
            if (jsonPreguntasDict)
                return JSON.stringfy(pregunta);
        }

        var ObjectLength_Legacy = function (object) {
            var length = 0;
            for (var key in object) {
                if (object.hasOwnProperty(key)) {
                    ++length;
                }
            }
            return length;
        }
    // Tipos de datos que necesito
    // Texto          textarea  type="text"
    // Fecha          input  type="date"
    // ENUM           select ok
    // void             
    // bool           select ok 
    // decimal        input type="number" step='0.01' value='0.00' placeholder='0.00'
    // AlfaNumerico   input type="text" pattern
    // Numerico       input type="number" placeholder='0'

    </script>
    <div class="panel-body">
    <p id="InitResult">No inicializado</p>
         
    
    <table id="cphBody_TableEmisionOpinion" class="table table-bordered">
        <tbody>
            <tr id="cphBody_0">
                <td style="background-color:LightGrey;border-color:Gray;border-width:1px;border-style:Solid;font-weight:bold;">
                </td>
                <td style="background-color:LightGrey;border-color:Gray;border-width:1px;border-style:Solid;font-weight:bold;">Requerimientos para Arrendamientos Inmobiliario</td><td align="center" style="background-color:LightGrey;border-color:Gray;border-width:1px;border-style:Solid;font-weight:bold;">Captura</td><td align="center" style="background-color:LightGrey;border-color:Gray;border-width:1px;border-style:Solid;font-weight:bold;">Fundamento Legal</td>
            </tr>

            <tr id="cphBody_1">
                <td style="border-width:1px;border-style:Solid;">1</td>
                <td style="border-width:1px;border-style:Solid;">Se cuenta con el dictamen de disponibilidad de inmuebles federales</td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <select
                            name="ctl00$cphBody$1DropDownListRespuesta"
                            pregunta ="1" id="P1" TipoDato ="binario"
                            class="form-control">
                            <option value="">--</option>
                            <option value="No">No</option>
                            <option value="Si">Si</option>
                        </select>
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <a onclick="window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=1&amp;TemaOpinion=3', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');" id="cphBody_1-LinkButtom" href="javascript:__doPostBack('ctl00$cphBody$1-LinkButtom','')">Consultar</a>
                </td>
            </tr>

            <tr id="cphBody_1.1">
                <td style="border-width:1px;border-style:Solid;">1.1</td>
                <td style="border-width:1px;border-style:Solid;">Justificación por la cual no cuenta con el Certificado, constancia de seguridad estructural</td><td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <textarea type="text" name="ctl00$cphBody$TextAreaJustificacionCertificadoSeguridad"
                            pregunta ="1.1" id="P1.1"
                            rows="2" 
                            cols="21"
                            disabled="true"
                            TipoDato ="texto"
                            ></textarea>
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                </td>
            </tr>

            <tr id="cphBody_2">
                <td style="border-width:1px;border-style:Solid;">2</td>
                <td style="border-width:1px;border-style:Solid;">Tiempo estimado que permanecerán arrendando el inmueble propuesto</td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <select name="ctl00$cphBody$2DropDownListRespuesta"
                            pregunta ="2"
                            id="P2" class="form-control"
                            TipoDato ="enum">
                            <option value="">--</option>
                            <option value="1">&lt;=2 años</option>
                            <option value="2">&gt;2 años, no lo sé</option>
                        </select>
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <a onclick="window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=2&amp;TemaOpinion=3', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');" id="cphBody_2-LinkButtom" href="javascript:__doPostBack('ctl00$cphBody$2-LinkButtom','')">Consultar</a>
                </td>
            </tr>

            <tr id="cphBody_3">
                <td style="border-width:1px;border-style:Solid;">3</td>
                <td style="border-width:1px;border-style:Solid;">El inmueble se encuentra en condiciones aceptables para su uso</td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <select
                            name="ctl00$cphBody$3DropDownListRespuesta" 
                            pregunta ="3"
                            id="P3"
                            class="form-control"
                            TipoDato ="binario">
                            <option value="">--</option>
                            <option value="No">No</option>
                            <option value="Si">Si</option>
                        </select>
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <a onclick="window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=3&amp;TemaOpinion=3', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');" id="cphBody_3-LinkButtom" href="javascript:__doPostBack('ctl00$cphBody$3-LinkButtom','')">Consultar</a>
                </td>
            </tr>

            <tr id="cphBody_4">
                <td style="border-width:1px;border-style:Solid;">4</td>
                <td style="border-width:1px;border-style:Solid;">Superficie en metros cuadrados</td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <input
                            type ="number"
                            placeholder='0'
                            name="ctl00$cphBody$3DropDownListRespuesta"
                            pregunta ="4"
                            id="P4"
                            class="form-control"
                            TipoDato ="numerico">
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <a onclick="window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=3&amp;TemaOpinion=3', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');" id="cphBody_3-LinkButtom" href="javascript:__doPostBack('ctl00$cphBody$3-LinkButtom','')">Consultar</a>
                </td>
            </tr>

            <tr id="cphBody_5">
                <td style="border-width:1px;border-style:Solid;">5</td>
                <td style="border-width:1px;border-style:Solid;">Captura de un numero</td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <input
                            type ="number"
                            placeholder='0'
                            name="ctl00$cphBody$3DropDownListRespuesta"
                            pregunta ="5"
                            id="P5"
                            class="form-control"
                            TipoDato ="numerico">
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <a onclick="window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=3&amp;TemaOpinion=3', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');" id="cphBody_3-LinkButtom" href="javascript:__doPostBack('ctl00$cphBody$3-LinkButtom','')">Consultar</a>
                </td>
            </tr>

            <tr id="cphBody_6">
                <td style="border-width:1px;border-style:Solid;">6</td>
                <td style="border-width:1px;border-style:Solid;">Captura de fecha</td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <input
                            type ="date"
                            name="ctl00$cphBody$3DropDownListRespuesta"
                            pregunta ="6"
                            id="P6"
                            class="form-control"
                            TipoDato ="fecha">
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <a onclick="window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=3&amp;TemaOpinion=3', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');" id="cphBody_3-LinkButtom" href="javascript:__doPostBack('ctl00$cphBody$3-LinkButtom','')">Consultar</a>
                </td>
            </tr>

            <tr id="cphBody_7">
                <td style="border-width:1px;border-style:Solid;">7</td>
                <td style="border-width:1px;border-style:Solid;">Captura de un numero decimal</td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <input
                            type ="number"
                            step='0.01'
                            value='0.00'
                            placeholder='0.00'
                            name="ctl00$cphBody$3DropDownListRespuesta"
                            pregunta ="7"
                            id="P7"
                            class="form-control"
                            TipoDato ="decimal">
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <a onclick="window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=3&amp;TemaOpinion=3', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');" id="cphBody_3-LinkButtom" href="javascript:__doPostBack('ctl00$cphBody$3-LinkButtom','')">Consultar</a>
                </td>
            </tr>

            <tr id="cphBody_8">
                <td style="background-color:LightGrey;border-color:Gray;border-width:1px;border-style:Solid;font-weight:bold;"></td>
                <td style="background-color:LightGrey;border-color:Gray;border-width:1px;border-style:Solid;font-weight:bold;">Implementación de buenas prácticas</td><td align="center" style="background-color:LightGrey;border-color:Gray;border-width:1px;border-style:Solid;font-weight:bold;">Captura</td>
                <td align="center" style="background-color:LightGrey;border-color:Gray;border-width:1px;border-style:Solid;font-weight:bold;">Fundamento Legal</td>
            </tr>
        
            <tr id="cphBody_9">
                <td style="border-width:1px;border-style:Solid;">8</td>
                <td style="border-width:1px;border-style:Solid;">El inmueble propuesto para arrendamiento cumple con las normas establecidas en las Disposiciones administrativas de carácter general en materia de eficiencia energética en los inmuebles, flotas vehiculares e instalaciones industriales de la Administración Pública Federal 2017</td>
                <td align="center" style="border-width:1px;border-style:Solid;"></td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <a onclick="window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=13&amp;TemaOpinion=3', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');" id="cphBody_13-LinkButtom" href="javascript:__doPostBack('ctl00$cphBody$13-LinkButtom','')">Consultar</a>
                </td>
            </tr>
        
            <tr id="cphBody_10">
                <td style="border-width:1px;border-style:Solid;">9</td>
                <td style="border-width:1px;border-style:Solid;">Norma&nbsp;Oficial Mexicana NOM-007-ENER-2014,&nbsp;Eficiencia energética para sistemas de alumbrado en edificios&nbsp;no residenciales</td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <select
                            name="ctl00$cphBody$13.1DropDownListRespuesta"
                            pregunta ="9"
                            id="P9"
                            class="form-control"
                            TipoDato ="binario">
                            <option value="">--</option>
                            <option value="No">No</option>
                            <option value="Si">Si</option>
                        </select>
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;"></td>
            </tr>

             <tr id="cphBody_11">
                <td style="border-width:1px;border-style:Solid;">10</td>
                <td style="border-width:1px;border-style:Solid;">Captura de un alfanumerico con regex</td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <div class="form-group">
                        <input
                            type ="text"
                            pattern ="[A-Za-z0-9]*"
                            name="ctl00$cphBody$Alfanumerico"
                            pregunta ="10"
                            id="P10"
                            class="form-control"
                            TipoDato ="alfanumerico">
                    </div>
                </td>
                <td align="center" style="border-width:1px;border-style:Solid;">
                    <a onclick="window.open('ConsultaFundamentoLegal.aspx?NumOrdenCpto=3&amp;TemaOpinion=3', '_blank', 'top = 30, left=150, toolbar = no, scrollbars = no, resizable = no, width = 1024, height = 650', 'true');" id="cphBody_3-LinkButtom" href="javascript:__doPostBack('ctl00$cphBody$3-LinkButtom','')">Consultar</a>
                </td>
            </tr>
        </tbody>
    </table>

    <table id="cphBody_TablePiePagina" class="table table-bordered">
        <tbody>
            <tr>
                <td align="center" colspan="4" style="background-color:LightGrey;border-color:Gray;border-width:1px;border-style:Solid;font-weight:bold;width:800px;">La información capturada es responsabilidad del servidor público y  la Institución que la envía; el INDAABIN se reserva el derecho a solicitar información adicional y/o probatoria.</td>
            </tr>
            <tr>
                <td align="left" colspan="4" style="border-width:1px;border-style:Solid;width:800px;">Nota: La presente evaluación no exime del cumplimiento de toda la normatividad que al respecto se publica en el Capítulo IX del MANUAL DE RECURSOS MATERIALES Y SERVICIOS GENERALES.</td>
            </tr>
        </tbody>
    </table>
        <p id="FinCuestionario">Le Fin</p>
        <script>
            document.getElementById("InitResult").innerHTML = IniciaCuestionario();
    </script>
</div>
</asp:Content>
