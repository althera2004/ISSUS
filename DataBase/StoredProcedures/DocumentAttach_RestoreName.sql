



CREATE PROCEDURE [dbo].[DocumentAttach_RestoreName] 
	@Id bigint,
	@Name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE DocumentAttach SET
		[Description] = @Name
	WHERE
		[Id] = @Id

END



