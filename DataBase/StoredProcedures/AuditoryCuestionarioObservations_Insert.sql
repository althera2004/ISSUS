


CREATE PROCEDURE [dbo].[AuditoryCuestionarioObservations_Insert]
	@Id bigint output,
	@CompanyId int,
	@AuditoryId bigint,
	@CuestionarioId bigint,
	@Text nvarchar(2000),
	@ApplicationUserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[AuditoryCuestionarioObservations]
           ([CompanyId]
           ,[AuditoryId]
           ,[CuestionarioId]
           ,[Text]
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
           ,@ApplicationUserId
           ,GETDATE()
           ,@ApplicationUserId
           ,GETDATE()
           ,1)

	SET @Id  = @@IDENTITY

END

