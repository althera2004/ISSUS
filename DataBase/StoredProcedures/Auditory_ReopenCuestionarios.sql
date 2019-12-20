


CREATE PROCEDURE [dbo].[Auditory_ReopenCuestionarios]
	@AuditoryId bigint,
	@PuntosFuertes nvarchar(2000),
	@Notes nvarchar(2000),
	@CompanyId int,
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[Auditory] SET
		[Notes] = @Notes,
		[PuntosFuertes] = @PuntosFuertes,
		[Status] = 2,
		[ReportEnd] = NULL,
		[ClosedBy] = NULL,
		[ClosedOn] = NULL,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @AuditoryId
	AND CompanyId = @CompanyId
END

