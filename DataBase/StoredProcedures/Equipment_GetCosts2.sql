





CREATE PROCEDURE [dbo].[Equipment_GetCosts2]
	@From Datetime,
	@To Datetime,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON; 

	SELECT
		SUM(D.CI) AS CI,
		SUM(D.CE) AS CE,
		SUM(D.VI) AS VI,
		SUM(D.VE) AS VE,
		SUM(D.MI) AS MI,
		SUM(D.ME) AS ME,
		SUM(D.RI) AS RI,
		SUM(D.RE) AS RE,
		D.Id,
		D.Description,
		D.Active
	FROM
	(
		   select
				CASE WHEN ECA.EquipmentCalibrationType = 0 THEN ISNULL(ECA.Cost,0) ELSE 0 END AS CI,
				CASE WHEN ECA.EquipmentCalibrationType = 1 THEN ISNULL(ECA.Cost,0) ELSE 0 END AS CE,
				0 AS VI,
				0 AS VE,
				0 AS MI,
				0 AS ME,
				0 AS RI,
				0 AS RE,
				ECA.EquipmentId as Id,
				E.Code + '-' + E.[Description] as Description,
				E.Active as Active,
				ECA.EquipmentCalibrationType AS Ext
			FROM EquipmentCalibrationAct ECA WITH(NOLOCK)
			INNER JOIN Equipment E WITH(NOLOCK)
			ON E.Id = ECA.EquipmentId
			AND E.Active = 1
			WHERE ECA.Active = 1
			AND ECA.CompanyId = @CompanyId
			AND (@From IS NULL OR @From <= ECA.Date)
			AND (@To IS NULL OR @To >= ECA.Date)
			AND Cost <> 0

			UNION ALL

			select
				0 AS CI,
				0 AS CE,
				CASE WHEN EquipmentVerificationType = 0 THEN ISNULL(Cost,0) ELSE 0 END AS VI,
				CASE WHEN EquipmentVerificationType = 1 THEN ISNULL(Cost,0) ELSE 0 END AS VE,
				0 AS MI,
				0 AS ME,
				0 AS RI,
				0 AS RE,
				EquipmentId as Id,
				E.Code + '-' + E.[Description] as Description,
				E.Active as Active,
				EVA.EquipmentVerificationType AS Ext
			FROM EquipmentVerificationAct EVA WITH(NOLOCK)
			INNER JOIN Equipment E WITH(NOLOCK)
			ON E.Id = EVA.EquipmentId
			AND E.Active = 1
			WHERE EVA.Active = 1
			AND EVA.CompanyId = @CompanyId
			AND (@From IS NULL OR @From <= EVA.Date)
			AND (@To IS NULL OR @To >= EVA.Date)
			AND Cost <> 0


			UNION ALL

			select
				0 AS CI,
				0 AS CE,
				0 AS VI,
				0 AS VE,
				CASE WHEN EMD.[Type] = 0 THEN ISNULL(EM.Cost,0) ELSE 0 END AS MI,
				CASE WHEN EMD.[Type] = 1 THEN ISNULL(EM.Cost,0) ELSE 0 END AS ME,
				0 AS RI,
				0 AS RE,
				EM.EquipmentId as Id,
				E.Code + '-' + E.[Description] as Description,
				E.Active as active,
				EMD.[Type] AS Ext
			FROM EquipmentMaintenanceAct EM WITH(NOLOCK)
			INNER JOIN EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
			ON	EMD.Id = EM.EquipmentMaintenanceDefinitionId
			INNER JOIN Equipment E WITH(NOLOCK)
			ON E.Id = EM.EquipmentId
			AND E.Active = 1
			WHERE EM.Active = 1
			AND EM.CompanyId = @CompanyId
			AND (@From IS NULL OR @From <= EM.Date)
			AND (@To IS NULL OR @To >= EM.Date)
			AND EM.Cost <> 0


			UNION ALL

			select
				0 AS CI,
				0 AS CE,
				0 AS VI,
				0 AS VE,
				0 AS MI,
				0 AS ME,
				CASE WHEN RepairType = 0 THEN ISNULL(R.Cost,0) ELSE 0 END AS RI,
				CASE WHEN RepairType = 1 THEN ISNULL(R.Cost,0) ELSE 0 END AS RE,
				EquipmentId as id,
				E.Code + '-' + E.[Description],
				E.Active as active,
				RepairType AS Ext
			FROM EquipmentRepair R WITH(NOLOCK)
			INNER JOIN Equipment E WITH(NOLOCK)
			ON E.Id = R.EquipmentId
			AND E.Active = 1
			WHERE R.Active = 1
			AND R.Cost <> 0
			AND R.CompanyId = @CompanyId
			AND (@From IS NULL OR @From <= R.Date)
			AND (@To IS NULL OR @To >= R.Date)

		) AS D

	GROUP BY
		D.Id,
		D.Description,
		D.Active

	HAVING
	SUM(D.CI) + SUM(D.CE)+ SUM(D.VI)+ SUM(D.VE)  +SUM(D.MI)  +SUM(D.ME) +SUM(D.RI) + SUM(D.RE) <> 0


END