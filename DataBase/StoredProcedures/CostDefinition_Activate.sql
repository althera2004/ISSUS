




CREATE PROCEDURE [dbo].[CostDefinition_Activate]
	@CostDefinitionId bigint,
	@CompanyId int,
	@ApplicationUserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CostDefinition SET
		Active = 1,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @CostDefinitionId
	AND CompanyId = @CompanyId

END





