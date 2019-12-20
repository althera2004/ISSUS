





CREATE PROCEDURE [dbo].[Incident_Delete]
	@IncidentId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE Incident SET 
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @IncidentId
	AND CompanyId = @CompanyId

	UPDATE IncidentAction SET
		Active = 0,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
	WHERE
		IncidentId = @IncidentId
	AND CompanyId = @CompanyId

	SELECT * FROM IncidentAction WHERE IncidentId = @IncidentId

END






