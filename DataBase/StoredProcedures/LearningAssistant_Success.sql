





CREATE PROCEDURE [dbo].[LearningAssistant_Success]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = 1,
		Success = 1,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
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
		12,
		@LearningAssitantId,
		4,
		GETDATE(),
		''
	)
	
	DECLARE @LearningId int;
	
	SELECT
		@LearningId =La.LearningId
	FROM LearningAssistant LA WITH(NOLOCK)
	WHERE
		LA.Id = @LearningAssitantId
	AND La.CompanyId = @CompanyId
	
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
		4,
		GETDATE(),
		'Assistant ' + CAST(@LearningAssitantId AS NVARCHAR(32)) + ' - Success'
	)


END






