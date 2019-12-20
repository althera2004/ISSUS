





CREATE PROCEDURE [dbo].[Company_GetCountries]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		CC.CountryId,
		CA.Description
	FROM CompanyCountries CC WITH(NOLOCK)
	INNER JOIN CountriesAvialables CA WITH (NOLOCK)
	ON	CC.CountryId = CA.Id	
	WHERE
		CC.CompanyId = @CompanyId
END






