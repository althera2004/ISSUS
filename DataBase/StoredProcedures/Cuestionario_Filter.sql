

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Cuestionario_Filter]
	@CompanyId int,
	@ProcessId bigint,
	@RuleId bigint,
	@ApartadoNorma nvarchar(50)
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
		C.Active,
		COUNT(CP.Id)
	FROM Cuestionario C WITH(NOLOCK)
	INNER JOIN Rules R WITH(NOLOCK)
	ON	R.Id = C.NormaId
	INNER JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = C.ProcessId
	LEFT JOIN CuestionarioPregunta CP WITH(NOLOCK)
	ON	CP.CuestionarioId = C.Id
	AND CP.CompanyId = C.CompanyId
	AND CP.Active = 1
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON CB.Id = C.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON MB.Id = C.ModifiedBy

	WHERE
		C.CompanyId = @CompanyId
	AND (C.NormaId = @RuleId OR @RuleId < 1)
	AND (C.ProcessId = @ProcessId OR @ProcessId < 1)
	AND (C.ApartadoNorma like '%' + @ApartadoNorma + '%' OR @ApartadoNorma = '')

	GROUP BY
		C.Id,
		C.CompanyId,
		C.[Description],
		C.NormaId,
		R.[Description],
		C.ApartadoNorma,
		C.ProcessId,
		P.[Description],
		C.Notes,
		C.CreatedBy,
		CB.[Login],
		C.CreatedOn,
		C.ModifiedBy,
		MB.[Login],
		C.ModifiedOn,
		C.Active

END

