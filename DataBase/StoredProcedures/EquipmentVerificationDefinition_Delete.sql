





CREATE PROCEDURE [dbo].[EquipmentVerificationDefinition_Delete]
	@EquipmentVerificationDefinitionId bigint,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentVerificationDefinition SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
      WHERE
		Id = @EquipmentVerificationDefinitionId

END






