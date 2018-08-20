//LOAD
$(document).ready(function () {

    //Listar Aulas
    ListarStatus();

    // Click Boãto Busca
    $("#buscaStatus").click(function () {
        ListarStatus();
    });

    // Click do Botão Salvar Página
    $("#salvarStatus").click(function () {
        var result = false;
        result = ValidarFormulario();
        if (result) {
            SalvarStatus();
        }
    });

    $("#limparStatus").click(function () {
        $("#termo").val("");
        $('#Estados')[0].selectedIndex = 0;
        ListarStatus();
    });

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaStatus').click();
            return false;
        }
    });

});

//Editar Status
function EditarStatus(idStatus) {

    var url = "/StatusSituacaoAula/Edit";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , data: { idStatus: idStatus }
        , success: function (data) {
            AbrirModal('Atualizar Status', data);
            AplicarMascara();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

//Nova Status
function NovoStatus() {

    var url = "/StatusSituacaoAula/Create";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , success: function (data) {
            AbrirModal('Novo Status', data);
            AplicarMascara();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

//Listar Status
function ListarStatus() {

    var idEstado    = $("#Estados option:selected").val();
    var termo       = $("#termo").val().replace(".", "").replace(".", "").replace("-", "");
    var ativo       = $("#ativolistar").is(":checked");
    var url         = "/StatusSituacaoAula/ListarStatus";
    var div         = $("#listaStatus");

    div.empty();

    if (idEstado == "") {
        idEstado = 0;
    }

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: { termo: termo, idEstado: idEstado, ativo: ativo }
        , beforeSend: function () {waitingDialog.show();}
        , complete: function () { waitingDialog.hide(); }
        , success: function (data) {
            div.html(data);
            AplicarDataTable('status');
            waitingDialog.hide();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });

}

//Salvar Aula
function SalvarStatus() {

    if (ValidarFormulario()) {

        var ativo = $("#ativo").is(':checked');

        var StatusSituacaoAulaViewModel =
        {
            Id                  : $("#Id").val()
            , Nome              : $("#Nome").val()
            , Identificador     : $("#Identificador").val()
            , IdEstado          : $("#IdEstado option:selected").val()
            , ListaEstado       : null
            , DtCadastro        : $("#DtCadastro").val()
            , DtExclusao        : ativo
        };

        if (!ativo) {
            StatusSituacaoAulaViewModel.DtExclusao = RetornaDataNowToString();
        } else {
            StatusSituacaoAulaViewModel.DtExclusao = null;
        }

        var url = "/StatusSituacaoAula/Create";

        if ($("#Id").val() != "" && $("#Id").val() != "0") {
            url = "/StatusSituacaoAula/Edit";
        }

        $.ajax({
            url: url
            , data: { model: StatusSituacaoAulaViewModel }
            , datatype: "json"
            , type: "POST"
            , success: function (data) {
                if (data.Resultado) {
                    FecharModalExcluir("myModalContent");
                    Message('Dados Salvos com Sucesso', 'sucesso');
                    ListarStatus();
                } else {
                    var message = "";

                    if (data.Mensagem == "")
                    {
                        message = "Erro ao gravar registro, verifique os dados e tente novamente";
                    }
                    else
                    {
                        message = data.Mensagem;
                    }

                    Message(message, 'erro');
                }
            }
            , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception);
            }
        });
    }
}

//Validar Formulario
function ValidarFormulario() {


    jQuery.validator.addMethod("notEqual", function (value, element, param) {
        return value != "";
    }, "*ESTADO é obrigatório.");


    //Efetua a Validação 
    $("#formStatus").validate({
        debug: true,
        rules: {
            Nome: {
                required: true
            }
            , Identificador: {
                required: true
                , min : 1
            }
            , IdEstado: {
                notEqual: true
            }

        },
        messages: {
            Nome: {
                required: "*NOME é obrigatório"
            }
            ,Identificador: {
                required: "*IDENTIFICADOR é obrigatório"
                , min : "IDENTIFICADOR deve ser maior que ZERO"
            }
             
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });
    //Retorna de válido ou não
    return $("#formStatus").valid();

}
