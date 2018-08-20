USE [ProSimulador]
GO
/****** Object:  StoredProcedure [dbo].[pr_selecionar_matriculas_historico_aluno]    Script Date: 17/05/2016 14:35:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==============================================================
-- AUTHOR:		Fernando Parmezani
-- CREATE DATE: 2016/06/1
-- DESCRIPTION:	Atualizar Status Modelo Aula
-- ==============================================================
CREATE PROCEDURE [dbo].[pr_atualizar_status_modelo_aula]
	@ID_AULA INT
	, @ID_STATUS INT
	, @ID_MODELO INT
AS 
BEGIN

	DECLARE @ID_AGENDA INT

	SET NOCOUNT ON;


	SELECT @ID_AGENDA = A.ID_CLIENTE_AGENDA FROM TB_AULA A WITH(NOLOCK) 
					   WHERE A.ID_AULA = @ID_AULA
					   
	
	IF(@ID_AGENDA > 0)
	BEGIN
		UPDATE TB_AULA SET ID_AULA_STATUS	= @ID_STATUS WHERE ID_AULA = @ID_AULA
		UPDATE TB_AGENDA SET EXERCICE_BLOCK = @ID_MODELO WHERE ID_AGENDA = @ID_AGENDA
	END
END
GO