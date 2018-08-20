$(document).ready(function () {

    $(".datepicker").datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
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

    $("#buscaLogAula").click(function () {


        var cpf = $("#cpf").val();
        var dtInicio = $("#dtInicio").val();
        var dtFim = $("#dtFim").val();
        var podepesquisar = false;

        if (cpf == "") {
            Message("*Informe um CPF para realizar esta consulta", "aviso");
            podepesquisar = false;
        } else {
            if (dtInicio == "" && dtFim == "") {
                Message('*Informe um período para realizar esta consulta.', 'erro');
                podepesquisar = false;
                return;
            }


            if (dtInicio != "") {
                if (!isDate(dtInicio)) {
                    Message("*Data Inicio é inválido. Favor informar um parâmetro para consulta", "erro");
                    return;
                }
            }

            if (dtFim != "") {
                if (!isDate(dtFim)) {
                    Message("*Data Fim é inválido. Favor informar um parâmetro para consulta", "erro");
                    return;
                }
            }

            if (dtInicio == "" && dtFim != "") {
                Message("*Data Início inválida. Favor informar um parâmetro para consulta.", "erro");
                return;
            }

            if (dtInicio != "" && dtFim == "") {
                Message("*Data Fim inválida. Favor informar um parâmetro para consulta.", "erro");
                return;
            }


            if (dtInicio != "") {
                if (dtFim == "") {
                    Message('*Informe a Data Final para realizar esta consulta', 'erro');
                    podepesquisar = false;
                }
                else {
                    var adtInicio = $("#dtInicio").val().split('/');
                    var adtFim = $("#dtFim").val().split('/');

                    var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
                    var dtFinal = new Date(adtFim[2], adtFim[1], adtFim[0]);

                    if (dtFinal >= dtInicial) {
                        podepesquisar = true;
                    } else {
                        Message("*Data Fim deve ser maior ou igual a Data Inicio.", "ERRO");
                        return;
                    }
                }
            }
            else {
                podepesquisar = true;
            }

            if (podepesquisar)
                ListarSituacaoLogAula();
        }
    });

    $("#limparLogAula").click(function () {
        $("#cpf").val("");
        $("#dtInicio").val("");
        $("#dtFim").val("");
    });

});



function ListarSituacaoLogAula() {

    var cpf = RetornaCPFSemPontos($("#cpf").val());
    var dtInicio = $("#dtInicio").val();
    var dtFim = $("#dtFim").val();
    var psa = $("#psa").val();

    var url = "/SituacaoLogAula/ListaLogAula";

    $.ajax({
        url: url
        , data: { cpf: cpf, dtInicio: dtInicio, dtFinal: dtFim, psa:psa }
        , type: "GET"
        , datatype: "html"
        , beforeSend: function () {
            waitingDialog.show();
        }
        , complete: function () {
            waitingDialog.hide();
        }
        , success: function (data) {
            $("#situacaologaula").html(data);
            AplicarDataTable("tblLogAula");
            waitingDialog.hide();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });



}


function AbreXMLDetalhe(xmlEntrada, xmlSaida) {
    var div = $("#xml");

    var bodyEntrada = $("#XmlBodyEntrada");
    var bodySaida = $("#XmlBodySaida");

    bodyEntrada.empty();
    bodySaida.empty();

    bodyEntrada.html('<br/><h4>XML Entrada</h4><textarea class="form-control" rows="15">' + xmlEntrada.toString() + '</textarea>');
    bodySaida.html('<br/><h4>XML Saída</h4><textarea class="form-control" rows="15">' + xmlSaida.toString() + '</textarea>');

    div.modal();
}


function FecharModalXML() {
    var div = $("#xml");
    div.modal('hide');
}