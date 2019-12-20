





CREATE PROCEDURE [dbo].[Learning_GetAssistance]
	@LearningId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		LA.Id,
		E.Id,
		ISNULL(E.Name,'') AS Name,
		ISNULL(E.LastName,'') AS LastName,
		LA.Completed,
		LA.Success,
		L.DateStimatedDate,
		ISNULL(LA.CargoId,0) AS JobPositionId,
		ISNULL(C.Description,'') AS JobPositionDescription
	FROM Learning L WITH(NOLOCK)
	INNER JOIN LearningAssistant LA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = LA.EmployeeId
		AND E.CompanyId = LA.CompanyId
	ON	LA.LearningId = L.Id
	AND	LA.CompanyId = L.CompanyId
	LEFT JOIN Cargos C WITH(NOLOCK)
	ON	C.Id = LA.CargoId
	AND C.CompanyId = La.CompanyId
	WHERE
		L.CompanyId = @CompanyId
	AND L.Id = @LearningId
END






