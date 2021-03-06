
CREATE PROCEDURE [dbo].[IndecidentCost_GetByBusinessRiskId]
	@BusinessRiskId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @code int;
	SELECT @Code = Code From BusinessRisk3 WHERE Id = @BusinessRiskId
	
	SELECT
		IC.Id,
		IC.CompanyId,
		IC.IncidentActionId,
		IC.[Description],
		IC.Amount,
		IC.Quantity,
		EMP.Id AS ResponsableId,
		EMP.[Name] AS ResponsableName,
		EMP.LastName AS ResponsableLastName,
		IC.Active,
		IC.[Date]
	FROM IncidentActionCost IC WITH(NOLOCK)
	INNER JOIN IncidentAction IA WITH(NOLOCK)
	ON	IA.BusinessRiskId = @BusinessRiskId
	ANd IC.IncidentActionId = IA.Id
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	EMP.Id = IC.Responsable
	
	WHERE
		IC.CompanyId = @CompanyId
	AND IC.Active = 1
END






