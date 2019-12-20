




CREATE PROCEDURE [dbo].[Employee_WithoutUser]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT
		E.Id,
		E.Name,
		E.LastName,
		EUA.*,
		AU.[Status]
	FROM Employee E WITH(NOLOCK)
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser AU WITH(NOLOCK)
		ON AU.Id = EUA.UserId
	ON	EUA.EmployeeId = E.Id

	WHERE
		E.CompanyId = @CompanyId
	AND AU.[Status] = 1
	AND E.Active = 1
	AND E.FechaBaja IS NULL
END





