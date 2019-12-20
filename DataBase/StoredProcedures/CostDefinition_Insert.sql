




CREATE PROCEDURE [dbo].[CostDefinition_Insert]
	@CostDefinitionId bigint output,
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
	INSERT INTO CostDefinition
	(
		CompanyId,
		[Description],
		Amount,
		Active,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		@Amount,
		1,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE()
	)

	SET @CostDefinitionId = @@IDENTITY
END





