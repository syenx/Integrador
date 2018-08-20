/// <reference path="Choosen/chosen.jquery.js" />
//LOAD
$(document).ready(function () {

    //Listar Páginas
    ListarPaginas();

    // Click Boãto Busca
    $("#buscaPagina").click(function () {
        ListarPaginas();
    });

    // Click do Botão Salvar Página
    $("#salvarPagina").click(function () {
        var result = false;
        result = ValidarFormulario();
        if (result) {
            SalvarPagina();
        }
    });

    $("#limparPagina").click(function () {
        $("#IdMenu")[0].selectedIndex = 0;
        $("#termo").val("");
    });

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key == 13) {
            $('#buscaPagina').click();
            return false;
        }
    });


    $('#tallModal').on('shown.bs.modal', function () {
        $('#tallModal').focus()
    });


    $('#tallModal').on('hidden.bs.modal', function () {
        $('#myModal').focus()
    });

});

// Seleciona Todos
function SelecionaTodos()
{
    var result = $("#todosEstados").prop("checked");

    if (result) {
        $("#IdEstado").find("option").each(function () {
            $(this).prop('selected', true);
        });
    }
    else
    {
        $('option').prop('selected', false);
    }

    $('#IdEstado').trigger('chosen:updated');
}

// Editar Pagina
function EditarPagina(idPerfil)
{
    var url = "/Pagina/Edit";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , data: { idPerfil: idPerfil }
        , success: function (data) {
            AbrirModal('Atualizar Página', data);
            $('#myModalContent').on('shown.bs.modal', function () {
                var config = {
                    '.chosen-select': {},
                    '.chosen-select-deselect': { allow_single_deselect: true },
                    '.chosen-select-no-single': { disable_search_threshold: 10 },
                    '.chosen-select-no-results': { no_results_text: 'Nenhum registro encontrado!' },
                    '.chosen-select-width': { width: "95%" },
                }
                for (var selector in config) {
                    $(selector).chosen(config[selector]);
                }
            });
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Nova Página
function NovaPagina() {

    var url = "/Pagina/Create";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , success: function (data) {
            AbrirModal('Nova Página', data);

            $('#myModalContent').on('shown.bs.modal', function () {
                var config = {
                    '.chosen-select': {
                        placeholder_text_multiple : "Escolha um ESTADO"
                    },
                    '.chosen-select-deselect': { allow_single_deselect: true },
                    '.chosen-select-no-single': { disable_search_threshold: 10 },
                    '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
                    '.chosen-select-width': { width: "95%" },
                }
                for (var selector in config) {
                    $(selector).chosen(config[selector]);
                }

            });
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Listar Paginas
function ListarPaginas()
{
    var menu = $("#IdMenu").val();

    var termo = $("#termo").val();

    var ativo = $("#ativolistar").is(':checked');

    var url = "/Pagina/ListarPaginas";

    var div = $("#listaPagina");

    div.empty();

    if (menu == "") {
        menu = 0;
    }


    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: { termo: termo, menu : menu, ativo : ativo }
        , beforeSend: function () { }
        , success: function (data) {
            div.html(data);
            AplicarDataTable('paginas');
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });

}

// Salvar Página
function SalvarPagina() {
    
    if (ValidarFormulario()) {

        var ativo = $("#ativop").prop('checked');

        var PaginaViewModel =
        {
            Id: $("#Id").val()
            , IdMenu: $("#Menus option:selected").val()
            , IdEstado: $("#IdEstado").val()
            , Url: $("#Url").val()
            , Nome: $("#Nome").val()
            , Icone: $("#Icone").val()
            , Ordem: $("#Ordem").val()
            , DtCadastro: $("#DtCadastro").val()
            , DtExclusao: $("#DtExclusao").val()
            , listaMenus: null
        }

        if (!ativo) {
            PaginaViewModel.DtExclusao = RetornaDataNowToString();
        } else {
            PaginaViewModel.DtExclusao = null;
        }

        var url = "/Pagina/Create";

        if ($("#Id").val() != "" && $("#Id").val() != "0") {
            url = "/Pagina/Edit";
        }

        $.ajax({
            url: url
            , data: { model: PaginaViewModel }
            , datatype: "json"
            , type: "POST"
            , success: function (data) {
                
                if (data.Resultado) {
                    FecharModalExcluir("myModalContent");
                    Message('Dados Salvos com Sucesso', 'sucesso');
                    ListarPaginas();
                }
                else {
                    Message('Erro ao gravar registro, verifique os dados e tente novamente', 'erro');
                }
                
            }
            , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception);
            }
        });
    } 
}

// Validar Formulario
function ValidarFormulario() {

    // Método que não permite caracter especial
    $.validator.addMethod("multi", function (value, element) {
        return !(value == null);
    }, "ESTADO é obrigatório");

    //Efetua a Validação 
    $("#formPagina").validate({
        debug: true,
        ignore: [],
        rules: {
            Nome: {
                required: true
                , maxlength: 255
            }
            , Url: {
                required: true
                , maxlength: 255
            }
            , Ordem: {
                required: true
                , min : 1
            },
            IdEstado: {
                multi: true
            }
        },
        messages: {
            Nome: {
                required: "*NOME é obrigatório"
                , maxlength : "*NOME deve ser menor que 255"
            }

            , Url: {
                required: "*URL é obrigatório"
                , maxlength: "*URL deve ser menor que 255"
            }
            , Ordem : {
                required : "*ORDEM é obrigatório"
                , min : "*ORDEM deve ser maior que ZERO"
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
    return $("#formPagina").valid();
}
