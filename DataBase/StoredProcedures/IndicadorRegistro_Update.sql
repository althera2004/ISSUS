


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IndicadorRegistro_Update]
	@Id int,
	@Date datetime,
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
	UPDATE IndicadorRegistro SET
		[Date] = @Date,
		Value = @Value,
		ResponsibleId = @ResponsibleId,
		Comments = @Comments,
		ModifiedBy = @ApplicationUserId,
		ModifiedOn = GETDATE()
	WHERE
		Id = @Id

END



