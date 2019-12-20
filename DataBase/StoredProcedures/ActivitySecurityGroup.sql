





CREATE PROCEDURE [dbo].[ActivitySecurityGroup]
	@UserId int,
	@CompanyId int,
	@TargetId int,
	@ActionId int,
	@ExtraData nvarchar(150)
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO ActivityLog
	(
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
		@UserId,
		@CompanyId,
		3,
		@TargetId,
		@ActionId,
		GETDATE(),
		@ExtraData
    )
END






