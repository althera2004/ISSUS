
CREATE PROCEDURE [dbo].[IndecidentActionCost_GetByCompanyId]
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
		EMP.Id AS ResponsableId,
		EMP.Name AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active,
		IC.[Date]
	FROM IncidentActionCost IC WITH(NOLOCK)
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	
	WHERE
		IC.CompanyId = @CompanyId
	AND IC.Active = 1
END
