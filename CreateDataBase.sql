



CREATE PROCEDURE SelectAllCustomers @City nvarchar(30), @PostalCode nvarchar(10)
AS


if db_id('INTEGRADOR') is not null
CREATE DATABASE INTEGRADOR

--use integrador
--


create table TbUsuario(
IdUsuario int identity(1,1),
Login varchar(30),
Senha varchar(30),
Email varchar(60),
Bloqueado bit,
Hash varchar(255),
DtExclusao datetime,
DtCadastro datetime
);


create table TbEstado(
IdEstado int identity(1,1),
Uf varchar(10),
Nome varchar(20)
)

create table TbMenu(
IdMenu int identity(1,1),
Nome varchar(60),
Ordem varchar(60),
Admin varchar(60),
Url varchar(60),
DtCadastro varchar(60),
DtExclusao varchar(60),
IdMenuPai int

)

create table TbPagina(
IdMenu int identity(1,1),

IdEstado int,
Url varchar(60),
Ordem int,
Nome varchar(60), 
Icone varchar(60),

DtCadastro varchar(60),
DtExclusao varchar(60)

)

create table TbPerfilEstado(
IdPerfilEstado int identity(1,1),
IdEstado int, 
IdPerfil int

)


create table TbPerfil(
IdPerfil int identity(1,1),

Nome varchar(60),
Admin bit,
DtCadastro varchar(60),
DtExclusao varchar(60)
)

create table TbPerfilPagina(
IdPerfilPagina int identity(1,1),
IdPagina int, 
IdPerfil int, 
Inserir bit,
Atualizar bit, 
Excluir bit,
Consultar bit


)


create table TB_REGIAO(
ID_REGIAO int identity(1,1),
DESCRICAO varchar(255) 
)


create table TB_UF(
ID_UF int identity(1,1),
NOME varchar(60),
SIGLA varchar(30),
ID_REGIAO int,
DIFERENCA_HORARIO int,
DATA_EXIGIBILIDADE datetime,



)

create table TbAluno(
IdAluno int identity(1,1),
Nome varchar(60),
CpfAluno varchar(60),
Renach varchar(60),
DtNascimento datetime,
IdSexo int,
DtCadastro varchar(60),
DtExclusao varchar(60)
)


create table TbAula(
IdAula int identity(1,1),
IdMatricula int,
IdStatusSituacaoAula int,
CodigoCfc varchar(60),
IdentificadorAula varchar(60),
CpfInstrutor varchar(60),
DataInicioAula datetime,
DataFimAula datetime,
TokenInicioAula varchar(60),
TokenFimAula varchar(60),
DtCadastro varchar(60),
DtExclusao varchar(60)
)


create table TbMatricula(
IdMatricula int identity(1,1),
IdAluno int,
IdEstado int,
QtdAula int,
CodigoCfc int,
HoraAula varchar(30),
PSA varchar(60),
DtCadastro varchar(60),
DtExclusao varchar(60)
)


create table TbSexo(
IdSexo int identity(1,1),
Nome varchar(60)
)


create table TbStatusSituacaoAula(
IdStatusSituacaoAula int identity(1,1),
Nome varchar(60),
Identificador int,
IdEstado int,
DtCadastro varchar(60),
DtExclusao varchar(60)

)

create table TbForcarErro(
IdForcarErro int identity(1,1),
IdAluno int,
IdTipoErro int,
DtCadastro datetime

)

create table TbErro(
IdErro int identity(1,1),
Nome varchar(60),
Descricao varchar(255),
 IdEstado int,
CodigoErro varchar(60),
DtCadastro datetime
)



GO;


--You can use the various metadata functions.
--if db_id('dbname') is not null
--if object_id('object_name', 'U') is not null -- for table
--if object_id('object_name', 'P') is not null -- for SP

