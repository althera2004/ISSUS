

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AuditoryCuestionarioPregunta_Toogle]
	@PreguntaId bigint,
	@Compliant bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE AuditoryCuestionarioPregunta SET
		Compliant = @Compliant
	WHERE
		Id = @PreguntaId

	DECLARE @First datetime
	DECLARE @AuditoryId bigint
	SELECT @First = ReportStart, @AuditoryId = A.Id FROM Auditory A WITH(NOLOCK)
	INNER JOIN AuditoryCuestionarioPregunta ACP WITH(NOLOCK)
	ON A.Id = ACP.AuditoryId
	WHERE
		ACP.Id = @PreguntaId

	IF @First IS NULL 
	BEGIN
		UPDATE Auditory SET
			ReportStart = GETDATE()
		WHERE
			Id = @AuditoryId
	END

	UPDATE Auditory SET Status = 2 WHERE Id = @AuditoryId

END

