





CREATE PROCEDURE [dbo].[Incident_GetById]
	@IncidentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		I.Id,
		I.CompanyId,
		I.Code,
		I.Origin,
		ISNULL(D.Id,0) AS DepartmentId,
		ISNULL(D.Name,'') AS DepartmentName,
		ISNULL(P.Id,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderName,
		ISNULL(C.Id,0) AS CustomerId,
		ISNULL(C.Description,'') AS CustomerName,
		I.WhatHappend,
		ISNULL(WH.Id,0) AS WhatHappendById,
		ISNULL(WH.Name,'') AS WhatHappendByName,
		ISNULL(WH.LastName,'') AS WhatHappendByLastname,
		I.WhatHappendOn,
		I.Causes,
		ISNULL(CAUSES.Id,0) AS WhatCausesById,
		ISNULL(CAUSES.Name,'') AS WhatCausesByName,
		ISNULL(CAUSES.LastName,'') AS WhatCausesByLastname,
		I.CausesOn,
		I.Actions,
		ISNULL(ACTIONS.Id,0) AS ActionsById,
		ISNULL(ACTIONS.Name,'') AS ActionsName,
		ISNULL(ACTIONS.LastName,'') AS ActionsLastname,
		I.ActionsOn,
		EXECUTER.Id,
		EXECUTER.Name,
		EXECUTER.LastName,
		I.ActionsSchedule,
		I.ApplyAction,
		ISNULL(CLOSED.Id,0) AS ClosedById,
		ISNULL(CLOSED.Name,'') AS ClosedByName,
		ISNULL(CLOSED.LastName,'') AS ClosedLastName,
		I.Active,
		I.ModifiedBy AS ModifiedByUserId,		
		AU.[Login] AS ModifiedByUserName,
		I.ModifiedOn,
		I.Description,
		I.Notes,
		I.ClosedOn,
		I.Anotations
    FROM Incident I WITH(NOLOCK)
    LEFT JOIN Department D WITH(NOLOCK)
    ON	D.Id = I.DepartmentId
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = I.ProviderId
    LEFT JOIN Customer C WITH(NOLOCK)
    ON	C.Id = I.CustomerId
    LEFT JOIN Employee WH WITH(NOLOCK)
    ON	WH.Id = WhatHappendBy
    LEFT JOIN Employee CAUSES WITH(NOLOCK)
    ON	CAUSES.Id = CausesBy
    LEFT JOIN Employee ACTIONS WITH(NOLOCK)
    ON	ACTIONS.Id = I.ActionsBy
    LEFT JOIN Employee EXECUTER WITH(NOLOCK)
    ON	EXECUTER.Id = I.ActionsExecuter
    AND EXECUTER.CompanyId = I.CompanyId
    LEFT JOIN Employee CLOSED WITH(NOLOCK)
    ON	CLOSED.Id = I.ClosedBy
    LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = I.ModifiedBy
    WHERE
		I.Id = @IncidentId
	AND I.CompanyId = @CompanyId
    
END






