
CREATE PROCEDURE [dbo].[Learning_AssistantDelete]
	@AssistantId int,

	@LearningId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	DELETE FROM LearningAssistant
	WHERE
		Id = @AssistantId

	AND CompanyId = @CompanyId
	AND LearningId = @LearningId
	
	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        [DateTime],
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		11,
		@LearningId,
		5,
		GETDATE(),
		'AssistantId:' + CAST(@AssistantId AS nvarchar(6))
    )
END
