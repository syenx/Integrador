//LOAD
$(document).ready(function () {

    $(".datepicker").datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        minDate: "-30d",
        constrainInput: true,
        buttonImageOnly: true
    });


    $("#buscaLogRelatorios").click(function () {
        if (ValidaDataInicioFim())
            ListarAulasRealizadas();
    });

    $("#limpaLogRelatorios").click(function () {
        $("#dtInicio").val("");
        $("#dtFim").val("");
        $("#IdUsuario")[0].selectedIndex = 0;
        $("#listalogrelatorios").empty();
    });
})

// Valida Data Inicio e FIM
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
        } else {
            var adtInicio = $("#dtInicio").val().split('/');
            var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
            if (adtInicio[2] < 2016) {
                Message("*A Data Inicial deve ser a partir de Janeiro/2016.", "erro");
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

function ListarAulasRealizadas() {

    var dtInicio    = $("#dtInicio").val();
    var dtFim       = $("#dtFim").val();
    var idUsuario = $("#IdUsuario option:selected").val();
    var url         = "/AulaRealizada/ListarLogRelatorios";

    $.ajax({
        url: url
        , datatype: "html"
        , type: "GET"
        , cache: true
        , async: true
        , data: {
            dtInicio: dtInicio
            , dtFim: dtFim
            , idUsuario: idUsuario
        }
        , beforeSend: function () {
            waitingDialog.show();
        }
        , complete: function () {
            waitingDialog.hide();
        }
        , success: function (data) {
            $("#listalogrelatorios").html(data);
            AplicarDataTable("logrelatorios", false);
            waitingDialog.hide();
        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}
