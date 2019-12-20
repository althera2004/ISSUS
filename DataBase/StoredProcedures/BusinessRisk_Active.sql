





CREATE PROCEDURE [dbo].[BusinessRisk_Active]
	@BusinessRiskId int,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE BusinessRisk3 SET
		Active = 1,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @BusinessRiskId
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
		@BusinessRiskId,
		3,
		GETDATE(),
		@Reason
    )
	
	
END




