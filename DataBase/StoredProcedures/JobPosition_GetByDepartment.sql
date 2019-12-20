





CREATE PROCEDURE [dbo].[JobPosition_GetByDepartment]
	@DepartmentId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		C.Id,
		C.Description,		
		D.Id AS DepartmentId,
		D.Name AS DepartmentName,
		ISNULL(C2.Id,0) AS ResponsableId,
		ISNULL(C2.Description,'') AS ResponsableDescription
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN Department D WITH(NOLOCK) 
	ON	D.CompanyId = C.CompanyId
	AND D.Id = @DepartmentId
	LEFT JOIN Cargos C2 WITH(NOLOCK)
	ON	C.ResponsableId = C2.Id
	AND C2.CompanyId = C.CompanyId
	WHERE
		C.CompanyId = @CompanyId
	AND C.DepartmentId = @DepartmentId
END






