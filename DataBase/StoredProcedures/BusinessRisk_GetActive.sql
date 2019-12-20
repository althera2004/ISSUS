




CREATE PROCEDURE [dbo].[BusinessRisk_GetActive]
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
	B.Result,
	B.ApplyAction,
	B.RuleId,
	R.Description As RuleDescription,
	R.Limit As RuleRangeId,
	B.ProbabilityId,
	B.SecurityId,
	B.Active,
	B.InitialValue,
	B.DateStart,
	B.ProcessId,
	PRO.Description As ProcessDescription,
	CAST (CASE WHEN B.FinalAction = 1 THEN 1 ELSE B.Assumed END AS BIT) AS Assumed,
	B.PreviousBusinessRiskId,
	B2.Id,
	B2.PreviousBusinessRiskId
	From BusinessRisk3 B with (Nolock)
		LEFT JOIN BusinessRisk B2 WITH(NOLOCK)
		ON	B2.PreviousBusinessRiskId = B.Id
	Inner Join Rules R With (Nolock)
	On R.Id = B.RuleId
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = B.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = B.ModifiedBy
	Inner Join Proceso PRO With (Nolock)
	On PRO.Id = B.ProcessId

	Where 
		B.CompanyId = @CompanyId
	And B.Active = 1
	AND B2.Id IS NULL
	Order By B.Code, B.Id
RETURN 0




