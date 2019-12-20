





CREATE PROCEDURE [dbo].[IncidentAction_GetById]
	@IncidentActionId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.CompanyId,
		IA.ActionType,
		IA.Origin,
		IA.[Description],
		IA.DepartmentId,
		IA.ProviderId,
		IA.CustomerId,
		IA.ReporterType,
		IA.BusinessRiskId,
		BR.Code,
		IA.IncidentId,
		I.Code,
		IA.Number,
		IA.WhatHappend,
		IA.WhatHappendBy AS WhatHappendResponsibleId,
		ISNULL(WH.Name,'') AS WhatHappendResponsibleName,
		ISNULL(WH.LastName,'') AS WhatHappendResponsibleLastName,
		IA.WhatHappendOn,
		IA.Causes,
		CAUSES.Id,
		CAUSES.Name,
		CAUSES.LastName,
		IA.CausesOn,
		IA.Actions,
		ACTIONS.Id AS ActionsResponsibleId,
		ISNULL(ACTIONS.Name,'') AS ActionsResponsibleName,
		ISNULL(ACTIONS.LastName,'') AS ActionsResponsibleLastName,
		IA.ActionsOn,
		EXECUTER.Id ExecuterId,
		ISNULL(EXECUTER.Name,'') ExecuterName,
		ISNULL(EXECUTER.LastName,'') ExecuterLastName,
		IA.ActionsSchedule ExecuterSchedule,
		ISNULL(IA.Monitoring,''),
		CLOSED.Id AS ClosedById,
		ISNULL(CLOSED.Name,'') AS ClosedByName,
		ISNULL(CLOSED.LastName,'') AS ClosedByLastName,
		IA.ClosedOn,
		CLOSEDEXECUTOR.Id,
		CLOSEDEXECUTOR.Name,
		CLOSEDEXECUTOR.LastName,
		IA.ClosedExecutorOn,
		ISNULL(IA.Notes,''),
		IA.Active,
		IA.ModifiedBy,
		AU.[Login],
		IA.ModifiedOn,
		IA.ObjetivoId,
		ISNULL(OB.[Name],''),
		IA.OportunityId,
		ISNULL(O.[Description],''),
		ISNULL(IA.AuditoryId,0) AS AuditoryId
    FROM IncidentAction IA WITH(NOLOCK)
    LEFT JOIN IncidentActionType IAT WITH(NOLOCK)
    ON	IAT.Id = IA.ActionType
    LEFT JOIN Department D WITH(NOLOCK)
    ON	D.Id = IA.DepartmentId
    AND	D.Id = IA.CompanyId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = IA.ProviderId
    AND P.CompanyId= IA.CompanyId
    LEFT JOIN Customer C WITH(NOLOCK)
    ON	C.Id = IA.CustomerId
    AND	C.CompanyId = IA.CompanyId
    LEFT JOIN Incident I WITH(NOLOCK)
    ON	I.Id = IA.IncidentId
    AND I.CompanyId = IA.CompanyId
	LEFT JOIN BusinessRisk3 BR WITH(NOLOCK)
	ON	BR.Id = IA.BusinessRiskId
	AND I.CompanyId = Ia.CompanyId
	LEFT JOIN Objetivo OB WITH(NOLOCK)
	ON	OB.Id = IA.ObjetivoId
	AND	OB.CompanyId = IA.CompanyId
	LEFT JOIN Oportunity O WITH(NOLOCK)
	ON	O.Id = IA.OportunityId
	AND	O.CompanyId = IA.CompanyId
    LEFT JOIN Employee WH WITH(NOLOCK)
    ON	WH.Id = IA.WhatHappendBy
    AND	WH.CompanyId = IA.CompanyId
    LEFT JOIN Employee CAUSES WITH(NOLOCK)
    ON	CAUSES.Id = IA.CausesBy
    AND CAUSES.CompanyId = IA.CompanyId
    LEFT JOIN Employee ACTIONS WITH(NOLOCK)
    ON	ACTIONS.Id = IA.ActionsBy
    AND ACTIONS.CompanyId = IA.CompanyId
    LEFT JOIN Employee EXECUTER WITH(NOLOCK)
    ON	EXECUTER.Id = IA.ActionsExecuter
    AND EXECUTER.CompanyId = IA.CompanyId
    LEFT JOIN Employee CLOSED WITH(NOLOCK)
    ON	CLOSED.Id = IA.ClosedBy
    AND CLOSED.CompanyId = IA.CompanyId    
    LEFT JOIN Employee CLOSEDEXECUTOR WITH(NOLOCK)
    ON	CLOSEDEXECUTOR.Id = IA.ClosedBy
    AND CLOSEDEXECUTOR.CompanyId = IA.CompanyId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = IA.ModifiedBy
	WHERE
		IA.Id = @IncidentActionId
	AND IA.CompanyId = @CompanyId
	--AND IA.Active = 1
    
END





