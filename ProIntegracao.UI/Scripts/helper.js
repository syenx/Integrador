var minDateGlobal = '-30d';

/// <reference path="jquery-ui.min.js" />
//LOAD
$(document).ready(function () {

    $(".icon-input-btn").each(function () {
        var btnFont = $(this).find(".btn").css("font-size");
        var btnColor = $(this).find(".btn").css("color");
        $(this).find(".glyphicon").css("font-size", btnFont);
        $(this).find(".glyphicon").css("color", btnColor);
        if ($(this).find(".btn-xs").length) {
            $(this).find(".glyphicon").css("top", "24%");
        }
    });

    $("#IdEstadoConsulta").val("0");
});

// Message
function Message(message, tipo) {
    
    var div = $("#message");

    if (($("#myModalContent").data('bs.modal') || {}).isShown) {
        div = $("#messageModal");
    }

    if (($("#myModal").data('bs.modal') || {}).isShown) {
        div = $("#messageModalDelete");
    }

    div.html("");

    switch (tipo.toLowerCase()) {
        case "sucesso":
            div.addClass("alert alert-success");
            break;
        case "info":
            div.addClass("alert alert-info");
            break;
        case "erro":
            div.addClass("alert alert-danger");
            break;
        case "aviso":
            div.addClass("alert alert-warning");
            break;
    }

    div.html(message)
        .show()
        .fadeOut(5000);
}

//Excluir
function Excluir(id, controller) {

    var url = "/" + controller + "/Excluir";

    $.ajax({
        url: url
        , datatype: 'json'
        , type: 'POST'
        , data: { id: id }
        , success: function (data) {

            if (data.Resultado) {

                FecharModalExcluir('myModal');

                switch (controller) {
                    case 'Usuario':
                        ListarUsuario();
                        break;
                    case 'Matricula':
                        ListarMatricula();
                        break;
                    case 'Pagina':
                        ListarPaginas();
                        break;
                    case 'Perfil':
                        ListarPerfil();
                        break;
                    case 'Configuracao':
                        ListarConfiguracoes();
                        break;
                    case 'Aula':
                        ListarAulas();
                        break;
                    case 'statussituacaoaula':
                        ListarStatus();
                        break;
                    case 'SimularErro':
                        ListarSimularErro();
                        break;
                    case 'Menu':
                        ListarMenuEdicao();
                        RetornaMenuPrincipal();
                        break;
                    case 'Aluno':
                        ListarAlunos();
                        break;
                    default: break;
                }
                Message("Registro excluído com sucesso", 'aviso');
            }
            else {
                Message('Erro ao excluir registro, verifique os dados e tente novamente', 'erro');
            }

        }
        , beforeSend: function () {

        }
        , complete: function () {

        }
        , error: function (jqXHR, exception) {
            var msg = '';
            var tipo = '';
            if (jqXHR.status === 0) {
                msg = 'Sem Conexão.\n Verifique rede.';
            } else if (jqXHR.status === 404) {
                msg = 'Página não encontrada. [404]';
            } else if (jqXHR.status === 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Erro desconhecido.\n' + jqXHR.responseText;
            }
        }
    });
}

// Abri Modal para Exclusão
function AbrirModalExcluir(id, controller) {

    $("#ModalTitle").html("Excluir " + controller);
    $("#btnExcluir").attr("onclick", "Excluir(" + id + ",'" + controller + "')");
    $("#myModal").modal();
}

// Abrir Modal Genérico
function AbrirModal(titulo, data) {

    $("#ModalTitleContent").html(titulo);

    $("#ModalBodyContent").html("");
    $("#ModalBodyContent").empty();

    $("#ModalBodyContent").html(data);
    $("#myModalContent").modal();

    $(".SomenteLetra").keyup(function () {
        var valor = $("#Nome").val().replace(/[^a-zA-Z ]+/g, '');
        $("#Nome").val(valor);
    });
}

// Fechar Modal Excluir
function FecharModalExcluir(modal){
    $("#" + modal).modal("hide");

}

