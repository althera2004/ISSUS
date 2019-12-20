

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Auditory_Inactivate]
	@AuditoryId bigint,
	@CompanyId int,
    @ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[Auditory] SET
		[Active] = 0,
		[ModifiedBy] = @ApplicationUserId,
		[ModifiedOn] = GETDATE()
	WHERE
		Id = @AuditoryId
	AND CompanyId = @CompanyId
END

