
CREATE PROCEDURE [dbo].[BusinessRisk_Delete]
	@BusinessRiskId bigint,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE BusinessRisk3 SET
		Active = 0,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @BusinessRiskId
	AND	CompanyId = @CompanyId

	UPDATE IncidentAction SET
		Active = 0,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		BusinessRiskId = @BusinessRiskId

	AND CompanyId = @CompanyId

	
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
