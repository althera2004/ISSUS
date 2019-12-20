





CREATE PROCEDURE [dbo].[Learning_AddAssistant]
	@LearningAssistantId int out,
	@LearningId int,
	@CompanyId int,
	@EmployeeId bigint,
	@JobPositionId bigint,
	@UserId int
AS
BEGIN
	SELECT
		@LearningAssistantId = LA.Id 
	FROM LearningAssistant LA WITH(NOLOCK)
	WHERE
		LA.LearningId = @LearningId
	AND LA.CompanyId = @CompanyId
	AND LA.EmployeeId = @EmployeeId
	
	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO LearningAssistant
		(
			LearningId,
			CompanyId,
			EmployeeId,
			CargoId,
			Completed,
			Success,
			CreatedBy,
			CreatedOn,
			ModifiedBy,
			ModifiedOn,
			Active
		)
		VALUES
		(
			@LearningId,
			@CompanyId,
			@EmployeeId,
			@JobPositionId,
			null,
			null,
			@UserId,
			GETDATE(),
			@UserId,
			GETDATE(),
			1
		)
		
		SET @LearningAssistantId = @@IDENTITY;
		
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
			'Add Assistant ' + CAST(@LearningAssistantId AS NVARCHAR(32)) 
		)
	END

END






