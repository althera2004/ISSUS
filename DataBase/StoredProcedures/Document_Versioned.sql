
CREATE PROCEDURE [dbo].[Document_Versioned]
	@DocumentId int,
	@CompanyId int,
	@Version int,
	@UserId int,
	@Reason nvarchar(100),
	@Date datetime
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO DocumentsVersion
    (
		DocumentId,
		Company,
		Version,
		UserCreate,
		Status,
		Date,
		Reason
	)
    VALUES
    (
		@DocumentId,
        @CompanyId,
        @Version,
        @UserId,
        1,
        @Date,

        @Reason
    )
    
    UPDATE Document SET
		ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @DocumentId
	AND CompanyId = @CompanyId

END

