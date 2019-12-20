





CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_Activate]
	@Id bigint,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].[ProbabilitySeverityRange]
	SET
		Active = 1,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		Id = @Id
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
		@Id,
		3,
		GETDATE(),
		@Reason
    )
	
	
END




