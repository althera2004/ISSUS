





CREATE PROCEDURE [dbo].[Provider_Update]
	@ProviderId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@UserId int
AS
BEGIN



    UPDATE Provider SET
		Description = @Description,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @ProviderId
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
		21,
		@ProviderId,
		2,
		GETDATE(),
		@Description
    )

END






