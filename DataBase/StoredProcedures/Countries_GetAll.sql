





CREATE PROCEDURE [dbo].[Countries_GetAll]
	@CompanyId int
AS
BEGIN
	SELECT DISTINCT
		CA.Id,
		CA.[Description],
		CASE WHEN CC.CountryId IS NULL THEN 0 ELSE 1 END AS Selected,
		CASE WHEN E.Id IS NULL THEN
			CASE WHEN AD.Id IS NULL THEN 1
			ELSE 0
			END
		ELSE 0 END AS Deletable,
		AD.[Address],
		E.[LastName]
	FROM CountriesAvialables CA WITH(NOLOCK)
	LEFT JOIN CompanyCountries CC WITH(NOLOCK)
	ON	CC.CountryId = CA.Id
	AND CC.CompanyId = @CompanyId
	LEFT JOIN Employee E WITH(NOLOCK)
	ON	E.Country = CA.[Description]
	AND	E.FechaBaja IS NULL
	AND E.CompanyId = @CompanyId
	LEFT JOIN CompanyAddress AD
	ON	AD.CompanyId = @CompanyId
	AND	AD.Country = CAST(CA.Id as nvarchar(4))
	AND AD.Active = 1
	
	--ORDER BY CA.Description ASC
END






