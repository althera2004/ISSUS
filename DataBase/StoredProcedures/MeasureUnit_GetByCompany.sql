





CREATE PROCEDURE [dbo].[MeasureUnit_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT
		J.Id,
		J.CompanyId,
		J.Description,
		J.Active,
		J.ModifiedOn,
		J.ModifiedBy AS ModifiedByUserId,
		E.Id AS ModifiedByEmployeeId,
		ISNULL(E.Name,'') AS ModifiedByName,
		ISNULL(E.LastName,'') AS ModifiedByLastName,
		P1.Type
	FROM MeasureUnit J WITH(NOLOCK)
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee E WITH(NOLOCK)
		ON	E.Id = EUA.EmployeeId
		AND E.CompanyId = EUA.CompanyId
	ON	EUA.UserId = J.ModifiedBy
	LEFT JOIN 
	(
		SELECT P.Type FROM Proceso P WITH(NOLOCK)
	) P1
	ON P1.Type = J.Id
	
	WHERE
		J.CompanyId = @CompanyId
END






