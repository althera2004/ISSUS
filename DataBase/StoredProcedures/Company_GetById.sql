





CREATE PROCEDURE [dbo].[Company_GetById]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		C.Id,
		C.Name,
		C.SubscriptionStart,
		C.SubscriptionEnd,
		C.Language,
		ISNULL(C.[NIF-CIF],'') AS [NIF-CIF],
		ISNULL(C.Code,'') AS Code,
		ISNULL(C.Logo,'') AS Logo,
		ISNULL(C.DiskQuote,100) AS DiskQuote,
		ISNULL(C.Agreement,0) AS Agreement,
		ISNULL(C.Headquarter, '') AS Headquarter
	FROM Company C WITH(NOLOCK)
	WHERE
		C.Id = @CompanyId
END






