




CREATE PROCEDURE [dbo].[Rules_GetActive]
	@CompanyId int
AS
BEGIN
	SELECT DISTINCT
		R.Id,
		R.Description,
		R.Notes,
		R.Limit,
		R.CreatedOn,
		R.CreatedBy,
		CB.Login As CreatedByName,
		R.ModifiedOn,
		R.ModifiedBy,
		MB.Login As ModifiedByName,
		R.Active,
		CASE WHEN BR.Id IS NULL AND C.Id IS NULL THEN 1 ELSE 0 END AS CanBeDeleted
	From Rules R With (Nolock) 
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = R.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = R.ModifiedBy
	LEFT JOIN BusinessRisk BR WITH(NOLOCK)
	ON	BR.RuleId = R.Id
	LEFT JOIN Cuestionario C WITH(NOLOCK)
	ON	C.NormaId = R.Id
	AND C.Active = 1

	WHERE
		R.CompanyId = @CompanyId 
	AND R.Active = 1

	ORDER BY 
		R.[Description]
END





