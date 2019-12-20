





CREATE PROCEDURE [dbo].[ProcessType_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT
		J.Id,
		J.CompanyId,
		J.Description,
		J.Active,
		J.ModifiedOn,
		J.ModifiedBy AS ModifiedByUserId,
		J.ModifiedBy AS ModifiedByEmployeeId,
		AU.[Login] AS ModifiedByName,
		'' AS ModifiedByLastName,
		P1.Type
	FROM ProcessType J WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.id = J.ModifiedBy
	LEFT JOIN 
	(
		SELECT P.Type FROM Proceso P WITH(NOLOCK)
	) P1
	ON P1.Type = J.Id
	
	WHERE
		J.CompanyId = @CompanyId
END






