





CREATE PROCEDURE [dbo].[EquipmentRepair_Delete]
	@EquipmentRepairId bigint,
	@CompanyId int,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentRepair SET
    	Active = 0,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @EquipmentRepairId
	AND CompanyId = @CompanyId


END






