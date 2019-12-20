




CREATE PROCEDURE [dbo].[CostDefinition_Update]
	@CostDefinitionId bigint,
	@CompanyId int,
	@Description nvarchar(100),
	@Amount decimal(18,3),
	@ApplicationUserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CostDefinition SET
		[Description] = @Description,
		Amount = @Amount,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @CostDefinitionId
	AND CompanyId = @CompanyId

END





