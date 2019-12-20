


CREATE PROCEDURE [dbo].[Auditory_SetReportStart]
	@AuditoryId bigint,
	@CompanyId int,
	@ReportStart datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[Auditory] SET
		[ReportStart] = @ReportStart
	WHERE
		Id = @AuditoryId
	AND CompanyId = @CompanyId
END

