



CREATE PROCEDURE [dbo].[DocumentAttach_Active]
	@Id bigint,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE DocumentAttach SET
		[Active] = 1,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @Id
		
END



