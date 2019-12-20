



CREATE PROCEDURE [dbo].[DocumentAttach_Delete]
	@Id bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM DocumentAttach
	WHERE
		[CompanyId] = @CompanyId
	AND [Id] = @Id
END



