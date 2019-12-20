





CREATE PROCEDURE [dbo].[Equipment_GetCosts]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
			'C',
			CASE WHEN ECA.EquipmentCalibrationType = 0 THEN 'I' ELSE 'E' END AS Type,
				ISNULL(ECA.Cost,0) AS Cost,
				ECA.Date,
				ECA.EquipmentId
			FROM EquipmentCalibrationAct ECA
			INNER JOIN Equipment E
			ON E.Id = ECA.EquipmentId
			WHERE ECA.Active = 1
			AND ECA.CompanyId = @CompanyId
			AND E.Active = 1

			UNION

			select
				'V',
				CASE WHEN EquipmentVerificationType = 0 THEN 'I' ELSE 'E' END AS Type,
				ISNULL(Cost,0) AS Cost,
				Date,
				EquipmentId
			FROM EquipmentVerificationAct EVA
			INNER JOIN Equipment E
			ON E.Id = EVA.EquipmentId
			WHERE EVA.Active = 1
			AND EVA.CompanyId = @CompanyId
			AND E.Active = 1

			UNION

			select
				'M',
				CASE WHEN EMD.Type = 0 THEN 'I' ELSE 'E' END AS Type,
				ISNULL(EM.Cost,0) AS Cost,
				EM.Date,
				EM.EquipmentId
			FROM EquipmentMaintenanceAct EM
			INNER JOIN EquipmentMaintenanceDefinition EMD
			ON	EMD.Id = EM.EquipmentMaintenanceDefinitionId
			INNER JOIN Equipment E
			ON E.Id = EM.EquipmentId
			WHERE EM.Active = 1
			AND EM.CompanyId = @CompanyId
			AND E.Active = 1

			UNION

			select
			'R',
			CASE WHEN RepairType = 0 THEN 'I' ELSE 'E' END AS Type,
				ISNULL(R.Cost,0) AS Cost,
				Date,
				EquipmentId
			FROM EquipmentRepair R
			INNER JOIN Equipment E
			ON E.Id = R.EquipmentId
			WHERE R.Active = 1
			AND R.CompanyId = @CompanyId
			AND E.Active = 1
END

