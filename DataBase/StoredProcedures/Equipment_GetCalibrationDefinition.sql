





CREATE PROCEDURE [dbo].[Equipment_GetCalibrationDefinition]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
		EC.Id,
		EC.CompanyId,
		EC.EquipmentId,
		EC.Type,
		EC.Operation,
		EC.Periodicity,
		EC.Uncertainty,
		EC.Pattern,
		EC.Cost,
		EC.Notes,
		EC.Range,
		ISNULL(EC.ProviderId,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription,
		EC.Responsable,
		RESP.Name,
		RESP.LastName,
		EC.ModifiedBy,
		AU.[Login] AS ModifiedByUserName,
		EC.ModifiedOn,
		EC.FirstDate
    FROM EquipmentCalibrationDefinition EC WITH(NOLOCK)
	LEFT JOIN Provider P WITH(NOLOCK)
	ON	P.Id = EC.ProviderId
    INNER JOIN Employee RESP WITH(NOLOCK)
    ON	RESP.Id = EC.Responsable
    INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = EC.ModifiedBy
	
	WHERE
		EC.EquipmentId = @EquipmentId
	AND EC.CompanyId = @CompanyId
	AND EC.Active = 1
END






