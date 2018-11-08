//LOAD
$(document).ready(function () {

    //Listar Tabela de Usuario
    ListarMatricula();

    // Click Boãto Busca
    $("#buscaMatricula").on("click", function () {
        var termo = $("#doc").val();
        ListarMatricula(termo);
    });

    ///Salvar Matricula CLICK
    $("#salvarMatricula").click(function () {
        var id = $("#Id").val();
        var result = false;
        result = ValidarFormulario();
        if (result) {
            SalvarMatricula();
        }
    });

    // Limpar Busca
    $("#limparMatricula").on("click", function () {
        LimparMatricula();
    });

    // Button Listar DEFAULT
    $(document).keypress(function (e) {
        var key = e.which;
        if (key === 13) {
            $('#buscaMatricula').click();
            return false;
        }
    });

    
});

// Limpar Busca Matricula
function LimparMatricula() {
    $("#doc").val("");
}

//Listar Usuario por Termo e Busca
function ListarMatricula(termo) {

    var url = "/Matricula/ListarMatriculas";

    var div = $("#listaMatricula");

    div.empty();

    $.ajax({
        url: url
        , datatype: 'html'
        , type: 'POST'
        , data: { termo: termo }
        , success: function (data) {
            div.html(data);
            AplicarDataTable('matriculas');
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });


}

// Editar Matricula
function EditarMatricula(Id) {

    var url = "/Matricula/Edit";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , data: { Id: Id }
        , success: function (data) {
            AbrirModal('Atualizar Matrícula', data);
            AplicarMascara();
            FocusNomeAluno();
        }
         , error: function (jqXHR, exception) {
             TratamendodeErro(jqXHR, exception);
         }
    });
}

// Nova Matricula
function NovaMatricula() {

    var url = "/Matricula/Create";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , success: function (data) {
            AbrirModal('Nova Matrícula', data);
            AplicarMascara();
            FocusNomeAluno();
        }
        , error: function (jqXHR, exception) {
            TratamendodeErro(jqXHR, exception);
        }
    });
}

//Foco em Nome Aluno
function FocusNomeAluno()
{
    $("#Cpf").focusout(function () {

        var cpf = RetornaCPFSemPontos($("#Cpf").val());

        if (cpf !== "") {
            var url = "/Aluno/RetornaNomePorCpf";
            cpf = cpf.replace(".", "").replace(".", "").replace("-", "");
            $.ajax({
                url: url
                , data: { cpf: cpf }
                , datatype: "json"
                , type: "GET"
                , success: function (data) {
                    $("#NomeAluno").val(data.NomeAluno);
                    $("#IdAluno").val(data.IdAluno);
                    if (data.IdAluno === "0" || data.IdAluno === 0)
                        Message("Nenhum registro encontrado", "aviso");

                }
                , error: function (jqXHR, exception) {
                    TratamendodeErro(jqXHR, exception);
                }
            });
        }
    });
}

// Salvar Matricula
function SalvarMatricula() {

    if (ValidarFormulario()) {

        var MatriculaViewModel = {
            Id              : $("#Id").val()
            , DtCadastro    : $("#DtCadastro").val()
            , DtExclusao    : $("#DtExclusao").val()
            , IdAluno       : $("#IdAluno").val()
            , CodigoCfc     : $("#CodigoCfc").val()
            , QtdAula       : $("#QtdAula").val()
            , IdEstado      : $("#IdEstado option:selected").val()
            , HoraAula      : $("#HoraAula").val()
            , PSA           : $("#PSA").val().toUpperCase()
       };

        var url = "/Matricula/Create";

        if ($("#Id").val() !== "" && $("#Id").val() !== "0") {
            url = "/Matricula/Edit";
        }

        $.ajax({
            url: url
            , data: { model: MatriculaViewModel }
            , datatype: "json"
            , type: "POST"
            , success: function (data) {
                if (data.Resultado) {
                    Message('Dados Salvos com Sucesso', 'sucesso');
                    FecharModalExcluir('myModalContent');
                }
                else {
                    Message('Erro ao gravar registro, verifique os dados e tente novamente', 'erro');
                }
                LimparMatricula();
                ListarMatricula();
            }
            ,error: function(jqXHR, exception){
                TratamendodeErro(jqXHR, exception);
            }
        });
    }
}

