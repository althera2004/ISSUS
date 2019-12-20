





CREATE PROCEDURE [dbo].[JobPosition_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		C.Id,
		C.Description,
		ISNULL(C.Responsabilidades,'') AS Responsabilidades,
		ISNULL(C.Notas,'') AS Notas,
		ISNULL(C.FormacionAcademicaDeseada,'') AS FormacionAcademicaDeseada,
		ISNULL(C.FormacionEspecificaDesdeada,'') AS FormacionEspecificaDeseada,
		ISNULL(C.ExperienciaLaboralDeseada,'') AS ExperienciaLaboralDeseada,
		ISNULL(C.HabilidadesDeseadas,'') AS HabilidadesDeseadas,
		C.DepartmentId,
		ISNULL(D.Name,'') AS DepartmentName,
		ISNULL(C2.Id, 0) AS ResponsableId,
		ISNULL(C2.Description,'') AS ResponsableDescription,
		D.Deleted AS DepartmentActive
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN Department D WITH(NOLOCK)		
	ON	D.Id = C.DepartmentId
	AND D.CompanyId = C.CompanyId
	LEFT JOIN Cargos C2 WITH(NOLOCK)
	ON	C2.Id = C.ResponsableId
	ANd	C2.Active = 1
	WHERE 
		C.CompanyId = @CompanyId
	AND C.Active = 1
	ORDER BY C.Description ASC
END






