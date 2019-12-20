





CREATE PROCEDURE [dbo].[Company_SetCountry]
	@CountryId int,
	@CompanyId int
AS
BEGIN
	INSERT INTO CompanyCountries
	(
		CountryId,
		CompanyId
	)
	VALUES
	(
		@CountryId,
		@CompanyId
	)

END






