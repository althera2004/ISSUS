





CREATE PROCEDURE [dbo].[Learning_Delete]
	@LearningId int,
	@CompanyId int,
	@Reason nvarchar(200),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE Learning SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningId
	ANd	CompanyId = @CompanyId
	
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
    SELECT
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		LA.Id,
		5,
		GETDATE(),
		@Reason
	FROM LearningAssistant LA
	WHERE
		LearningId = @LearningId
	AND CompanyId = @CompanyId
	AND Active = 1
		
	
	UPDATE LearningAssistant SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		LearningId = @LearningId
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
		11,
		@LearningId,
		6,
		GETDATE(),
		@Reason
    )
END






