//LOAD
$(document).ready(function () {

    AplicarMascara();
    //Listar Páginas
    ListarForcarErro();
    //Click Boãto Busca
    Cpf: $('#Cpf').val();

    $("#buscaForcarErro").click(function () {
        ListarForcarErro();
    });

    // Click do Botão Salvar Página
    $("#salvarForcarErro").click(function () {
        var result = false;
        result = ValidarFormulario();
        if (result) {
            SalvarForcarErro();
        }
    });

});

function ListaForcarErros() {

    var termo = $("#termo").val();

    var url = "/ForcarErro/ListarForcarErro";
    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: { termo: termo }
        , beforeSend: function () {
            $("#loading").show();
        }
        , complete: function () {
            $("#loading").hide();
        }
        , success: function (data) {
            $("#loading").hide();
        }
        , beforeSend: function () {
            $("#loading").show();
        }
        , complete: function () {
            $("#loading").hide();
        }
        , error: function (jqXHR, exception) {
            var msg = '';
            var tipo = '';
            if (jqXHR.status === 0) {
                msg = 'Sem Conexão.\n Verifique rede.';
            } else if (jqXHR.status == 404) {
                msg = 'Página não encontrada. [404]';
            } else if (jqXHR.status == 500) {
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

            Message(msg, 'erro');

            $("#loading").hide();
        }
    })
}


// Editar ForcarErro
function EditarForcarErro(model) {
    var url = "/ForcarErro/Excluir";



    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , data: { idPerfil: idPerfil }
        , success: function (data) {
            AbrirModal('Excluir Erro', data);
        }
        , beforeSend: function () {
            $("#loading").show();
        }
        , complete: function () {
            $("#loading").hide();
        }
        , error: function (jqXHR, exception) {
            var msg = '';
            var tipo = '';
            if (jqXHR.status === 0) {
                msg = 'Sem Conexão.\n Verifique rede.';
            } else if (jqXHR.status == 404) {
                msg = 'Página não encontrada. [404]';
            } else if (jqXHR.status == 500) {
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
            Message(msg, 'erro');
        }
    });
}

// Nova Página
function NovoForcarErro() {



    var url = "/ForcarErro/Create";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , success: function (data) {
            AbrirModal('Novo Erro', data);
            AplicarMascara();

        }
        , error: function (jqXHR, exception) {
            var msg = '';
            var tipo = '';
            if (jqXHR.status === 0) {
                msg = 'Sem Conexão.\n Verifique rede.';
            } else if (jqXHR.status == 404) {
                msg = 'Página não encontrada. [404]';
            } else if (jqXHR.status == 500) {
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
            Message(msg, 'erro');
        }
    });
}

// Listar ForcarErro
function ListarForcarErro() {


    var termo = $("#termo").val().replace(".", "").replace(".", "").replace("-", "");

    var url = "/ForcarErro/ListarForcarErro";

    var div = $("#listaForcarErro");

    div.empty();

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: { termo: termo }
        , beforeSend: function () { }
        , success: function (data) {
            div.html(data);
            AplicarDataTable('ForcarErro');
        }
        , error: function (jqXHR, exception) {
            var msg = '';
            var tipo = '';
            if (jqXHR.status === 0) {
                msg = 'Sem Conexão.\n Verifique rede.';
            } else if (jqXHR.status == 404) {
                msg = 'Página não encontrada. [404]';
            } else if (jqXHR.status == 500) {
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
            Message(msg, 'erro');
        }
    });

}

// Salvar Página
function SalvarForcarErro() {

    AplicarMascara();

    if (ValidarFormulario()) {

        var ForcarErroViewModel =
        {
            Cpf: $('#Cpf').val().replace(".", "").replace(".", "").replace("-", "")
            , IdTipoErro: $("#IdTipoErro option:selected").val()

        }

        var url = "/ForcarErro/Create";

        $.ajax({
            url: url
            , data: { model: ForcarErroViewModel }
            , datatype: "json"
            , type: "POST"
            , success: function (data) {

                if (data.Resultado) {
                    FecharModalExcluir("myModalContent");
                    Message('Dados Salvos com Sucesso', 'sucesso');
                    ListarForcarErro();
                }
                else {
                    Message('Erro ao gravar registro, verifique os dados e tente novamente', 'erro');
                }
                $("#loading").hide();
            }
            , beforeSend: function () {
                $("#loading").show();
            }
            , complete: function () {
                $("#loading").hide();
            }
            , error: function (jqXHR, exception) {
                var msg = '';
                var tipo = '';
                if (jqXHR.status === 0) {
                    msg = 'Sem Conexão.\n Verifique rede.';
                } else if (jqXHR.status == 404) {
                    msg = 'Página não encontrada. [404]';
                } else if (jqXHR.status == 500) {
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
                Message(msg, 'erro');
                $("#loading").hide();
            },
        });
    }
}

// Validar Formulario
function ValidarFormulario() {

    //Efetua a Validação 
    $("#formForcarErro").validate({
        debug: true,
        rules: {
            Cpf: {
                required: true
                , maxlength: 255
                , remote: {
                    url: "/Matricula/ValidaCpfExistente"
                    , type: 'POST'
                    , data: {
                        Cpf: function () {
                            return $("#Cpf").val();
                        }
                    }
                    , async: true
                }
            }
            , Url: {
                required: true
                , maxlength: 255
            }
        },
        messages: {
            Nome: {
                required: "*Campo NOME é obrigatório"
                , maxlength: "*Campo NOME deve ser menor que 255"
            }

            , Url: {
                required: "*Campo URL é obrigatório"
                , maxlength: "*Campo URL deve ser menor que 255"
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
    return $("#formForcarErro").valid();


}
