





CREATE PROCEDURE [dbo].[EquipmentCalibrationDefinition_Update]
	@EquipmentCalibrationDefinitionId bigint,
	@EquipmentId bigint,
    @CompanyId int,
    @Operation nvarchar(50),
    @CalibrationType int,
    @Periodicity int,
    @Uncertainty numeric(18,6),
    @Range nvarchar(50),
    @Pattern nvarchar(50),
    @Cost numeric(18,3),
    @Notes text,
    @Responsable int,
    @Provider int,
	@FirstDate datetime,
    @UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentCalibrationDefinition SET
		EquipmentId = @EquipmentId,
		Type = @CalibrationType,
		Operation = @Operation,
		Periodicity = @Periodicity,
		Uncertainty = @Uncertainty,
		Range = @Range,
		Pattern = @Pattern,
		Cost = @Cost,
		Notes = @Notes,
		Responsable = @Responsable,
		ProviderId = @Provider,
		FirstDate = @FirstDate,
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
      WHERE
		Id = @EquipmentCalibrationDefinitionId
	AND CompanyId = @CompanyId

END






