





CREATE PROCEDURE [dbo].[IncidentAction_Delete]
	@IncidentActionId bigint,
	@CompanyId int,
	@UserId int 
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE IncidentAction SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE 
		Id = @IncidentActionId
	AND CompanyId = @CompanyId


END






