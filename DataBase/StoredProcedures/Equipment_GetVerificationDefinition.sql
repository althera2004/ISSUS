





CREATE PROCEDURE [dbo].[Equipment_GetVerificationDefinition]
	@EquipmentId bigint,
	@CompanyId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
		EV.Id,
		EV.CompanyId,
		EV.EquipmentId,
		EV.VerificationType,
		EV.Operation,
		EV.Periodicity,
		EV.Uncertainty,
		EV.Pattern,
		EV.Cost,
		EV.Notes,
		EV.Range,
		EV.Responsable,
		RESP.Name,
		RESP.LastName,
		EV.ModifiedBy AS ModifiedByUserId,
		AU.[Login] AS ModifiedByUserName,
		EV.ModifiedOn,
		ISNULL(EV.ProviderId,0) AS ProviderId,
		ISNULL(P.Description,'') AS ProviderDescription,
		EV.FirstDate
    FROM EquipmentVerificationDefinition EV WITH(NOLOCK)
    INNER JOIN Employee RESP WITH(NOLOCK)
    ON	RESP.Id = EV.Responsable
	INNER JOIN ApplicationUser AU WITH(NOLOCK)
	ON	AU.Id = EV.ModifiedBy
	LEFT JOIN Provider P WITH(NOLOCK)
	ON	P.Id = EV.ProviderId
	
	WHERE
		EV.EquipmentId = @EquipmentId
	AND EV.CompanyId = @CompanyId
	AND EV.Active = 1
END






