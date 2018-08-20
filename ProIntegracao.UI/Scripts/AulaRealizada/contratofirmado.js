$(document).ready(function () {

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaContratosFirmados').click();
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
        minDate: '01/01/2016',
        constrainInput: true,
        buttonImageOnly: true,
    });

    $("#buscaContratosFirmados").click(function () {
        if (ValidaDataInicioFim())
            ListarContratosFirmados();
    });

});

function ValidaDataInicioFim() {
    var dtInicio = $("#dtInicio").val();
    var podepesquisar = true;


    if (dtInicio == "") {
        Message('*Informe um período para realizar esta consulta.', 'erro');
        podepesquisar = false;
        return;
    }

    if (dtInicio != "") {
        if (!isDate(dtInicio)) {
            Message("*Data Inicio é inválido. Favor informar um parâmetro para consulta", "erro");
            podepesquisar = false;
            return;
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

    return podepesquisar;
}

function LimparContratosFirmados() {
    $("#dtInicio").val("");
    $("#idEstado")[0].selectedIndex = 0;
}

//Listar Contratos Firmados
function ListarContratosFirmados()
{
    var dtInicio = $("#dtInicio").val();
    var idEstado = $("#idEstado option:selected").val();
    var url = "/AulaRealizada/ListarContratosFirmados";
    $.ajax({
        url: url
        , data: { dtInicio: dtInicio, idEstado: idEstado }
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
            $("#listacontratosfirmados").html(data);
            AplicarDataTableCustom("contratosfirmados", true);
            waitingDialog.hide();
        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

function ExportarContratosFirmados() {

    var dtInicio = $("#dtInicio").val();
    var dtFim = $("#dtFim").val();
    var idEstado = $("#idEstado option:selected").val();
    var url = "/AulaRealizada/ExportarContratos?dtInicio=" + dtInicio + "&idEstado=" + idEstado;
    if (ValidaDataInicioFim()) {
        window.location = url;
    }
}