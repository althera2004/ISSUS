
CREATE PROCEDURE [dbo].[IncidentCost_Update]
	@IncidentCostId bigint,
	@IncidentId bigint,
	@BusinessRiskId bigint,
	@CompanyId int,
	@Description nvarchar(50),
	@Date datetime,
	@Amount numeric(18,3),
	@Quantity numeric(18,3),
	@ResponsableId int,
	@UserId int,
	@Differences text
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE IncidentCost SET	
		[Description] = @Description,
		[Date] = @Date,
		Amount = @Amount,
		Quantity = @Quantity,
		Responsable = @ResponsableId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentCostId
	AND IncidentId = @IncidentId
	AND BusinessRiskId = @BusinessRiskId
	AND CompanyId = @CompanyId


END






