
CREATE PROCEDURE [dbo].[IncidentActionCost_Update]
	@IncidentActionCostId bigint,
	@IncidentActionId bigint,
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
	
	UPDATE IncidentActionCost SET	
		[Description] = @Description,
		[Date] = @Date,
		Amount = @Amount,
		Quantity = @Quantity,
		Responsable = @ResponsableId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentActionCostId
	AND IncidentActionId = @IncidentActionId
	AND CompanyId = @CompanyId


END
