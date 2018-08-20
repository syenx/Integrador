//LOAD
$(document).ready(function () {
    var idAluno = $("#IdAluno").val();
    if (idAluno != '') {
        ListarMatriculas(idAluno);
    }
});

// Listar Matriculas
function ListarMatriculas(idAluno) {
    var url = "/HistoricoAluno/ListarMatriculas";

    $.ajax({
        url: url
        , data: { idAluno: idAluno }
        , type: "GET"
        , datatype: "html"
        , success: function (data) {
            $("#tablematricula").show();
            $("#listaMatriculas").html(data);
            AplicarDataTable('tabeladeMatriculas');

            $("#collapseThree").collapse("hide");
            $("#collapseFour").collapse("hide");

        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Listar Agendas
function ListarAgendas(idMatricula) {

    var url = "/HistoricoAluno/ListarAgenda";
    $.ajax({
        url: url
        , data: { idMatricula: idMatricula }
        , type: "GET"
        , datatype: "html"
        , success: function (data) {

            $("#tableagenda").show();

            $("#listaAgendas").html(data);

            ListarHistoricoAulas(idMatricula);

            AplicarDataTable('tabeladeagendas');

            $("#collapseThree").collapse("show");


        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });

}

// Listar Historico Aulas
function ListarHistoricoAulas(idMatricula) {

    var url = "/HistoricoAluno/ListarHistoricoAula";
    $.ajax({
        url: url
        , data: { idMatricula: idMatricula }
        , type: "GET"
        , datatype: "html"
        , success: function (data) {
            $("#tablehistorico").show();
            $("#listaHistoricoAulas").html(data);
            AplicarDataTable('tabeladehistorico');
            $("#collapseFour").collapse("show");

            $(".info").popover();

        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });

}

/// Detalhe Historico
function DetalheHistorico(idAula) {

    var idAluno = $("#IdAluno").val();

    var url = "/HistoricoAluno/RetornaDetalhe";

    $.ajax({
        url: url
        , datatype: "html"
        , type: "GET"
        , data: { idAula: idAula, idAluno: idAluno }
        , success: function (data) {

            $("#tallModal").modal();
            $(".modal-wide").on("show.bs.modal", function () {
                var height = $(window).height() - 200;
                $(this).find(".modal-body").css("max-height", height);
            });
            $("#ModalBodyFont").html(data);
            $("#modalTallTitle").html("Histórico de Aulas - Detalhes");

        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    })
}

// Alterar Status
function AlterarStatus(idAula, idStatus, idModelo, idSimulador) {

    var label = $("#lbst" + idAula);
    var labelmd = $("#lbmd" + idAula);



    var td = $("#tdst" + idAula);

    var linkSave    = $("#lnkSave" + idAula);
    var linkCheck   = $("#lnkPencil" + idAula);
    var linkCancel  = $("#lnkCancel" + idAula);

    var lnkInfo     = $("#lnkInfo" + idAula);
    var lnkSearch   = $("#lnkSearch" + idAula);
    var lnkDetail   = $("#lnkDetail" + idAula);
    var lnkTrash    = $("#lnkTrash" + idAula);
    var lnkSend     = $("#lnkSend" + idAula);

    var selectid = "slid" + idAula;

    label.hide();
    labelmd.hide();

    CriarSelectStatus(idAula, idStatus);
    CriarSelectModelo(idAula, idModelo, idSimulador);

    linkSave.show();
    linkCancel.show();
    linkCheck.hide();

    lnkInfo.hide();
    lnkSearch.hide();
    lnkDetail.hide();
    lnkTrash.hide();
    lnkSend.hide();
}

// Salvar Status
function SalvarStatus(idAula) {

    var label = $("#lbst" + idAula);
    var labelmd = $("#lbmd" + idAula);

    var td = $("#tdst" + idAula);
    var tdmd = $("#tdmd" + idAula);

    var linkSave = $("#lnkS" + idAula);
    var linkCheck = $("#lnkC" + idAula);

    var selectid = $("#slid" + idAula);
    var selectmid = $("#slmid" + idAula);

    var novoIdStatus = $("#slid" + idAula + " option:selected").val();
    var novoIdModelo = $("#slmid" + idAula + " option:selected").val();

    var novoNomeStatus = $("#slid" + idAula + " option:selected").text();
    var novoNomeModelo = $("#slmid" + idAula + " option:selected").text();

    SalvarStatusModelo(idAula, novoIdStatus, novoIdModelo, novoNomeStatus, novoNomeModelo);

    if (!label.is(":visible") && !labelmd.is(":visible")) {
        RestaurarEdicaoStatusModelo(idAula);
    }
}

/// Cancelar alteração do Modelo e Status
function CancelarAlteracao(idAula) {

    var label = $("#lbst" + idAula);
    var labelmd = $("#lbmd" + idAula);

    RestaurarEdicaoStatusModelo(idAula);
    
}

function RestaurarEdicaoStatusModelo(idAula) {

    var label = $("#lbst" + idAula);
    var labelmd = $("#lbmd" + idAula);

    var linkSave = $("#lnkSave" + idAula);
    var linkCancel = $("#lnkCancel" + idAula);
    var linkCheck = $("#lnkPencil" + idAula);

    var lnkInfo = $("#lnkInfo" + idAula);
    var lnkSearch = $("#lnkSearch" + idAula);
    var lnkDetail = $("#lnkDetail" + idAula);
    var lnkTrash = $("#lnkTrash" + idAula);
    var lnkSend = $("#lnkSend" + idAula);


    lnkInfo.show();
    lnkSearch.show();
    lnkDetail.show();
    lnkTrash.show();


    var selectid = $("#slid" + idAula);
    var selectmid = $("#slmid" + idAula);

    var novoIdStatus = $("#slid" + idAula + " option:selected").val();
    var novoIdModelo = $("#slmid" + idAula + " option:selected").val();

    selectid.remove();
    selectmid.remove();

    label.show();
    labelmd.show();

    linkSave.hide();
    linkCancel.hide();
    linkCheck.show();
    lnkSend.show();
}

/// Salvar Status Modelo
function SalvarStatusModelo(idAula, novoIdStatus, novoIdModelo, novoNomeStatus, novoNomeModelo) {

    var url = "/HistoricoAluno/AlterarStatusModeloAula";

    $.ajax({
        url: url
        , datatype: "json"
        , type: "GET"
        , data: { idAula: idAula, novoIdStatus: novoIdStatus, novoIdModelo: novoIdModelo }
        , success: function (data) {
            if (data.Resultado) {
                
                toastr.success("Registro alterado com sucesso!");


                var label = $("#lbst" + idAula);
                var labelmd = $("#lbmd" + idAula);

                label.html(novoNomeStatus);
                labelmd.html(novoNomeModelo);


            } else {
                Message("Erro ao gravar registro, verifique os dados e tente novamente", "erro");
            }
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Enviar Aula
function EnviarAula(idAgenda, idAula) {

    var url = "/HistoricoAluno/EnviarAula";

    $.ajax({

        url: url
        , datatype: "json"
        , type: "GET"
        , data: { idAgenda: idAgenda }
        , success: function (data) {
            if (data.Resultado) {
                toastr.success("Aula Reenviada com Sucesso!");
                var lnkSend = $("#lnkSend" + idAula);
                lnkSend.hide();
            } else {
                toastr.error("Erro ao alterar registro, verifique os dados e tente novamente");
            }
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    })
}

// Cancelar Aula
function CancelarEnviarAula(idAgenda) {

    var url = "/HistoricoAluno/CancelarAula";

    $.ajax({

        url: url
        , datatype: "json"
        , type: "GET"
        , data: { idAgenda: idAgenda }
        , success: function (data) {
            if (data.Resultado) {
                toastr.success("Aula Reenviada com Sucesso!");
                FecharModalExcluir("myModalCancelarAula");
            } else {
                toastr.error("Erro ao alterar registro, verifique os dados e tente novamente");
            }
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    })
}

/// Criar Select Status
function CriarSelectStatus(idAula, idStatus) {
    var url = "/HistoricoAluno/RetornaStatusAulaPro";
    var selectid = "slid" + idAula;
    var td = $("#tdst" + idAula);

    $.ajax({
        url: url
        , datatype: "json"
        , type: "GET"
        , success: function (data) {
            if (data.length > 0) {
                var dadosGrid = data;
                var select = $("<select>").attr("id", selectid).addClass("form-control input-sm width150");

                $.each(dadosGrid, function (indice, item) {

                    var opt = $('<option value="' + item["Id"] + '">' + item["Nome"] + '</option>');

                    var id = item["Id"];

                    if (id == idStatus) {
                        opt.attr("selected", "selected");
                    }

                    select.append(opt);
                });

                td.append(select);
            }
        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

/// Criar Select Modelo
function CriarSelectModelo(idAula, IdModelo, idSimulador) {
    var url = "/HistoricoAluno/RetornaListaModeloPro";
    var selectid = "slmid" + idAula;
    var td = $("#tdmd" + idAula);

    $.ajax({
        url: url
        , datatype: "json"
        , type: "GET"
        , data: {idSimulador : idSimulador}
        , success: function (data) {
            if (data.Resultado.length > 0) {
                var dadosGrid = data.Resultado;
                var select = $("<select>").attr("id", selectid).addClass("form-control input-sm  width250");

                $.each(dadosGrid, function (indice, item) {

                    var opt = $('<option value="' + item["ID_MODELO"] + '">' + item["NOME"] + '</option>');

                    var id = item["ID_MODELO"];

                    if (id == IdModelo) {
                        opt.attr("selected", "selected");
                    }

                    select.append(opt);
                });

                td.append(select);
            }
        }, error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Abrir Modal Para Cancelar Aula ou não
function AbrirModalCancelarAula(idAgenda, idAula) {
    $("#myModalCancelarAula").modal("show");
    $("#idAulaCancelar").val(idAula);
    $("#idAgendaCancelar").val(idAgenda);
} 

// Cancelar Aula
function CancelarAula() {

    var idAula = $("#idAulaCancelar").val();
    var idAgenda = $("#idAgendaCancelar").val();

    var url = "/HistoricoAluno/CancelarAula";

    if (idAula == "") return;

    $.ajax({
        url: url
        , datatype: "json"
        , type: "GET"
        , data: { idAgenda: idAgenda }
        , success: function (data) {
            if (data) {
                toastr.success("Registro alterado com sucesso!");
                var lnkTrash = $("#lnkTrash" + idAula);
                lnkTrash.hide();

            } else {
                toastr.error("Erro ao gravar registro, verifique os dados e tente novamente");
            }
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });

}

// Consulta Detran
function ConsultaDetran(cpf, identificadoraula)
{
    var url = "/HistoricoAluno/ConsultaDetran";
    var nome = $("#nome").html();
    
    $.ajax({
        url: url
        , type: "GET"
        , datatype: "html"
        , data: { nome: nome, cpf: cpf, identificadoraula: identificadoraula }
        , success: function (data) {
            AbrirModal('Resultado da Consulta no DETRAN', data);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}
