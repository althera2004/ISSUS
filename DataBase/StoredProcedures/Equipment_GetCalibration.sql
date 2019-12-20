





CREATE PROCEDURE [dbo].[Equipment_GetCalibration]
	@EquipmentId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		EC.Id,
		EC.CompanyId,
		EC.EquipmentCalibrationType,
		EC.Date,
		EC.Cost,
		EC.ProviderId,
		P.Description,
		EC.Responsable,
		E.Name,
		E.LastName,
		EC.ModifiedBy,
		EMP2.Name,
		EMP2.LastName,
		EC.ModifiedOn
	FROM EquipmentCalibrationAct EC WITH(NOLOCK)
	INNER JOIN Provider P WITH(NOLOCK)
	ON	P.Id = EC.ProviderId
	INNER JOIN Employee E WITH(NOLOCK)
	ON	E.Id = EC.Responsable
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee EMP2 WITH(NOLOCK)
		ON EMP2.Id = EUA.EmployeeId
	ON	EUA.UserId = EC.ModifiedBy
	WHERE
		EquipmentId = @EquipmentId
END






