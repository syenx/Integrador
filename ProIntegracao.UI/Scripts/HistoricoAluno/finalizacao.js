$(document).ready(function () {

    $(".collapse").collapse("hide");

    $("#collapseOne").collapse("show");

    $("#selectall").click(function () {

        var checked = $(this).is(":checked");
        if (checked) {
            $(".selectall").prop("checked", true);
        } else {
            $(".selectall").prop("checked", false);
        }
    });

});

/// Enviar Finalização
/// <reference path="C:\Users\freis\Documents\Visual Studio 2015\Projetos\ProIntegracao\ProIntegracao.UI\Views/HistoricoAluno/_listaEventos.cshtml" />
function EnviarFinalizacao()
{
    var listAulas = [];
    
    $('input:checkbox.selectall').each(function () {
        if ($(this).is(":checked")) {

            var dados           = $(this).attr("id").toString().split('-');
            var idModelo        = dados[1];
            var idAula          = dados[2];
            var cfc             = $("#cfc").html().split('-');
            var listEvents      = [];

            var FinalizarAula = {
                ID_MODELO: parseInt(idModelo)
                , ID_AULA: parseInt(idAula)
                , NOME: $("#nome").html()
                , ESTADO: $("#uf").html()
                , ID_CFC: parseInt(cfc[0].trim())
                , NOME_FANTASIA: cfc[1]
                , ID_MATRICULA: parseInt($("#matricula").html())
                , MODELO: ""
                , CPF: ""
                , Evetos: []
                , ID_AGENDA : $("#ID_AGENDA").val()
            };
            
            var tbody = "#tbody" + idModelo;

            $(tbody + "> tr").each(function () {
                var valor       = "v-" + idAula + " - " + idModelo;
                var EventosFinalizacao = {
                    MODELO                  : ""
                    , ID_MODELO             : idModelo
                    , ID_EVENTO             : $(this).children("td:first").next().text()
                    , TIPO_EVENTO           : $(this).children("td:first").next().next().text()
                    , ID_AULA               : idAula
                    , DESCRICAO             : ""
                    , DISTANCIA             : $("#distancia"+idModelo).html()
                    , TEMPOCIRCULACAO       : $("#tempo"+idModelo).html()
                    , VELOCIDADEMAXIMA      : $("#velmaxima"+idModelo).html()
                    , VELOCIDADEMEDIA       : $("#velmedia" + idModelo).html()
                    , EVENTO                : ""
                    , QTD_INFRACOES         : $(this).children("td:first").next().next().next().next().text()
                };

                listEvents.push(EventosFinalizacao);

            });
            
            FinalizarAula.Eventos = listEvents;

            listAulas.push(FinalizarAula);
        };

    });

    var url         = "/HistoricoAluno/FinalizarAulaModelo";
    var matricula   = $("#matricula").html();


    $.ajax({
        url: url
        , type: 'POST'
        , datatype: "json"
        , data: { listAulas: listAulas }
        , success: function (data) {
            if (data.Resultado)
            {
                Message("Aula finalizada com sucesso", "sucesso");
            } else {
                Message("Aula não finalizada, encaminhar para análise, ERRO", "erro");
            }

            window.setTimeout(function () {
                window.location = "/HistoricoAluno/Finalizar?idMatricula=" + matricula;
            }, 3000);
            
        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}
