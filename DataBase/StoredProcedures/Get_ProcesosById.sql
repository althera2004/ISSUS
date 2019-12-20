





CREATE PROCEDURE [dbo].[Get_ProcesosById]
	@Id int,
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
		P.ModifiedBy,
		P.ModifiedOn
	FROM Proceso P WITH(NOLOCK)
	WHERE
		P.Id = @Id
	AND	P.CompanyId = @CompanyId
END






