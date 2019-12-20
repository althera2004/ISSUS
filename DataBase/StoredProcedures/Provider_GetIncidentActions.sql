





CREATE PROCEDURE [dbo].[Provider_GetIncidentActions]
	@ProviderId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		Item.*
	FROM
	(
		SELECT
			'Incident' AS ItemType,
			I.Id,
			I.Description,
			I.WhatHappendOn,
			I.CausesOn,
			I.ActionsOn,
			I.ClosedOn,
			-1 AS Origin,
			ISNULL(IA.Id,'') AS AssociantedId,
			ISNULL(IA.Description,'') AS AssociatedDescription,
			I.Code AS IncidentCode,
			ISNULL(IA.Number,0) AS ActionCode
		FROM Incident I WITH(NOLOCK)
		LEFT JOIN IncidentAction IA WITH(NOLOCK)
		ON	IA.IncidentId = I.Id
		AND	IA.CompanyId = I.CompanyId
		AND IA.Active = 1
		WHERE
			I.ProviderId = @ProviderId
		AND	I.CompanyId = @CompanyId
		AND I.Active = 1
		
		UNION
		
		
		SELECT
			'Action' AS ItemType,
			IA.Id,
			IA.Description,
			IA.WhatHappendOn,
			IA.CausesOn,
			IA.ActionsOn,
			IA.ClosedOn,
			IA.Origin,
			ISNULL(I.Id,0) AS AssociantedId,
			ISNULL(I.Description,'') AS AssociatedDescription,
			ISNULL(I.Code,0) AS IncidentCode,
			IA.Number AS ActionCode
		FROM IncidentAction IA WITH(NOLOCK)
		LEFT JOIN Incident I WITH(NOLOCK)
		ON	I.Id = IA.IncidentId
		AND I.CompanyId = Ia.CompanyId
		WHERE
			IA.ProviderId = @ProviderId
		AND	IA.CompanyId = @CompanyId
		AND IA.Active = 1
	) AS Item
	
END






