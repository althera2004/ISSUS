
CREATE PROCEDURE [dbo].[EquipmentScaleDivision_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT
		ESD.Id,
		ESD.CompanyId,
		ESD.Description,
		ESD.Active,
		ESD.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		ESD.ModifiedOn,		
		CASE WHEN EQ.Id IS NULL THEN 0 ELSE 1 END AS InEquipment
	FROM EquipmentScaleDivision ESD WITH(NOLOCK)
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)

		INNER JOIN ApplicationUser AU WITH(NOLOCK)
		ON	AU.Id = EUA.UserId
	ON EUA.UserId = ESD.ModifiedBy
	LEFT JOIN Equipment EQ WITH(NOLOCK)
	ON	EQ.MeasureUnit = ESD.Id
	AND	EQ.Active = 1
	
	WHERE
		ESD.CompanyId = @CompanyId
	AND ESD.Active = 1
END
