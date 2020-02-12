
    function Habilitar() {
        debugger;

        $("input:checkbox[id*='CheckCtrlCumple']").each(function (index, e) {
            var $this = $(this);

            if ($this.is(":checked")) {
                var g = $this.attr("id");

                $('#' + g).parents("tr").find("[id*='TextBoxObservaciones']").removeAttr("disabled");

            }

            else {
                var g = $this.attr("id");

                $('#' + g).parents("tr").find("[id*='TextBoxObservaciones']").prop("disabled", true);

                $('#' + g).parents("tr").find("[id*='TextBoxObservaciones']").val('');
            }
        });





    }


//funcion para validar si algun checkbox esta seleccionado
    function ValidarCheck()
    {
        debugger;
        $("input:checkbox[id*='CheckCtrlCumple']:checked").each(
            function () {
                var $this = $(this);

                var g = $this.attr("id");

                $('#' + g).parents("tr").find("[id*='TextBoxObservaciones']").removeAttr("disabled");
            }
            );
    }





