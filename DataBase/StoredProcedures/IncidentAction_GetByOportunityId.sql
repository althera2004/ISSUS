





CREATE PROCEDURE [dbo].[IncidentAction_GetByOportunityId]
	@OportunityId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.CompanyId,
		IA.ActionType,
		IA.Origin,
		IA.Description,
		IA.DepartmentId,
		IA.ProviderId,
		IA.CustomerId,
		IA.ReporterType,
		IA.BusinessRiskId,
		NULL,
		IA.IncidentId,
		'',--I.Code,
		IA.Number,
		IA.WhatHappend,
		WH.Id,
		WH.Name,
		WH.LastName,
		IA.WhatHappendOn,
		IA.Causes,
		CAUSES.Id,
		CAUSES.Name,
		CAUSES.LastName,
		IA.CausesOn,
		IA.Actions,
		ACTIONS.Id,
		ACTIONS.Name,
		ACTIONS.LastName,
		IA.ActionsOn,
		EXECUTER.Id ExecuterId,
		EXECUTER.Name ExecuterName,
		EXECUTER.LastName ExecuterLastName,
		IA.ActionsSchedule ExecuterSchedule,
		IA.Monitoring,
		CLOSED.Id,
		CLOSED.Name,
		CLOSED.LastName,
		IA.ClosedOn,
		CLOSEDEXECUTOR.Id,
		CLOSEDEXECUTOR.Name,
		CLOSEDEXECUTOR.LastName,
		IA.ClosedExecutorOn,
		IA.Notes,
		IA.Active,
		IA.ModifiedBy,
		AU.[Login],
		IA.ModifiedOn,
		O.Id,
		O.[Description]
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
    LEFT JOIN Oportunity O WITH(NOLOCK)
    ON	O.Id = IA.OportunityId
    AND O.CompanyId = IA.CompanyId
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
    LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = IA.ModifiedBy
	AND AU.CompanyId = IA.CompanyId
	LEFT JOIN Employee CLOSEDEXECUTOR WITH(NOLOCK)
	ON	CLOSEDEXECUTOR.Id = IA.ClosedExecutor
	WHERE
		IA.OportunityId = @OportunityId
	AND IA.CompanyId = @CompanyId
	AND IA.Active = 1
    
END

