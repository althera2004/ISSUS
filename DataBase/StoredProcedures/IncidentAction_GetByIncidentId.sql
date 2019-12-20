





CREATE PROCEDURE [dbo].[IncidentAction_GetByIncidentId]
	@IncidentId bigint,
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
		0,--BR.Code,
		IA.IncidentId,
		I.Code,
		IA.Number,
		IA.WhatHappend,
		WH.Id AS WhatHappendById,
		WH.Name AS WhatHappendByName,
		WH.LastName AS WhatHappendByLastName,
		IA.WhatHappendOn,
		IA.Causes,
		CAUSES.Id AS CausesById,
		CAUSES.Name AS CausesByName,
		CAUSES.LastName AS CausesByLastName,
		IA.CausesOn,
		IA.Actions,
		ACTIONS.Id AS ActionsById,
		ACTIONS.Name AS ActionsByName,
		ACTIONS.LastName AS ActionsByLastName,
		IA.ActionsOn,
		EXECUTER.Id ExecuterId,
		EXECUTER.Name ExecuterName,
		EXECUTER.LastName ExecuterLastName,
		IA.ActionsSchedule ExecuterSchedule,
		IA.Monitoring,
		CLOSED.Id AS ClosedById,
		CLOSED.Name AS ClosedByIdName,
		CLOSED.LastName AS ClosedByIdLastName,
		IA.ClosedOn,
		CLOSEDEXECUTOR.Id AS ClosedExecutorId,
		CLOSEDEXECUTOR.Name AS ClosedExecutorName,
		CLOSEDEXECUTOR.LastName AS ClosedExecutorLastName,
		IA.ClosedExecutorOn,
		IA.Notes,
		IA.Active,
		IA.ModifiedBy AS ModifiedByUserId,
		AU.Login AS ModifiedByUserName,
		IA.ModifiedOn,
		IA.ReporterType
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
		IA.IncidentId = @IncidentId
	AND IA.CompanyId = @CompanyId
	AND IA.Active = 1
    
END






