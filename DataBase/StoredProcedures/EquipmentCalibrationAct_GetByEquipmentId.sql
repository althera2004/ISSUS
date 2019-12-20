





CREATE PROCEDURE [dbo].[EquipmentCalibrationAct_GetByEquipmentId]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		ECA.Id,
		ECA.CompanyId,
		ECA.EquipmentId,
		ECA.EquipmentCalibrationType,
		ECA.Date,
		ECA.Vto,
		ECA.Result,
		ECA.MaxResult,
		ECA.Cost,
		ISNULL(ECA.ProviderId,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderName,
		ECA.Responsable AS ResponsableId,
		RESP.Name AS ResponsableName,
		RESP.LastName AS ResponsableLastName,
		ECA.Active,
		ECA.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		ECA.ModifiedOn
    FROM EquipmentCalibrationAct ECA WITH(NOLOCK)
    INNER JOIN Employee RESP WITH(NOLOCK)
    ON	RESP.Id = ECA.Responsable
    LEFT JOIN Provider P WITH(NOLOCK)
    ON	P.Id = ECA.ProviderId
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = ECA.ModifiedBy
	
	WHERE
		ECA.EquipmentId = @EquipmentId
	AND ECA.CompanyId = @CompanyId
	AND ECA.Active = 1
	
	ORDER BY ECA.Date DESC
END






