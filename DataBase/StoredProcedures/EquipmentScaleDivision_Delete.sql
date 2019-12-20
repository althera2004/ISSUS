





CREATE PROCEDURE [dbo].[EquipmentScaleDivision_Delete]
	@EquipmentScaleDivisionId bigint,
	@CompanyId int,
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE EquipmentScaleDivision SET
		Active = 0
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
		3,
		GETDATE(),
		''
    )	
END






