





CREATE PROCEDURE [dbo].[Equipment_Update]
	@EquipmentId bigint,
	@CompanyId int,
	@Code nvarchar(50),
	@Description nvarchar(150),
	@TradeMark nvarchar(50),
	@Model nvarchar(50),
	@SerialNumber nvarchar(50),
	@Location varchar(50),
	@MeasureRange nvarchar(50),
	@ScaleDivision numeric(18,4),
	@MeasureUnit bigint,
	@Responsable int,
	@IsCalibration bit,
	@IsVerification bit,
	@IsMaintenance bit,
	@Observations nvarchar(500),
	@Notes nvarchar(500),
	@Active bit,
	@UserId int,
	@Trace nvarchar(50),
	@StartDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE Equipment SET
		[Code] = @Code,
		[Description] = @Description,
		[TradeMark] = @TradeMark,
		[Model] = @Model,
		[SerialNumber] = @Serialnumber,
		[Location] = @Location,
		[MeasureRange] = @MeasureRange,
		[ScaleDivision] = @ScaleDivision,
		[MeasureUnit] = @MeasureUnit,
		[Resposable] = @Responsable,
		[IsCalibration] = @IsCalibration,
		[IsVerification] = @IsVerification,
		[IsMaintenance] = @IsMaintenance,
		[Observations] = @Observations,
		[Notes] = @Notes,
		[Active] = @Active,
		[ModifiedBy] = @UserId,
		[ModifiedOn] = GETDATE(),
		[StartDate] = @StartDate
	WHERE
		Id = @EquipmentId
	AND CompanyId = @CompanyId	
	
		DELETE FROM EquipmentCalibrationDefinition
		WHERE
			EquipmentId = @EquipmentId
		AND @IsCalibration = 0 
		
		DELETE FROM EquipmentVerificationDefinition
		WHERE
			EquipmentId = @EquipmentId
		AND @IsVerification = 0 

	INSERT INTO ActivityLog
	(
		ActivityId,
		UserId,
        CompanyId,
        TargetType,
        TargetId,
        ActionId,
        DateTime,
		ExtraData
    )
    VALUES
	(
		NEWID(),
		@UserId,
		@CompanyId,
		13,
		@EquipmentId,
		2,
		GETDATE(),
		@Trace
    )
END






