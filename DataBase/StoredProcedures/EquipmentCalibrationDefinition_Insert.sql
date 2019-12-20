





CREATE PROCEDURE [dbo].[EquipmentCalibrationDefinition_Insert]
	@EquipmentCalibrationDefinitionId bigint output,
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

    INSERT INTO EquipmentCalibrationDefinition
    (
		EquipmentId,
		CompanyId,
		Operation,
		Type,
		Periodicity,
		Uncertainty,
		Range,
		Pattern,
		Cost,
		Notes,
		Responsable,
		ProviderId,
		FirstDate,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@EquipmentId,
		@CompanyId,
		@Operation,
		@CalibrationType,
		@Periodicity,
		@Uncertainty,
		@Range,
		@Pattern,
		@Cost,
		@Notes,
		@Responsable,
		@Provider,
		@FirstDate,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)

	SET @EquipmentCalibrationDefinitionId = @@IDENTITY
	 
	UPDATE Equipment SET
		IsCalibration = 1
		WHERE
		Id = @EquipmentId


END





