$(document).ready(function () {


    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaSimualdoresAtivos').click();
            return false;
        }
    });




    $('input[placeholder]').on('paste', function (e) {
        e.preventDefault();
        var clipboardCurrentData = (e.originalEvent || e).clipboardData.getData('text/plain');
        window.document.execCommand('insertText', false, clipboardCurrentData);
    });

    $(".datepicker").datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        minDate : '01/01/2016',
        onSelect: function (selectedDate) {
            var id = $(this).attr("id");
            if (id == "dtInicio") {
                $("#dtFim").datepicker('option', 'minDate', selectedDate);
            }
            else {
                $("#dtInicio").datepicker('option', 'maxDate', selectedDate);
            }
        },

        constrainInput: true,
        buttonImageOnly: true,
    });


    $("#buscaSimualdoresAtivos").click(function () {

        //var dtInicio = $("#dtInicio").val();
        //var dtFim = $("#dtFim").val();
        //var podepesquisar = true;

        //if (dtInicio == "" && dtFim == "") {
        //    Message('*Informe um período para realizar esta consulta.', 'erro');
        //    podepesquisar = false;
        //    return;
        //}


        //if (dtInicio != "") {
        //    if (!isDate(dtInicio)) {
        //        Message("*Data Inicio é inválido. Favor informar um parâmetro para consulta", "erro");
        //        return;
        //    }
        //}

        //if (dtFim != "") {
        //    if (!isDate(dtFim)) {
        //        Message("*Data Fim é inválido. Favor informar um parâmetro para consulta", "erro");
        //        return;
        //    }
        //}

        //if (dtInicio == "" && dtFim != "") {
        //    Message("*Data Início inválida. Favor informar um parâmetro para consulta.", "erro");
        //    return;
        //}

        //if (dtInicio != "" && dtFim == "") {
        //    Message("*Data Fim inválida. Favor informar um parâmetro para consulta.", "erro");
        //    return;
        //}


        //if (dtInicio != "") {
        //    if (dtFim == "") {
        //        Message('*Informe a Data Final para realizar esta consulta', 'erro');
        //        podepesquisar = false;
        //    }
        //    else {

        //        var adtInicio = $("#dtInicio").val().split('/');
        //        var adtFim = $("#dtFim").val().split('/');

        //        var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
        //        var dtFinal = new Date(adtFim[2], adtFim[1], adtFim[0]);

        //        if (dtFinal >= dtInicial) {
        //            podepesquisar = true;
        //        } else {
        //            Message("*Data Fim deve ser maior ou igual a Data Inicio.", "ERRO");
        //            return;
        //        }
        //    }
        //}
        //else {
        //    podepesquisar = true;
        //}

        if (ValidaDataInicioFim())
            ListarSimuladoresAtivos();
    });

});


// Valida Data Inicio e Fim
function ValidaDataInicioFim() {
    var dtInicio = $("#dtInicio").val();
    var dtFim = $("#dtFim").val();
    var podepesquisar = false;

    if (dtInicio == "") {
        Message('*Informe uma Data Inicial para realizar esta consulta.', 'erro');
        return podepesquisar;
    }


    if (dtInicio != "") {
        if (!isDate(dtInicio)) {
            Message("*Data Inicio é inválido. Favor informar um parâmetro para consulta", "erro");
            return podepesquisar;
        }
        else {
            var adtInicio = $("#dtInicio").val().split('/');
            var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);

            if (adtInicio[2] < 2016) {
                Message("*Data Inicial deve ser a partir de Janeiro/ 2016", "erro");
                return podepesquisar;
            }

        }
    }

    if (dtFim != "") {
        if (!isDate(dtFim)) {
            Message("*Data Fim é inválido. Favor informar um parâmetro para consulta", "erro");
            return podepesquisar;
        }
    }

    if (dtInicio == "" && dtFim != "") {
        Message("*Data Início inválida. Favor informar um parâmetro para consulta.", "erro");
        return podepesquisar;
    }


    if (dtInicio != "" && dtFim != "") {
        var adtInicio = $("#dtInicio").val().split('/');
        var adtFim = $("#dtFim").val().split('/');
        var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
        var dtFinal = new Date(adtFim[2], adtFim[1], adtFim[0]);

        if (dtFinal >= dtInicial) {
            podepesquisar = true;
        } else {
            Message("*Data Fim deve ser maior ou igual a Data Inicio.", "ERRO");
            return podepesquisar;
        }
    }
    else {
        podepesquisar = true;
    }

    return podepesquisar;
}

//Listar Contratos Firmados
function ListarSimuladoresAtivos() {


    var dtInicio    = $("#dtInicio").val();
    var dtFim       = $("#dtFim").val();
    var idEstado    = $("#idEstado option:selected").val();

    var url = "/AulaRealizada/ListarSimuladoresAtivos";

    $.ajax({
        url: url
        , data: { dtInicio: dtInicio, dtFim : dtFim, idEstado: idEstado }
        , type: "GET"
        , cache: false
        , datatype: "html"
        , beforeSend: function () {
            waitingDialog.show();
        }
        , complete: function () {
            waitingDialog.hide();
        }
        , success: function (data) {
            $("#listasimuladoresativos").html(data);
            AplicarDataTableCustom("contratosfirmados", false);
            waitingDialog.hide();
        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    })
}

function LimparSimuladorAtivo()
{
    $("#dtInicio").val("");
    $("#dtFim").val("");
    $("#idEstado")[0].selectedIndex = 0;
    //$("#idEstado option:first").attr('selected', 'selected');

}

// Exportar Aulas por Simulador
function ExportarSimuladoresAtivos() {

    var dtInicio    = $("#dtInicio").val();
    var dtFim       = $("#dtFim").val();
    var idEstado    = $("#idEstado option:selected").val();

    var url = "/AulaRealizada/ExportarSimuladoresAtivos?dtInicio=" + dtInicio + "&dtFim=" + dtFim + "&idEstado=" + idEstado;

    if (ValidaDataInicioFim()) {
        window.location = url;
    }
}