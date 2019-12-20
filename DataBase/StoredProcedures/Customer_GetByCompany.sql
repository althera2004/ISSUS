





CREATE PROCEDURE [dbo].[Customer_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DISTINCT
		C.Id,
		C.CompanyId,
		C.Description,
		C.Active,
		C.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		C.ModifiedOn,
		CASE WHEN I.Id IS NULL THEN 0 ELSE 1 END AS InIncident,
		CASE WHEN IA.Id IS NULL THEN 0 ELSE 1 END AS InActionIncident
    FROM Customer C WITH(NOLOCK)
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = C.ModifiedBy
	LEFT JOIN Incident I WITH(NOLOCK)
	ON	I.CustomerId = C.Id
	AND	I.Active = 1
	LEFT JOIN IncidentAction IA WITH(NOLOCK)
	ON	IA.CustomerId = C.Id
	AND	IA.Active = 1
	WHERE
		C.CompanyId = @CompanyId
END






