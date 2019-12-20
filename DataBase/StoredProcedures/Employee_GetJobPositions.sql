





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Employee_GetJobPositions]
	@EmployeeId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		ECA.FechaAlta,
		ECA.FechaBaja,
		C.Id,
		C.DepartmentId,
		C.Description,
		E.Id,
		E.Name,
		E.LastName,
		'',
		D.Name
	FROM EmployeeCargoAsignation ECA WITH(NOLOCK)
	INNER JOIN Cargos C WITH(NOLOCK)
	ON	ECA.CompanyId = C.CompanyId
	AND ECA.CargoId = C.Id
	AND C.Active = 1
	INNER JOIN Employee E WITH(NOLOCK)
	ON	ECA.EmployeeId = E.Id
	AND C.CompanyId = E.CompanyId
	INNER JOIN Department D WITH(NOLOCK)
	ON	C.DepartmentId = D.Id
	AND C.CompanyId = D.CompanyId
	WHERE
		ECA.EmployeeId = @EmployeeId
	AND ECA.CompanyId = @CompanyId
	AND	C.CompanyId = E.CompanyId
	ORDER BY ECA.FechaAlta ASC
	
END






