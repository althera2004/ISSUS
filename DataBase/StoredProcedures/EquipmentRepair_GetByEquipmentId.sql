





CREATE PROCEDURE [dbo].[EquipmentRepair_GetByEquipmentId]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		R.Id,
		R.EquipmentId,
		R.CompanyId,
		R.RepairType,
		R.Date,
		R.Description,
		R.Tools,
		R.Observations,
		R.Cost,
		ISNULL(R.ProviderId,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription,
		R.ResponsableId AS ResponsableUserId,
		RESP.Id AS ResponsableEmployeeId,
		RESP.Name AS ResponsableName,
		RESP.LastName AS ResponsableName,
		R.Active,
		R.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		R.ModifiedOn
	FROM EquipmentRepair R WITH(NOLOCK)
	INNER JOIN Employee RESP WITH(NOLOCK)
	ON	RESP.Id = R.ResponsableId
	AND RESP.CompanyId = R.CompanyId
	LEFT JOIN Provider P WITH(NOLOCK)
	ON	P.Id = R.ProviderId
	AND P.CompanyId = R.CompanyId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = R.ModifiedBy
	
	WHERE
		R.EquipmentId = @EquipmentId
	AND R.CompanyId = @CompanyId
	
	ORDER BY R.Date
END






