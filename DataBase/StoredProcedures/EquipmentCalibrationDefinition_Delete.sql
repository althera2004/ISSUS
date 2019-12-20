





CREATE PROCEDURE [dbo].[EquipmentCalibrationDefinition_Delete]
	@EquipmentCalibrationDefinitionId bigint,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentCalibrationDefinition SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
      WHERE
		Id = @EquipmentCalibrationDefinitionId

END






