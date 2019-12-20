





CREATE PROCEDURE [dbo].[EquipmentVerificationAct_Delete]
	@EquipmentVerificationActId bigint output,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentVerificationAct SET
		Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE	
		Id = @EquipmentVerificationActId

END






