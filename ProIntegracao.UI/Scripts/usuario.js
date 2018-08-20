//LOAD
$(document).ready(function () {

    //Listar Tabela de Usuario
    ListarUsuario();

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13){
            $('#buscaUsuario').click();
            return false;
        }
    });

    // Click Boãto Busca
    $("#buscaUsuario").click(function () {
        ListarUsuario();
    });

    // Somente letras maiusculas e minusculas
    $(".maiusculo").keyup(function () {
        var obj = $("#Login").val();
        $("#Login").val(obj.toUpperCase());
    });

    // Limpar Busca
    $("#limparUsuario").click(function () {
        $("#termo").val("");
    });

});

//Validação prévia Salvar Usuario
function ValidarSalvarUsuario() {

    var id = $("#Id").val();

    var result = false;

    if (id == "0") {
        result = ValidarFormulario();
    } else {
        result = ValidarFormularioEdicao();
    }
    if (result) {
        SalvarUsuario();
    }
}

//Novo Usuario
function NovoUsuario() {

    
    var url = "/Usuario/Create";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , cache : false
        , success: function (data) {
            AbrirModal('Novo Usuário', data);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    })
}

//Novo Usuario
function EditarUsuario(idUsuario) {
    var url = "/Usuario/Edit";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , cache: false
        , data: { id: idUsuario }
        , success: function (data) {
            AbrirModal('Atualizar Usuário', data);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    })
}

// Salvar Usuario
function SalvarUsuario() {

    var id = $("#Id").val();

    var result = false;

    var ativo = $("#ativo").prop('checked');
    

    if (id == "0") {
        result = ValidarFormulario();
    } else {
        result = ValidarFormularioEdicao();
    }
    if (result) {

            var UsuarioViewModel = 
            {
                 Id                 : $("#Id").val()
                , Login             : $("#Login").val()
                , Email             : $("#Email").val()
                , IdPerfil          : $("#IdPerfil option:selected").val()
                , DtCadastro        : $("#DtCadastro").val()
                , listaPerfis       : null
                , listaBloqueado    : null
            }

            if (!ativo && ativo != undefined) {
                UsuarioViewModel.DtExclusao = RetornaDataNowToString();
            }

                
            if ($("#bloqueado").length > 0) {
                var bloqueado = $("#bloqueado").prop('checked');
                UsuarioViewModel.Bloqueado = bloqueado;
            }

            var url = "/Usuario/Create";

            if ($("#Id").val() != "" && $("#Id").val() != "0") {
                url = "/Usuario/Edit";
            }

            $.ajax({
                url: url
                , data: { model: UsuarioViewModel}
                , datatype: "json"
                , type: "POST"
                , cache: false
                , beforeSend: function () {
                     waitingDialog.show();
                }
                , complete: function () {
                        waitingDialog.hide();
                }
                , success: function (data) {
                    if (data.Resultado){
                        FecharModalExcluir("myModalContent");
                        Message('Dados Salvos com Sucesso', 'sucesso');
                    } else {
                        Message('Erro ao gravar registro, verifique os dados e tente novamente', 'erro');
                    }
                    ListarUsuario();
                    waitingDialog.hide();
                    
                }
                , error: function (jqXHR, exception) {
                    TratamendodeErro(jqXHR, exception);
                    waitingDialog.hide();
                }
            });
    }
}

// Valida o Formulário de Edição e Novo Usuário
function ValidarFormulario()
{
    // Método que não permite caracter especial
    $.validator.addMethod("alpha", function (value, element) {
        return this.optional(element) || /^[A-Za-z0-9]+$/.test(value)
    }, "Não são permitidos caracteres especiais");

    //Efetua a Validação 
    $("#formUsuario").validate({
        debug: true,
        rules: {
            Login: {
                required: true
                , maxlength: 50
                , remote: {
                    url: "/Usuario/VerificaUsuario"
                    , type: "POST"
                    , cache: false
                    , data: { 
                        Login : function () {
                            return $("#Login").val();
                        }
                    }
                    , async: false
                },
                alpha : true
            }
            , Email: {
                required: true
                , email: true
            }
        },
        messages: {
            Login: {
                required: "*LOGIN é obrigatório"
            }
            , Email: {
                required: "*E-MAIL é obrigatório"
                , email : "*E-mail inválido"
            }
        },
        highlight: function(element) {
                $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function(element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function(error, element) {
            if(element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    //Retorna de válido ou não
    return $("#formUsuario").valid();
}

// Valida o Formulário de Edição e Novo Usuário
function ValidarFormularioEdicao() {

    // Método que não permite caracter especial
    $.validator.addMethod("alpha", function (value, element) {
        return this.optional(element) || /^[A-Za-z0-9]+$/.test(value)
    }, "Não são permitidos caracteres especiais");

    //Efetua a Validação 
    $("#formUsuario").validate({
        debug: true,
        rules: {
            Login: {
                required: true
                , maxlength: 50
                , alpha : true
            }
            , Email: {
                required: true
                , email: true
            }
        },
        messages: {
            Login: {
                required: "*LOGIN é obrigatório"
            }
            , Email: {
                required: "*LOGIN é obrigatório"
                , email : "*Email inválido"
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
    return $("#formUsuario").valid();

}

//Listar Usuario por Termo e Busca
function ListarUsuario() {
    
    var termo       = $("#termo").val();
    var ativo       = $("#ativolistar").is(":checked");
    var url         = "/Usuario/ListarUsuarios";
    var div         = $("#listaUsuario");

    //Limpar DIV
    div.empty();

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , cache: false
        , data: { termo: termo , ativo : ativo}
        , beforeSend: function () { }
        , success: function (data) {
            div.html(data);
            AplicarDataTable('usuarios');

            if (!ativo)
                $("#buttonExcluir").hide();

        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Alterar Senha
function AlterarSenha()
{
    if(ValidarAlteracaoSenha())
    {
        var senhaAtual = $("#SenhaAtual").val();
        var novaSenha = $("#NovaSenha").val();
        var url = "/Usuario/AlterarSenha";

        $.ajax({
            url: url
            , datatype: "json"
            , type: "POST"
            , cache : false
            , beforeSend: function () { waitingDialog.show(); }
            , complete: function () { waitingDialog.hide(); }
            , data: {senhaAtual : senhaAtual, novaSenha : novaSenha}
            , success: function (data) {
                if (data.Resultado) {
                    Message('Dados Salvos com Sucesso', 'sucesso');
                    window.location = "/Home/Index";
                } else {
                    Message('Erro ao gravar registro, verifique os dados e tente novamente', 'erro');
                }
                waitingDialog.hide();
            }
            , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception);
            }
        });



    }
}

//Validar Alteração de Senha
function ValidarAlteracaoSenha()
{

    $.validator.setDefaults({
        debug: true,
        success: "valid"
    });
    
    $("#formTrocarSenha").validate({
        rules: {
            SenhaAtual: {
                required:true
            }
            , NovaSenha: {
                required : true
            }
            ,ConfirmarSenha: {
                equalTo:"#NovaSenha"
            }
        },
        messages: {
            SenhaAtual: {
                required: "*SENHA ATUAL é obrigatório"
            }
            , NovaSenha: {
                required: "*NOVA SENHA é obrigatório"
            }
            , ConfirmarSenha: {
                equalTo: "*CONFIRMAR SENHA não confere"
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
    
    //Retorno validação!
    return $("#formTrocarSenha").valid();
}