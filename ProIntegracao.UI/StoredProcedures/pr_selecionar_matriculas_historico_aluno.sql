USE [ProSimulador]
GO

/****** Object:  StoredProcedure [dbo].[pr_selecionar_matriculas_historico_aluno]    Script Date: 17/05/2016 14:35:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==============================================================
-- AUTHOR:		Fernando Parmezani
-- CREATE DATE: 2016/05/17
-- DESCRIPTION:	Selecionar Matriculas Historico Aluno
-- ==============================================================
CREATE PROCEDURE [dbo].[pr_selecionar_matriculas_historico_aluno]
	@IDALUNO INT
AS BEGIN

	SET NOCOUNT ON;
	SELECT 
		 M.ID_MATRICULA
		 ,AG.ID_AGENDA
		 , M.QUANTIDADE_AULAS [QTD_AULA]
		 , M.DATA
		 , (SELECT U.NOME 
				FROM TB_USUARIO U (NOLOCK) 
				WHERE U.ID_USUARIO = M.ID_USUARIO) [USUARIO]
		 , E.ID_EMPRESA
		 , E.CODIGO [CODIGO_CFC]
		 , E.RAZAO_SOCIAL [CFC_ORIGEM]
		 , E.CNPJ CNPJ
		 , M.STATUS_MATRICULA
	FROM TB_MATRICULA M				(NOLOCK)
		INNER JOIN TB_AGENDA AG		(NOLOCK) ON M.ID_MATRICULA = AG.ID_MATRICULA
		INNER JOIN TB_EMPRESA E		(NOLOCK) ON E.ID_EMPRESA = AG.ID_CFC  
	WHERE M.ID_ALUNO = @IDALUNO
END
GO

-- ==============================================================
-- AUTHOR:		Fernando Parmezani
-- CREATE DATE: 2016/05/17
-- DESCRIPTION:	Selecionar Agenda ho Historico Aluno
-- ==============================================================
CREATE PROCEDURE [dbo].[pr_selecionar_agendas_historico_aluno]
	@ID_MATRICULA INT
AS BEGIN
SET NOCOUNT ON;

	SELECT  
		AG.ID_MATRICULA
		, AG.ID_AGENDA
		, SI.NUM_SEQUENCIA SEQUENCIA
		, CONVERT(VARCHAR(10),	A.HORARIO_INICIO,108) [HORARIO_INICIO]
		, SI.CODIGO_PRO [PSA]
		, SI.CODIGO [SMC]
		, P2.CPF [CPF_INSTRUTOR]
		, P2.NOME [NOME_INSTRUTOR]
		, MO.NOME [MODELO] 
		, S.[STATUS]
	FROM TB_AGENDA AG (NOLOCK)
		INNER JOIN TB_AULA A		(NOLOCK) ON AG.ID_AGENDA = A.ID_CLIENTE_AGENDA
		INNER JOIN TB_SIMULADOR SI	(nolock) ON SI.ID_SIMULADOR = AG.ID_SIMULADOR
		INNER JOIN TB_AULA_STATUS S (nolock) ON S.ID_AULA_STATUS = A.ID_AULA_STATUS
		INNER JOIN TB_PESSOA P2		(nolock) ON AG.ID_INSTRUTOR = P2.ID_PESSOA 
		INNER JOIN TB_MODELO MO		(nolock) ON AG.EXERCICE_BLOCK = MO.ID_MODELO 
	WHERE AG.ID_MATRICULA = @ID_MATRICULA

END
GO

-- ==============================================================
-- AUTHOR:		Fernando Parmezani
-- CREATE DATE: 2016/05/17
-- DESCRIPTION:	Selecionar Aulas ho Historico Aluno
-- ==============================================================
CREATE PROCEDURE [dbo].[pr_selecionar_aulas_historico_aluno]
	@ID_AGENDA INT
	
AS BEGIN

	SET NOCOUNT ON;

	SELECT 
		A.ID_AULA
		, AG.ID_AGENDA
		, A.HORARIO_INICIO [HORARIO_INICIO] 
		, A.HORARIO_FIM [HORARIO_FIM] 
		, A.SESSION_ID
		, S.[STATUS] 
		, MO.NOME [MODELO] 
		FROM TB_AULA A (NOLOCK)
		INNER JOIN TB_AGENDA AG		(NOLOCK) ON A.ID_CLIENTE_AGENDA = AG.ID_AGENDA
		INNER JOIN TB_MODELO MO		(nolock) ON AG.EXERCICE_BLOCK = MO.ID_MODELO 
		INNER JOIN TB_AULA_STATUS S (nolock) ON S.ID_AULA_STATUS = A.ID_AULA_STATUS  
	WHERE	
		AG.ID_AGENDA = @ID_AGENDA
END
GO