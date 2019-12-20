
CREATE PROCEDURE [dbo].[EquipmentMaintenance_GetByEquipmentId]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		M.Id,
		M.EquipmentId,
		M.CompanyId,
		M.Operation,
		M.Type,
		M.Periodicity,
		M.Accessories,
		M.Cost,
		M.Active,
		ISNULL(P.Id,-1) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription,
		M.ResponsableId,
		R.Name,
		R.LastName,
		M.ModifiedBy AS ModifiedByUserId,
		0 AS ModifiedByEmployeeId,
		AU.[Login] AS ModifiedByName,
		'' AS ModifiedByLastName,
		M.ModifiedOn,
		M.FirstDate
    FROM EquipmentMaintenanceDefinition M WITH(NOLOCK)
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = M.ModifiedBy
	LEFT JOIN Provider P
	ON P.Id = M.ProviderId
	INNER JOIN Employee R WITH(NOLOCK)
	ON	R.Id = M.ResponsableId
	WHERE
		M.EquipmentId = @EquipmentId
	AND M.CompanyId = @CompanyId
	AND M.Active = 1
END
