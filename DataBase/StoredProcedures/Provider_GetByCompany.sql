





CREATE PROCEDURE [dbo].[Provider_GetByCompany]
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
		AU.[Login] AS ModifiedByName,
		--'' AS ModifiedByLastName,
		CASE WHEN ECA.Id IS NULL THEN 0 ELSE 1 END AS InCalibrationAct,
		CASE WHEN ECD.Id IS NULL THEN 0 ELSE 1 END AS InCalibrationDefinition,
		CASE WHEN EMA.Id IS NULL THEN 0 ELSE 1 END AS InMaintenanceAct,
		CASE WHEN EMD.Id IS NULL THEN 0 ELSE 1 END AS InMaintenanceDefinition,
		CASE WHEN R.Id IS NULL THEN 0 ELSE 1 END AS InRepair,
		CASE WHEN I.Id IS NULL THEN 0 ELSE 1 END AS InIncident,
		CASE WHEN IA.Id IS NULL THEN 0 ELSE 1 END AS InIncidentAction		
	FROM Provider J WITH(NOLOCK)
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = J.ModifiedBy
	LEFT JOIN EquipmentCalibrationAct ECA WITH(NOLOCK)
	ON	ECA.ProviderId = J.Id
	AND	ECA.Active = 1
	LEFT JOIN EquipmentCalibrationDefinition ECD WITH(NOLOCK)
	ON	ECD.ProviderId = J.Id
	AND ECD.Active = 1
	LEFT JOIN EquipmentMaintenanceAct EMA WITH(NOLOCK)
	ON	EMA.ProviderId = J.Id
	AND	EMA.Active = 1
	LEFT JOIN EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
	ON	EMD.ProviderId = J.Id
	AND EMD.Active = 1
	LEFT JOIN EquipmentRepair R WITH(NOLOCK)
	ON	R.ProviderId = J.Id
	AND R.Active = 1
	LEFT JOIN Incident I WITH(NOLOCK)
	ON	I.ProviderId = J.Id
	AND I.Active = 1
	LEFT JOIN IncidentAction IA WITH(NOLOCK)
	ON	IA.ProviderId = J.Id
	AND	IA.Active = 1
	
	WHERE
		J.CompanyId = @CompanyId

	ORDER BY
		J.Description ASC
END