//Aplicar o DataTable
function AplicarDataTable(nomeTabela) {

    var extendedColumns = [];
    var cpfColumn = -1;


    $("#"+ nomeTabela +" thead th").each(function (index) {

        if ($(this).text().toString().toUpperCase() === "CPF")
        {
            cpfColumn = index;
        }

        extendedColumns.push(index);
    });

    extendedColumns.pop();

    //DataTable
    var table = $("#" + nomeTabela).DataTable({

        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
        , searching: false
        , dom: 'Blfrtip'
        , buttons: [
                {
                    extend: 'excel',
                    text: 'Excel',
                    orientation: 'landscape'
                    , exportOptions: {
                        columns: extendedColumns
                        ,format: {
                            body: function (data, columnIndex) {
                                return verificaImagemAtivo(data, columnIndex, cpfColumn);
                            }
                        }
                    }
                }
                , {
                    extend: 'print'
                    , text: 'Imprimir'
                    , orientation: 'landscape'
                    , exportOptions: {
                        columns: extendedColumns
                        , format: {
                            body: function (data, columnIndex) {
                                return verificaImagemAtivo(data, columnIndex, cpfColumn);
                            }
                        }
                    }
                    ,customize: function (win) {
                        $(win.document.body)
                            .css('font-size', '9px !important')
                            .prepend(
                                '<img src="http://prosimulador.com.br/Recursos/images/logo.png" style="position:absolute; top:0; left:0;" />'
                            );

                        $(win.document.body).find('table')
                            .addClass('compact')
                            .css('font-size', '9px !important');

                        $(win.document.body).find('html')
                            .css('height', 'auto !important');

                        $(win.document.body).find('body')
                            .css('height', 'auto !important');
                            
                    }
                    
                }
        ]
    });

    table.buttons().container()
        .appendTo('#'+nomeTabela+' .col-sm-6:eq(0)');
}

// Aplicar DataTable para Menu de Relatórios Espanha
function AplicarDataTableCustom(nomeTabela, paging) {

    //var extendedColumns = [];
    var cpfColumn = -1;


    $("#" + nomeTabela + " thead th").each(function (index) {

        if ($(this).text().toString().toUpperCase() === "CPF") {
            cpfColumn = index;
        }

       // extendedColumns.push(index);
    });

    //extendedColumns.pop();

    //DataTable
    var table = $("#" + nomeTabela).DataTable({
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
        , searching: false
        , ordering: false
        , dom: 'Blfrtip'
        , paging: paging
        , buttons: [
                //{
                //    extend: 'excel',
                //    text: 'Excel',
                //    orientation: 'landscape'
                //    , exportOptions: {
                //        //columns: extendedColumns
                //         format: {
                //            body: function (data, columnIndex) {
                //                return verificaImagemAtivo(data, columnIndex, cpfColumn);
                //            }
                //        }
                //    }
                //}
                //,
                {
                    extend: 'print'
                    , text: 'Imprimir'
                    , orientation: 'landscape'
                    , exportOptions: {
                        //columns: extendedColumns
                         format: {
                            body: function (data, columnIndex) {
                                return verificaImagemAtivo(data, columnIndex, cpfColumn);
                            }
                        }
                    }
                    , customize: function (win) {
                        $(win.document.body)
                            .css('font-size', '10pt')
                            .prepend(
                                '<img src="http://prointegracao.prosimulador.com.br/content/images/logo_bg_opacity.png" style="position:absolute; top:0; left:0;z-index:-1;" />'
                            );

                        $(win.document.body).find('table')
                            .addClass('compact')
                            .css('font-size', '10px !important')
                            .css('filter', 'alpha(opacity=60)')
                            .css('opacity', '0.6');
                    }
                }
        ]
    });

    table.buttons().container()
        .appendTo('#' + nomeTabela + ' .col-sm-6:eq(0)');
}

function verificaImagemAtivo(data, columnIndex, cpfColumn) {

    var returnData = "";

    if (data === '<i class="fa fa-check"></i>') {
        returnData = "SIM";
    }
    else if (data === '<i class="fa fa-times"></i>') {
        returnData = "NÂO";
    }
    else {
        returnData = data;
    }

    if (columnIndex === cpfColumn)
    {
        returnData = mascaraCpf(returnData);
    }

    return returnData;

}

// Mascara CPF
function mascaraCpf(valor) {
    return valor.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, "\$1.\$2.\$3\-\$4");
}

//Definir Estado para Consulta
function DefinirEstadoConsulta(funcao)
{
    $("#myModalEstados").modal({show:true});
}

// So Num
function soNum(obj) {
    var tecla = window.event ? event.keyCode : obj.which;
    if (tecla > 47 && tecla < 58) {
        return true;
    } else {
        if (tecla !== 8) {
            return false;
        } else {
            return true;
        }
    }
}

