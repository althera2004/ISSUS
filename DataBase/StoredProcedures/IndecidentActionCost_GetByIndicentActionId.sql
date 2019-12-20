
CREATE PROCEDURE [dbo].[IndecidentActionCost_GetByIndicentActionId]
	@IncidentActionId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		IC.Id,
		IC.CompanyId,
		IC.IncidentActionId,
		IC.[Description],
		IC.Amount,
		IC.Quantity,
		CAST(IC.Responsable AS bigint) AS ResponsableId,
		EMP.[Name] AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active,
		IC.[Date]
	FROM IncidentActionCost IC WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	
	WHERE
		IC.IncidentActionId = @IncidentActionId
	AND IC.CompanyId = @CompanyId
	AND IC.Active = 1
END
