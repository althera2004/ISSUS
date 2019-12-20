





CREATE PROCEDURE [dbo].[Learning_GetAll]
	@CompanyId int,
	@Year int,
	@Status int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		L.Id,
		L.Description,
		LA.Id,
		E.Id,
		ISNULL(E.Name,'') AS Name,
		ISNULL(E.LastName,'') AS LastName,
		'' AS SecondLastName,
		LA.Completed,
		LA.Success,
		L.DateStimatedDate
	FROM Learning L WITH(NOLOCK)
	INNER JOIN LearningAssistant LA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = LA.EmployeeId
		AND E.CompanyId = LA.CompanyId
	ON	LA.LearningId = L.Id
	AND	LA.CompanyId = L.CompanyId
	WHERE
		L.CompanyId = @CompanyId
	AND (@Year IS NULL OR L.Year = @Year)
	AND (@Status IS NULL OR L.Status = @Status)
END






