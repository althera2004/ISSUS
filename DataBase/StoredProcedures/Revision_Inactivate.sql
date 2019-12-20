


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_Inactivate]
	@Id int,
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE [dbo].[Revision] SET		
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn]= GETDATE(),
		[Active] = 0
	WHERE
		Id = @ID
END



