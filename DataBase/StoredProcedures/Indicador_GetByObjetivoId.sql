



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetByObjetivoId]
	@ObjetivoId int,
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
		I.ProcessId,
		P.[Description],
		-1,
		'',
		I.MetaComparer,
		I.Meta,
		I.AlarmaComparer,
		I.Alarma,
		ISNULL(I.Calculo,'') AS Calculo,
		NULL,
		'' AS EndReason,
		-1,
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
		EMPRES.Id AS ResponsableEmpoyeeId,
		EMPRES.Name AS ResponsableEmpoyeeId,
		EMPRES.LastName AS ResponsableEmpoyeeId,
		USERRES.[Login],
		-1,
		-1,
		'',
		'',
		'',
		I.StartDate
	FROM Indicador I WITH(NOLOCK)
	INNER JOIN ApplicationUser CB WITH(NOLOCK)
	ON	CB.Id = I.CreatedBy
	INNER JOIN ApplicationUser MB WITH(NOLOCK)
	ON	MB.Id = I.ModifiedBy

	INNER JOIN Employee EMPRES WITH(NOLOCK)
	ON	EMPRES.Id = I.ResponsableId
	INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN ApplicationUser USERRES WITH(NOLOCK)
		ON	USERRES.Id = EUA.UserId
		ANd	USERRES.[Status] = 1
	ON	EUA.EmployeeId = I.ResponsableId
	
	INNER JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	INNER JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId
	--D I.ObjetivoId = @ObjetivoId
	AND I.EndDate IS NULL
	AND I.Active = 1

END



