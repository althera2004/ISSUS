

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Auditory_Insert]
	@AuditoryId bigint output,
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
    @Status int,
    @ApplicationUserId int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Auditory]
	(
		[CompanyId],
		[Type],
		[CustomerId],
		[ProviderId],
		[PreviewDate],
		[Nombre],
		[NormaId],
		[Amount],
		[InternalResponsible],
		[Description],
		[Scope],
		[EnterpriseAddress],
		[Notes],
		[PlannedBy],
		[PlannedOn],
		[ClosedBy],
		[ClosedOn],
		[ValidatedBy],
		[ValidatedOn],
		[ValidatedUserBy],
		[ValidatedUserOn],
		[Status],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active],
		[AuditorTeam],
		[CompanyAddressId]
	)
	VALUES
	(
		@CompanyId,
        @Type,
        @CustomerId,
        @ProviderId,
		@PreviewDate,
        @Nombre,
        @NormaId,
        @Amount,
        @InternalResponsible,
        @Description,
        @Scope,
        @EnterpriseAddress,
        @Notes,
        @PlannedBy,
        @PlannedOn, 
        @ClosedBy,
        @ClosedOn,
        @ValidatedBy,
        @ValidatedOn,
        @ValidatedUserBy,
        @ValidatedUserOn,
        @Status,
        @ApplicationUserId,
        GETDATE(),
        @ApplicationUserId,
        GETDATE(),
        1,
        @AuditorTeam,
        @CompanyAddressId
	)

	SET @AuditoryId = @@IDENTITY

END

