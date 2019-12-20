





CREATE PROCEDURE [dbo].[Equipment_GetVerification]
	@EquipmentId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		EV.Id,
		EV.CompanyId,
		EV.VerificationType,
		EV.CreatedOn,
		EV.Cost,
		EV.Responsable,
		E.Name,
		E.LastName,
		EV.ModifiedBy,
		EMP2.Name,
		EMP2.LastName,
		EV.ModifiedOn
	FROM EquipmentVerificationDefinition EV WITH(NOLOCK)
	INNER JOIN Employee E WITH(NOLOCK)
	ON	E.Id = EV.Responsable
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee EMP2 WITH(NOLOCK)
		ON EMP2.Id = EUA.EmployeeId
	ON	EUA.UserId = EV.ModifiedBy
	WHERE
		EV.Active = 1
	AND EquipmentId = @EquipmentId
END






