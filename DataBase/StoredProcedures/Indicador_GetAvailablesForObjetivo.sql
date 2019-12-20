



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetAvailablesForObjetivo]
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
		0,--I.ObjetivoId,
		'',--ISNULL(O.[Description],'') AS ObjetivoDescription,
		I.MetaComparer,
		I.Meta,
		I.AlarmaComparer,
		I.Alarma,
		ISNULL(I.Calculo,'') AS Calculo,
		I.EndDate,
		ISNULL(I.EndReason,'') AS EndReason,
		I.EndResponsible,
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
		USERRES.[Login],
		I.EndResponsible,
		DELRES.Id,
		DELRES.Name,
		DELRES.LastName,
		USERDEL.[Login],
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
		AND USERRES.[Status] = 1
	ON	EUA.EmployeeId = I.ResponsableId
	
	LEFT JOIN ApplicationUser USERDEL WITH(NOLOCK)
	ON	USERDEL.Id = I.EndResponsible
	LEFT JOIN EmployeeUserAsignation EUA2 WITH(NOLOCK)
		LEFT JOIN Employee DELRES WITH(NOLOCK)
		ON	DELRES.Id = EUA2.EmployeeId
	ON	EUA.UserId = I.EndResponsible
	AND	EMPRES.CompanyId = I.CompanyId
	
	LEFT JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	--LEFT JOIN Objetivo O WITH(NOLOCK)
	--ON	O.Id = I.ObjetivoId
	LEFT JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId
	-- AND (I.ProcessId IS NULL OR I.ProcessId = 0)
	AND I.Active = 1

END




