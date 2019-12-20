

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Auditory_SetQuestionaries]
	@AuditoryId bigint,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Normas nvarchar(2000)

	SELECT @Normas = NormaId FROM Auditory WHERE Id = @AuditoryId

    -- Insert statements for procedure here
	INSERT INTO [AuditoryCuestionarioPregunta]
	(
		[CompanyId],
		[AuditoryId],
		[CuestionarioId],
		[QuestionId],
		[Question],
		[Compliant],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	SELECT
		C.CompanyId AS CompanyId,
		@AuditoryId AS AuditoryId,
		C.Id AS CuestionaioId,
		CP.Id AS CuestionarioPreguntaId,
		CP.Question,
		NULL AS Compliant,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	FROM Cuestionario C WITH(NOLOCK)
	INNER JOIN CuestionarioPregunta CP WITH(NOLOCK)
	ON	CP.CuestionarioId = C.Id
	INNER JOIN AuditoryPlanning AP WITH(NOLOCK)
	ON AP.AuditoryId = @AuditoryId
	AND C.ProcessId = AP.ProcessId
	AND CP.Active = 1
	WHERE CAST(C.NormaId as nvarchar(10)) in (select item from fn_split(@Normas,'|'))

	UPDATE Auditory SET Status = 1 where Id = @AuditoryId
END
