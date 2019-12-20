



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Indicador_Insert] 
	@IndicadorId int output,
	@CompanyId int,
	@Description nvarchar(150),
	@ResponsableId int,
	@ProcessId int,
	--@ObjetivoId int,
	@Calculo nvarchar(2000),
	@MetaComparer nvarchar(10),
	@Meta decimal(18,6),
	@AlarmaComparer nvarchar(10),
	@Alarma decimal(18,6),
	@Periodicity int,
	@StartDate datetime,
	@EndDate datetime,
	@EndReason nvarchar(500),
	@EndResponsible int,
	@UnidadId int,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[Indicador]
           ([Descripcion]
		   ,[CompanyId]
		   ,[ResponsableId]
           ,[ProcessId]
           --,[ObjetivoId]
           ,[Calculo]
           ,[MetaComparer]
           ,[Meta]
           ,[AlarmaComparer]
           ,[Alarma]
           ,[Periodicity]
		   ,[StartDate]
           ,[EndDate]
           ,[EndReason]
           ,[EndResponsible]
           ,[UnidadId]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@Description
		   ,@CompanyId
		   ,@ResponsableId
           ,@ProcessId
           --,@ObjetivoId
           ,@Calculo
           ,@MetaComparer
           ,@Meta
           ,@AlarmaComparer
           ,@Alarma
           ,@Periodicity
		   ,@StartDate
           ,@EndDate
           ,@EndReason
           ,@EndResponsible
           ,@UnidadId
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @IndicadorId = @@IDENTITY

END




