

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Auditory_Update]
	@AuditoryId bigint,
	@CompanyId int,
    @Type int,
    @CustomerId bigint,
    @ProviderId bigint,
	@PreviewDate datetime,
    @Nombre nvarchar(150),
    @NormaId nvarchar(200),
    @Amount decimal(18,3),
    @InternalResponsible int,
    @Description nvarchar(2000),
    @PuntosFuertes nvarchar(2000),
    @Scope nvarchar(150),
	@CompanyAddressId int,
    @EnterpriseAddress nvarchar(500),
    @Notes nvarchar(2000),
	@AuditorTeam nvarchar(500),
    @PlannedBy int,
    @PlannedOn datetime,
    @ClosedBy int,
    @ClosedOn datetime,
    @ValidatedBy int,
    @ValidatedOn datetime,
    @ValidatedUserBy int,
    @ValidatedUserOn datetime,
	@ReportEnd datetime,
    @Status int,
    @ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

	-- Cuando es de tipo externa la fecha final viene desde el formulario
	IF @Type = 1
	BEGIN
		UPDATE [dbo].[Auditory] SET
			[ReportEnd] = @ReportEnd
		WHERE
			Id = @AuditoryId
		AND CompanyId = @CompanyId

	END
	UPDATE [dbo].[Auditory] SET
		[Type] = @Type,
		[CustomerId] = @CustomerId,
		[ProviderId] = @ProviderId,
		[PreviewDate] = @PreviewDate,
		[Nombre] = @Nombre,
		[NormaId] = @NormaId,
		[Amount] = @Amount,
		[InternalResponsible] = @InternalResponsible,
		[Description] = @Description,
		[PuntosFuertes] = @PuntosFuertes,
		[Scope] = @Scope,
		[EnterpriseAddress] = @EnterpriseAddress,
		[Notes] = @Notes,
		[PlannedBy] = @PlannedBy,
		[PlannedOn] = @PlannedOn,
		--[ClosedBy] = @ClosedBy,
		--[ClosedOn] = @ClosedOn,
		--[ValidatedBy] = @ValidatedBy,
		--[ValidatedOn] = @ValidatedOn,
		--[ValidatedUserBy] = @ValidatedUserBy,
		--[ValidatedUserOn] = @ValidatedUserOn,
		[Status] = @Status,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE(),
		--[ReportEnd] = @ReportEnd,
		[AuditorTeam] = @AuditorTeam,
		[CompanyAddressId] = @CompanyAddressId
	WHERE
		Id = @AuditoryId
	AND CompanyId = @CompanyId
END

