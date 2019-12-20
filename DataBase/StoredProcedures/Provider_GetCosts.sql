





CREATE PROCEDURE [dbo].[Provider_GetCosts]
	@ProviderId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		TOTAL.Id,
		TOTAL.Item,
		TOTAL.Date,
		TOTAL.Operation,
		TOTAL.Cost,
		TOTAL.Responsable,
		EMP.Name,
		EMP.LastName,
		TOTAL.Active,
		TOTAL.EquipmentId,
		E.Description AS EquipmentDescription,
		E.Code
	FROM
	(
		SELECT
			ECA.Id,
			ECA.CompanyId,
			ECA.Date,
			'Calibration' AS Item,
			ECA.EquipmentCalibrationType AS Type,
			ECA.Operation,
			ECA.Responsable,
			ECA.Cost,
			ECA.Active,
			ECA.EquipmentId
		FROM EquipmentCalibrationAct ECA WITH(NOLOCK)
		WHERE 
			ECA.ProviderId = @ProviderId
		AND ECA.CompanyId = @CompanyId
		AND ECA.Active = 1
		
		UNION	

		SELECT
			EVA.Id,
			EVA.CompanyId,
			EVA.Date,
			'Verification' AS Item,
			EVA.EquipmentVerificationType AS Type,
			EVA.Operation,
			EVA.Responsable,
			EVA.Cost,
			EVA.Active,
			EVA.EquipmentId
		FROM EquipmentVerificationAct EVA WITH(NOLOCK)
		WHERE 
			EVA.ProviderId = @ProviderId
		AND EVA.CompanyId = @CompanyId
		AND EVA.Active = 1
		
		UNION	
		
		SELECT
			EMA.Id,
			EMA.CompanyId,
			EMA.Date,
			'Maintenance' AS Item,
			EMD.Type,
			EMA.Operation AS Operation,
			EMA.ResponsableId,
			EMA.Cost,
			EMA.Active,
			EMA.EquipmentId
		FROM EquipmentMaintenanceAct EMA WITH(NOLOCK)
		INNER JOIN EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
		ON	EMD.Id = EMA.EquipmentMaintenanceDefinitionId
		AND EMD.CompanyId = EMA.CompanyId
		WHERE
			EMA.ProviderId = @ProviderId
		AND EMA.CompanyId = @CompanyId
		AND EMA.Active = 1
		
		UNION	
		
		SELECT
			R.Id,
			R.CompanyId,
			R.Date,
			'Repair' AS Item,
			R.RepairType AS Type,
			CAST(R.Description AS nvarchar(50)) AS Operation,
			R.ResponsableId,
			R.Cost,
			R.Active,
			R.EquipmentId
		FROM EquipmentRepair R WITH(NOLOCK)
		WHERE 
			R.ProviderId = @ProviderId
		AND R.CompanyId = @CompanyId
		AND R.Active = 1
	) TOTAL
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	TOTAL.Responsable = EMP.Id
	AND	TOTAL.CompanyId = EMP.CompanyId
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	TOTAL.EquipmentId = E.Id
	AND TOTAL.CompanyId = E.CompanyId
	
	ORDER BY TOTAL.Date DESC
END






