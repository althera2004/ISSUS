





CREATE PROCEDURE [dbo].[LearningAssistant_SetStatus]
	@LearningAssitantId int,
	@CompanyId int,
	@UserId int,
	@Completed bit,
	@Success bit
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE LearningAssistant SET
		Completed = @Completed,
		Success = @Success,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
	AND CompanyId = @CompanyId
END






