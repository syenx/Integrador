//LOAD
$(document).ready(function () {

    var idEstado = $("#idEstado option:selected").val();

    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaAulaRealizada').click();
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
        minDate : "01/01/2016",
        onSelect: function (selectedDate) {
            var id = $(this).attr("id");

            if (id == "dtInicio") {
                if ($("#dtFim").val() == "" && $("#idEstado option:selected").val() != "") {
                    $("#buscaAulaRealizada").removeAttr("disabled");
                }
                else {

                    if ($("#dtInicio").val != "") {

                        var dtInicio = $("#dtInicio").val();
                        var dtFim = $("#dtFim").val();

                        if (isDate(dtFim) && isDate(dtInicio)) {

                            var adtInicio = $("#dtInicio").val().split('/');
                            var adtFim = $("#dtFim").val().split('/');
                            var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
                            var dtFinal = new Date(adtFim[2], adtFim[1], adtFim[0]);


                            var diff = (adtFim[1] - adtInicio[1]);

                            if (diff > 0) {
                                DesabilitaBotaBuscar();
                            } else {
                                if ($("#idEstado option:selected").val() != "") {
                                    HabilitaBotaoBuscar();
                                }
                            }
                        }
                    }
                }
            }
            else {
                $("#dtInicio").datepicker('option', 'maxDate', selectedDate);
            }

            if (id == "dtFim") {
                var dtInicio = $("#dtInicio").val();
                var dtFim = $("#dtFim").val();

                if (isDate(dtFim)) {

                    var adtInicio = $("#dtInicio").val().split('/');
                    var adtFim = $("#dtFim").val().split('/');
                    var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
                    var dtFinal = new Date(adtFim[2], adtFim[1], adtFim[0]);
                    

                    var diff = (adtFim[1] - adtInicio[1]);

                    if (diff > 0)
                    {
                        DesabilitaBotaBuscar();
                    } else
                    {
                        if ($("#idEstado option:selected").val() != "") {
                            HabilitaBotaoBuscar();
                        }
                    }
                }
            }
        },

        constrainInput: true,
        buttonImageOnly: true,
    });

    $("#dtFim").focus(function () {
        if ($("#dtFim").val() == "" && $("#idEstado option:selected").val() != "") {
            $("#buscaAulaRealizada").removeAttr("disabled");
        }
    });


    $("#dtFim").keypress(function () {
        
        var dtInicio = $("#dtInicio").val();
        var dtFim = $("#dtFim").val();

        if (isDate(dtFim)) {

            var adtInicio = $("#dtInicio").val().split('/');
            var adtFim = $("#dtFim").val().split('/');
            var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
            var dtFinal = new Date(adtFim[2], adtFim[1], adtFim[0]);

            var diff = (adtFim[1] - adtInicio[1]);

            if (diff > 0) {
                DesabilitaBotaBuscar();
            } else {

                if ($("#idEstado option:selected").val() != "") {
                    HabilitaBotaoBuscar()
                   
                }

            }
        }

    });


    $("#idEstado").change(function () {

        var idEstado = $("#idEstado option:selected").val();

        if (idEstado != "") {

            var dtInicio = $("#dtInicio").val();
            var dtFim = $("#dtFim").val();

            if (isDate(dtInicio) && dtFim == "") {
                HabilitaBotaoBuscar();
            } else {

                if (isDate(dtFim)) {

                    var adtInicio = $("#dtInicio").val().split('/');
                    var adtFim = $("#dtFim").val().split('/');
                    var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
                    var dtFinal = new Date(adtFim[2], adtFim[1], adtFim[0]);
                    var diff = (adtFim[1] - adtInicio[1]);
                    if (diff > 0) {
                        DesabilitaBotaBuscar();
                    } else {

                        if ($("#idEstado option:selected").val() != "") {
                            HabilitaBotaoBuscar()
                        } $("#idEstado option:selected").val()
                    }
                }
            }
        }
        else {
            DesabilitaBotaBuscar();
        }
    });


    $("#buscaAulaRealizada").click(function () {
        
        if (ValidaDataInicioFim())
            ListarAulasRealizadas();
    });

    $("#limpaAulaRealizada").click(function () {
        $("#dtInicio").val("");
        $("#dtFim").val("");
        $("#idEstado")[0].selectedIndex = 0;
        $("#listaaularealizada").empty();
        DesabilitaBotaBuscar();
    });

});


function DesabilitaBotaBuscar()
{
    $("#buscaAulaRealizada").attr("disabled", "disabled");
    $("#buscaAulaRealizada").removeClass("btn-info");
    $("#buscaAulaRealizada").addClass("btn-default");

}

function HabilitaBotaoBuscar()
{
    $("#buscaAulaRealizada").removeAttr("disabled");
    $("#buscaAulaRealizada").removeClass("btn-default");
    $("#buscaAulaRealizada").addClass("btn-info");
}

// Valida Data Inicio e FIM
function ValidaDataInicioFim()
{
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
        } else
        {
            var adtInicio = $("#dtInicio").val().split('/');
            var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);

            if (adtInicio[2] < 2016)
            {
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

    var dtInicio = $("#dtInicio").val();
    var dtFim = $("#dtFim").val();
    var idEstado = $("#idEstado option:selected").val();
    var url = "/AulaRealizada/ListarMetodos";

    $.ajax({
        url: url
        , datatype: "html"
        , type: "GET"
        , cache: true
        , async : true
        , data: {
            dtInicio: dtInicio
            , dtFim: dtFim
            , idEstado: idEstado
        }
        , beforeSend: function () {
            waitingDialog.show();
        }
        , complete: function () {
            waitingDialog.hide();
        }
        , success: function (data) {
            $("#listaaularealizada").html(data);
            AplicarDataTableCustom("aulasrealizadas",false);
            waitingDialog.hide();
        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }

    });
}


function Exportar()
{
    var dtInicio = $("#dtInicio").val();
    var dtFim = $("#dtFim").val();
    var idEstado = $("#idEstado option:selected").val();
    
    var url = "/AulaRealizada/Exportar?dtInicio="+dtInicio + "&dtFim=" + dtFim + "&idEstado=" + idEstado;

    

    if (ValidaDataInicioFim()) {
        window.location = url;
    }

    //var data = { dtInicio: dtInicio, dtFim: dtFim, idEstado: idEstado }
    //$.post(url, data);

    //$.ajax({
    //    url: url
    //    , datatype: "html"
    //    , type: "POST"
    //    , cache: true
    //    , async: true
    //    , data: {
    //        dtInicio: dtInicio
    //        , dtFim: dtFim
    //        , idEstado: idEstado
    //    }
    //    , beforeSend: function () {
    //        waitingDialog.show();
    //    }
    //    , complete: function () {
    //        waitingDialog.hide();
    //    }
    //    , success: function (data) {
    //        //$("#listaaularealizada").html(data);
    //        //AplicarDataTableCustom("aulasrealizadas", false);
    //        alert("Sucesso");
    //        waitingDialog.hide();
    //    }, error: function (jqXHR, exception) {
    //        TratamendodeErro(jqXHR, exception);
    //        alert("Erro");
    //    }

    //});
    
}