





CREATE PROCEDURE [dbo].[LearningAssistant_Reset]
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
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @LearningAssitantId
	AND CompanyId = @CompanyId
END






