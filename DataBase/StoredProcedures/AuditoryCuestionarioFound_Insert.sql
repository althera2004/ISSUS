


CREATE PROCEDURE [dbo].[AuditoryCuestionarioFound_Insert]
	@Id bigint output,
	@CompanyId int,
	@AuditoryId bigint,
	@CuestionarioId bigint,
	@Text nvarchar(2000),
	@Requeriment nvarchar(2000),
	@UnConformity nvarchar(2000),
	@Action bit,
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[AuditoryCuestionarioFound]
           ([CompanyId]
           ,[AuditoryId]
           ,[CuestionarioId]
           ,[Text]
           ,[Requeriment]
           ,[UnConformity]
		   ,[Action]
           ,[CreatedBy]
           ,[CreatedOn]
           ,[ModifiedBy]
           ,[ModifiedOn]
           ,[Active])
     VALUES
           (@CompanyId
           ,@AuditoryId
           ,@CuestionarioId
           ,@Text
           ,@Requeriment
           ,@UnConformity
		   ,@Action
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @Id  = @@IDENTITY

END

