





CREATE PROCEDURE [dbo].[Provider_Insert]
	@ProviderId bigint output,
	@CompanyId int,
	@Description nvarchar(100),
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Provider
	(
		CompanyId,
		Description,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @ProviderId = @@IDENTITY
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		21,
		@ProviderId,
		1,
		GETDATE(),
		@Description
    )

END






