
CREATE PROCEDURE [dbo].[ScheduleTask_GetByEmployee] 
	@EmployeeId int,
	@CompanyId int
AS
	BEGIN
		-- SET NOCOUNT ON added to prevent extra result sets from
		-- interfering with SELECT statements.
		SET NOCOUNT ON;

		SELECT
			TOTAL.OperationType,
			TOTAL.Id,
			TOTAL.ItemId,
			TOTAL.[Description],
			TOTAL.Vto,
			TOTAL.Operation,
			ISNULL(TOTAL.Code,0) AS Code,
			TOTAL.Action,
			TOTAL.Responsable,
			TOTAL.ProviderId,
			E.[Name] AS EmployeeName,
			E.LastName AS EmployeeLastName,
			P.[Description] AS Provider,
			TOTAL.[Type]
		FROM
		(

		SELECT
			'C' AS OperationType,
			ECA.Id,
			E.Id AS ItemId,
			E.[Description],
			ECA.Vto,
			ECA.Operation,
			ECA.EquipmentCalibrationType AS Code,
			ECD.Id AS Action,
			ECD.Responsable,
			ISNULL(ECD.ProviderId,0) AS ProviderId,
			ECD.[Type]
		FROM EquipmentCalibrationAct ECA
		INNER JOIN Equipment E
		ON	E.Id = ECA.EquipmentId
		AND	E.Active = 1
		AND E.CompanyId = ECA.CompanyId
		AND ECA.Active = 1
		AND E.EndDate IS NULL
		INNER JOIN EquipmentCalibrationDefinition ECD
		ON	ECD.EquipmentId = ECA.EquipmentId
		AND	ECD.Type = ECA.EquipmentCalibrationType
		AND ECD.Active = 1
		WHERE
			ECA.CompanyId = @CompanyId

		UNION 

		SELECT
			'V' AS OperationType,
			EVA.Id,
			E.Id AS ItemId,
			E.[Description],
			EVA.Vto,
			EVA.Operation,
			EVA.EquipmentVerificationType,
			EVD.Id AS Action,
			EVD.Responsable,
			ISNULL(EVD.ProviderId,0) AS ProviderId,
			EVD.VerificationType AS Type
		FROM EquipmentVerificationAct EVA
		INNER JOIN Equipment E
		ON	E.Id = EVA.EquipmentId
		AND	E.Active = 1
		AND E.CompanyId = EVA.CompanyId
		AND EVA.Active = 1
		AND E.EndDate IS NULL
		INNER JOIN EquipmentVerificationDefinition EVD
		ON	EVD.EquipmentId = EVA.EquipmentId
		AND	EVD.VerificationType = EVA.EquipmentVerificationType
		AND EVD.Active = 1
		WHERE
			EVA.CompanyId = @CompanyId
			
		UNION

		SELECT
			'M' AS OperationType,
			EMD.Id,
			E.Id AS ItemId,
			E.[Description],
			EMD.FirstDate,
			EMD.Operation,
			EMA.Id,
			EMD.Id AS Action,
			EMD.ResponsableId,
			ISNULL(EMA.ProviderId,0) AS ProviderId,
			EMD.[Type]
		FROM EquipmentMaintenanceDefinition EMD
		INNER JOIN Equipment E
		ON	E.Id = EMD.EquipmentId
		AND	E.Active = 1
		AND E.CompanyId = EMD.CompanyId
		AND EMD.Active = 1
		AND EMD.FirstDate IS NOT NULL
		LEFT JOIN EquipmentMaintenanceAct EMA
		ON  EMA.EquipmentMaintenanceDefinitionId = EMD.Id
		AND	EMA.Active = 1
		WHERE
			EMD.CompanyId = @CompanyId
		AND EMA.Id IS NULL

		UNION 

		SELECT
			'M' AS OperationType,
			EMA.Id,
			E.Id AS ItemId,
			E.[Description],
			EMA.Vto,
			EMA.Operation,
			EMA.Id,
			EMD.Id AS Action,
			EMD.ResponsableId,
			ISNULL(EMA.ProviderId,0) AS ProviderId,
			EMD.[Type]
		FROM EquipmentMaintenanceAct EMA
		INNER JOIN Equipment E
		ON	E.Id = EMA.EquipmentId
		AND	E.Active = 1
		AND E.CompanyId = EMA.CompanyId
		AND EMA.Active = 1
		ANd E.EndDate IS NULL
		INNER JOIN EquipmentMaintenanceDefinition EMD
		ON  EMA.EquipmentMaintenanceDefinitionId = EMD.Id
		AND	EMD.Active = 1
		WHERE
			EMA.CompanyId = @CompanyId

		UNION

		SELECT
			'A' AS OperationType,
			IA.Id,
			IA.Id AS ItemId,
			IA.[Description],
			IA.ActionsOn AS Vto,
			IA.[Description],
			0,
			0,
			IA.ActionsBy AS Responsable,
			0 AS ProviderId,
			0
		FROM IncidentAction IA WITH(NOLOCK)
		WHERE
			ClosedOn IS NULL
		AND IA.Active = 1
		AND IA.ActionsBy IS NOT NULL
		AND IA.CompanyId = @CompanyId

		UNION 

		SELECT
			'I' AS OperationType,
			I.Id,
			I.Id AS ItemId,
			I.[Description],
			I.ActionsOn AS Vto,
			I.[Description],
			0,
			0,
			I.ActionsBy AS Responsable,
			0 AS ProviderId,
			0 AS Type
		FROM Incident I WITH(NOLOCK)
		WHERE
			ClosedOn IS NULL
		AND I.Active = 1
		AND I.ActionsOn IS NOT NULL
		AND I.CompanyId = @CompanyId

		UNION

		SELECT
			'X' AS OperationType,
			I.Id,
	