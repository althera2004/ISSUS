





CREATE PROCEDURE [dbo].[Document_Delete]
	@DocumentId bigint,
	@CompanyId int,
	@UserId int,
	@ExtraData nvarchar(200)
AS
BEGIN
	SET NOCOUNT ON;

    UPDATE Document SET
		Activo = 0
	WHERE
		Id = @DocumentId
	AND CompanyId = @CompanyId
	
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
		4,
		@DocumentId,
		3,
		GETDATE(),
		@ExtraData
    )
END






