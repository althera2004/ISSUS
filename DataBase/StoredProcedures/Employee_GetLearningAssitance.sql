
CREATE PROCEDURE [dbo].[Employee_GetLearningAssitance]
	@EmployeeId bigint
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		LA.Success,
		LA.Completed,
		L.DateStimatedDate,
		L.Id AS LearningId,
		L.Description AS LearningDescription,
		C.Id AS CargoId,
		C.Description AS CargoDescription,
		D.Id AS DepartmentId,
		D.Name AS DepartmentDescription,
		La.Id,
		L.Status AS LearningStatus,
		L.RealFinish AS LearningFinishDate
	FROM LearningAssistant LA WITH(NOLOCK)
	INNER JOIN Learning L WITH(NOLOCK)
	ON	LA.LearningId = L.Id

	AND	LA.CompanyId = L.CompanyId
	LEFT JOIN Cargos C WITH(NOLOCK)
	ON	C.CompanyId = LA.CompanyId
	AND C.Id = LA.CargoId
	LEFT JOIN Department D WITH(NOLOCK)
	ON	D.CompanyId = C.CompanyId
	AND	D.Id = C.DepartmentId
	WHERE
		LA.EmployeeId = @EmployeeId
	AND L.Active = 1


	ORDER BY L.DateStimatedDate ASC
END
