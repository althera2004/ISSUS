




CREATE PROCEDURE [dbo].[ProbabilitySeverityRange_GetAll]
	@CompanyId int
AS
	SELECT
	P.Id,
	P.Description,
	P.Code,
	P.Type,
	P.CreatedOn,
	P.CreatedBy,
	CB.Login As CreatedByName,
	P.ModifiedOn,
	P.ModifiedBy,
	MB.Login As ModifiedByName,
	P.Active
	From ProbabilitySeverityRange P With (Nolock)
	Inner Join ApplicationUser CB With (Nolock)
	On CB.Id = P.CreatedBy
	Inner Join ApplicationUser MB With (Nolock)
	On MB.Id = P.ModifiedBy

	Where P.CompanyId = @CompanyId
	Order By P.Type, P.Code
RETURN 0




