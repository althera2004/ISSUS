

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorHistorico_Insert]
	@Id int output,
	@IndicadorId int,
	@CompanyId int,
	@ActionDate datetime,
	@Reason nvarchar(500),
	@EmployeeId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[IndicadorHistorico]
           ([CompanyId]
           ,[IndicadorId]
           ,[ActionDate]
           ,[Reason]
           ,[EmployeeId]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@CompanyId
           ,@IndicadorId
           ,@ActionDate
           ,@Reason
           ,@EmployeeId
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @Id = @@IDENTITY
END

