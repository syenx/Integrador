/// <reference path="jquery.datetime.js" />
//LOAD
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
        }
        ,constrainInput: true,
        buttonImageOnly: true
    });

    $("#dtFim").datepicker('option', 'minDate', new Date($.now()));
    $("#dtInicio").datepicker('option', 'maxDate', new Date($.now()));


    AplicarMascara();

    $("#buscaSituacaoAula").click(function () {
        

        var cpf = $("#cpf").val();
        var renach = $("#Renach").val();
        var dtInicio = $("#dtInicio").val();
        var dtFim = $("#dtFim").val();

        var idEstado = "0";

        if ($("#IdUF option:selected").text() != "--" || $("#IdUF option:selected").text() != "") {
            idEstado = $("#IdUF option:selected").val();
        }

        var status = "0";
        if ($("#IdStatus option:selected").text() != "--") status = $("#IdStatus option:selected").val();

        var curso = "0";
        if ($("#IdTipoCurso option:selected").text() != "--") curso = $("#IdTipoCurso option:selected").val();

        var podepesquisar = false;

        if (cpf != ""  && (dtInicio == "" && dtFim == "")) {
            podepesquisar = true;
        }
        else {
            if (idEstado == "0" && renach == "" && dtInicio == "" && dtFim == "" && status == "0" && curso == "0") {
                Message("*Informe os parâmetros para realizar esta consulta", "info");
                podepesquisar = false;

            } else {
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

                //REF0019 - Obrigatório selecionar um range de datas (inicio e fim)  
                //ao selecionar um dos campos: UF, Status ou Curso.
                if ((idEstado != "" || status != "" || curso != "") && renach == "") {
                    if (dtInicio == "" && dtFim == "") {
                        //Message("*Data Fim deve ser maior ou igual a Data Início.", "info");
                        Message("*Informe um período para realizar esta consulta", "info")
                        return;
                    }
                }

                if (renach != "") {
                    podepesquisar = true;
                }

                //REF0020 - Obrigatório selecionar ao menos 3 (três) critérios para a pesquisa, 
                //exceto quando a pesquisa for por CPF ou Renach.
                if (cpf == "" && renach == "") {
                    if ((idEstado != "" || status != "" || curso != "") && dtInicio == "" && dtFim == "") {
                        //Message("*Informe ao menos 3 parâmetros para realizar esta consulta", "info");
                        Message("*Informe um período para realizar esta consulta", "info")
                        podepesquisar = false;
                    }
                }



                if (dtInicio != "" && dtFim != "")
                {
                    if (idEstado == "" || status == "" || curso == "")
                    {
                        Message("*Informe um parâmetro a mais para realizar esta consulta", "info")
                        podepesquisar = false;
                        return;
                    }
                }



                if (dtInicio != '') {
                    if (dtFim == '') {
                        Message('Informe a Data Final para realizar esta consulta', 'erro');
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
                            Message("A Data Final deve ser maior ou igual a Data Inicial", "ERRO");
                            return;
                        }
                    }
                }
                else {
                    podepesquisar = true;
                }
            }
        }

        if (podepesquisar) {
            ListarSituacaoAula();
        } else {
            div = $("#situacaoaula");
            div.html("");
        }
    });

    $("#limpaSituacaoAula").click(function () {
        $("#cpf").val("");
        $("#Renach").val("");
        $("#dtInicio").val("");
        $("#dtFim").val("");
        $("#IdUF")[0].selectedIndex = 0;
        $("#IdStatus")[0].selectedIndex = 0;
        $("#IdTipoCurso")[0].selectedIndex = 0;
    });

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaSituacaoAula').click();
            return false;
        }
    });

    $("button[data-number=1]").click(function () {
        $('#xml').modal('hide');
    });

    $("button[data-number=2]").click(function () {
        $('.modal').modal('hide');
    });


});

