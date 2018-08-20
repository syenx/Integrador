$(document).ready(function () {

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaAulasPorSimulador').click();
            return false;
        }
    });

    $('input[placeholder]').on('paste', function (e) {
        e.preventDefault();
        var clipboardCurrentData = (e.originalEvent || e).clipboardData.getData('text/plain');
        window.document.execCommand('insertText', false, clipboardCurrentData);
    });
    
    $("#buscaAulasPorSimulador").click(function () {
        if (ValidaDataInicioFim) {
            ListarAulasSimuladorAtivo();
        }
    });
});

//Listar Contratos Firmados
function ListarAulasSimuladorAtivo() {

    var dtInicio = $("#dtInicio").val();
    var dtFim = $("#dtFim").val();
    var idEstado = $("#idEstado option:selected").val();
    var url = "/AulaRealizada/ListarAulasPorSimulador";

    $.ajax({
        url: url
        , data: { dtInicio: dtInicio, dtFim: dtFim, idEstado: idEstado }
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
            AplicarDataTableCustom("aulasporsimulador",false);
            waitingDialog.hide();
        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    })
}

// Exportar Aulas por Simulador
function ExportarAulasPorSimulador() {

    var dtInicio = $("#dtInicio").val();
    var dtFim = $("#dtFim").val();
    var idEstado = $("#idEstado option:selected").val();

    var url = "/AulaRealizada/ExportarAulasPorSimulador?dtInicio=" + dtInicio + "&dtFim=" + dtFim + "&idEstado=" + idEstado;

    if (ValidaDataInicioFim()) {
        window.location = url;
    }
}

function ValidaDataInicioFim()
{

    var dtInicio = "01/" + $("#dtInicio").val();
    var dtFim = "01/" + $("#dtFim").val();
    var podepesquisar = true;

    if ($("#dtInicio").val() == "" && $("#dtFim").val() == "") {
        Message('*Informe um período para realizar esta consulta.', 'erro');
        podepesquisar = false;
        return;
    }

    if ($("#dtInicio").val() != "") {
        if ($("#dtFim").val() != "") {

            var adtInicio = ("01/" + $("#dtInicio").val()).split('/');
            var adtFim = ("01/" + $("#dtFim").val()).split('/');

            var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
            var dtFinal = new Date(adtFim[2], adtFim[1], adtFim[0]);

            if (dtFinal >= dtInicial) {
                podepesquisar = true;
            } else {
                Message("*Mês Fim deve ser maior ou igual a Mês Início.", "ERRO");
                return;
            }
        }
    }

    if ($("#dtInicio").val() != "") {
        if (!isDate(dtInicio)) {
            Message("*Mês Início inválido. Favor informar um parâmetro para consulta", "erro");
            podepesquisar = false;
            return;
        }
        else {
            var adtInicio = dtInicio.split('/');
            var dtInicial = new Date(adtInicio[2], adtInicio[1], adtInicio[0]);
            if (adtInicio[2] < 2016) {
                Message("*Mês Inicial deve ser a partir de Janeiro/ 2016", "erro");
                return podepesquisar;
            }
        }
    }

    return podepesquisar;

}


// Limpar Aulas por Simulador
function LimparAulasPorSimulador() {
    $("#dtInicio").val("");
    $("#dtFim").val("");
    $("#idEstado")[0].selectedIndex = 0;
}

//function ListarAulasSimuladorAtivoJson() {

//    debugger;

//    var dtInicio = $("#dtInicio").val();
//    var dtFim = $("#dtFim").val();
//    var idEstado = $("#idEstado option:selected").val();

//    var url = "/AulaRealizada/ListarAulasPorSimuladorJson";

//    $.ajax({
//        url: url
//        , data: { dtInicio: dtInicio, dtFim: dtFim, idEstado: idEstado }
//        , type: "GET"
//        , cache: false
//        , datatype: "html"
//        , beforeSend: function () {
//            waitingDialog.show();
//        }
//        , complete: function () {
//            waitingDialog.hide();
//        }
//        , success: function (data1) {
//            var extendedColumns = [];
//            var cpfColumn = -1;

