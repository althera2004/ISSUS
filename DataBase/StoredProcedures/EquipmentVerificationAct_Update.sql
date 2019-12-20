





CREATE PROCEDURE [dbo].[EquipmentVerificationAct_Update]
	@EquipmentVerificationActId bigint,
	@EquipmentId bigint,
	@CompanyId int,
	@EquipmentVerificationType int,
	@Date datetime,
	@Vto datetime,
	@Result numeric(18,6),
	@MaxResult numeric(18,6),
	@Cost numeric(18,3),
	@ProviderId bigint,
	@ResponsableId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentVerificationAct SET
		EquipmentVerificationType = @EquipmentVerificationType,
		Date = @Date,
		Vto = @Vto,
		Result = @Result,
		MaxResult = @MaxResult,
		Cost = @Cost,
		ProviderId = @ProviderId,
		Responsable = @ResponsableId,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE	
		Id = @EquipmentVerificationActId
	AND	EquipmentId = @EquipmentId
	AND CompanyId = @CompanyId

END






