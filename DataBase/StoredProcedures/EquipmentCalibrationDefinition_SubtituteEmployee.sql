




CREATE PROCEDURE [dbo].[EquipmentCalibrationDefinition_SubtituteEmployee]
	@ItemId bigint,
	@CompanyId int,
	@NewEmployee bigint,
	@UserId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE EquipmentCalibrationDefinition SET
		Responsable = @NewEmployee,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @ItemId
	AND CompanyId = @CompanyId
END




