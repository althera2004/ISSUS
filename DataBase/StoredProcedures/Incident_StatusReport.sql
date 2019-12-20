





CREATE PROCEDURE [dbo].[Incident_StatusReport]
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	
	
		SELECT
			I.Id AS IncidentId,
			I.WhatHappendOn AS OpenDate,
			I.CausesOn AS CausesDate,
			I.ActionsOn AS ActionsDate,
			ISNULL(I.DepartmentId,0) AS DepartmentId,
			ISNULL(D.Name,'') AS DepartmentName,
			ISNULL(P.Id,0) AS ProviderId,
			ISNULL(P.Description,'') AS ProviderDescription,
			ISNULL(C.Id,0) AS CustomerId,
			ISNULL(C.Description,'') AS CustomerDescription,
			I.Description,
			I.Code,
			I.ClosedOn AS CloseDate,
			I.Origin
				
		FROM Incident I WITH(NOLOCK)
		LEFT JOIN Department D WITH(NOLOCK)
		ON	D.Id = I.DepartmentId
		AND D.CompanyId = I.CompanyId
		LEFT JOIN Provider P WITH(NOLOCK)
		ON	P.Id = I.ProviderId
		AND P.CompanyId = I.CompanyId
		LEFT JOIN Customer C WITH(NOLOCK)
		ON	C.Id = CustomerId
		AND	C.CompanyId = I.CompanyId
		LEFT JOIN IncidentAction IA WITH(NOLOCK)
		ON	IA.IncidentId = I.Id
		AND	IA.CompanyId = I.CompanyId
		AND IA.Active = 1
		LEFT JOIN IncidentCost IC WITH(NOLOCK)
		ON	IC.IncidentId = I.Id
		AND	IC.CompanyId = I.CompanyId
		AND IC.Active = 1
		WHERE
			I.CompanyId = @CompanyId
		AND I.Active = 1
		
		GROUP BY
			I.Id ,
			I.WhatHappendOn,
			I.DepartmentId,
			D.Name,
			P.Id,
			P.Description,
			C.Id,
			C.Description,
			I.Description,
			I.Code,
			I.ClosedOn,
			IA.Id,
			IA.Number,
			I.Origin,
			I.ActionsOn,
			I.CausesOn
END