// ListarSituacaoAula
function ListarSituacaoAula() {

    var url = "/SituacaoAula/ListarSituacaoAula";
    var div = $("#situacaoaula");
    var cpf = $("#cpf").val();
    var renach = $("#Renach").val();
    var dtInicio = $("#dtInicio").val();
    var dtFim = $("#dtFim").val();
    var idEstado = "0";

    if ($("#IdUF option:selected").text() != "--" && $("#IdUF option:selected").text() != "") {
        idEstado = $("#IdUF option:selected").val();
    }

    var status = 0;

    if ($("#IdStatus option:selected").text() != "--") status = $("#IdStatus option:selected").val();

    var curso = 0;

    if ($("#IdTipoCurso option:selected").text() != "--") curso = $("#IdTipoCurso option:selected").val();

    if (cpf != '') cpf = cpf.replace(".", "").replace(".", "").replace("-", "");

    var data = {
        cpf: cpf
        , idEstado: idEstado
        , renach: renach
        , dtInicio: dtInicio
        , dtFim: dtFim
        , status: status
        , curso: curso
    }

    $.ajax({
        url: url
        , datatype: "html"
        , type: "GET"
        , data: data
        , beforeSend: function () {
            waitingDialog.show();
        }
        , complete: function () {
            waitingDialog.hide();
        }
        , success: function (data) {
            div.html(data);
            AplicarDataTable('frmSituacaoAula');
            waitingDialog.hide();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });



    //div.DataTable({
    //    "processing": true,
    //    "serverSide": true,
    //    "ajax": $.fn.dataTable.pipeline({
    //        url: url,
    //        data : data,
    //        pages: 5 // number of pages to cache
    //    })
    //});




}

// Convert To Date
function ConvertToDate(date) {
    var newDate = date.toString().Split('/');
    var retDate = new Date(newDate[2], newDate[1], newDate[0]);
    return retDate;
}

// Retorna Intervalo de Dias
function RetornaIntervaloDias(dtInicial, dtFinal) {
    var dtInicio = ConvertToDate(dtInicial);
    var dtFim = ConvertToDate(dtFinal);
    var resultado = (dtFim - dtInicio)
    return resultado;
}

// Retorna Log Aula
function RetornaLogAula(cpf, dtInicio) {

    var url = "/SituacaoAula/RetornaLogAula";

    $.ajax({
        url: url
        , datatype: "html"
        , type: "GET"
        , data: { cpf: cpf, dtInicio: dtInicio }
        , beforeSend: function () {
            waitingDialog.show();
        }
        , complete: function () {
            waitingDialog.hide();
        }
        , success: function (data) {
            AbrirModal("XML Entrada e Saída", data);
            waitingDialog.hide();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

function AbreXMLDetalhe(xml)
{
    var div = $("#xml");
    var body = $("#XmlBody");
    body.html('<textarea class="form-control" rows="15">' + xml.toString() + '</textarea>');
    div.modal();
}

function FecharModalXML() {
    var div = $("#xml");
    div.modal('hide');
}



//
// Pipelining function for DataTables. To be used to the `ajax` option of DataTables
//
$.fn.dataTable.pipeline = function (opts) {
    // Configuration options
    var conf = $.extend({
        pages: 5,     // number of pages to cache
        url: '',      // script url
        data: null,   // function or object with parameters to send to the server
        // matching how `ajax.data` works in DataTables
        method: 'GET' // Ajax HTTP method
    }, opts);

    // Private variables for storing the cache
    var cacheLower = -1;
    var cacheUpper = null;
    var cacheLastRequest = null;
    var cacheLastJson = null;

    return function (request, drawCallback, settings) {
        var ajax = false;
        var requestStart = request.start;
        var drawStart = request.start;
        var requestLength = request.length;
        var requestEnd = requestStart + requestLength;

        if (settings.clearCache) {
            // API requested that the cache be cleared
            ajax = true;
            settings.clearCache = false;
        }
        else if (cacheLower < 0 || requestStart < cacheLower || requestEnd > cacheUpper) {
            // outside cached data - need to make a request
            ajax = true;
        }
        else if (JSON.stringify(request.order) !== JSON.stringify(cacheLastRequest.order) ||
                  JSON.stringify(request.columns) !== JSON.stringify(cacheLastRequest.columns) ||
                  JSON.stringify(request.search) !== JSON.stringify(cacheLastRequest.search)
        ) {
            // properties changed (ordering, columns, searching)
            ajax = true;
        }

        // Store the request for checking next time around
        cacheLastRequest = $.extend(true, {}, request);

        if (ajax) {
            // Need data from the server
            if (requestStart < cacheLower) {
                requestStart = requestStart - (requestLength * (conf.pages - 1));

                if (requestStart < 0) {
                    requestStart = 0;
                }
            }

            cacheLower = requestStart;
            cacheUpper = requestStart + (requestLength * conf.pages);

            request.start = requestStart;
            request.length = requestLength * conf.pages;

            // Provide the same `data` options as DataTables.
            if ($.isFunction(conf.data)) {
                // As a function it is executed with the data object as an arg
                // for manipulation. If an object is returned, it is used as the
                // data object to submit
                var d = conf.data(request);
                if (d) {
                    $.extend(request, d);
                }
            }
            else if ($.isPlainObject(conf.data)) {
                // As an object, the data given extends the default
                $.extend(request, conf.data);
            }

            settings.jqXHR = $.ajax({
                "type": conf.method,
                "url": conf.url,
                "data": request,
                "dataType": "json",
                "cache": false,
                "success": function (json) {
                    cacheLastJson = $.extend(true, {}, json);

                    if (cacheLower != drawStart) {
                        json.data.splice(0, drawStart - cacheLower);
                    }
                    if (requestLength >= -1) {
                        json.data.splice(requestLength, json.data.length);
                    }

                    drawCallback(json);
                }
            });
        }
        else {
            json = $.extend(true, {}, cacheLastJson);
            json.draw = request.draw; // Update the echo for each response
            json.data.splice(0, requestStart - cacheLower);
            json.data.splice(requestLength, json.data.length);

            drawCallback(json);
        }
    }
};

// Register an API method that will empty the pipelined data, forcing an Ajax
// fetch on the next draw (i.e. `table.clearPipeline().draw()`)
$.fn.dataTable.Api.register('clearPipeline()', function () {
    return this.iterator('table', function (settings) {
        settings.clearCache = true;
    });
});


//
// DataTables initialisation
//
//$(document).ready(function () {
//    $('#example').DataTable({
//        "processing": true,
//        "serverSide": true,
//        "ajax": $.fn.dataTable.pipeline({
//            url: 'scripts/server_processing.php',
//            pages: 5 // number of pages to cache
//        })
//    });
//});

