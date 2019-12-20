



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetAll]
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		I.Id,
		I.CompanyId,
		I.Descripcion,
		CASE WHEN I.ProcessId < 1 THEN NULL ELSE I.ProcessId END AS ProcessId,
		P.[Description],
		0,--I.ObjetivoId,
		'',--O.[Description],
		I.MetaComparer,
		I.Meta,
		I.AlarmaComparer,
		I.Alarma,
		ISNULL(I.Calculo,'') AS Calculo,
		I.EndDate,
		ISNULL(I.EndReason,'') AS EndReason,
		CASE WHEN I.EndResponsible  <0 THEN NULL ELSE I.EndResponsible END AS EndResponsible,
		I.UnidadId,
		ISNULL(U.[Description],'') AS UnidadDescripcion,
		I.Periodicity,
		I.CreatedBy,
		CB.[Login] AS CreatedByName,
		I.CreatedOn,
		I.ModifiedBy,
		MB.[Login] AS ModifiedByName,
		I.ModifiedOn,
		I.Active,
		I.ResponsableId,
		EMPRES.Id,
		EMPRES.Name,
		EMPRES.LastName,
		ISNULL(USERRES.[Login],''),
		CASE WHEN I.EndResponsible  <0 THEN NULL ELSE I.EndResponsible END AS EndResponsible,
		EMPDEL.Id,
		EMPDEL.Name,
		EMPDEL.LastName,
		DELRES.[Login],
		I.Startdate
	FROM Indicador I WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = I.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = I.ModifiedBy

	INNER JOIN Employee EMPRES WITH(NOLOCK)
	ON	EMPRES.Id = I.ResponsableId
	LEFT JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser USERRES WITH(NOLOCK)
		ON	USERRES.Id = EUA.UserId
		AND USERRES.[Status] = 1
	ON	EUA.EmployeeId = I.ResponsableId
	
	LEFT JOIN Employee EMPDEL WITH(NOLOCK)
	ON	EMPDEL.Id = I.EndResponsible
	LEFT JOIN EmployeeUserAsignation EUA2 WITH(NOLOCK)
		LEFT JOIN ApplicationUser DELRES WITH(NOLOCK)
		ON	DELRES.Id = EUA2.UserId
	ON	EUA2.EmployeeId = I.EndResponsible

	LEFT JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	--LEFT JOIN Objetivo O WITH(NOLOCK)
	--ON	O.Id = I.ObjetivoId
	LEFT JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId

END




