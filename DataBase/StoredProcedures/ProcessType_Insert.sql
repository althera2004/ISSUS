





CREATE PROCEDURE [dbo].[ProcessType_Insert]
	@ProcessTypeId int out,
	@CompanyId int,
	@Description nvarchar(50),
	@UserId int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO ProcessType
    (
		CompanyId,
		Description,
		Active,
		CreatedBy,
		CreatedOn,
		ModifiedBy,
		ModifiedOn
	)
	VALUES
	(
		@CompanyId,
		@Description,
		1,
		@UserId,
		GETDATE(),
		@UserId,
		GETDATE()
	)
	
	SET @ProcessTypeId = @@IDENTITY	

END