//            $("#aulasporsimulador  thead th").each(function (index) {
//                if ($(this).text().toString().toUpperCase() == "CPF") {
//                    cpfColumn = index;
//                }
//                extendedColumns.push(index);
//            });

//            extendedColumns.pop();

//            var table = $("#aulasporsimulador").DataTable({
//                 searching: false
//                , dom: 'Blfrtip'
//                , buttons: [
//                {
//                    extend: 'excel',
//                    text: 'Excel',
//                    orientation: 'landscape'
//                    , exportOptions: {
//                        columns: extendedColumns
//                        , format: {
//                                body: function (data, columnIndex) {
//                                return verificaImagemAtivo(data, columnIndex, cpfColumn);
//                                }
//                    }
//                                }
//                 }
//                 ,
//                 {
//                    extend: 'print'
//                    , text: 'Imprimir'
//                        , orientation: 'landscape'
//                    , exportOptions: {
//                        columns: extendedColumns
//                                , format: {
//                                        body: function (data, columnIndex) {
//                                        return verificaImagemAtivo(data, columnIndex, cpfColumn);
//                                }
//                        }
//                    }
//                    , customize: function (win) {
//                        $(win.document.body)
//                            .css('font-size', '9px !important')
//                            .prepend(
//                                '<img src="http://prosimulador.com.br/Recursos/images/logo.png" style="position:absolute; top:0; left:0;" />'
//                            );

//                        $(win.document.body).find('table')
//                            .addClass('compact')
//                            .css('font-size', '9px !important');

//                        $(win.document.body).find('html')
//                            .css('height', 'auto !important');

//                        $(win.document.body).find('body')
//                            .css('height', 'auto !important');

//                        }
//                    }
//                ]
//                , "data": data1
//                , "columns": [
//                {
//                    "title": "MÊS"
//                    , "data": "Mes"
//                },
//                {
//                    "title": "ESTADO"
//                    , "data": "Estado"
//                },
//                {
//                    "title": "NR. MÉDIO DE AULAS"
//                    , "data": "NrMedioAulas"
//                },
//                {
//                    "title": "NR. SIMULADORES"
//                    , "data": "NrSimuladores"
//                },
//                {
//                    "title": "MÉDIA"
//                    , "data": "Media"
//                }

//                ]
//                ,"drawCallBack": function (settings) {

//                    var api = this.api();
//                    var rows = api.rows({
//                        page : 'current'
//                    }).nodes();

//                    var last = null;

//                    api.column(0, {
//                        page : 'current'
//                    }).data().each(function (group, i) {
//                        console.log(group);
//                        if (last !== group) {
//                            $(rows).eq(i).before(

//                            $("<tr></tr>", {
//                                "class": "group",
//                                "data-id": group
//                            }).append($("<td></td>", {
//                                "colspan": 3,
//                                "class": "pocell",
//                                "text": "Média # " + group
//                            })).append($("<td></td>", {
//                                "id": "e" + group,
//                                "class": "noCount",
//                                "text": "0.00"
//                            })).prop('outerHTML'));
//                            last = group;
//                        }

//                        val = api.row(api.row($(rows).eq(i)).index()).data();

//                        $("#e" + val.Estado).text(parseFloat($("#e" + val.NrMediaAulas).text()) + parseFloat(val.NrSimuladores));
                   
//                    });
//                }
//                ,
//                "footerCallback": function (row, data, start, end, display) {
//                    var api = this.api();
//                    $(api.column(3).footer()).html(
//                        api.column(3).data().reduce(function (a, b) {
//                            return parseFloat(a) + parseFloat(b);
//                        })
//                    );
//                }
//        });
//            table.buttons().container().appendTo('#aulasporsimulador .col-sm-6:eq(0)');
//            waitingDialog.hide();
//        }, error: function (jqXHR, exception) {
//            TratamendodeErro(jqXHR, exception);
//        }
//    })

//}


