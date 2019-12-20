

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AuditoryCuestionarioImprovement_Update]
	@Id bigint output,
	@CompanyId int,
	@Text nvarchar(2000),
	@Action bit,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE AuditoryCuestionarioImprovement SET
		[Text] = @Text,
		[Action] = @Action,
		[ModifiedBy] = @ApplicationUserId,
        [ModifiedOn] = GETDATE()
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

END

