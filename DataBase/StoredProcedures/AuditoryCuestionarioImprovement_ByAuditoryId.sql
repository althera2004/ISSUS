


CREATE PROCEDURE [dbo].[AuditoryCuestionarioImprovement_ByAuditoryId]
	@CompanyId bigint,
	@AuditoryId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
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
		I.Active,
		I.Action

	FROM AuditoryCuestionarioImprovement I WITH(NOLOCK)
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = I.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = I.ModifiedBy 

	WHERE
		I.CompanyId = @CompanyId
	AND	I.AuditoryId = @AuditoryId

END

