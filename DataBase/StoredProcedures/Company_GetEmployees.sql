





CREATE PROCEDURE [dbo].[Company_GetEmployees]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,
		E.Active AS Active,
		E.FechaBaja AS FechaBaja,
		ISNULL(E.NIF,'') AS NIF,
		ISNULL(E.Email,'') AS Email,
		ISNULL(E.Phone,'') AS Phone,
		ISNULL(E.Address,'') AS Address,
		ISNULL(E.PostalCode,'') AS PostalCode,
		ISNULL(E.City,'') AS City,
		ISNULL(E.Province,'') AS Province,
		ISNULL(E.Country,0) AS Country,
		CASE WHEN AU.Id IS NULL THEN 0 ELSE 1 END AS HasUserAssigned,
		CASE WHEN ACTIONS.EmployeeId IS NULL THEN 0 ELSE 1 END AS HasActionsAssigned
	FROM Employee E WITH(NOLOCK)
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser AU WITH(NOLOCK)
		ON	AU.Id = EUA.UserId
		AND AU.[Status] = 1
	ON	EUA.EmployeeId = E.Id

	LEFT JOIN
	(
		SELECT
			EQ.Resposable AS EmployeeId
		FROM Equipment EQ WITH(NOLOCK)
		WHERE
			EQ.Active = 1

		UNION

		SELECT
			ECD.Responsable AS EmployeeId
		FROM EquipmentCalibrationDefinition ECD WITH(NOLOCK)
		WHERE
			ECD.Active = 1

		UNION

		SELECT
			EVD.Id AS EmployeeId
		FROM EquipmentVerificationDefinition EVD WITH(NOLOCK)
		WHERE
			EVD.Active = 1

		UNION

		SELECT
			EMD.ResponsableId AS EmployeeId
		FROM EquipmentMaintenanceDefinition EMD WITH(NOLOCK)
		WHERE
			EMD.Active = 1

		UNION

		SELECT
			I.Id AS EmployeeId
		FROM Incident I WITH(NOLOCK)
		WHERE
			I.Active = 1
		AND I.ActionsSchedule > GETDATE()
	) ACTIONS
	ON ACTIONS.EmployeeId = E.Id

	WHERE
		E.CompanyId = @CompanyId
	ANd E.Active = 1
	ORDER BY E.Name ASC, E.LastName
	
END






