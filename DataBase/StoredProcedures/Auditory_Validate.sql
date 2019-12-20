


CREATE PROCEDURE [dbo].[Auditory_Validate]
	@AuditoryId bigint,
	@CompanyId int,
	@ValidatedBy int,
	@ValidatedOn datetime,
	@Notes nvarchar(2000),
	@PuntosFuertes nvarchar(2000),
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
		[Status] = 5,
		[ValidatedBy] = @ValidatedBy,
		[ValidatedOn] = @ValidatedOn,
		[ValidatedUserBy] = @ApplicationUserId,
		[ValidatedUserOn] = GETDATE()
	WHERE
		Id = @AuditoryId
	AND CompanyId = @CompanyId
END

