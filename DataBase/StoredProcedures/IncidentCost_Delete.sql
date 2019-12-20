





CREATE PROCEDURE [dbo].[IncidentCost_Delete]
	@IncidentCostId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE IncidentCost SET	
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentCostId
	AND CompanyId = @CompanyId


END






