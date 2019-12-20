





CREATE PROCEDURE [dbo].[Provider_GetDefinitions]
	@ProviderId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		TOTAL.Id,
		TOTAL.Item,
		TOTAL.Periodicity,
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
			ECD.Id,
			ECD.CompanyId,
			ECD.Periodicity,
			'Calibration' AS Item,
			ECD.Operation,
			ECD.Responsable,
			ECD.Cost,
			ECD.Active,
			ECD.EquipmentId
		FROM EquipmentCalibrationDefinition ECD WITH(NOLOCK)
		WHERE 
			ECD.ProviderId = @ProviderId
		AND ECD.CompanyId = @CompanyId
		AND ECD.Active = 1
		
		UNION	
		
		SELECT
			EVD.Id,
			EVD.CompanyId,
			EVD.Periodicity,
			'Verification' AS Item,
			EVD.Operation,
			EVD.Responsable,
			EVD.Cost,
			EVD.Active,
			EVD.EquipmentId
		FROM EquipmentVerificationDefinition EVD WITH(NOLOCK)
		WHERE 
			EVD.ProviderId = @ProviderId
		AND EVD.CompanyId = @CompanyId
		AND EVD.Active = 1
		
		UNION	
		
		SELECT
			EMD.Id,
			EMD.CompanyId,
			EMD.Periodicity,
			'Maintenance' AS Item,
			EMD.Operation AS Operation,
			EMD.ResponsableId,
			EMD.Cost,
			EMD.Active,
			EMD.EquipmentId
		FROM EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
		WHERE
			EMD.ProviderId = @ProviderId
		AND EMD.CompanyId = @CompanyId
		AND EMD.Active = 1
	) TOTAL
	INNER JOIN Employee EMP WITH(NOLOCK)
	ON	TOTAL.Responsable = EMP.Id
	AND	TOTAL.CompanyId = EMP.CompanyId
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	TOTAL.EquipmentId = E.Id
	AND TOTAL.CompanyId = E.CompanyId
END






