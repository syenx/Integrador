USE [ProSimulador]
GO

/****** Object:  StoredProcedure [dbo].[pr_selecionar_situacao_aula]    Script Date: 30/05/2016 10:05:12 ******/
DROP PROCEDURE [dbo].[pr_selecionar_situacao_aula]
GO

/****** Object:  StoredProcedure [dbo].[pr_selecionar_situacao_aula]    Script Date: 30/05/2016 10:05:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ==============================================================
-- AUTHOR:		Fernando Parmezani
-- CREATE DATE: 2016/03/31
-- DESCRIPTION:	Selecionar Situação Aula
-- ==============================================================
-- exec pr_selecionar_situacao_aula '12944469673',0,'','','',0,0
-- exec pr_selecionar_situacao_aula '',0,'AC406533555','','',0,0

CREATE PROCEDURE [dbo].[pr_selecionar_situacao_aula]
	@CPF				VARCHAR(11) = '',
	@IDESTADO			INT = 0,
	@RENACH				VARCHAR(15) = '',
	@DTINICIO			VARCHAR(10) = '',
	@DTFIM				VARCHAR(10) = '',
	@IDSTATUS			INT = 0 ,
	@IDCURSO			INT = 0 
AS
BEGIN

	SET NOCOUNT ON;

	declare @DTINICIODT datetime;
	declare @DTFIMDT datetime;
	declare @SQL_Str nvarchar(4000);
	declare @Par_Def nvarchar(4000);

	if (@DTINICIO = '' AND @DTFIM = '')
	begin
		set @DTINICIODT = '1753-01-01 00:00:00.000'
		set @DTFIMDT = '9999-12-31 23:59:59.997'
	end
	else
	begin
		set @DTINICIODT	= convert(datetime, @DTINICIO, 103);
		set @DTFIMDT = convert(datetime, @DTFIM, 103);
	end


	set @Par_Def = '@CPF varchar(11),
					@IDESTADO int,
					@RENACH varchar(15),
					@DTINICIODT datetime,
					@DTFIMDT datetime,
					@IDSTATUS int,
					@IDCURSO int'


	set @SQL_Str = 
	'SELECT 
		U.SIGLA UF
		, E.CODIGO [CFC]
		, E.ID_EMPRESA
		, A.ID_AULA 
		, M.NOME [MUNICIPIO] 
		, E.CNPJ 
		, E.RAZAO_SOCIAL 
		, P.CPF 
		, p.NOME
		, CONVERT(VARCHAR(10),	P.DATA_NASCIMENTO,103) [DATA_NASCIMENTO] 
		, AL.NUMERO_HABILITACAO [RENACH]
		, CONVERT(VARCHAR(10),	AG.HORARIO_AULA ,103) [DATA_AULA]
		, CONVERT(VARCHAR(10),	AG.HORARIO_AULA ,108) [HORA_AULA]
		, CONVERT(VARCHAR(10),	A.HORARIO_INICIO,108) [HORARIO_INICIO]
		, CONVERT(VARCHAR(10),	A.HORARIO_FIM,108) [HORARIO_FIM]
		, SI.CODIGO_PRO [PSA]
		, S.[STATUS] 
		, AG.ID_AGENDA [AGENDA]
		, AG.ID_AGENDA_SEQUENCIA [SEQUENCIA]
		, CASE MA.TIPO_CURSO WHEN ''1'' THEN ''PH'' WHEN ''2'' THEN ''AV'' END AS ''TIPO_CURSO''
		, MO.NOME [MODELO] 
		, A.SESSION_ID 
	FROM TB_AULA A (nolock)
	INNER JOIN TB_AULA_STATUS S (nolock) ON S.ID_AULA_STATUS = A.ID_AULA_STATUS  
	INNER JOIN TB_AGENDA AG (nolock) ON AG.ID_AGENDA = A.ID_CLIENTE_AGENDA  
	INNER JOIN TB_SIMULADOR SI (nolock) ON SI.ID_SIMULADOR = AG.ID_SIMULADOR  
	INNER JOIN TB_EMPRESA E (nolock) ON E.ID_EMPRESA = AG.ID_CFC  
	INNER JOIN TB_ENDERECO EN (nolock) ON EN.ID_ENDERECO = E.ID_ENDERECO  
	INNER JOIN TB_MUNICIPIO M (nolock) ON M.ID_MUNICIPIO = EN.ID_MUNICIPIO  
	INNER JOIN TB_UF U (nolock)ON U.ID_UF = M.ID_UF 
	INNER JOIN TB_PESSOA P (nolock)ON P.ID_PESSOA = AG.ID_ALUNO 
	INNER JOIN TB_MATRICULA MA (nolock)ON MA.ID_MATRICULA = AG.ID_MATRICULA 
	INNER JOIN TB_ALUNO AL (nolock) on AG.ID_ALUNO = AL.ID_ALUNO 
	INNER JOIN TB_MODELO MO (nolock) ON AG.EXERCICE_BLOCK = MO.ID_MODELO 
	INNER JOIN TB_PESSOA P2 (nolock) ON AG.ID_INSTRUTOR = P2.ID_PESSOA 
	WHERE AG.HORARIO_AULA BETWEEN @DTINICIODT AND @DTFIMDT '

	if (@CPF <> '')
	set @SQL_Str = @SQL_Str + 
		'and P.CPF = @CPF '

	if (@IDESTADO <> 0)
	set @SQL_Str = @SQL_Str + 
		'and U.ID_UF = @IDESTADO '

	if (@RENACH <> '')
	set @SQL_Str = @SQL_Str + 
		'and AL.NUMERO_HABILITACAO = @RENACH '

	if (@IDSTATUS <> 0)
	set @SQL_Str = @SQL_Str + 
		'and S.ID_AULA_STATUS = @IDSTATUS '

	if (@IDCURSO <> 0)
	set @SQL_Str = @SQL_Str + 
		'and MA.TIPO_CURSO = @IDCURSO '

	set @SQL_Str = @SQL_Str + '
	ORDER BY 
		AG.HORARIO_AULA
		, AG.ID_AGENDA
		, AG.ID_AGENDA_SEQUENCIA'


	execute sp_executesql 
		@SQL_Str, 
		@Par_Def,
		@CPF = @CPF,
		@IDESTADO = @IDESTADO,
		@RENACH = @RENACH,
		@DTINICIODT = @DTINICIODT,
		@DTFIMDT = @DTFIMDT,
		@IDSTATUS = @IDSTATUS,
		@IDCURSO = @IDCURSO


END
GO
