





CREATE PROCEDURE [dbo].[Incident_GetTimed]
	@CompanyId int,
	@Days int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
	*
	FROM Incident I WITH(NOLOCK)
	WHERE
		I.Active = 1
	AND WhatHappendOn >= GETDATE() - @Days
	
END






