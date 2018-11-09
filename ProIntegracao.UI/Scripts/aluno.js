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


// Editar AlunoIntegracao
function EditarAluno(Id) {

    var url = "/Aluno/Edit";

    $.ajax({
        url: url
        , datatype: "html"
        , type: 'GET'
        , cache: false
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

