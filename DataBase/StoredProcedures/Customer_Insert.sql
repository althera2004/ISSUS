





CREATE PROCEDURE [dbo].[Customer_Insert]
	@CustomerId bigint output,
	@CompanyId int,
	@Description nvarchar(100),
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Customer
	(
		[CompanyId],
		[Description],
		[Active],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn]
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
	
	SET @CustomerId = @@IDENTITY
	
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
		20,
		@CustomerId,
		1,
		GETDATE(),
		@Description
    )

END






