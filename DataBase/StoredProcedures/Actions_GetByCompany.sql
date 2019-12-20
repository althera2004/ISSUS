





CREATE PROCEDURE [dbo].[Actions_GetByCompany]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		IA.Id,
		IA.CompanyId,
		IA.ActionType,
		IA.Origin,
		D.Id,
		D.Name,
		P.Id,
		P.Description,
		C.Id,
		C.Description,
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
		IA.Monitoring,
		CLOSED.Id,
		CLOSED.Name,
		CLOSED.LastName,
		IA.ClosedOn,
		IA.Notes,
		IA.Active,
		EUA.UserId,
		EMP.Id,
		EMP.Name,
		EMP.LastName,
		IA.ModifiedOn
    FROM IncidentAction IA WITH(NOLOCK)
    INNER JOIN EmployeeUserAsignation EUA WITH(NOLOCK)
		INNER JOIN Employee EMP
		ON	EMP.Id = EUA.EmployeeId
		AND EMP.CompanyId = @CompanyId
	ON EUA.UserId = IA.ModifiedBy
    LEFT JOIN Department D WITH(NOLOCK)
    ON	D.Id = IA.DepartmentId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = IA.ProviderId
    LEFT JOIN Customer C WITH(NOLOCK)
    ON	C.Id = IA.CustomerId
    LEFT JOIN Incident I WITH(NOLOCK)
    ON	I.Id = IA.IncidentId
    LEFT JOIN Employee WH WITH(NOLOCK)
    ON	WH.Id = IA.WhatHappendBy
    LEFT JOIN Employee CAUSES WITH(NOLOCK)
    ON	CAUSES.Id = IA.CausesBy
    LEFT JOIN Employee ACTIONS WITH(NOLOCK)
    ON	ACTIONS.Id = IA.ActionsBy
    LEFT JOIN Employee CLOSED WITH(NOLOCK)
    ON	CLOSED.Id = IA.ClosedBy
END






