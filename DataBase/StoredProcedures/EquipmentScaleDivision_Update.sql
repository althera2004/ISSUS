





CREATE PROCEDURE [dbo].[EquipmentScaleDivision_Update]
	@EquipmentScaleDivisionId bigint,
	@Description nvarchar(20),
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;
	
    UPDATE EquipmentScaleDivision SET
		Description = @Description
	WHERE
		Id = @EquipmentScaleDivisionId
		
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
		19,
		@EquipmentScaleDivisionId,
		2,
		GETDATE(),
		'Description:' + @Description
    )	
END






