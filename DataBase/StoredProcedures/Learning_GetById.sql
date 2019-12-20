





CREATE PROCEDURE [dbo].[Learning_GetById]
	@LearningId int,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		L.Id,
		L.CompanyId,
		ISNULL(L.Description,'') AS Description,
		L.DateStimatedDate,
		L.Hours,
		L.Amount,
		L.Master,
		ISNULL(L.Notes,'') AS Notes,
		L.Status,
		L.Year,
		L.RealStart,
		L.RealFinish,
		ISNULL(L.Objetivo,'') AS Objetivo,
		ISNULL(L.Metodologia,'') AS Metodologia,
		L.ModifiedOn,
		L.ModifiedBy AS ModifiedByUserId,
		ISNULL(AU.[Login],'')
	FROM Learning L WITH(NOLOCK)
	LEFT JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = L.ModifiedBy
	WHERE
		L.CompanyId = @CompanyId
	AND L.Id = @LearningId
END






