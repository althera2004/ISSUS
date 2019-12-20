





CREATE PROCEDURE [dbo].[IncidentActionCost_Delete]
	@IncidentActionCostId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE IncidentActionCost SET	
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentActionCostId
	AND CompanyId = @CompanyId


END