//function ValidaCPF Existente
function ValidaCpfExistente(cpf)
{

    var termo = $("#" + cpf).val();

    var url = "/Matricula/ValidaCpfExistente";
    var data = { cpf: termo };

    $.ajax({
        url: url
        , type: "POST"
        , datatype: "json"
        , data: data
        , success: function (data) {
            if (data.Resultado) {
                $("#FormCpf").append("<i id='ok' class='glyphicon glyphicon-edit'></i>");
            }
            else {
                $("#ok").remove();
                $("#" + cpf).val("");
            }
        }
        , beforeSend: function () {

        }
        , complete: function () {

        }
        , error: function (jqXHR, exception) {
            var msg = '';
            var tipo = '';
            if (jqXHR.status === 0) {
                msg = 'Sem Conexão.\n Verifique rede.';
            } else if (jqXHR.status === 404) {
                msg = 'Página não encontrada. [404]';
            } else if (jqXHR.status === 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Erro desconhecido.\n' + jqXHR.responseText;
            }
            Message(msg, 'erro', 'messageModal');
        }
    });


}

// AutoComplete
function AutoComplete(component)
{
    var url = "/Matricula/RetornaListaJson";
    
    $.ajax({
        url: url
        , datatype: 'json'
        , type: 'GET'
        , success: function (data) {
            $("#" + component).autocomplete({
                source: data.Resultado,
                minLength: 0,
                scroll: true
            }).focus(function () {
                $(this).autocomplete("search", "");
            }).each(function() {
                $(this).autocomplete("widget").insertAfter($("#myModalContent").parent());
            });
            
        }
        , error: function (jqXHR, exception) {
            var msg = '';
            var tipo = '';
            if (jqXHR.status === 0) {
                msg = 'Sem Conexão.\n Verifique rede.';
            } else if (jqXHR.status === 404) {
                msg = 'Página não encontrada. [404]';
            } else if (jqXHR.status === 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Erro desconhecido.\n' + jqXHR.responseText;
            }
            Message(msg, 'erro', 'messageModal');
        }
    });
}

// Arruma o CPF para Leitura
function RetornaCPFSemPontos(cpf)
{
    cpf = cpf.replace(".", "").replace(".", "").replace("-", "");
    return cpf;
}

function RetornaDataNowToString() {

    var d = new Date();

    var ano = d.getFullYear();
    var mes = d.getMonth() + 1;
    var dia = d.getDate();


    return ano.toString() + "/" + mes.toString() + "/" + dia.toString();

}

//*** This code is copyright 2002-2003 by Gavin Kistner, !@phrogz.net
//*** It is covered under the license viewable at http://phrogz.net/JS/_ReuseLicense.txt

/*

token:     description:             example:
#YYYY#     4-digit year             1999
#YY#       2-digit year             99
#MMMM#     full month name          February
#MMM#      3-letter month name      Feb
#MM#       2-digit month number     02
#M#        month number             2
#DDDD#     full weekday name        Wednesday
#DDD#      3-letter weekday name    Wed
#DD#       2-digit day number       09
#D#        day number               9
#th#       day ordinal suffix       nd
#hhhh#     2-digit 24-based hour    17
#hhh#      military/24-based hour   17
#hh#       2-digit hour             05
#h#        hour                     5
#mm#       2-digit minute           07
#m#        minute                   7
#ss#       2-digit second           09
#s#        second                   9
#ampm#     "am" or "pm"             pm
#AMPM#     "AM" or "PM"             PM

var now = new Date;
console.log( now.customFormat( "#DD#/#MM#/#YYYY# #hh#:#mm#:#ss#" ) );

*/
Date.prototype.customFormat = function (formatString) {
    var YYYY, YY, MMMM, MMM, MM, M, DDDD, DDD, DD, D, hhhh, hhh, hh, h, mm, m, ss, s, ampm, AMPM, dMod, th;
    YY = ((YYYY = this.getFullYear()) + "").slice(-2);
    MM = (M = this.getMonth() + 1) < 10 ? '0' + M : M;
    MMM = (MMMM = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'][M - 1]).substring(0, 3);
    DD = (D = this.getDate()) < 10 ? '0' + D : D;
    DDD = (DDDD = ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'][this.getDay()]).substring(0, 3);
    th = D >= 10 && D <= 20 ? 'th' : (dMod = D % 10) === 1 ? 'st' : dMod === 2 ? 'nd' : dMod === 3 ? 'rd' : 'th';
    formatString = formatString.replace("#YYYY#", YYYY).replace("#YY#", YY).replace("#MMMM#", MMMM).replace("#MMM#", MMM).replace("#MM#", MM).replace("#M#", M).replace("#DDDD#", DDDD).replace("#DDD#", DDD).replace("#DD#", DD).replace("#D#", D).replace("#th#", th);
    h = hhh = this.getHours();
    if (h === 0) h = 24;
    if (h > 12) h -= 12;
    hh = h < 10 ? '0' + h : h;
    hhhh = h < 10 ? '0' + hhh : hhh;
    AMPM = (ampm = hhh < 12 ? 'am' : 'pm').toUpperCase();
    mm = (m = this.getMinutes()) < 10 ? '0' + m : m;
    ss = (s = this.getSeconds()) < 10 ? '0' + s : s;
    return formatString.replace("#hhhh#", hhhh).replace("#hhh#", hhh).replace("#hh#", hh).replace("#h#", h).replace("#mm#", mm).replace("#m#", m).replace("#ss#", ss).replace("#s#", s).replace("#ampm#", ampm).replace("#AMPM#", AMPM);
};

// Página de ICONES
function CheatSheet() {
    var url = "/Home/CheatSheet";
    $.ajax({
        url: url
        , datatype: "html"
        , type: "GET"
        , success: function (data) {
            $("#tallModal").modal();
            $(".modal-wide").on("show.bs.modal", function () {
                var height = $(window).height() - 200;
                $(this).find(".modal-body").css("max-height", height);
            });
            $("#ModalBodyFont").html(data);
        }
    });
}

// Validar Data
function isDate(date) {

    var currVal = date;

    if (currVal === '')
        return false;

    //Declare Regex  
    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray === null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[3];
    dtDay = dtArray[1];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth === 4 || dtMonth === 6 || dtMonth === 9 || dtMonth === 11) && dtDay === 31)
        return false;
    else if (dtMonth === 2) {
        var isleap = dtYear % 4 === 0 && (dtYear % 100 !== 0 || dtYear % 400 === 0);
        if (dtDay > 29 || dtDay === 29 && !isleap)
            return false;
    }
    return true;
}

