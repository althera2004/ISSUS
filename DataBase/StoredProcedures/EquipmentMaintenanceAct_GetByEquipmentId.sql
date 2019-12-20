





CREATE PROCEDURE [dbo].[EquipmentMaintenanceAct_GetByEquipmentId]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		EMA.Id,
		EMA.EquipmentId,
		EMA.EquipmentMaintenanceDefinitionId,
		EMA.CompanyId,
		EMA.[Date],
		EMA.Operation,
		EMA.Observations,
		ISNULL(P.Id,-1) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription,
		CASE WHEN E.Resposable < 1 THEN NULL ELSE E.Resposable END AS ResponsableId, 
		ISNULL(R.Name, RE.Name) AS ResponsableName,
		ISNULL(R.LastName, RE.LastName) AS ResponsableLastName,
		EMA.Cost,
		EMA.Vto,
		EMA.Active,
		EMA.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		EMA.ModifiedOn 
    FROM EquipmentMaintenanceAct EMA WITH(NOLOCK)
    LEFT JOIN Employee R WITH(NOLOCK)
    ON	R.Id = EMA.ResponsableId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = EMA.ProviderId
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = EMA.ModifiedBy
	INNER JOIN Equipment E WITH (NOLOCK)
	ON	E.Id = EMA.EquipmentId
	INNER JOIN Employee RE WITH(NOLOCK)
	ON	E.Resposable = RE.Id
	
	WHERE
		EMA.EquipmentId = @EquipmentId
	AND EMA.CompanyId = @CompanyId
	AND EMA.Active = 1
	
	ORDER BY EMA.Date
END






