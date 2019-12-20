




CREATE PROCEDURE [dbo].[BusinessRisk_GetByRules]
	@RulesId bigint,
	@CompanyId int
AS
	SELECT DISTINCT
	B.Id,
	B.Description,
	B.CreatedBy,
	B.CreatedOn,
	CB.Login As CreatedByName,
	B.ModifiedBy,
	B.ModifiedOn,
	MB.Login As ModifiedByName,
	B.Code,
	B.ItemDescription,
	B.StartControl,
	B.Notes,
	ISNULL(B.StartResult,0) AS StartResult,
	ISNULL(B.StartAction,0) AS StartAction,
	B.RuleId,
	R.Description As RuleDescription,
	R.Limit As RuleRangeId,
	ISNULL(B.StartProbability,0) AS StartProbability,
	ISNULL(B.StartSeverity,0) AS StartSeverity,
	B.Active,
	B.InitialValue,
	B.DateStart,
	B.ProcessId,
	PRO.Description As ProcessDescription,
	B.Assumed,
	B.PreviousBusinessRiskId,
	B.Causes,
	ISNULL(B.FinalProbability,0) AS FinalProbability,
	ISNULL(B.FinalSeverity,0) AS FinalSeverity,
	ISNULL(B.FinalResult,0) AS FinalResult,
	B.FinalDate,
	ISNULL(b.FinalAction,0) AS FinalAction,
	ISNULL(b.StartAction,0) AS StartAction,
	ISNULL(b.StartResult,0) AS StartResult
	From BusinessRisk3 B with (Nolock)
	Inner Join Rules R With (Nolock)
	On R.Id = B.RuleId
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = B.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = B.ModifiedBy
	Inner Join Proceso PRO With (Nolock)
	On PRO.Id = B.ProcessId

	WHERE 
		B.CompanyId = @CompanyId 
	AND	B.Active = 1
	AND B.RuleId = @RulesId
	ORDER BY 
		B.Code, B.Id





