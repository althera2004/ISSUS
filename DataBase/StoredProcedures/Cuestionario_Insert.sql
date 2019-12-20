

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Cuestionario_Insert]
	@Id bigint output,
	@CompanyId int,
	@Description nvarchar(150),
	@NormaId bigint,
	@ProcessId bigint,
	@ApartadoNorma nvarchar(50),
	@Notes nvarchar(2000),
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
     VALUES
           (@CompanyId
           ,@Description
           ,@NormaId
           ,@ProcessId
           ,@ApartadoNorma
           ,@Notes
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

		SET @Id = @@IDENTITY

END

