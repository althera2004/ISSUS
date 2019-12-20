





CREATE PROCEDURE [dbo].[Process_GetById]
	@Id bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
    
	SELECT 
		P.Id,
		P.CompanyId,
		P.Type,
		ISNULL(P.Inicio,'') AS Inicio,
		ISNULL(P.Desarrollo,'') AS Desarrollo,
		ISNULL(P.Fin,'') AS Fin,
		ISNULL(P.Description,'') AS Description,
		P.CargoId,
		P.ModifiedOn,
		P.ModifiedBy AS ModifiedByUserId,
		UA.Login AS ModifiedByUserName,
		P.Active AS Active,
		1 AS Deletable,
		P.DisabledBy,
		ISNULL(DB.[Login],'') AS DisabledByUserName,
		P.DisabledOn
	FROM Proceso P WITH(NOLOCK)
	INNER JOIN ApplicationUser UA WITH(NOLOCK)
	ON	UA.Id = P.ModifiedBy
	LEFT JOIN ApplicationUser DB WITH(NOLOCK)
	ON	DB.id = P.DisabledBy
	
	WHERE
		P.Id = @Id
	AND	P.CompanyId = @CompanyId

END