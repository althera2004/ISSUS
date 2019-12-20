



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_GetById]
	@IndicadorId int,
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
		ISNULL(EMPRES.Name,''),
		ISNULL(EMPRES.LastName,''),
		ISNULL(USERRES.[Login],'no user'),
		I.EndResponsible,
		DELRES.Id,
		DELRES.Name,
		DELRES.LastName,
		ISNULL(USERDEL.[Login],'') AS Login,
		I.StartDate
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

	LEFT JOIN Employee DELRES WITH(NOLOCK)
	ON	DELRES.Id = I.EndResponsible
	LEFT JOIN EmployeeUserAsignation EUADEL WITH(NOLOCK)
		INNER JOIN ApplicationUser USERDEL WITH(NOLOCK)
		ON	USERDEL.Id = EUADEL.UserId
		AND USERDEL.[Status] = 1
	ON	EUA.EmployeeId = I.EndResponsible
	
	/*LEFT JOIN ApplicationUser USERDEL WITH(NOLOCK)
	ON	USERDEL.Id = I.EndResponsible
	LEFT JOIN EmployeeUserAsignation EUA2 WITH(NOLOCK)
		LEFT JOIN Employee DELRES WITH(NOLOCK)
		ON	DELRES.Id = EUA2.EmployeeId
	ON	EUA.UserId = I.EndResponsible
	AND	EMPRES.CompanyId = I.CompanyId*/
	
	LEFT JOIN Proceso P WITH(NOLOCK)
	ON	P.Id = I.ProcessId
	--LEFT JOIN Objetivo O WITH(NOLOCK)
	--ON	O.Id = I.ObjetivoId
	LEFT JOIN Unidad U WITH(NOLOCK)
	ON	U.Id = I.UnidadId

	WHERE
		I.CompanyId = @CompanyId
	AND I.Id = @IndicadorId

END




