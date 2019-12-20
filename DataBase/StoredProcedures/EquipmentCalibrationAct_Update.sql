





CREATE PROCEDURE [dbo].[EquipmentCalibrationAct_Update]
	@EquipmentCalibrationActId bigint output,
	@EquipmentId bigint,
	@CompanyId int,
	@EquipmentCalibrationType int,
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

    UPDATE EquipmentCalibrationAct SET
		EquipmentCalibrationType = @EquipmentCalibrationType,
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
		Id = @EquipmentCalibrationActId
	AND	EquipmentId = @EquipmentId
	AND CompanyId = @CompanyId

END






