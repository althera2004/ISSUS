





CREATE PROCEDURE [dbo].[Learning_Insert]
	@LearningId int out,
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
	IF @RealFinish IS NOT NULL
	BEGIN
		SET @Status = 1
	END
	
	INSERT INTO Learning
	(
		CompanyId,
		Description,
		Status,
		DateStimatedDate,
		RealStart,
		RealFinish,
		Master,
		Hours,
		Amount,
		Notes,
		ModifiedBy,
		ModifiedOn,
		Year,
		Active,
		Objetivo,
		Metodologia
	)
	VALUES
	(
		@CompanyId,
		@Description,
		@Status,
		@DateStimatedDate,
		@RealStart,
		@RealFinish,
		@Master,
		@Hours,
		@Amount,
		@Notes,
		@UserId,
		GETDATE(),
		@Year,
		1,
		@Objetivo,
		@Metodologia
	)
	
	SET @LearningId = @@IDENTITY	

END