// RetornaDataBR
function RetornaDataBR(date)
{
    var currVal = date;

    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray === null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[3];
    dtDay = dtArray[1];
    dtYear = dtArray[5];

    return new Date(dtYear, dtMonth-1, dtDay);
}


function RetornaNewDate(value)
{

    var first = value.split('/');

    var day = first[0];
    var month = first[1];
    var second = first[2].split(' ');
    var year = second[0];
    var horas = second[1].split(':');
    var hour = horas[0];
    var minute = horas[1];
   
    //new Date(ano, mês, dia, hora, minuto, segundo, milissegundo);
    return new Date(year,month,day,hour,minute,second);
}


/// Tratamento Genérico de Erro
function TratamendodeErro(jqXHR, exception)
{
    var msg = '';

    var tipo = 'erro';

    if (jqXHR.status === 0) {
        msg = 'Sem Conexão.\n Verifique rede.';
    } else if (jqXHR.status === 500) {
        msg = 'Erro ao gravar registro, verifique os dados e tente novamente.';
    } else if (exception === 'timeout') {
        msg = 'Time out error.';
    } else if (exception === 'abort') {
        msg = 'Operação cancelada, verifique os dados e tente novamente.';
    } else {
        msg = 'Erro desconhecido.\n Verifique os dados e tente novamente.' + jqXHR.responseText;
    }

    Message(msg, tipo);

    waitingDialog.hide();
}

//Validar CPF
//function validarCPF(cpf) {
//        //get input
//    if (cpf) {
//        var input = cpf.toString();

//        var numeros = [];
//        var pesos_A = [10, 9, 8, 7, 6, 5, 4, 3, 2];
//        var pesos_B = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
//        var sum = 0;
//        var x1 = 0;
//        var x2 = 0;

//        for (var i = 0; i = 2) {
//            x1 = 11 - mod;
//        }

//        //calcula digito 2
//        sum = 0;
//        for (var i = 0; i = 2) {
//            x2 = 11 - mod;
//        }

//        if (x1 == input[9] && x2 == input[10]) {
//            return true;
//        } else {
//            return false;
//        }
//    } else {
//        return false;
//    }
//}

// Aplicar DatePicker

function AplicarDatePicker() {

    $(".datepicker").datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        constrainInput: true,
        buttonImageOnly: true
    });

}

// Retorna Menu Principal
function RetornaMenuPrincipal() {

    var url = "/Menu/ListarMenu";

    $.ajax({
        url: url
        , type: "GET"
        , datatype: "json"
        , success: function (data) {
            $("#navbar").empty();
            $("#navbar").html(data);
        }
        , error: function (jqXHR, exception) {
            var msg = '';
            var tipo = '';
            if (jqXHR.status === 0) {
                msg = 'Sem Conexão.\n Verifique rede.';
            } else if (jqXHR.status === 404) {
                msg = 'Página não encontrada. [404]';
            } else if (jqXHR.status === 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Erro desconhecido.\n' + jqXHR.responseText;
            }
        }
    });
}

// Funcão de Random Minímo, Maximo com Randomico
function rand(min, max, interval) {
    if (typeof interval === 'undefined') interval = 1;
    var r = Math.floor(Math.random() * (max - min + interval) / interval);
    return r * interval + min;
}
