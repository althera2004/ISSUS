





CREATE PROCEDURE [dbo].[Equipment_Insert]
	@EquipmentId bigint output,
	@CompanyId int,
	@Code nvarchar(50),
	@Description nvarchar(150),
	@TradeMark nvarchar(50),
	@Model nvarchar(50),
	@SerialNumber nvarchar(50),
	@Location nvarchar(50),
	@MeasureRange nvarchar(50),
	@ScaleDivision numeric(18,4),
	@MeasureUnit bigint,
	@Responsable int,
	@IsCalibration bit,
	@IsVerification bit,
	@IsMaintenance bit,
	@Observations nvarchar(500),
	@UserId int,
	@Notes nvarchar(500),
	@StartDate datetime
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Equipment
	(
		CompanyId,
		Code,
		Description,
		TradeMark,
		Model,
		SerialNumber,
		Location,
		MeasureRange,
		ScaleDivision,
		MeasureUnit,
		Resposable,
		IsCalibration,
		IsVerification,
		IsMaintenance,
		Observations,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn,
		Notes,
		StartDate
	)
	VALUES
	(
		@CompanyId,
		@Code,
		@Description,
		@TradeMark,
		@Model,
		@SerialNumber,
		@Location,
		@MeasureRange,
		@ScaleDivision,
		@MeasureUnit,
		@Responsable,
		@IsCalibration,
		@IsVerification,
		@IsMaintenance,
		@Observations,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE(),
		@Notes,
		@StartDate
	)
	
	SET @EquipmentId = @@IDENTITY;

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
		1,
		GETDATE(),
		@Description
    )
END






