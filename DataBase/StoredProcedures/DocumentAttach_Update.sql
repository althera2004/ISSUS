



CREATE PROCEDURE [dbo].[DocumentAttach_Update]
	@Id bigint,
	@Description nvarchar(50),
	@Extension nvarchar(10),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE DocumentAttach SET
		[Description] = @Description,
		[Extension] = @Extension,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @Id
		
END



