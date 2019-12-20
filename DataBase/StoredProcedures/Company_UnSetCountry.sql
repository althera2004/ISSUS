





CREATE PROCEDURE [dbo].[Company_UnSetCountry]
	@CountryId int,
	@CompanyId int
AS
BEGIN
	DELETE FROM CompanyCountries
	WHERE
		CountryId = @CountryId
	AND	CompanyId = @CompanyId

END






