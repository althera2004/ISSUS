
CREATE PROCEDURE [dbo].[IndecidentCost_GetByIndicentId]
	@IncidentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		IC.Id,
		IC.CompanyId,
		IC.IncidentId,

		IC.[Description],
		IC.Amount,
		IC.Quantity,
		EMP.Id AS ResponsableId,
		EMP.[Name] AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active,
		IC.[Date]
	FROM IncidentCost IC WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	
	WHERE
		IC.IncidentId = @IncidentId
	AND IC.CompanyId = @CompanyId
	AND IC.Active = 1
END
