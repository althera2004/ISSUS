

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CuestionarioPregunta_Insert] 
	@Id bigint output,
	@CompanyId int,
	@CuestionarioId bigint,
	@Question nvarchar(2000),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[CuestionarioPregunta]
           ([CompanyId]
           ,[CuestionarioId]
           ,[Question]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@CompanyId
           ,@CuestionarioId
           ,@Question
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
		   ,1)

	SET @Id = @@IDENTITY
END

