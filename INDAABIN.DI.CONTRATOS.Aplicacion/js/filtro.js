function addFilterHandlerInput() {
    //console.log("Ciclo de revisión de filtros");
    var inputs = document.querySelectorAll('input[data-filter]');
    
    for (var i = 0; i < inputs.length; i++) {
        var reSpace = new RegExp("\\s+", 'gim');
        var delay = 500;
        var identidad = 0;
        var input = inputs[i]; // Input
        if (input.getAttribute("data-filter") === "yes") {
            //console.log("Se filtra: >" + input.id + "<");
            if (input.parentNode.getElementsByTagName('span').length > 0) {
                var spans = input.parentNode.getElementsByTagName('span');
                for (var j = 0; j < spans.length; j++) {
                    if (input.id == spans[j].id.replace("span", "txt"))
                        errorSpan = spans[j];
                }
                //debugger;
                //console.log("Error Span TAG found >" + errorSpan.id + "<");
            }

            var escapeChars = ' (#$|*"\\\'_.;9)';
            var errorAry = [escapeChars];

            var regex = input.getAttribute("data-INDAABINREGEX");
            if (regex == null
                || regex == undefined
                || regex.length == 0) {
                errorAry.push('filter pattern MISSING');
            }

            var errMsg = input.getAttribute("data-errorMsg");
            if (errMsg == null
                || errMsg == undefined
                || errMsg.length == 0) {
                errorAry.push('errorMsg MISSING');
                input.errorMsg = 'errorMsg MISSING';
            }
            
            if (errorSpan == null
                || errorSpan == undefined) {
                errorAry.push('errorSpan MISSING');
            }
            if (errorAry.length > 1) {
                input.value = input.defaultValue = errorAry.join(', ');
            }
            //console.log("Asignamos handler a >" + input.id + "<, >" + input.value + "<");

            input.addEventListener('input', RegexHandler(input, errorSpan, true, false));
        }
    }

    function RegexHandler(input, eSpan, isOneSpace, isUpper) {
        var remplazo = isOneSpace == true ? " " : ""; 
        return function () {

            //console.log("Escuchamos caracteres en: >" + input.id + "<");

            var left = input.selectionStart;
            var right = input.selectionEnd;
            if(isUpper)
                input.value = input.value.replace(reSpace, remplazo).toUpper();
            else
                input.value = input.value.replace(reSpace, remplazo);
            input.selectionStart = left;
            input.selectionEnd = right;

            input.token = identida = identidad + 1;
            var token = identida;

            setTimeout(function () {
                //console.log("Se cumplió el time out en: >" + input.id + "<");
                //console.log("token: " + token + ", input.token: " + input.token);

                if (token == input.token) {
                    var value = input.value;
                    //console.log("input.value: " + input.value + "<");
                    var m, a = 0, p = 0, q, t = value.length;
                    var msgE = ["<small>" + input.getAttribute("data-errorMsg"), ': <br>"'];
                    // Se requiere usar la bandera m en la función regex para procesar los caracteres ^ y $ en la    expresión regular
                    // Se reemplazan como protección únicamente, ya que se podrían configurar en el html y no se están usando
                    var regex = input.getAttribute("data-INDAABINREGEX").replace("^", "").replace("$", "");
                    //console.log("Regex found: >" + regex + "<");
                    var re = new RegExp(regex, 'gim');

                    var i = 0;
                    var e = 0;
                    //debugger;
                    while ((m = re.exec(value))) {
                        p = m.index;
                        q = p + m[0].length;
                        //console.log("value: " + m[0] + "; loc: " + p);
                        if (p > a) {
                            e = e + 1;
                            //Marca Texto
                            msgE.push('<errorTag>');
                            msgE.push(value.substring(a, p).replace(" ", "&nbsp;"));
                            msgE.push('</errorTag>');
                        }
                        msgE.push(m[0].replace(" ", "&nbsp;"));
                        a = q;
                        i = i + 1;
                    }
                    if (i == 0 && t > 0 || a < t) {
                        e = e + 1;
                        msgE.push('<errorTag>');
                        msgE.push(value.substring(a, t).replace(" ", "&nbsp;"));
                        msgE.push('</errorTag>');
                    }
                    if (e > 0) {
                        msgE.push('"</small>');
                        //console.log(msgE);
                        //console.log("input TAG name for error: >" + input.id + "<, errorSpan Tag: >" + errorSpan.id + "<");
                        eSpan.innerHTML = msgE.join("");
                    }
                    else
                        eSpan.innerHTML = "";
                }
            }, delay);
        }
    }
}