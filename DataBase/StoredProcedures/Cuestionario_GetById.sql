

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Cuestionario_GetById]
	@Id bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		C.Id,
		C.CompanyId,
		C.[Description],
		C.NormaId,
		R.[Description],
		ISNULL(C.ApartadoNorma,''),
		C.ProcessId,
		P.[Description],
		ISNULL(C.Notes,''),
		C.CreatedBy,
		CB.[Login],
		C.CreatedOn,
		C.ModifiedBy,
		MB.[Login],
		C.ModifiedOn,
		C.Active
	FROM Cuestionario C WITH(NOLOCK)
	INNER JOIN Rules R WITH(NOLOCK)
	ON	R.Id = C.NormaId
	INNER JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = C.ProcessId
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON CB.Id = C.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON MB.Id = C.ModifiedBy

	WHERE
		C.Id = @Id
	AND C.CompanyId = @CompanyId
END

