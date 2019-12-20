


CREATE PROCEDURE [dbo].[AuditoryCuestionarioObservations_ById]
	@CompanyId bigint,
	@AuditoryId bigint,
	@CuestionarioId bigint
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		I.Id,
		I.CompanyId,
		I.AuditoryId,
		I.CuestionarioId,
		I.[Text],
		I.CreatedBy,
		I.CreatedOn,
		CB.Login As CreatedByName,
		I.ModifiedBy,
		I.ModifiedOn,
		MB.Login As ModifiedByName,
		I.Active

	FROM AuditoryCuestionarioObservations I WITH(NOLOCK)
	INNER JOIN ApplicationUser CB With (Nolock)
	On CB.Id = I.CreatedBy
	INNER JOIN ApplicationUser MB With (Nolock)
	On MB.Id = I.ModifiedBy 

	WHERE
		I.CompanyId = @CompanyId
	AND	I.AuditoryId = @AuditoryId
	AND	I.CuestionarioId = @CuestionarioId

END

