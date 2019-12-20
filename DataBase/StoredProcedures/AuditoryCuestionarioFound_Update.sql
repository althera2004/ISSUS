


CREATE PROCEDURE [dbo].[AuditoryCuestionarioFound_Update]
	@Id bigint,
	@CompanyId int,
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
	UPDATE AuditoryCuestionarioFound SET
           [Text] = @Text
           ,[Requeriment] = @Requeriment
           ,[UnConformity] = @UnConformity
		   ,[Action] = @Action
           ,[ModifiedBy] = @ApplicationUserId
           ,[ModifiedOn] = GETDATE()
     
	 WHERE
		Id = @Id
	AND CompanyId = @CompanyId
		
END

