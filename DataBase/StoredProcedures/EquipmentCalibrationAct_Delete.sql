





CREATE PROCEDURE [dbo].[EquipmentCalibrationAct_Delete]
	@EquipmentCalibrationActId bigint output,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentCalibrationAct SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE	
		Id = @EquipmentCalibrationActId

END






