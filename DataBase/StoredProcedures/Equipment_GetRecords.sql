





CREATE PROCEDURE [dbo].[Equipment_GetRecords]
	@EquipmentId bigint,
	@CompanyId int,
	@CalibrationInt bit,
	@CalibrationExt bit,
	@VerificationInt bit,
	@VerificationExt bit,
	@MaintenanceInt bit,
	@MaintenanceExt bit,
	@RepairInt bit,
	@RepairExt bit,
	@DateFrom datetime,
	@DateTo datetime
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		TOTAL.Date,
		TOTAL.Item,
		TOTAL.Type,
		TOTAL.Operation,
		TOTAL.Cost,
		TOTAL.Responsable,
		EMP.Name,
		EMP.LastName,
		TOTAL.Active
	FROM
	(
		SELECT
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
		
		UNION	
		
		SELECT
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
		
		UNION	
		
		SELECT
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
		
		UNION	
		
		SELECT
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
	) TOTAL
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	TOTAL.Responsable = EMP.Id
	
	WHERE
		TOTAL.CompanyId = @CompanyId
	AND	TOTAL.Active = 1
	AND TOTAL.EquipmentId = @EquipmentId
	AND
	(
		(TOTAL.Item='Calibration' AND TOTAL.Type=0 AND @CalibrationInt=1)
		OR
		(TOTAL.Item='Calibration' AND TOTAL.Type=1 AND @CalibrationExt=1)
		OR
		(TOTAL.Item='Verification' AND TOTAL.Type=0 AND @VerificationInt=1)
		OR
		(TOTAL.Item='Verification' AND TOTAL.Type=1 AND @VerificationExt=1)
		OR
		(TOTAL.Item='Maintenance' AND TOTAL.Type=0 AND @MaintenanceInt=1)
		OR
		(TOTAL.Item='Maintenance' AND TOTAL.Type=1 AND @MaintenanceExt=1)
		OR
		(TOTAL.Item='Repair' AND TOTAL.Type=0 AND @RepairInt=1)
		OR
		(TOTAL.Item='Repair' AND TOTAL.Type=1 AND @RepairExt=1)		
	)
	AND
	(@DateFrom IS NULL OR TOTAL.Date >= @DateFrom)
	AND
	(@DateTo IS NULL OR TOTAL.Date <= @DateTo)
	
	ORDER BY TOTAL.Date
END






