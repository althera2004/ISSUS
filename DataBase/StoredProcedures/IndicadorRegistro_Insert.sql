


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorRegistro_Insert]
	@Id int output,
	@IndicadorId int,
	@CompanyId int,
	@Date datetime,
	@MetaComparer nvarchar(10),
	@Meta decimal(18,6),
	@AlarmComparer nvarchar(10),
	@Alarm decimal (18,6),
	@Value decimal (18,6),
	@ResponsibleId int,
	@Comments nvarchar(500),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[IndicadorRegistro]
           ([CompanyId]
           ,[IndicadorId]
		   ,[MetaComparer]
           ,[Meta]
		   ,[AlarmComparer]
           ,[Alarm]
           ,[Date]
           ,[Value]
           ,[ResponsibleId]
           ,[Comments]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@CompanyId
           ,@IndicadorId
		   ,@MetaComparer
           ,@Meta
		   ,@AlarmComparer
           ,@Alarm
           ,@Date
           ,@Value
           ,@ResponsibleId
           ,@Comments
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @Id = @@IDENTITY

END



