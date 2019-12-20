



CREATE PROCEDURE [dbo].[DocumentAttach_Insert]
	@Id bigint output,
	@DocumentId bigint,
	@CompanyId int,
	@Version int,
	@Description nvarchar(50),
	@Extension nvarchar(10),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM DocumentAttach
	WHERE
		[CompanyId] = @CompanyId
	AND [Version] = @Version
	AND [DocumentId] = @DocumentId 


    -- Insert statements for procedure here
	INSERT INTO [DocumentAttach]
	(
		[CompanyId],
		[DocumentId],
		[Version],
		[Description],
		[Extension],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@CompanyId,
		@DocumentId,
		@Version,
		@Description,
		@Extension,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	)

	SET @Id = @@IDENTITY
END



