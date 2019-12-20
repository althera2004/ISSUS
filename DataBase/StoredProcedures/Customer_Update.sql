





CREATE PROCEDURE [dbo].[Customer_Update]
	@CustomerId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE Customer SET
		Description = @Description,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @CustomerId
	AND	CompanyId = @CompanyId
	
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
		2,
		GETDATE(),
		@Description
    )

END






