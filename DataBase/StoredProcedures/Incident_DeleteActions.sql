





CREATE PROCEDURE [dbo].[Incident_DeleteActions]
	@IncidentId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    Update IncidentAction SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		IncidentId = @IncidentId
	AND	CompanyId = @CompanyId
	AND Active = 1
END






