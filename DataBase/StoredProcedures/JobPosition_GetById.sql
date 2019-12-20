





CREATE PROCEDURE [dbo].[JobPosition_GetById]
	@Id int,
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
		ISNULL(C.ResponsableId, 0) AS ResponsableId,
		ISNULL(C.Description, '') AS ResponsableDescription,
		C.DepartmentId,
		D.Name AS DepartmentName,
		C.ModifiedOn,
		AU.Id AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName
	FROM Cargos C WITH(NOLOCK)
	INNER JOIN Department D WITH(NOLOCK)
	ON	D.Id = C.DepartmentId
	AND D.CompanyId = C.CompanyId	
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON AU.Id = C.ModifiedBy	
	LEFT JOIN Cargos C2 WITH(NOLOCK)
	ON	C2.Id= C.ResponsableId
	WHERE 
		C.CompanyId = @CompanyId
	AND C.Id = @Id
END






