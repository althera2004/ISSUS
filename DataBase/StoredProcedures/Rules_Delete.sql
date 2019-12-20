





CREATE PROCEDURE [dbo].[Rules_Delete]
	@RulesId int,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Rules SET
		Active = 0,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @RulesId
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
		5,
		@RulesId,
		3,
		GETDATE(),
		@Reason
    )
	
	
END




