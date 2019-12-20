





CREATE PROCEDURE [dbo].[Learning_Update]
	@LearningId int,
	@CompanyId int,
	@Description nvarchar(100),
	@Status int,
	@DateStimatedDate date,
	@RealStart date,
	@RealFinish date,
	@Master nvarchar(100),
	@Hours int,
	@Amount numeric(18,3),
	@Notes text,
	@UserId int,
	@Year int,
	@Objetivo text,
	@Metodologia text
AS
BEGIN
	DECLARE @Assistants int
	SELECT @Assistants = COUNT(LA.Id) FROM LearningAssistant LA
	WHERE
		LA.Active = 1
	AND LA.LearningId = @LearningId

	DECLARE @AssistantsEvaluated int
	SELECT @AssistantsEvaluated = COUNT(LA.Id) FROM LearningAssistant LA
	WHERE
		LA.Active = 1
	AND LA.LearningId = @LearningId
	AND LA.Success IS NOT NULL
	

	IF @RealStart IS NULL
	BEGIN
		SET @Status = 0
															  
	END
	ELSE
		BEGIN
			IF @RealFinish IS NULL
				BEGIN
					SET @Status = 1
				END
			ELSE
				BEGIN
					IF @Assistants = @AssistantsEvaluated
						BEGIN
							SET @Status = 3
						END
					ELSE
						BEGIN
							SET @Status = 2
						END
				END
		END
	

	UPDATE Learning SET
		[Description] = @Description,
		[Status] = @Status,
		DateStimatedDate = @DateStimatedDate,
		RealStart = @RealStart,
		RealFinish = @RealFinish,
		[Master] = @Master,
		[Hours] = @Hours,
		Amount = @Amount,
		Notes = @Notes,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE(),
		Year = @Year,
		Objetivo = @Objetivo,
		Metodologia = @Metodologia
	WHERE
		Id = @LearningId
	AND CompanyId = @CompanyId
	
	

END






