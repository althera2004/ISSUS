





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Company_GetAdress]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		CA.Id,
		CA.CompanyId,
		ISNULL(CA.Address,'') AS Address,
		ISNULL(CA.PostalCode,'') AS PostalCode,
		ISNULL(CA.City,'') AS City,
		ISNULL(CA.Province,'') AS Province,
		ISNULL(CA.Country,'') AS Country,
		ISNULL(CA.Phone,'') AS Phone,
		ISNULL(CA.Mobile,'') AS Mobile,
		ISNULL(CA.Fax,'') AS Fax,
		ISNULL(CA.Email,'') AS Email,
		ISNULL(CA.Notes,'') AS Notes,
		CASE WHEN CA.Id = C.DefaultAddress THEN 1 ELSE 0 END AS DefaultAddress
	FROM CompanyAddress CA WITH (NOLOCK)
	INNER JOIN Company C WITH(NOLOCK)
	ON	CA.CompanyId = C.Id
	AND C.Id = @CompanyId
	AND CA.Active = 1
END






