




CREATE PROCEDURE [dbo].[Employee_GetActions]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT
		'E' AS AssignationType,
		E.Id AS ItemPageType,
		E.Id AS ItemType,
		E.Description AS ItemDescription
	FROM Equipment E WITH(NOLOCK)
	WHERE
		E.CompanyId = @CompanyId
	AND	E.Active = 1
	AND E.Resposable = @EmployeeId

	UNION

	SELECT
		CASE WHEN ECD.Type = 0 THEN 'ECDI' ELSE 'ECDE' END AS AssignationType,
		ECD.EquipmentId AS ItemPageType,
		ECD.Id AS ItemType,
		E.Description + ' - ' + ECD.Operation AS ItemDescription
	FROM EquipmentCalibrationDefinition ECD WITH(NOLOCK)
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	E.Id = ECD.EquipmentId
	AND	E.Active = 1
	WHERE
		ECD.CompanyId = @CompanyId
	AND	ECD.Active = 1
	AND ECD.Responsable = @EmployeeId

	UNION

	SELECT
		CASE WHEN EVD.VerificationType = 0 THEN 'EVDI' ELSE 'EVDE' END AS AssignationType,
		EVD.EquipmentId AS ItemPageType,
		EVD.Id AS ItemType,
		E.Description + ' - ' + EVD.Operation AS ItemDescription
	FROM EquipmentVerificationDefinition EVD WITH(NOLOCK)
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	E.Id = EVD.EquipmentId
	AND	E.Active = 1
	WHERE
		EVD.CompanyId = @CompanyId
	AND	EVD.Active = 1
	AND EVD.Responsable = @EmployeeId

	UNION

	SELECT
		CASE WHEN EMD.Type = 0 THEN 'EMDI' ELSE 'EMDE' END AS AssignationType,
		EMD.EquipmentId AS ItemPageType,
		EMD.Id AS ItemType,
		E.Description + ' - ' + EMD.Operation AS ItemDescription
	FROM EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
	INNER JOIN Equipment E WITH(NOLOCK)
	ON	E.Id = EMD.EquipmentId
	AND	E.Active = 1
	WHERE
		EMD.CompanyId = @CompanyId
	AND	EMD.Active = 1
	AND EMD.ResponsableId = @EmployeeId

	UNION

	SELECT
		'IAE',
		I.Id AS ItemTypePage,
		I.Id AS ItemId,
		RIGHT('0000' + CAST(I.Code AS NVARCHAR(5)),5) + ' - ' + I.Description AS ItemDescription
	FROM Incident I WITH(NOLOCK)
	WHERE
		I.CompanyId = @CompanyId
	AND I.Active = 1
	AND I.ActionsExecuter = @EmployeeId
	AND I.ActionsSchedule > GETDATE()
END





