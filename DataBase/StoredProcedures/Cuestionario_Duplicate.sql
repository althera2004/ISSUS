

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Cuestionario_Duplicate]
	@Id bigint,
	@Description nvarchar(150),
	@NewId bigint output,
	@CompanyId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO Cuestionario
           ([CompanyId]
           ,[Description]
           ,[NormaId]
           ,[ProcessId]
           ,[ApartadoNorma]
           ,[Notes]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
	SELECT 
           CompanyId
           ,@Description
           ,NormaId
           ,ProcessId
           ,ApartadoNorma
           ,Notes
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1
	FROM Cuestionario 
	WHERE
		Id = @Id
	AND CompanyId = @CompanyId

	SET @NewId = @@IDENTITY

	INSERT INTO CuestionarioPregunta
           ([CompanyId]
           ,[Question]
		   ,[CuestionarioId]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     SELECT
	       CompanyId
           ,Question
		   ,@NewId
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1
	FROM CuestionarioPregunta
	WHERE
		CuestionarioId = @Id
	AND CompanyId = @CompanyId
	AND Active = 1


END

