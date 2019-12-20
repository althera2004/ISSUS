


-- =============================================
-- Author:		Juan Castilla Calderón
-- Create date: 12/10/2016
-- Description:	Inserta un adjunto
-- =============================================
CREATE PROCEDURE [dbo].[UploadFiles_Insert]
	@Id bigint output,
	@CompanyId bigint,
	@ItemLinked int,
	@ItemId bigint,
	@FileName nvarchar(250),
	@Description nvarchar(100),
	@Extension nvarchar(10),
	@ApplicationUserId int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


INSERT INTO [dbo].[UploadFiles]
	(
		[CompanyId],
		[ItemLinked],
		[ItemId],
		[FileName],
		[Description],
		[Extension],
		[CreatedBy],
		[CreatedOn],
		[ModifiedBy],
		[ModifiedOn],
		[Active]
	)
	VALUES
	(
		@CompanyId,
		@ItemLinked,
		@ItemId,
		@FileName,
		@Description,
		@Extension,
		@ApplicationUserId,
		GETDATE(),
		@ApplicationUserId,
		GETDATE(),
		1
	)

	SET @Id = @@IDENTITY
END



