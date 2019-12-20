-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Revision_Activate]
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
		[Active] = 1
	WHERE
		Id = @ID
END



