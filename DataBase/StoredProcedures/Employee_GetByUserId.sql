





CREATE PROCEDURE [dbo].[Employee_GetByUserId]
	@USerId bigint
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		E.Id,
		E.Name,
		E.LastName,
		ISNULL(E.Email,'') AS Email,
		ISNULL(E.Phone,'') AS Phone,
		ISNULL(E.NIF,'') AS Nif,
		ISNULL(E.Address,'') AS Address,
		ISNULL(E.PostalCode,'') AS PostalCode,
		ISNULL(E.City,'') AS City,
		ISNULL(E.Country,'') AS Country,
		ISNULL(E.Province,'') AS Province
	FROM Employee E WITH(NOLOCK)
	INNER JOIN EmployeeUserAsignation EUA3 WITH(NOLOCK)
	ON	EUA3.EmployeeId = E.Id
	WHERE
		EUA3.UserId = @UserId
	
END






