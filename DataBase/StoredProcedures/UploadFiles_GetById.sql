


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UploadFiles_GetById]
	@Id bigint,
	@CompanyId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		UF.Id,
		UF.CompanyId,
		UF.ItemLinked,
		UF.ItemId,
		UF.[FileName],
		UF.[Description],
		UF.Extension,
		UF.CreatedBy,
		CB.[Login],
		UF.CreatedOn,
		UF.ModifiedBy,
		MB.[Login],
		UF.ModifiedOn,
		UF.Active
	FROM UploadFiles UF WITH(NOLOCK)
	INNER JOIN ApplicationUser CB With (Nolock)
	On CB.Id = UF.CreatedBy
	INNER JOIN ApplicationUser MB With (Nolock)
	On MB.Id = UF.ModifiedBy
	WHERE
		UF.Id = @Id
END



