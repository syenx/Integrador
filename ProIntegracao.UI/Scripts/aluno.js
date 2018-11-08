//LOAD
$(document).ready(function () {

    ListarAlunos();
    AplicarMascara();

    $("#buscaAluno").click(function () {
        var cpf = $("#cpf").val();
         if (cpf === "") {
             ListarAlunos();
        }
        else {
            AplicarMascara();
            ListarAlunos(cpf);

        }
    });

    // Limpar Busca
    $("#limparAluno").click(function () {
        $("#cpf").val("");
        ListarAlunos();
        AplicarMascara();
    });

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key === 13) {
            $('#buscaAluno').click();
            return false;
        }
    });

});

//Listar Alunos
function ListarAlunos(cpf) {
    
    cpf = RetornaCPFSemPontos($("#cpf").val());

    var url = "/Aluno/ListarAlunos";
    var div = $("#ListarAluno");

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , cache: false
        , data: { cpf: cpf }
        , success: function (data) {
            div.empty();
            div.html(data);
            AplicarDataTable('alunos');

            waitingDialog.hide();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Novo AlunoIntegracao
function NovoAluno() {

    var url = "/Aluno/Create";

  

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , success: function (data) {
            AbrirModal('Novo Aluno', data);

            AplicarMascara();
            AplicarDatePicker();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

// Salvar AlunoIntegracao
function SalvarAluno() {

    if (ValidarFormulario()) {

        var AlunoViewModel =
        {
            Id: $("#Id").val()
            , CpfAluno: $('#CpfAluno').val().replace(".", "").replace(".", "").replace("-", "")
            , Nome: $("#Nome").val()
            , Renach: $("#Renach").val()
            , IdSexo: $("#Sexo").val()
            , DtNascimento: $("#DtNascimento").val()
            , ListaAluno: null
            , DtCadastro: $("#DtCadastro").val()

        };

        
        var masculino = $("#masculino").is(':checked');

        if (masculino) {
            AlunoViewModel.IdSexo = 1;
        } else {
            AlunoViewModel.IdSexo = 2;
        }


        var url = "/Aluno/Create";

        if ($("#Id").val() !== "0") {
            url = "/Aluno/Edit";
        }

        $.ajax({
            url: url
            , data: { AlunoViewModel: AlunoViewModel }
            , datatype: "json"
            , type: "POST"
            , cache: false
            , success: function (data) {
                if (data.Resultado) {
                    FecharModalExcluir("myModalContent");
                    ListarAlunos();
                    Message('Dados Salvos com Sucesso', 'sucesso');
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

    jQuery.validator.addMethod("dateBR", function (value, element, param) {
        return this.optional(element) || /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/.test(value);
    }, "*Data de Nascimento inválida.");




    jQuery.validator.addMethod("cpf", function (value, element) {
        value = jQuery.trim(value);

        value = value.replace('.', '');
        value = value.replace('.', '');
        cpf = value.replace('-', '');
        while (cpf.length < 11) cpf = "0" + cpf;
        var expReg = /^0+$|^1+$|^2+$|^3+$|^4+$|^5+$|^6+$|^7+$|^8+$|^9+$/;
        var a = [];
        var b = new Number;
        var c = 11;
        for (i = 0; i < 11; i++) {
            a[i] = cpf.charAt(i);
            if (i < 9) b += a[i] * --c;
        }
        if ((x = b % 11) < 2) { a[9] = 0; } else { a[9] = 11 - x; }
        b = 0;
        c = 11;
        for (y = 0; y < 10; y++) b += a[y] * c--;
        if ((x = b % 11) < 2) { a[10] = 0; } else { a[10] = 11 - x; }

        var retorno = true;
        if (cpf.charAt(9) !== a[9] || cpf.charAt(10) !== a[10] || cpf.match(expReg)) retorno = false;

        return this.optional(element) || retorno;

    }, "*CPF inválido");


    //Efetua a Validação 
    $("#formAluno").validate({
        rules: {
            CpfAluno: {
                required: true
                , cpf: true
                , remote: {
                    url: "/Matricula/ValidaCpfExistente"
                    , type: 'POST'
                    , data: {
                        cpf: function () {
                            return $("#CpfAluno").val();
                        }, Id: function () {
                            return $("#Id").val();
                        }
                    }
                    , async: false
                }
            }
            , Nome: {
                required: true
                , maxlength: 255
            }
            , Renach: {
                required: true
                , maxlength: 11
            }
            , DtNascimento: {
                required: true
                ,dateBR : true
            }
            , sexo: {
                required : true
            }

        },
        messages: {
            CpfAluno: {
                required: "*CPF é obrigatório."
            }
            , Nome: {
                required: "*NOME é obrigatório."
                , maxlength: "*NOME deve ser menor que 255 caracteres."
            }

            , Renach: {
                required: "*RENACH é obrigatório."
                , maxlength: "*RENACH deve ser menor que 11."
            }
            , sexo: {
                required: "*SEXO é requerido."
            }
            , DtNascimento: {
                required: "*Data Nascimento é obrigatório"
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
    return $("#formAluno").valid();
}

// Editar AlunoIntegracao
function EditarAluno(Id) {

    var url = "/Aluno/Edit";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , cache : false
        , data: { Id: Id }
        , success: function (data) {
            AbrirModal('Editar Aluno', data);
            AplicarMascara();
            AplicarDatePicker();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

