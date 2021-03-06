﻿ALTER PROCEDURE [dbo].[pr_selecionar_aulas_historico_aluno]
	@ID_MATRICULA INT
	
AS BEGIN
	SET NOCOUNT ON;

	SELECT 
		A.ID_AULA
		, AG.ID_AGENDA
		, ISNULL(AG.TICKET,0) [IDENTIFICADORAULA]
		, (SELECT P.CPF FROM TB_PESSOA P WITH(NOLOCK) WHERE P.ID_PESSOA = A.ID_ALUNO) [CPF]
		, AG.ID_SIMULADOR
		, A.HORARIO_INICIO [HORARIO_INICIO] 
		, A.HORARIO_FIM [HORARIO_FIM] 
		, AL.CATEGORIA_PRETENDIDA [CURSO]
		, CASE UPPER(S.[STATUS])
			WHEN 'RECUSADA DETRAN' THEN (
											SELECT TOP 1 I.MENSAGEM 
											FROM TB_INTEGRACAO_LOG I (NOLOCK) 
											WHERE I.ID_AULA = A.ID_AULA
										)
		  
		  END [INFO]
		, A.SESSION_ID
		, S.ID_AULA_STATUS [ID_AULA_STATUS]
		, S.[STATUS] 
		, MO.ID_MODELO
		, MO.NOME [MODELO] 
		, (
			SELECT U.SIGLA FROM TB_UF U (NOLOCK)
			INNER JOIN TB_MUNICIPIO M	(NOLOCK) ON U.ID_UF = M.ID_UF
			INNER JOIN TB_ENDERECO ED	(NOLOCK) ON M.ID_MUNICIPIO = ED.ID_MUNICIPIO
			INNER JOIN TB_EMPRESA E		(NOLOCK) ON ED.ID_ENDERECO = E.ID_ENDERECO
			WHERE E.ID_EMPRESA = AG.ID_CFC
		) AS UF
		, M.ID_MATRICULA
		FROM TB_AULA A (NOLOCK)
		INNER JOIN TB_AGENDA AG		(NOLOCK) ON A.ID_CLIENTE_AGENDA = AG.ID_AGENDA
		INNER JOIN TB_MATRICULA M	(NOLOCK) ON AG.ID_MATRICULA = M.ID_MATRICULA
		INNER JOIN TB_MODELO MO		(nolock) ON AG.EXERCICE_BLOCK = MO.ID_MODELO 
		INNER JOIN TB_AULA_STATUS S (nolock) ON S.ID_AULA_STATUS = A.ID_AULA_STATUS  
		INNER JOIN TB_ALUNO AL		(nolock) ON A.ID_ALUNO = AL.ID_ALUNO
	WHERE	
		M.ID_MATRICULA = @ID_MATRICULA
END
GO