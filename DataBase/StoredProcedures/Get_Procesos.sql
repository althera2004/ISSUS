





CREATE PROCEDURE [dbo].[Get_Procesos]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    
	SELECT DISTINCT
		P.Id,
		P.CompanyId,
		P.Type,
		ISNULL(P.Inicio,'') AS Inicio,
		ISNULL(P.Desarrollo,'') AS Desarrollo,
		ISNULL(P.Fin,'') AS Fin,
		ISNULL(P.Description,'') AS Description,
		C.Id AS CargoId,
		C.Description AS CargoDescription,
		CASE WHEN I.Id IS NULL AND Q.Id IS NULL THEN CAST(1 AS bit) ELSE CAST(0 AS Bit) END AS CanBeDeleted,
		P.DisabledBy,
		ISNULL(DB.[Login],'') AS DisabledByUserName,
		P.DisabledOn
	FROM Proceso P WITH(NOLOCK)
	LEFT JOIN Cargos C WITH(NOLOCK)
	ON	C.Id = P.CargoId
	AND C.Active = 1
	LEFT JOIN Indicador I WITH(NOLOCK)
	ON	I.ProcessId = P.Id
	AND	I.Active = 1
	LEFT JOIN Cuestionario Q WITH(NOLOCK)
	ON	Q.ProcessId = P.Id
	AND	Q.Active = 1
	LEFT JOIN ApplicationUser DB WITH(NOLOCK)
	ON	DB.id = P.DisabledBy
	WHERE
		P.CompanyId = @CompanyId
	AND P.Active = 1
END