// Validar Formulario
function ValidarFormulario() {

    jQuery.validator.addMethod("notEqual", function (value, element, param) {
        return this.optional(element) || value !== "00:00";
    }, "*Hora Aula deve ser maior que ZERO");

    jQuery.validator.addMethod("cpfvalido", function (value, element, param) {
        return validarCPF(value);
    },"*CPF Inválido");

    $("#formMatricula").validate({
        rules: {
            Cpf: {
                required: true
                , cpfvalido:true
            }
            , QtdAula: {
                required: true
                , min: 1
            }
            , HoraAula: {
                required: true
                , notEqual: "00:00"
            }
            , PSA: {
                required: true
            },
            CodigoCfc :{
                required: true
                , min : 1
            }
        },
        messages: {
            Cpf: {
                required: "*CPF é obrigatório"
                
            }
            ,QtdAula: {
                required: "*QUANTIDADE DE AULAS é obrigatório"
               , min: "*Qtd. Aulas deve ser maior que ZERO."
            }
            , HoraAula: {
                required: "*HORA DA AULA é obrigatório"
            }
            , PSA: {
                required: "*PSA é obrigatório"
            },
            CodigoCfc: {
                required: "*Código CFC é obrigatório"
                , min: "*Código CFC é obrigatório"
            }
        }
        , highlight: function (element) {
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

    return $("#formMatricula").valid();
}

function dois_pontos(tempo) {
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.returnValue = false;
    }

    if (tempo.value.length === 2 || tempo.value.length === 5) {
        tempo.value += ":";
    }
}

function valida_horas(tempo) {

    horario = tempo.value.split(":");

    var horas = horario[0];

    var minutos = horario[1];

    var segundos = horario[2];

    if (horas > 24) { //para relógio de 12 horas altere o valor aqui

        Message("Horas inválidas", erro); event.returnValue = false; relogio.focus();
    }

    if (minutos > 59) {

        Message("MINUTOS inválidos"); event.returnValue = false; relogio.focus();
    }

    if (segundos > 59) {

        Message("Segundos inválidos"); event.returnValue = false; relogio.focus();
    }
}

function dataAtualFormatada() {
    var data = new Date();
    var dia = data.getDate();
    if (dia.toString().length === 1)
        dia = "0" + dia;
    var mes = data.getMonth() + 1;
    if (mes.toString().length === 1)
        mes = "0" + mes;
    var ano = data.getFullYear();
    return dia + "/" + mes + "/" + ano;
}

function soLetras(obj) {
    var tecla = window.event? event.keyCode : obj.which;
    if (tecla > 65 && tecla < 90 || tecla > 97 && tecla < 122)
        return true;
    else {
        if (tecla !== 8) return false;
        else return true;
    }
}

function remove(str, sub) {
    i = str.indexOf(sub);
    r = "";
    if (i === -1) return str;
    {
        r += str.substring(0, i) + remove(str.substring(i + sub.length), sub);
    }

    return r;
}

function mascara(o, f) {
    v_obj = o;
    v_fun = f;
    setTimeout("execmascara()", 1);
}

function execmascara() {
    v_obj.value = v_fun(v_obj.value);
}

function cpf_mask(v) {
    v = v.replace(/\D/g, "");        //Remove tudo o que não é dígito
    v = v.replace(/(\d{3})(\d)/, "$1.$2");  //Coloca ponto entre o terceiro e o quarto dígitos
    v = v.replace(/(\d{3})(\d)/, "$1.$2");   //Coloca ponto entre o setimo e o oitava dígitos
    v = v.replace(/(\d{3})(\d)/, "$1-$2");  //Coloca ponto entre o decimoprimeiro e o decimosegundo dígitos
    return v;
}

