





CREATE PROCEDURE [dbo].[LearningAssistant_Unevaluated]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = NULL,
		Success = NULL,
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
		3,
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
		'Assistant ' + CAST(@LearningAssitantId AS NVARCHAR(32)) + ' - Complete'
	)


END






