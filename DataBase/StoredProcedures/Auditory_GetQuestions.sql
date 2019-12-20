

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Auditory_GetQuestions]
	@AuditoryId bigint,
	@CuestionarioId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		ACP.Id,
		ACP.CompanyId,
		ACP.QuestionId,
		ACP.Question,
		ACP.CuestionarioId,
		ACP.AuditoryId,
		ACP.Compliant
	FROM AuditoryCuestionarioPregunta ACP WITH(NOLOCK)
	WHERE
		ACP.AuditoryId = @AuditoryId
	AND ACP.CuestionarioId = @CuestionarioId
	AND ACP.CompanyId = @CompanyId
	AND ACP.Active = 1
END

