//LOAD
$(document).ready(function () {

    $(".fa-pencil").on("mousedown", function (event) {
        event.preventDefault();
        return false;
    });

    $(".fa-pencil").on("click", function (event) {
        var idMenu = this.getAttribute('data-id');
        EditarMenu(idMenu);
        event.preventDefault();
        return false;
    });

    $(".fa-trash").on("mousedown", function (event) {
        event.preventDefault();
        return false;
    });

    $(".fa-trash").on("click", function (event) {
        var idMenu = this.getAttribute('data-id');
        AbrirModalExcluir(idMenu, "Menu");
        event.preventDefault();
        return false;
    });

});

// Listar Menu Edição
function ListarMenuEdicao()
{
    var url = "/Menu/ListarMenuEdicao";
    var div = $("#listamenu");

    $.ajax({
        url: url
        , type: "GET"
        , datatype: "json"
        , success: function (data) {
            div.empty();
            div.html(data.Resultado);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Editar Menu
function EditarMenu(id) {

    var url = "/Menu/Edit";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , data: { id: id }
        , success: function (data) {
            AbrirModal('Atualizar Menu', data);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });

}

// Nova Página
function NovoMenu() {

    var url = "/Menu/Create";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , success: function (data) {
            AbrirModal('Novo Menu', data);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Editar Menu
function EditarMenu(idMenu)
{
    var url = "/Menu/Edit";
    $.ajax({
        url: url
        , datatype: "html"
        , type: "GET"
        , data: { idMenu: idMenu }
        , success: function (data) {
            AbrirModal('Editar Menu', data);
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });


}

/// Retorna Children
function RetornaChildren(item)
{
    if (item.children != null || item.children != undefined)
    {
        var array = [];

        if (item.children.length > 0) {
            $.each(item.children, function (ind, it) {
                var children = {
                    id: it['id']
                    , tipo : it['tipo']
                    , children:RetornaChildren(it)
                    }
                array.push(children);
            });
            return array;
       }
    }
}

// Atualizar MENU
function AtualizarMenu()
{
    var url = "/Menu/Update";
    
    var menuArray = $('#nestable').nestable("serialize");

    var MenuNestead = [];

    if (menuArray.length > 0) {
        $.each(menuArray, function (indice, item) {
            var menu = {
                id: item['id']
                , tipo : item['tipo']
                , children: RetornaChildren(item)
            }
            MenuNestead.push(menu);
        });
    }

    var MenuViewModel =
    {
        MenuNestead: MenuNestead
    }

    $.ajax({
        url: url
            , data: { model: MenuViewModel }
            , datatype: "json"
            , type: "POST"
            , success: function (data) {

                if (data.Resultado) {
                    Message('Dados Salvos com Sucesso', 'sucesso');
                }
                else {
                    Message('Erro ao gravar registro, verifique os dados e tente novamente', 'erro');
                }

                RetornaMenuPrincipal();
                
            }
            , error: function (jqXHR, exception) {
                TratamendodeErro(jqXHR, exception);
            }
    });


}

// Salvar Página
function SalvarMenu() {

    if (ValidarFormulario()) {

        var ativo = $("#ativo").prop('checked');

        var MenuViewModel =
        {
            Id                  :   $("#Id").val()
            , IdMenuPai         :   $("#IdMenuPai option:selected").val()
            , Url               :   $("#Url").val()
            , Nome              :   $("#Nome").val()
            , Ordem             :   $("#Ordem").val()
            , DtCadastro        :   $("#DtCadastro").val()
        }

        if (!ativo) {
            MenuViewModel.DtExclusao = RetornaDataNowToString();
        } else {
            MenuViewModel.DtExclusao = null;
        }

        var url = "/Menu/Create";

        if ($("#Id").val() != "" && $("#Id").val() != "0") {
            url = "/Menu/Edit";
        }

        $.ajax({
            url: url
            , data: { model: MenuViewModel }
            , datatype: "json"
            , type: "POST"
            , success: function (data) {
                if (data.Resultado) {
                    FecharModalExcluir("myModalContent");
                    Message('Dados Salvos com Sucesso', 'sucesso');
                    RetornaMenuPrincipal();
                    window.location = "/Menu";
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

    var id = $("#Id").val();

    // Método que não permite caracter especial
    $.validator.addMethod("home", function (value, element) {
        return !(value.toUpperCase() == "HOME" && id == "0");
    }, "O Nome HOME não pode ser utilizado");

    //Efetua a Validação 
    $("#formMenu").validate({
        debug: true,
        rules: {
            Nome: {
                required: true
                , maxlength: 255
                , home: true
            }
        },
        messages: {
            Nome: {
                required: "*NOME é obrigatório"
                , maxlength: "*NOME deve ser menor que 255"
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
    return $("#formMenu").valid();


}